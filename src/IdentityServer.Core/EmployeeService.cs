using IdentityServer.Core.Shared;
using IdentityServer.Domain;
using IdentityServer.Persistence;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityServer.Core
{
    class EmployeeService : IEmployeeService
    {
        private IPersistenceContext _persistenceContext;


        public EmployeeService(IPersistenceContext persistenceContext)
        {
            PersistenceContext = persistenceContext;
        }

        public IPersistenceContext PersistenceContext { get => _persistenceContext; set => _persistenceContext = value; }

        //public void AddActivityDate(ActivityDate newActivityDate)
        //{
        //    PersistenceContext.ActivityRepository.AddActivityDate(newActivityDate);
        //    PersistenceContext.Complete();
        //}

        //public void AddStudentToActivity(string student, int id)
        //{
        //    var studentLoaded = PersistenceContext.StudentsRepository.GetStudentByName(student);
        //    PersistenceContext.ActivityRepository.AddActivityDetails(studentLoaded, id);
        //    PersistenceContext.Complete();
        //}

        //public void AddStudentsToActivity(List <string> students, int id)
        //{
        //    foreach (var student in students)
        //    {
        //        var studentLoaded = PersistenceContext.StudentsRepository.GetStudentByName(student);
        //        PersistenceContext.ActivityRepository.AddActivityDetails(studentLoaded, id);
        //    }
        //    PersistenceContext.Complete();

        //}

        public void AddEmployee(ApplicationUser user)
        {
            if (user != null)
            {
                var employee = new Employee
                {
                    Name = user.Email,
                    Username = user.Email
                };

                PersistenceContext.EmployeeRepository.Add(employee);
                PersistenceContext.Complete();
            }
        }

        public void AddEmployee(Employee employee)
        {
            PersistenceContext.EmployeeRepository.Add(employee);
            PersistenceContext.Complete();
        }

        public List<Employee> GetAllEmployees()
        {
            if(PersistenceContext.EmployeeRepository.ListAll().Count()!=0)
                return PersistenceContext.EmployeeRepository.ListAll().ToList();
            return new List<Employee>();
        }

        public IEnumerable<Position> GetAllPositions()
        {
            if (PersistenceContext.EmployeeRepository.GetAllPositions().Count() != 0)
                return PersistenceContext.EmployeeRepository.GetAllPositions().ToList();
            return new List<Position>();

        }

        public Employee GetEmployee(int idEmployee)
        {
            return PersistenceContext.EmployeeRepository.GetEmployeeById(idEmployee);
        }


        public string GetEmployeeRoleToString(int idEmployee)
        {
            return PersistenceContext.EmployeeRepository.GetEmployeeById(idEmployee).Position.RoleName;
        }

        public Position GetPositionByName(string position)
        {
            return PersistenceContext.EmployeeRepository.GetPositionByName(position);
        }

        public Department GetDepartmentByName(string department)
        {
            return PersistenceContext.EmployeeRepository.GetDepartmentByName(department);
        }

        public IEnumerable<Department> GetAllDepartments()
        {
            if (PersistenceContext.EmployeeRepository.GetAllPositions().Count() != 0)
                return PersistenceContext.EmployeeRepository.GetAllDepartments().ToList();
            return new List<Department>();
        }

        public IEnumerable<Team> GetAllTeams()
        {
            if (PersistenceContext.EmployeeRepository.GetAllTeams().Count() != 0)
                return PersistenceContext.EmployeeRepository.GetAllTeams().ToList();
            return new List<Team>();
        }

        public void UpdateEmployee(Employee employee)
        {
            var newEmployee = PersistenceContext.EmployeeRepository.GetEmployeeById(employee.Id);

            newEmployee.Name = employee.Name;
            newEmployee.Picture = employee.Picture;
            newEmployee.Position = employee.Position;
            newEmployee.Team = employee.Team;
            newEmployee.Department = employee.Department;
            newEmployee.CV = employee.CV;
            newEmployee.Active = employee.Active;
            PersistenceContext.Complete();
        }

        //public Activity GetActivity(int idActivity)
        //{
        //    return PersistenceContext.ActivityRepository.GetEntity(idActivity);
        //}

        //public ActivityDate GetActivityDate(int idActivityDate)
        //{
        //    return PersistenceContext.ActivityRepository.GetActivityDate(idActivityDate);
        //}

        //public ActivityDate GetActivityDate(int idActivityDate, int idStudent)
        //{
        //    return PersistenceContext.ActivityRepository.GetActivityDate(idActivityDate, idStudent);
        //}

        //public IEnumerable<ActivityDate> GetActivityDates(int idActivity, int studentId)
        //{
        //    return PersistenceContext.ActivityRepository.GetActivityDates(idActivity, studentId).ToList();
        //}

        //public IEnumerable<ActivityDate> GetActivityDates(int idActivity)
        //{
        //    return PersistenceContext.ActivityRepository.GetActivityDates(idActivity);
        //}

        //public IEnumerable<Student> GetActivityStudents(int idActivity)
        //{
        //    return PersistenceContext.ActivityRepository.GetStudentsFromActivity(idActivity);
        //}

        //public IEnumerable<Student> GetAllStudents()
        //{
        //    return PersistenceContext.StudentsRepository.ListAll();
        //}

        //public Student GetStudent(int idStudent)
        //{
        //    return PersistenceContext.StudentsRepository.GetEntity(idStudent);
        //}

        //public IEnumerable<Activity> GetTeacherActivities(string username)
        //{
        //    return PersistenceContext.ActivityRepository.GetTeacherActivities(username);
        //}

        //public void UpdateActivityDate(ActivityDate newActivityDate)
        //{
        //    ActivityDate toBeUpdatedRecord = PersistenceContext.ActivityRepository.GetActivityDate(newActivityDate.Id);

        //    if (toBeUpdatedRecord != null)
        //    {
        //        toBeUpdatedRecord.Grade = newActivityDate.Grade;
        //        toBeUpdatedRecord.Date = newActivityDate.Date;
        //        toBeUpdatedRecord.Attendance = newActivityDate.Attendance;
        //        PersistenceContext.Complete();
        //    }

        //}
    }
}
