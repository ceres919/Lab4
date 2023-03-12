using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notepad.Models.ExplorerEntities
{
    public sealed class DirectoryViewModel : FileAndDirectoryEntityViewModel
    {
        public DirectoryViewModel(string name) : base(name)
        {
            FullName = name;
            Icon= "Assets/dir.png";
        }
        public DirectoryViewModel(string name, string icon) : base(name)
        {
            FullName = name;
            Icon = icon;
        }
        public DirectoryViewModel(DirectoryInfo directory) : base(directory.Name)
        {
            FullName = directory.FullName;
            Icon = "Assets/dir.png";
        }
    }
}
