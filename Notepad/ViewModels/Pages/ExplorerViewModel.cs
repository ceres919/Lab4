using Avalonia.Input;
using Avalonia.Logging;
using Microsoft.VisualBasic;
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
        
        public string? currentText = "";
        public string currentPathName = "";
        public bool savePage;
        public string FilePath { get; set; }
        public string? CurrentText { get; private set; }

        public FileAndDirectoryEntityViewModel? selectedEntity;
        public FileAndDirectoryEntityViewModel? SelectedEntity
        {
            get => selectedEntity;
            set
            {
                SelectItem(value);
                this.RaiseAndSetIfChanged(ref selectedEntity, value);
            }
        }

        //public ICommand OpenCommand { get; }

        public ExplorerViewModel()
        {
            //OpenCommand = new DelegateCommand(Open);
            savePage = false;
            FilePath = Directory.GetCurrentDirectory();
            OpenDirectory();
            CancelCommand = ReactiveCommand.Create(() => { });
            OkCommand = ReactiveCommand.Create<FileAndDirectoryEntityViewModel>(Open);

        }

        public ExplorerViewModel(string? text, bool mode)
        {
            CurrentText = text;
            savePage = mode;
            FilePath = Directory.GetCurrentDirectory();
            OpenDirectory();
            CancelCommand = ReactiveCommand.Create(() => { });
            OkCommand = ReactiveCommand.Create<FileAndDirectoryEntityViewModel>(Open);

        }

        private void OpenDirectory()
        {
            DirectoriesAndFilesCollection.Clear();
            var directoryInfo = new DirectoryInfo(FilePath);
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
                if (directoryViewModel.FullName is "..")
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
            else 
            { 
                if (parameter is FileViewModel fileViewModel)
                {
                    if (savePage) 
                    {
                        File.WriteAllText(fileViewModel.FullName, CurrentText);
                        savePage = false;
                    }
                    else
                    {
                        CurrentText = File.ReadAllText(fileViewModel.FullName);
                    }
                }
                else 
                {
                    string path = Path.Combine(FilePath, fileName);
                    if (File.Exists(path)) 
                    {
                        if (savePage)
                        {
                            File.WriteAllText(path, CurrentText);
                            savePage = false;
                        }
                        else
                        {
                            CurrentText = File.ReadAllText(path);
                        }
                    }
                    else
                    {
                        if (savePage)
                        {
                            File.WriteAllText(path, CurrentText);
                            savePage = false;
                        }
                    }
                }
            }
        }
        private void SelectItem(FileAndDirectoryEntityViewModel item)
        {
            if (item == null)
            {
                OkButtonContent = "Открыть";
                return;
            }
            if (item is FileViewModel)
            {
                FileName = item.Name;
                if(savePage)
                {
                    OkButtonContent = "Сохранить";
                }
            }
            else
            {
                FileName = "";
                OkButtonContent = "Открыть";
            }
        }

        private string fileName = "";
        private string okButtonContent = "Открыть";
        FileAndDirectoryEntityViewModel? selectedItem;

        public string FileName { get => fileName; set => this.RaiseAndSetIfChanged(ref fileName, value); }
        public string OkButtonContent { get => okButtonContent; set => this.RaiseAndSetIfChanged(ref okButtonContent, value); }
        public FileAndDirectoryEntityViewModel? SelectedItem { get => selectedItem; set { selectedItem = value; SelectItem(value); } }
        public ObservableCollection<FileAndDirectoryEntityViewModel> DirectoriesAndFilesCollection
        {
            get => directoriesAndFilesCollection;
            set => this.RaiseAndSetIfChanged(ref directoriesAndFilesCollection, value);
        }
        public ReactiveCommand<FileAndDirectoryEntityViewModel, Unit> OkCommand { get; }

        public ReactiveCommand<Unit, Unit> CancelCommand { get; }
    }
}
