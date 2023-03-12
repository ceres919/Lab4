using Avalonia.Controls.Shapes;
using Avalonia.Logging;
using Notepad.Models.Commands;
using Notepad.Models.ExplorerEntities;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reactive;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Notepad.ViewModels.Pages
{
    public class ExplorerViewModel : ViewModelBase
    {
        

        private ObservableCollection<FileAndDirectoryEntityViewModel> directoriesAndFilesCollection = new ObservableCollection<FileAndDirectoryEntityViewModel>();
        private FileAndDirectoryEntityViewModel selectedEntity;
        

        public string FilePath { get; set; }
        public string currentText = "";
        public string currentPathName = "";
        public string CurrentText { get; private set; }

        public FileAndDirectoryEntityViewModel SelectedEntity
        {
            get => selectedEntity;
            set => this.RaiseAndSetIfChanged(ref selectedEntity, value);
        }
       
        public ICommand OpenCommand { get; }

        public ExplorerViewModel()
        {
            OpenCommand = new DelegateCommand(Open);

            FilePath = Directory.GetCurrentDirectory();
            OpenDirectory();
           //foreach (var d in Directory.GetLogicalDrives())
           //{
           //    DirectoriesAndFilesCollection.Add(new DirectoryViewModel(d));
           //}
            CancelCommand = ReactiveCommand.Create(() => { });
            OkCommand = ReactiveCommand.Create<FileAndDirectoryEntityViewModel>(Open);

        }

        private void OpenDirectory()
        {
            DirectoriesAndFilesCollection.Clear();
            var directoryInfo = new DirectoryInfo(FilePath);
            if (Directory.GetParent(FilePath) != null)
                DirectoriesAndFilesCollection.Add(new DirectoryViewModel("..", "Assets/back_dir.png"));
            foreach (var dir in directoryInfo.GetDirectories())
            {
                DirectoriesAndFilesCollection.Add(new DirectoryViewModel(dir));
            }
            foreach (var file in directoryInfo.GetFiles())
            {
                DirectoriesAndFilesCollection.Add(new FileViewModel(file));
            }
        }

        private void OpenRoot()
        {
            DirectoriesAndFilesCollection.Clear();
            foreach (var d in Directory.GetLogicalDrives())
            {
                DirectoriesAndFilesCollection.Add(new DirectoryViewModel(d));
            }
        }

        private void Open(object parameter)
        {
            if (parameter is DirectoryViewModel directoryViewModel)
            {
                if(directoryViewModel.FullName is "..")
                {
                    var parentDir = Directory.GetParent(FilePath);
                    if (parentDir != null)
                    {
                        FilePath = parentDir.FullName;
                        OpenDirectory();
                        
                    }
                    else
                    {
                        OpenRoot();
                    }
                    
                }
                else 
                { 
                    FilePath = directoryViewModel.FullName;
                    OpenDirectory();
                }
            }
            if(parameter is FileViewModel fileViewModel) 
            {
               CurrentText = File.ReadAllText(fileViewModel.FullName);
            }

        }
        public ObservableCollection<FileAndDirectoryEntityViewModel> DirectoriesAndFilesCollection
        {
            get => directoriesAndFilesCollection;
            set => this.RaiseAndSetIfChanged(ref directoriesAndFilesCollection, value);
        }
        public ReactiveCommand<FileAndDirectoryEntityViewModel, Unit> OkCommand { get; }

        public ReactiveCommand<Unit, Unit> CancelCommand { get; }
    }
}
