using IdentityServer.Core.Shared;
using IdentityServer.Domain;
using IdentityServer.Persistence;
using System;
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

        public void DeleteEmployee(int idEmployee)
        {
            var employee = PersistenceContext.EmployeeRepository.GetEmployeeById(idEmployee);
            if (employee != null)
            {
                if (employee.Position.AccessLevel > 3)
                {
                    employee.Active = false;
                    employee.Department = null;
                    employee.Team = null;
                    employee.Position = PersistenceContext.EmployeeRepository.GetDeveloperPosition();
                    PersistenceContext.Complete();
                }
            }
        }

        public Employee GetEmployeeByName(string employeeName)
        {
            return PersistenceContext.EmployeeRepository.GetEmployeeByName(employeeName);
        }

        public Department GetDepartment(int idDepartment)
        {
            return PersistenceContext.EmployeeRepository.GetDepartmentById(idDepartment);
        }

        public Team GetTeam(int idTeam)
        {
            return PersistenceContext.EmployeeRepository.GetTeamById(idTeam);
        }

        public void UpdateDepartment(Department department)
        {
            var newDepartment = PersistenceContext.EmployeeRepository.GetDepartmentById(department.Id);
            newDepartment.Name = department.Name;
            newDepartment.Description = department.Description;
            PersistenceContext.Complete();
        }

        public void UpdateTeam(Team team)
        {
            var newTeam = PersistenceContext.EmployeeRepository.GetTeamById(team.Id);
            newTeam.Name = team.Name;
            newTeam.Description = team.Description;
            PersistenceContext.Complete();
        }

        public void DeleteDepartment(int idDepartment)
        {
            var department = PersistenceContext.EmployeeRepository.GetDepartmentById(idDepartment);
            if (department != null)
            {
                var departmentManager = GetDepartmentManager(department);
                departmentManager.Position = PersistenceContext.EmployeeRepository.GetDeveloperPosition();
                foreach(var team in department.Teams)
                {
                    team.Department = null;
                }

                foreach (var employee in department.Employees)
                {
                    employee.Department = null;
                }

                PersistenceContext.EmployeeRepository.DeleteDepartment(department);
                PersistenceContext.Complete();
            }
        }

        public void DeleteTeam(int idTeam)
        {
            var team = PersistenceContext.EmployeeRepository.GetTeamById(idTeam);
            if (team != null)
            {
                var teamManager = GetTeamLeader(team);
                teamManager.Position = PersistenceContext.EmployeeRepository.GetDeveloperPosition();
                foreach (var employee in team.Employees)
                {
                    employee.Team = null;
                }
              
                PersistenceContext.EmployeeRepository.DeleteTeam(team);
                PersistenceContext.Complete();
            }
        }

        public void AddDepartment(Department department)
        {
            PersistenceContext.EmployeeRepository.AddDepartment(department);
            PersistenceContext.Complete();
        }

        public void AddTeam(Team team)
        {
            PersistenceContext.EmployeeRepository.AddTeam(team);
            PersistenceContext.Complete();
        }

        public IEnumerable<Employee> GetAllUnassignedEmployees()
        {
            return PersistenceContext.EmployeeRepository.GetAllUnassignedEmployees();
        }

        public void UpdateTeamLeader(Team team, Employee employee)
        {
            var exTeamLeader = GetTeamLeader(team);

            if (team.Employees.Contains(employee))
            {
                if (exTeamLeader != null)
                {
                    exTeamLeader.Position = PersistenceContext.EmployeeRepository.GetDeveloperPosition();
                }
                employee.Position = PersistenceContext.EmployeeRepository.GetTeamLeaderPosition();
            }
            else
            {
                if (exTeamLeader != null)
                {
                    exTeamLeader.Position = PersistenceContext.EmployeeRepository.GetDeveloperPosition();
                }
                team.Employees.Add(employee);
                employee.Team = team;
                employee.Department = team.Department;
                employee.Position = PersistenceContext.EmployeeRepository.GetTeamLeaderPosition();
            }
            PersistenceContext.Complete();

        }

        public Employee GetTeamLeader(Team team)
        {
            var teamLeadPosition = PersistenceContext.EmployeeRepository.GetTeamLeaderPosition();

            foreach (var employee in team.Employees)
            {
                if (employee.Position == teamLeadPosition)
                    return employee;
            }
            return null;
        }

        public Employee GetDepartmentManager(Department department)
        {
            foreach (var employee in department.Employees)
            {
                if (employee.Position == PersistenceContext.EmployeeRepository.GetDepartmentManagerPosition())
                    return employee;
            }
            return null;
        }

        public IEnumerable<Position> GetRegisterPositionsByAccessLevel(string username)
        {
            var employee = PersistenceContext.EmployeeRepository.GetEmployeeByName(username);
            var accessLevel = employee.Position.AccessLevel;

            var allPositions = GetAllPositions().ToList();
            foreach(var position in allPositions)
            {
                if (position.AccessLevel > accessLevel)
                    allPositions.Remove(position);
            }
            return allPositions;
        }

        public Position GetDepartmentManagerPosition()
        {
            return PersistenceContext.EmployeeRepository.GetDepartmentManagerPosition();
        }

        public Position GetTeamLeaderPosition()
        {
            return PersistenceContext.EmployeeRepository.GetTeamLeaderPosition();
        }

        public void UpdateDepartmentManager(Department department, Employee employee)
        {
            var exDepartmentManager = GetDepartmentManager(department);

            if (department.Employees.Contains(employee))
            {
                if (exDepartmentManager != null)
                {
                    exDepartmentManager.Position = PersistenceContext.EmployeeRepository.GetDeveloperPosition();
                }
                employee.Position = PersistenceContext.EmployeeRepository.GetDepartmentManagerPosition();
            }
            else
            {
                if (exDepartmentManager != null)
                {
                    exDepartmentManager.Position = PersistenceContext.EmployeeRepository.GetDeveloperPosition();
                }
                department.Employees.Add(employee);
                employee.Department = department;
                employee.Position = PersistenceContext.EmployeeRepository.GetDepartmentManagerPosition();
            }
            PersistenceContext.Complete();
        }

        public void UpdateCV(Employee employee, string filePath)
        {
            employee.CV = filePath;
            PersistenceContext.Complete();
        }

        public void UpdateImage(Employee employee, string filePath)
        {
            employee.Picture = filePath;
            PersistenceContext.Complete();
        }
    }
}
