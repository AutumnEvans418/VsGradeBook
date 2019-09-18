using System.Collections.ObjectModel;
using Grader;

namespace AsyncToolWindowSample.ToolWindows
{
    public class ClassesViewModel : BindableViewModel
    {
        private ObservableCollection<Class> _classes;
        private Class _selectedClass;

        public Class SelectedClass
        {
            get => _selectedClass;
            set => SetProperty(ref _selectedClass,value);
        }

        public ObservableCollection<Class> Classes
        {
            get => _classes;
            set => SetProperty(ref _classes,value);
        }

        public ClassesViewModel()
        {
            Classes = new ObservableCollection<Class>();
        }
    }
}