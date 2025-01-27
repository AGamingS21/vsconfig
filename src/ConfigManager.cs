using System.Diagnostics.Contracts;
using vscode.Models;
using vscode.Helpers;
using System.Text.Json;
using System.Diagnostics;
using System.Text;
using System.Xml.Linq;
using System.IO;

namespace vscode
{
    public class ConfigManager{

        public string ProfilePath {get; set;}
        public string CliName {get; set;}
        public string CodePath {get; set;}
        public string UserFolder {get;} = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
        public List<Profile> Profiles {get; set;}

        public ConfigManager(string profilePath, string cliName)
        {
            ProfilePath = profilePath;
            Profiles = FileHelper.CreateObjectsFromJsonDirectory<Profile>(ProfilePath);
            CliName = cliName;
            CodePath = $"{UserFolder}/.config/{CliName}/User/"; 
        }

        public void CreateProfile()
        {
            CreateStorageFile();
            
            foreach(var profile in Profiles)
            {    
                Console.WriteLine($"Installing profile: {profile.Name}");
                CreateProfileFolders(profile);
                UninstallExtensions(profile);
                InstallExtensions(profile);
            }
        }

        private void CreateStorageFile()
        {
            var userDataProfiles = new List<userDataProfiles>();
            foreach(var profile in Profiles)
            {
                userDataProfiles.Add( new userDataProfiles
                {
                    location = profile.Name,
                    name = profile.Name,
                    useDefaultFlags = profile.UseDefaultFlags
                });
            }

            var storage = new StorageFile();
            storage.userDataProfiles = userDataProfiles;

            string userFolder = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            
            var path = $"{userFolder}/.config/{CliName}/User/globalStorage/storage.json";
            //var path1 = $"./storage.json";
            if(FileHelper.CheckIfFileExists(path))
            {
                    // Read the entire JSON file
                string jsonString = File.ReadAllText(path);

                // Step 2: Deserialize the JSON string into a Dictionary
                var jsonDict = JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(jsonString);

                // Step 3: Modify the specific property (e.g., "City")
                if (jsonDict != null && jsonDict.ContainsKey("userDataProfiles"))
                {
                    jsonDict["userDataProfiles"] = JsonSerializer.SerializeToElement(userDataProfiles);
                }
                else
                {
                    jsonDict.Add("userDataProfiles", JsonSerializer.SerializeToElement(userDataProfiles));
                }

                // Step 4: Serialize the modified dictionary back into a JSON string
                string modifiedJson = JsonSerializer.Serialize(jsonDict, new JsonSerializerOptions { WriteIndented = true });

                // Step 5: Write the modified JSON back to the file
                File.WriteAllText(path, modifiedJson);
            }
            else
            {
                FileHelper.CreateFile(path, storage);
            }
        }

        private void CreateProfileFolders(Profile profile)
        {
            string userFolder = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            var path = $"{userFolder}/.config/{CliName}/User/profiles/{profile.Name}";
            if(!FileHelper.CheckIfDirectoryExists(path))
            {
                Directory.CreateDirectory(path);
            }

            // Create settings files for the profile    
            FileHelper.CreateFile(path + "/settings.json", profile.Settings);
            FileHelper.CreateFile(path + "/keybindings.json", profile.Keybindings);
            FileHelper.CreateFile(path + "/tasks.json", profile.Tasks);
        }


        private void InstallExtensions(Profile profile)
        {            
            // Find out all of the installed extensions
            var output = MakeTerminalCommand($"{CliName.ToLower()} --list-extensions --profile {profile.Name}");

            // Get list of extensions to be installed from config file
            List<ExtensionStatus> installedExtensions = profile.Extensions.Select(item => new ExtensionStatus() { Name = item } ).ToList();
            var installedOnVsCode = output.Split('\n');
            // remove last item from index as it is an empty string
            installedOnVsCode = installedOnVsCode.Take(installedOnVsCode.Length - 1).ToArray();
            // Check which secrets are not installed
            var extensionsToInstall = installedExtensions.Where(list => !output.Contains(list.Name)).ToList();
            
            // Install the extensions
            extensionsToInstall = UpdateExtensions(extensionsToInstall, true, profile.Name);               
        }

        private void UninstallExtensions(Profile profile)
        {

            // Get All extensions from config file
            var extensionsConfig = new List<string>(profile.Extensions); 
            // Find out all of the installed extensions
            var output = MakeTerminalCommand($"{CliName.ToLower()} --list-extensions --profile {profile.Name}");
            var installedOnVsCode = output.Split('\n');
            // remove last item from index as it is an empty string
            installedOnVsCode = installedOnVsCode.Take(installedOnVsCode.Length - 1).ToArray();

            var extensionsToUninstall = installedOnVsCode.Where(i => !extensionsConfig.Contains(i))
            .ToList()
            .Select(item => new ExtensionStatus() { Name = item, Installed = true } ).ToList();

            
            // Uninstall the extensions
            // loop through extensions and uninstall them.
            for(int i = 0; i < 5; i++)
            {
                if(extensionsToUninstall.Count == 0)
                {
                    i = 5;
                }
                else
                {
                    extensionsToUninstall = UpdateExtensions(extensionsToUninstall, false, profile.Name);            
                    extensionsToUninstall = extensionsToUninstall.Where(item => item.Installed == true).ToList();
                }
            }
        }


        
        private List<ExtensionStatus> UpdateExtensions(List<ExtensionStatus> extensions, bool install, string profile)
        {
            string text;
            if(install)
                text = "install-extension";
            else 
                text = "uninstall-extension";        
            
            // To Do: Make the output looks nicer by getting substrings of the outputted text that I want to show.
            foreach(var extension in extensions)
            {
                extension.Attempts++;
                var output = MakeTerminalCommand($"{CliName.ToLower()} --{text} {extension.Name} --profile {profile}");
                if(output.Contains("Cannot uninstall") && (output.Contains("extension depends on this.") || output.Contains("extension depend on this.")))
                {
                    // add to list of extensions
                    Console.WriteLine(output);
                }
                else if(output.Contains("is not installed."))
                {
                    Console.WriteLine(output);
                    extension.Installed = false;
                }
                else if(output.Contains("was successfully uninstalled!"))
                {
                    Console.WriteLine(output);
                    extension.Installed = false;
                }
                else if(output.Contains("was successfully installed.") || output.Contains("is already installed."))
                {
                    Console.WriteLine(output);
                    extension.Installed = true;
                }
                else
                {
                    Console.WriteLine("unknown error");
                    Console.WriteLine(output);
                }

            }

            return extensions;
        }

        // Executes a command on the terminal and returns the result
        private string MakeTerminalCommand(string command)
        {
            var psi = new ProcessStartInfo();
            psi.FileName = "/bin/bash";
            psi.Arguments = $"-c \"{command}\"";
            psi.RedirectStandardError = true;
            psi.RedirectStandardOutput = true;
            psi.UseShellExecute = false;
            psi.CreateNoWindow = true;

            using var process = Process.Start(psi);
            process.WaitForExit();

            if(process.ExitCode == 1)
                return process.StandardError.ReadToEnd();
            else
                return process.StandardOutput.ReadToEnd();
        }

    }
}
