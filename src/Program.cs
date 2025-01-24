using System.Diagnostics;
using vscode.Models;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Xml;
using System.Collections;

namespace vscode
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Console.WriteLine("Hello, World!");
            
            var path = "./profiles.json";
            var filename = "dotnet";

            var config = JsonSerializer.Deserialize<Config>(File.ReadAllText(path));

            // To do:
            // Create a list of extensions and if they were installed. This will ensure that if it is uninstalled from vscode it will be removed from config
            // Create the output file.
            List<ExtensionStatus> extensionsToInstall = config.extensions.Select(item => new ExtensionStatus() { Name = item } ).ToList();
            extensionsToInstall = InstallExtensions(extensionsToInstall, true, filename);      
            
            
            // To do: Read in the file from vscode config and then compare with what has alread been installed and then that is the list.
            // For loop 5 times of the files that do need to be uninstalled.
            List<ExtensionStatus> extensionsToUninstall = config.extensions.Select(item => new ExtensionStatus() { Name = item } ).ToList();
            var unInstalledextensions = InstallExtensions(extensionsToUninstall, false, filename);            
            
            
            
            //MoveFile();
        }

        private static void MoveFile()
        {
            File.Create("./profiles/config.json");
            Directory.CreateDirectory("profiles");
        }

        private static List<ExtensionStatus> InstallExtensions(List<ExtensionStatus> extensions, bool install, string profile)
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
                var output = VsCodeCommand($"code --{text} {extension.Name} --profile {profile}");
                if(output.Contains("Cannot uninstall") && (output.Contains("extension depends on this.") || output.Contains("extension depend on this.")))
                {
                    // add to list of extensions
                    Console.WriteLine(output);
                }
                else if(output.Contains("is not installed."))
                {
                    Console.WriteLine(output);
                }
                else if(output.Contains("was successfully uninstalled!"))
                {
                    Console.WriteLine(output);
                }
                else if(output.Contains("was successfully installed.") || output.Contains("is already installed."))
                {
                    Console.WriteLine(output);
                    extension.Installed = true;
                }
                else
                {
                    Console.WriteLine("unknown error");
                }

            }

            return extensions;
        }

        private static string VsCodeCommand(string command)
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
