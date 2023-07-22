using System;

namespace CryptoViewer.Utilities.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    internal class ItemColumnDataAttribute : Attribute
    {
        public string Name { get; set; }
        public int Width { get; set; }
        public string StringFormat { get; set; }
        public string Group { get; set; }

        public ItemColumnDataAttribute() { }

        public ItemColumnDataAttribute(string name, int width = 100, string stringFormat = "", string group = "")
        {
            Name = name;
            Width = width;
            StringFormat = stringFormat;
            Group = group;
        }
    }
}
