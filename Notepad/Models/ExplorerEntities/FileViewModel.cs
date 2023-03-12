using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notepad.Models.ExplorerEntities
{
    public sealed class FileViewModel : FileAndDirectoryEntityViewModel
    {
        public FileViewModel(string name) : base(name) {
            Icon = "Assets/file_icon.png";
        }
        public FileViewModel(FileInfo file) : base(file.Name) 
        {
            FullName = file.FullName;
            Icon = "Assets/file_icon.png";
        }

    }
}
