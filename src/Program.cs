using System.CommandLine;
using System.CommandLine.Invocation;
using Microsoft.VisualBasic;


namespace vscode
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
                string path = $"{homeFolder}/.config/configium/";
            #endif

            var rootCommand = new RootCommand
            {
                new Option<int>("--number", "An integer option")
            };

            rootCommand.Description = "vsconfig is a tool to setup vscode or any fork of it.";

            // configure subcommand setup            
            var cliOption = new Option<string>("--cli", "The cli you would like to configure. vscode, vscodium, vscode-oss, etc.");
            cliOption.SetDefaultValue("vscode");
            cliOption.AddAlias("-c");
            
            var pathOption = new Option<string>("--path", "The path to the folder where the config files are located.");
            pathOption.SetDefaultValue(path);
            pathOption.AddAlias("-p");
            
            var configureSubCommand = new Command("configure", "Create profiles based on config files.");
            configureSubCommand.AddOption(cliOption);
            configureSubCommand.AddOption(pathOption);
            


            configureSubCommand.SetHandler((path, cli) =>
            {
                //Console.WriteLine(path);
                Console.WriteLine(cli);
                new ConfigManager(path, cli)
                    .CreateProfile();
            }, 
            pathOption, cliOption);

            rootCommand.Add(configureSubCommand);
            
        
            return rootCommand.Invoke(args);

    
        
        }

    }
}
