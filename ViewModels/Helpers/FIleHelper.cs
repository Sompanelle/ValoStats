    using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using ValorantTrackerApp.Models;
using System.Text.Json;

namespace ValoStats.ViewModels.Helpers
{
    public class FileHelper
    {
        //make folder, store json file in it. not too hard.
        private static string currentDirectory = Directory.GetCurrentDirectory();
        private static string configDir = @$"{currentDirectory}\settings.json";

        public static bool SettingsExist()
        {
            return File.Exists(configDir);
        }
        
        public static Config? ReadConfig()
        {
            if (!SettingsExist())
            {
                return null;
            }
            else
            {
                string data = File.ReadAllText(configDir);
                Config? _ = JsonSerializer.Deserialize<Config>(data);
                if (_ == null)
                {
                    File.Delete(configDir);
                    return null;
                }
                return new Config()
                {
                    Name = _.Name,
                    Tag = _.Tag,
                    Region = _.Region,
                    Key = _.Key,
                };
            }
        }

        public static void WriteConfig(Config Config)
        {
            string data = JsonSerializer.Serialize<Config>(Config);
            File.WriteAllText(configDir,data);
        }


    }
}
