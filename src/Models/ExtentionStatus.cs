using System.Diagnostics.Contracts;

namespace vscode.Models
{
    public class ExtensionStatus{
        public string? Name {get; set;}
        public bool Installed {get;set;} = false;
        public int Attempts {get; set;} = 0;
    }
}