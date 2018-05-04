using StudentsManagement.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace StudentsManagement.Core.Shared
{
    public interface ITeacherServices
    {
        IEnumerable<Activity> GetTeacherActivities(string username);
        IEnumerable<ActivityDate> GetActivityDates(int idActivity, int studentId);
        IEnumerable<ActivityDate> GetActivityDates(int idActivity);
        IEnumerable<Student> GetActivityStudents(int idActivity);
        Activity GetActivity(int idActivity);
        Student GetStudent(int idStudent);
        ActivityDate GetActivityDate(int idActivityDate);
        ActivityDate GetActivityDate(int idActivityDate, int idStudent);
        void AddActivityDate(ActivityDate newActivityDate);
        void UpdateActivityDate(ActivityDate newActivityDate );
        void AddTeacher(ApplicationUser user);
        IEnumerable<Student> GetAllStudents();
        void AddStudentToActivity(string student, int id);
        void AddStudentsToActivity(List<string> studentNames, int actId);

    }
}
