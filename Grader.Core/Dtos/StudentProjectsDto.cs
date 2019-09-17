using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Grader
{
    public class StudentProjectsDto
    {
        public StudentProjectsDto(IEnumerable<StudentProjectDto> projects)
        {
            ToDo = new ObservableCollection<StudentProjectDto>(projects.Where(p=>p.Status == StudentProjectStatus.Todo));
            InProgress = new ObservableCollection<StudentProjectDto>(projects.Where(p=>p.Status == StudentProjectStatus.InProgress));
            Submitted = new ObservableCollection<StudentProjectDto>(projects.Where(p=>p.Status == StudentProjectStatus.Submitted));
        }
        public ObservableCollection<StudentProjectDto> ToDo { get; set; }
        public ObservableCollection<StudentProjectDto> InProgress { get; set; }
        public ObservableCollection<StudentProjectDto> Submitted { get; set; }
    }
}