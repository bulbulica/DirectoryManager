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

        public Department GetDepartmentById(int idDepartment)
        {
            return EmployeeDbContext.Departments.Find(idDepartment);
        }

        public Team GetTeamById(int idTeam)
        {
            return EmployeeDbContext.Teams.Find(idTeam);
        }

        public void DeleteDepartment(Department department)
        {
            EmployeeDbContext.Departments.Remove(department);
        }

        public void DeleteTeam(Team team)
        {
            EmployeeDbContext.Teams.Remove(team);
        }

        public IEnumerable<Employee> GetAllUnassignedEmployees()
        {
            var employees = EmployeeDbContext.Employees.ToList();

            List<Employee> unassignedEmployees = new List<Employee>();

            foreach(var employee in employees)
            {
                if (employee.Team == null)
                    unassignedEmployees.Add(employee);
            }
            return unassignedEmployees;
        }

        public Position GetDeveloperPosition()
        {
            return EmployeeDbContext.Positions.SingleOrDefault(testp => testp.RoleName.Equals("Developer"));
        }

        public Position GetTeamLeaderPosition()
        {
            return EmployeeDbContext.Positions.SingleOrDefault(testp => testp.RoleName.Equals("TeamLead"));
        }

        public Position GetDepartmentManagerPosition()
        {
            return EmployeeDbContext.Positions.SingleOrDefault(testp => testp.RoleName.Equals("DepManager"));
        }

        public EmployeeDbContext EmployeeDbContext
        {
            get
            {
                return Context as EmployeeDbContext;
            }
        }
    }
}
