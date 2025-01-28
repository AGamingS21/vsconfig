using System.CommandLine;
using System.CommandLine.Invocation;
using Microsoft.VisualBasic;


namespace vsconfig
{
    internal class Program
    {
        static int Main(string[] args)
        {

            // Determine user folder
            var homeFolder = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            
            // if in debug mode use the profiles in the repo
            #if DEBUG
                var path = "./src/profiles/";
            #else   
                string path = $"{homeFolder}/.config/vsconfig/";
            #endif

            var rootCommand = new RootCommand();

            rootCommand.Description = "vsconfig is a tool to setup vscode or any fork of it.";

            // configure subcommand setup            
            var forkOption = new Option<string>("--fork", "The fork you would like to configure. vscode, vscodium, vscode-oss, etc.");
            forkOption.SetDefaultValue("vscode");

            forkOption.AddAlias("-f");
            
            var pathOption = new Option<string>("--path", "The path to the folder where the config files are located.");
            pathOption.SetDefaultValue(path);
            pathOption.AddAlias("-p");
            
            var configureSubCommand = new Command("configure", "Create profiles based on config files.");
            configureSubCommand.AddOption(forkOption);
            configureSubCommand.AddOption(pathOption);
            


            configureSubCommand.SetHandler((path, fork) =>
            {
                new ConfigManager(path, fork)
                    .CreateProfile();
            }, 
            pathOption, forkOption);

            rootCommand.Add(configureSubCommand);
            
        
            return rootCommand.Invoke(args);

    
        
        }

    }
}
