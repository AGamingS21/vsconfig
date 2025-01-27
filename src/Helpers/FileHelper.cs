using System.Diagnostics.Contracts;
using System.Text.Json;
using System.Text;

namespace vscode.Helpers
{
    public static class FileHelper
    {
        public static void CreateFile(string path, object ob)
        {
            DeleteFileIfExists(path);
            using (StreamWriter writer = new StreamWriter(path, true, Encoding.ASCII))
            {
                writer.Write(JsonSerializer.Serialize(ob));
            }
        }

        public static List<T> CreateObjectsFromJsonDirectory<T>(string path)
        {
            var list = new List<T>();
            var directory = Directory.GetFiles(path);

            foreach(var file in directory)
            {            
                var options = new JsonSerializerOptions
                {
                    ReadCommentHandling = JsonCommentHandling.Skip
                };
                list.Add(JsonSerializer.Deserialize<T>(File.ReadAllText(file), options));
            }

            return list;
        }


        public static void DeleteFileIfExists(string path)
        {
            if(File.Exists(path))
            {
                File.Delete(path);
            }
        }

        public static bool CheckIfFileExists(string path)
        {
            if(File.Exists(path))
            {
                return true;
            }
            else 
            {
                return false;
            }
                
        }

        public static bool CheckIfDirectoryExists(string path)
        {
            if(Directory.Exists(path))
            {
                return true;
            }
            else 
            {
                return false;
            }
                
        }

    }
}