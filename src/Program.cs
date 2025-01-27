using System.CommandLine;
using System.CommandLine.Invocation;


namespace vscode
{
    internal class Program
    {
        static int Main(string[] args)
        {
            
            var rootCommand = new RootCommand
            {
                new Option<int>("--number", "An integer option"),
                //new Option<bool>("--flag", "A boolean option"),
                //new Argument<string>("input", "A required input argument")
            };

            rootCommand.Description = "A simple CLI app";
            // rootCommand.Handler = CommandHandler.Create<int, bool, string>((number, flag, input) =>
            // {
            //     Console.WriteLine($"Number: {number}");
            //     Console.WriteLine($"Flag: {flag}");
            //     Console.WriteLine($"Input: {input}");
            // });

            // How to have a --help setup
            // output errors if no profile can be found.
            // Have a default and an injected profile .configium path


            var homeFolder = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            //var path = $"{userFolder}/.config/Code/User/globalStorage/storage.json";
            #if DEBUG
                var path = "./src/profiles/";
            #else   
                //string path = $"{homeFolder}/.config/configium/";
            #endif
            var cliName = "Code";
            var configManager = new ConfigManager(path, cliName);

            rootCommand.SetHandler(() => 
            {
                configManager.CreateProfile();
            });


            
        
            return rootCommand.Invoke(args);

    
        
        }

    }
}
