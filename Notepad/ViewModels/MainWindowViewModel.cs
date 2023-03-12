using DynamicData;
using Notepad.Models.Commands;
using Notepad.Models.ExplorerEntities;
using ReactiveUI;
using System.Collections.ObjectModel;
using System.IO;
using System.Reactive.Linq;
using System.Reactive;
using System.Windows.Input;
using Notepad.ViewModels.Pages;
using Notepad.Models;
using System.Reactive.Concurrency;
using System;

namespace Notepad.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private NoteViewModel notepadViewModel;
        private ViewModelBase content;


        public MainWindowViewModel()
        {
            Content = notepadViewModel = new NoteViewModel();

            OpenCommand = ReactiveCommand.Create(() =>
            {
                ExplorerViewModel viewModel = new ExplorerViewModel();
                Observable.Merge(
                    viewModel.OkCommand,
                    viewModel.CancelCommand)
                .Take(1)
                .Subscribe(todoItem =>
                {
                    if (viewModel.CurrentText != null)
                    {
                        notepadViewModel.TextFile = viewModel.CurrentText;
                    }

                    Content = notepadViewModel;
                });

                Content = viewModel;
            });

            SaveCommand = ReactiveCommand.Create(() =>
            {
                ExplorerViewModel viewModel = new ExplorerViewModel();
                Observable.Merge(
                    viewModel.OkCommand,
                    viewModel.CancelCommand)
                .Subscribe(todoItem =>
                {
                    if (viewModel.CurrentText != null)
                    {
                        notepadViewModel.TextFile = viewModel.CurrentText;
                        Content = notepadViewModel;
                    }

                    
                });

                Content = viewModel;
            });
        }

        public ReactiveCommand<Unit, Unit> OpenCommand { get; }
        public ReactiveCommand<Unit, Unit> SaveCommand { get; }


        public ViewModelBase Content
        {
            get => content;
            set
            {
                this.RaiseAndSetIfChanged(ref content, value);
            }
        }

        //private string currentPathName = Directory.GetCurrentDirectory();

        //private ObservableCollection<FileAndDirectoryEntityViewModel> directoriesAndFilesCollection = new ObservableCollection<FileAndDirectoryEntityViewModel>();
        //private FileAndDirectoryEntityViewModel selectedEntity;

        //public string FilePath { get; set; }
        //public FileAndDirectoryEntityViewModel SelectedEntity 
        //{ 
        //    get => selectedEntity;
        //    set => this.RaiseAndSetIfChanged(ref selectedEntity, value);
        //}
        //public ICommand OpenCommand { get; }

        //public MainWindowViewModel() 
        //{
        //    OpenCommand = new DelegateCommand(Open);

        //    foreach (var d in Directory.GetLogicalDrives())//GetFiles(currentPathName)
        //    {
        //        DirectoriesAndFilesCollection.Add(new DirectoryViewModel(d));
        //    }
        //}

        //public ObservableCollection<FileAndDirectoryEntityViewModel> DirectoriesAndFilesCollection 
        //{ 
        //    get => directoriesAndFilesCollection;
        //    set => this.RaiseAndSetIfChanged(ref directoriesAndFilesCollection, value);
        //}
        //private void Open(object parameter)
        //{

        //    if (parameter is DirectoryViewModel directoryViewModel) 
        //    {
        //        //if (directoryViewModel.FullName == "..")
        //        FilePath = directoryViewModel.FullName;
        //        DirectoriesAndFilesCollection.Clear();
        //        var directoryInfo = new DirectoryInfo(FilePath);
        //        if (Directory.GetParent(FilePath) != null )
        //            DirectoriesAndFilesCollection.Add(new DirectoryViewModel("..", "Assets/back_dir.png"));
        //        foreach (var dir in directoryInfo.GetDirectories())
        //        {
        //            DirectoriesAndFilesCollection.Add(new DirectoryViewModel(dir));
        //        }
        //        foreach (var file in directoryInfo.GetFiles())
        //        {
        //            DirectoriesAndFilesCollection.Add(new FileViewModel(file));
        //        }
        //    }
        //}
    }
}