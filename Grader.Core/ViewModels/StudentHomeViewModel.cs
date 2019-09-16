using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Grader;

namespace AsyncToolWindowSample.ToolWindows
{

    public class NavigationParameter : Dictionary<string,object>, INavigationParameter
    {
    }
    public interface INavigationAware
    {
        void Initialize(INavigationParameter parameter);
    }
    public interface INavigationAwareAsync
    {
        Task InitializeAsync(INavigationParameter parameter);
    }
    public interface INavigationParameter : IDictionary<string,object>
    {
    }

    public class BindableViewModel : BindableBase, INavigationAware, INavigationAwareAsync
    {
        public virtual void Initialize(INavigationParameter parameter)
        {
        }

        public virtual async Task InitializeAsync(INavigationParameter parameter)
        {
        }
    }
    public class StudentHomeViewModel : BindableViewModel
    {
        private readonly IGradeBookRepository _repository;
        private ObservableCollection<StudentProjectViewModel> _toDoList;
        private ObservableCollection<StudentProjectViewModel> _inProgressList;
        private ObservableCollection<StudentProjectViewModel> _submittedList;

        public StudentHomeViewModel(IGradeBookRepository repository)
        {
            _repository = repository;
            ToDoList = new ObservableCollection<StudentProjectViewModel>();
            InProgressList = new ObservableCollection<StudentProjectViewModel>();
            SubmittedList = new ObservableCollection<StudentProjectViewModel>();
        }

        public override void Initialize(INavigationParameter parameter)
        {
            if (parameter["Projects"] is IEnumerable<StudentProjectDto> projects)
            {
                ToDoList.Clear();
                InProgressList.Clear();
                SubmittedList.Clear();
                foreach (var proj in projects)
                {
                    if (proj.Status == StudentProjectStatus.Todo)
                    {
                        ToDoList.Add(new StudentProjectViewModel(proj)
                        {
                            CommandText = "Start",
                            Date = $"Due: {proj.DueDate:D}"
                        } );
                    }
                    if (proj.Status == StudentProjectStatus.InProgress)
                    {
                        InProgressList.Add(new StudentProjectViewModel(proj)
                        {
                            CommandText = "Continue",
                            Date = $"Due: {proj.DueDate:D}"
                        });
                    }
                    if (proj.Status == StudentProjectStatus.Submitted)
                    {
                        SubmittedList.Add(new StudentProjectViewModel(proj)
                        {
                            CommandText = proj.IsBeingGraded ? "Grading..." : "View",
                            Date = proj.DateGraded != null ? $"Graded: {proj.DateGraded:D}" : $"Submitted: {proj.DateSubmitted:D}"
                        });
                    }
                }
            }
            base.Initialize(parameter);
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