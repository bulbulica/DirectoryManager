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
            if (employee != null) {
                PersistenceContext.EmployeeRepository.Delete(employee);
                PersistenceContext.Complete();
            }
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
                PersistenceContext.EmployeeRepository.DeleteDepartment(department);
                PersistenceContext.Complete();
            }
        }

        public void DeleteTeam(int idTeam)
        {
            var team = PersistenceContext.EmployeeRepository.GetTeamById(idTeam);
            if (team != null)
            {
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
            var updatedTeam = PersistenceContext.EmployeeRepository.GetTeamById(team.Id);

            if(updatedTeam.TeamLeader != null)
            {
                var OldTeamLeader = updatedTeam.TeamLeader;
                OldTeamLeader.Position = _persistenceContext.EmployeeRepository.GetDeveloperPosition();

            }
            updatedTeam.TeamLeader = employee;
            employee.Position = PersistenceContext.EmployeeRepository.GetTeamLeaderPosition();
            PersistenceContext.Complete();
        }
    }
}
