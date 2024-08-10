using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using Avalonia.Media.Imaging;

namespace ValoStats.Models
{
    public class PageItemTemplate
    {

        public PageItemTemplate(Type Type, string IconKey)
        {
            ModelType = Type;
            Label = Type.Name.Replace("PageViewModel", "");
            Application.Current!.TryFindResource(IconKey, out var res);
            Icon = (StreamGeometry)res;
        }


        public string Label { get; }
        public Type ModelType { get; }
        public StreamGeometry Icon { get; }
    }
}
