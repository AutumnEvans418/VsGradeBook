using System;

namespace AsyncToolWindowSample.ToolWindows
{
    public class AddDocumentationViewModel : BindableBase
    {
        private readonly Action _exit;
        private readonly string _path;
        private readonly TextViewSelection _selection;
        private string _code;
        private string _documentation;

        public string Code
        {
            get => _code;
            set => SetProperty(ref _code,value);
        }

        public string Documentation
        {
            get => _documentation;
            set => SetProperty(ref _documentation,value);
        }

        public DelegateCommand SaveCommand { get; }
        public DelegateCommand CancelCommand { get; }
        

        public AddDocumentationViewModel(Action exit, string path, TextViewSelection selection)
        {
            _exit = exit;
            _path = path;
            _selection = selection;
            SaveCommand = new DelegateCommand(Save);
            CancelCommand = new DelegateCommand(Cancel);
            Code = selection.Text;
        }

        private void Cancel()
        {
            _exit();
        }

        private void Save()
        {
            
        }
    }
}