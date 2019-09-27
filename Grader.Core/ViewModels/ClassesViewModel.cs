using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Grader;

namespace AsyncToolWindowSample.ToolWindows
{
    public class ClassesViewModel : BindableViewModel
    {
        private readonly INavigationService _navigationService;
        private readonly IGradeBookRepository _repository;
        private ObservableCollection<Class> _classes;
        private Class _selectedClass;

        public Class SelectedClass
        {
            get => _selectedClass;
            set => SetProperty(ref _selectedClass, value);
        }

        public ObservableCollection<Class> Classes
        {
            get => _classes;
            set => SetProperty(ref _classes, value);
        }

        public DelegateCommand AddCommand { get; set; }
        public DelegateCommand OpenCommand { get; set; }
        public DelegateCommand DeleteCommand { get; set; }
        public DelegateCommand CreateCommand { get; set; }
        public ClassesViewModel(INavigationService navigationService, IGradeBookRepository repository)
        {
            _navigationService = navigationService;
            _repository = repository;
            Classes = new ObservableCollection<Class>();
            OpenCommand = new DelegateCommand(Open);
            AddCommand = new DelegateCommand(Add);
        }

        private async void Add()
        {
           await _navigationService.ToModalPage("AddClassView", new NavigationParameter());
           await Refresh(personId);
        }

        private int personId;
        public override async Task InitializeAsync(INavigationParameter parameter)
        {
            if (parameter["PersonId"] is int id)
            {
                personId = id;
                await Refresh(id);
            }
        }

        private async Task Refresh(int id)
        {
            var classes = await _repository.GetClasses(id);
            Classes = new ObservableCollection<Class>(classes);
        }

        private async void Open()
        {
            await _navigationService.ToPage("ProjectsView", new NavigationParameter() { { "ClassId", SelectedClass.Id } });
        }
    }
}