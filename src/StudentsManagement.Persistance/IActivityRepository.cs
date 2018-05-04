using StudentsManagement.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StudentsManagement.Persistence
{
    public interface IActivityRepository : IRepository<Activity>
    {
        Activity GetActivityByName(string name);
        int GetProfessorId(int id);
        string GetProfessorName(int id);
        int GetActivityTypeId(int id);
        string GetActivityTypeName(int id);
        IEnumerable<Activity> GetUserActivities(string username);
        IEnumerable<Activity> GetTeacherActivities(string username);
        IEnumerable<ActivityDate> GetActivityDates(int activityId, string username);
        IEnumerable<ActivityDate> GetActivityDates(int activityId);
        IEnumerable<ActivityDate> GetActivityDates(int activityId, int studentId);
        void AddActivityDate(ActivityDate activityDate);
        ActivityDate GetActivityDate(int Id);
        ActivityDate GetActivityDate(int Id, int studentId);
        IEnumerable<Student> GetStudentsFromActivity(int idActivity);
        void AddActivityDetails(Student student, int id);
    }
}
