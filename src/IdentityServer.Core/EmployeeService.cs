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

            if (employee.Team != null)
                newEmployee.Team = employee.Team;

            if (employee.Department != null)
                newEmployee.Department = employee.Department;
            newEmployee.CV = employee.CV;

            if (employee.Active != null)
                newEmployee.Active = employee.Active;

            PersistenceContext.Complete();
        }

        public void DeleteEmployee(int idEmployee)
        {
            var employee = PersistenceContext.EmployeeRepository.GetEmployeeById(idEmployee);
            if (employee != null)
            {
                if (employee.Position.AccessLevel > Constants.GeneralManagerAccessLevel)
                {
                    employee.Active = false;
                    employee.Department = null;
                    employee.Team = null;
                    employee.Position = PersistenceContext.EmployeeRepository.GetDeveloperPosition();
                    PersistenceContext.Complete();
                }
                else
                {
                    employee.Active = false;
                    employee.Position = PersistenceContext.EmployeeRepository.GetDeveloperPosition();
                    PersistenceContext.Complete();
                }
            }
        }

        void IEmployeeService.ActivateEmployee(int idEmployee)
        {
            var employee = PersistenceContext.EmployeeRepository.GetEmployeeById(idEmployee);
            if (employee != null)
            {
                employee.Active = true;
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
                if (departmentManager != null)
                {
                    departmentManager.Position = PersistenceContext.EmployeeRepository.GetDeveloperPosition();
                }

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
                var teamLeader = GetTeamLeader(team);

                if (teamLeader != null)
                {
                    teamLeader.Position = PersistenceContext.EmployeeRepository.GetDeveloperPosition();
                }
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
            List<Position> positionsToBeReturned = new List<Position>();
            foreach(var position in allPositions)
            {
                if (position.AccessLevel > accessLevel 
                    && position.AccessLevel != Constants.OfficeManagerAccessLevel)

                    positionsToBeReturned.Add(position);
            }
            return positionsToBeReturned;
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

        public List<Employee> GetAllEmployeesFromTeam(Team team)
        {
            return PersistenceContext.EmployeeRepository.GetAllEmployeesFromTeam(team);
        }

        public List<Employee> GetAllEmployeesFromDepartment(Department department)
        {
            return PersistenceContext.EmployeeRepository.GetAllEmployeesFromDepartment(department);
        }

        public void UpdateEmployeeName(Employee employee, string name)
        {
            employee.Name = name;
            PersistenceContext.Complete();
        }

        public Position GetDeveloperPosition()
        {
            return PersistenceContext.EmployeeRepository.GetDeveloperPosition();
        }

        public void UpdateEmployeePosition(Position position, Employee employee)
        {
            employee.Position = position;
            PersistenceContext.Complete();
        }

        public List<Team> GetAllTeamsFromDepartment(Department department)
        {
            return PersistenceContext.EmployeeRepository.GetAllTeamsFromDepartment(department);
        }

        public IEnumerable<Team> GetAllUnassignedTeams()
        {
            return PersistenceContext.EmployeeRepository.GetAllUnassignedTeams();
        }

        public void AddTeamToDepartment(Department department, Team team)
        {
            var exDepartment = team.Department;
            exDepartment.Teams.Remove(team);
            department.Teams.Add(team);
            team.Department = department;
            foreach (var employee in team.Employees)
            {
                employee.Department = department;
            }
            PersistenceContext.Complete();
        }

        public void AddEmployeeToTeam(Employee employee, Team team)
        {
            employee.Team = team;
            team.Employees.Add(employee);
            if(team.Department!= null)
            {
                employee.Department = team.Department;
                team.Department.Employees.Add(employee);
            }
            employee.Position = PersistenceContext.EmployeeRepository.GetDeveloperPosition();
            PersistenceContext.Complete();
        }
    }
}
