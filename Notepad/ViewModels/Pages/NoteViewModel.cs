using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notepad.ViewModels.Pages
{
    public class NoteViewModel : ViewModelBase
    {
        private string? _textFile = "";
        public string? TextFile
        {
            get => _textFile;
            set => this.RaiseAndSetIfChanged(ref _textFile, value); 
        }

    }
}
