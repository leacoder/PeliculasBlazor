using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorApp1.Shared
{
    public class FlatFolder
    {
        public int Id { get; set; }

        public string Text { get; set; }

        public int? Parent { get; set; }

        public string Icon { get; set; }

        public bool HasChildren { get; set; }
    }

    public enum ElementType
    {
        folder = 0,
        file = 1
    }
}
