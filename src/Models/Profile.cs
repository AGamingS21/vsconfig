using System.Diagnostics.Contracts;
using System.Text.Json;

namespace vscode.Models
{
    public class Profile{
        public string? Name {get; set;}
        public string[]? Extensions {get;set;}
        public List<Keybindings> Keybindings {get;set;}
        public Dictionary<string, JsonElement> Settings {get;set;}
        public Dictionary<string, JsonElement> Tasks {get;set;}
        public Dictionary<string, bool> UseDefaultFlags {get; set;}
    }

    public class Keybindings{
        public string? key {get; set;}
        public string? command {get;set;}
        public string? when {get;set;}
    }

    public class StorageFile
    {
        public List<userDataProfiles> userDataProfiles {get;set;}
    }

    public class userDataProfiles
    {
        public string location {get; set;}
        public string name {get;set;}
        public Dictionary<string, bool> useDefaultFlags {get;set;}
    }
}