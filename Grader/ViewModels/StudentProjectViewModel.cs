using Grader;

namespace AsyncToolWindowSample.ToolWindows
{
    public class StudentProjectViewModel : BindableBase
    {
        public StudentProjectDto Project { get; }
        public DelegateCommand NavigateToProjectViewCommand { get; set; }
        public StudentProjectViewModel(StudentProjectDto studentProjectDto)
        {
            Project = studentProjectDto;
        }
    }
}