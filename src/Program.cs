using System.Diagnostics;
using vscode.Models;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Xml;

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
            foreach(var extension in config.extensions)
            {
                var output = VsCodeCommand($"code --install-extension {extension} --profile {filename}");
                // improve output text to only use the parts that look nice from the text.
                if(output.Contains("was successfully installed.") || output.Contains("is already installed."))
                {
                    Console.WriteLine(output);
                }
                else
                {
                    Console.WriteLine("unknown error");
                }
                
            }
            
            // Add a list of extensions that were not uninstalled and do about 5 reruns perextension after a loop through of extensions due to dependencies
            foreach(var extension in config.extensions)
            {
                
                var output = VsCodeCommand($"code --uninstall-extension {extension} --profile {filename}");
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
                else
                {
                    Console.WriteLine("unknown error");
                }
                
            }
            
            
            
            
            //MoveFile();
        }

        private static void MoveFile()
        {
            File.Create("./profiles/config.json");
            Directory.CreateDirectory("profiles");
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
