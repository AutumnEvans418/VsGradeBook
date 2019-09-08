using System.Collections.ObjectModel;
using Grader;

namespace AsyncToolWindowSample.ToolWindows
{
    public class StudentHomeViewModel : BindableBase
    {
        private readonly IGradeBookRepository _repository;
        private ObservableCollection<StudentProjectViewModel> _toDoList;
        private ObservableCollection<StudentProjectViewModel> _inProgressList;
        private ObservableCollection<StudentProjectViewModel> _submittedList;

        public StudentHomeViewModel(IGradeBookRepository repository)
        {
            _repository = repository;
        }

        public void Initialize()
        {
            
        }

        public ObservableCollection<StudentProjectViewModel> ToDoList
        {
            get => _toDoList;
            set => SetProperty(ref _toDoList,value);
        }

        public ObservableCollection<StudentProjectViewModel> InProgressList
        {
            get => _inProgressList;
            set => SetProperty(ref _inProgressList,value);
        }

        public ObservableCollection<StudentProjectViewModel> SubmittedList
        {
            get => _submittedList;
            set => SetProperty(ref _submittedList,value);
        }
    }
}