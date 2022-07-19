using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorApp1.Shared
{
    public class Folder
    {
        public string Name { get; set; }
        public string? Description { get; set; }
        public int? ParentFolderId { get; set; }
        public Folder? ParentFolder { get; set; }
        public virtual List<Folder> SubFolders { get; set; } = new List<Folder>();
        public bool IsExpanded { get; set; }
    }
}
