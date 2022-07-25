using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorApp1.Shared
{
    public class Folder
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public Folder? ParentFolder { get; set; }
        public virtual List<Folder> SubFolders { get; set; } = new List<Folder>();
        public virtual List<FileManaged> Files { get; set; } = new List<FileManaged>();
        public bool IsExpanded { get; set; } = false;
    }

    public class FileManaged
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public Guid FolderId { get; set; }

        public string Path { get; set; }
    }
}
