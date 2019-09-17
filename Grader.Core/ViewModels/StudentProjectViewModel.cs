using Grader;

namespace AsyncToolWindowSample.ToolWindows
{
    public class StudentProjectViewModel : BindableBase
    {
        public StudentProjectSummaryDto Project { get; }

        public int Id => Project.Id;
        public string Name => Project.Name;

        public string Date { get; set; }
        public string CommandText { get; set; }
        public DelegateCommand NavigateToProjectViewCommand { get; set; }
        public StudentProjectViewModel(StudentProjectSummaryDto studentProjectDto)
        {
            Project = studentProjectDto;
        }
    }
}