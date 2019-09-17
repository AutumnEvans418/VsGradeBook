using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Grader
{

    public class ProjectSubmissionDto
    {
        public CodeProject CodeProject { get; set; }
        public Submission Submission { get; set; }
    }
    public class StudentProjectsDto
    {
        public StudentProjectsDto(IEnumerable<StudentProjectSummaryDto> projects)
        {
            ToDo = new ObservableCollection<StudentProjectSummaryDto>(projects.Where(p=>p.Status == StudentProjectStatus.Todo));
            InProgress = new ObservableCollection<StudentProjectSummaryDto>(projects.Where(p=>p.Status == StudentProjectStatus.InProgress));
            Submitted = new ObservableCollection<StudentProjectSummaryDto>(projects.Where(p=>p.Status == StudentProjectStatus.Submitted));
        }
        public ObservableCollection<StudentProjectSummaryDto> ToDo { get; set; }
        public ObservableCollection<StudentProjectSummaryDto> InProgress { get; set; }
        public ObservableCollection<StudentProjectSummaryDto> Submitted { get; set; }
    }
}