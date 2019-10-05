using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Grader
{
    public interface IGradeBookRepository
    {
        Task<int> GetPersonId(string userName);
        Task<Person> AddPerson(Person person);
        Task<Person> UpdatePerson(Person person);
        Task DeletePerson(int person);

        //Task<RepositoryResult<IEnumerable<StudentProjectSummaryDto>>> StudentLogin(string userName, string classCode);
       // Task<RepositoryResult<IEnumerable<CodeProject>>> TeacherLogin(string userName, string classId);
        Task<IEnumerable<Class>> GetClasses(int personId);
        Task<Class> UpdateClass(Class cClass);
        Task<Class> AddClass(Class cClass);
        Task DeleteClass(string id);
        Task<IEnumerable<Enrollment>> GetEnrollments();
        Task<Enrollment> GetEnrollment(int enrollmentId);
        Task<Enrollment> AddEnrollment(Enrollment enrollment);
        Task<Enrollment> UpdateEnrollment(Enrollment enroll);
        Task DeleteEnrollment(int enrollmentId);
        Task<IEnumerable<CodeProject>> GetCodeProjects(Guid studentCode);
    }
}