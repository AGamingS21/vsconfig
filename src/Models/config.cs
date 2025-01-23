using System.Diagnostics.Contracts;

namespace vscode.Models
{
    public class Config{
        public string? profileName {get; set;}
        public string[]? extensions {get;set;}
    }
}