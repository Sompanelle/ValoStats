using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Avalonia;

namespace ValoStats.Models
{
    public class PageItemTemplate
    {

        public PageItemTemplate(Type Type, string IconKey)
        {
            ModelType = Type;
            Label = Type.Name.Replace("PageViewModel", "");

        }


        public string Label { get; }
        public Type ModelType { get; }

    }
}
