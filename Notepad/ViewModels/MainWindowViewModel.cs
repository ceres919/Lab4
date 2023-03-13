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
                Content = viewModel;

                Observable.AsObservable(
                    viewModel.OkCommand)
                .Subscribe(todoItem =>
                {
                    if (viewModel.CurrentText != null)
                    {
                        notepadViewModel.TextFile = viewModel.CurrentText;
                        Content = notepadViewModel;
                    }
                });
                Observable.AsObservable(viewModel.CancelCommand)
                .Subscribe(todoItem =>
                {
                    Content = notepadViewModel;
                });
            });

            SaveCommand = ReactiveCommand.Create(() =>
            {
                ExplorerViewModel viewModel = new ExplorerViewModel(notepadViewModel.TextFile, true);
                Content = viewModel;

                Observable.AsObservable(viewModel.OkCommand)
                .Subscribe(todoItem =>
                {
                    if (viewModel.savePage == false)
                    {
                        Content = notepadViewModel;
                    }
                });
                Observable.AsObservable(viewModel.CancelCommand)
                .Subscribe(todoItem =>
                {
                    Content = notepadViewModel;
                });
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
    }
}