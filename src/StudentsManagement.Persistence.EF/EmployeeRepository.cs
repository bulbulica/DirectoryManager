using IdentityServer.Domain;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace IdentityServer.Persistence.EF
{
    class EmployeeRepository : Repository<Employee>, IEmployeeRepository
    {
        public EmployeeRepository(DbContext context) : base(context)
        {
        }

        //public void Add(Employee entity)
        //{
        //    throw new System.NotImplementedException();
        //}

        //public void Delete(Employee entity)
        //{
        //    throw new System.NotImplementedException();
        //}

        //public UserEntity GetTeacherByName(string name)
        //{
        //    throw new System.NotImplementedException();
        //}

        //Employee IRepository<Employee>.GetEntity(int id)
        //{
        //    throw new System.NotImplementedException();
        //}

        //Employee IUserRepository.GetTeacherByName(string name)
        //{
        //    throw new System.NotImplementedException();
        //}

        //IEnumerable<Employee> IRepository<Employee>.ListAll()
        //{
        //    throw new System.NotImplementedException();
        //}


        //public StudentsManagementDbContext StudentsManagementDbContext
        //{
        //    get
        //    {
        //        return Context as StudentsManagementDbContext;
        //    }
        //}

        //public Teacher GetTeacherByName(string name)
        //{
        //    Teacher retVal = null;

        //    if (StudentsManagementDbContext != null)
        //    {
        //        retVal = StudentsManagementDbContext.Teachers.SingleOrDefault(t => t.Name.Equals(name));
        //    }

        //    return retVal;
        //}
        public Employee GetEmployeeByName(string name)
        {
                return EmployeeDbContext.Employees.SingleOrDefault(testc => testc.Name.Equals(name));
        }

        public IEnumerable<Employee> GetAllEmployees()
        {
            return EmployeeDbContext.Employees;
        }

        public Employee GetEmployeeById(int idEmployee)
        {
            return EmployeeDbContext.Employees.Find(idEmployee);
        }

        public IEnumerable<Team> GetAllTeams()
        {
            return EmployeeDbContext.Teams.ToList();
        }

        public void AddPositions(List<Position> list)
        {
            foreach (var position in list)
                EmployeeDbContext.Positions.Add(position);
        }

        public void AddTeam(Team team2)
        {
            EmployeeDbContext.Teams.Add(team2);
        }

        public void AddDepartment(Department dep)
        {
            EmployeeDbContext.Departments.Add(dep);
        }

        public IEnumerable<Position> GetAllPositions()
        {
            return EmployeeDbContext.Positions.ToList();
        }

        public Position GetPositionByName(string name)
        {
            return EmployeeDbContext.Positions.SingleOrDefault(testc => testc.RoleName.Equals(name));
        }

        public Department GetDepartmentByName(string department)
        {
            return EmployeeDbContext.Departments.SingleOrDefault(testc => testc.Name.Equals(department));
        }

        public IEnumerable<Department> GetAllDepartments()
        {
            return EmployeeDbContext.Departments.ToList();
        }

        //E posibil sa nu le folosim

        //public Team GetTeam(int idEmployee)
        //{
        //    return EmployeeDbContext.Employees.Find(idEmployee).Team;
        //}

        //public Department GetDepartment(int idEmployee)
        //{
        //    return EmployeeDbContext.Employees.Find(idEmployee).Department;
        //}

        //public Position GetPosition (int idEmployee)
        //{
        //    return EmployeeDbContext.Employees.Find(idEmployee).Position;
        //}



        public EmployeeDbContext EmployeeDbContext
        {
            get
            {
                return Context as EmployeeDbContext;
            }
        }
    }
}
