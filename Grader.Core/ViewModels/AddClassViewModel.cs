using Grader;

namespace AsyncToolWindowSample.ToolWindows
{
    public class AddClassViewModel : BindableViewModel
    {
        private readonly IGradeBookRepository _repository;
        private readonly INavigationService _navigationService;
        private Class _class;

        public AddClassViewModel(IGradeBookRepository repository, INavigationService navigationService)
        {
            _repository = repository;
            _navigationService = navigationService;
            Class = new Class();
            AddCommand = new DelegateCommand(Add);
        }

        private async void Add()
        {
           await _repository.AddClass(Class);
           await _navigationService.Back();

        }

        public DelegateCommand AddCommand { get; set; }
        public Class Class
        {
            get => _class;
            set => SetProperty(ref _class,value);
        }
    }
}