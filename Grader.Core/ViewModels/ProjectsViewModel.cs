using System.Collections.ObjectModel;
using Grader;

namespace AsyncToolWindowSample.ToolWindows
{
    public class ProjectsViewModel : BindableViewModel
    {
        private CodeProject _selectedProject;
        private ObservableCollection<CodeProject> _projects;

        public CodeProject SelectedProject
        {
            get => _selectedProject;
            set => SetProperty(ref _selectedProject,value);
        }

        public ObservableCollection<CodeProject> Projects
        {
            get => _projects;
            set => SetProperty(ref _projects,value);
        }

        public ProjectsViewModel()
        {
            Projects = new ObservableCollection<CodeProject>();
        }
    }
}