using Notepad.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notepad.Models.ExplorerEntities
{
    public abstract class FileAndDirectoryEntityViewModel : ViewModelBase
    {
        public string Name { get; }
        public string FullName { get; set; }
        public string Icon { get; set; }
        public FileAndDirectoryEntityViewModel(string name)
        {
            Name = name;
        }
    }
}
