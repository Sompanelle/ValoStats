using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ValoStats.Models
{
    public class Config
    {
        public string Name { get; set; }
        public string Tag { get; set; }
        public string Region { get; set; }
        public string Key { get; set; }


        public Config(string Name, string Tag, string Region, string Key)
        {
            this.Name = Name;
            this.Tag = Tag;
            this.Region = Region;
            this.Key = Key;
        }


        
    }
}
