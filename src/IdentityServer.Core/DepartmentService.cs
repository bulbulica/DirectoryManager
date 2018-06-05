using IdentityServer.Core.Shared;
using IdentityServer.Domain;
using IdentityServer.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IdentityServer.Core
{
    public class DepartmentService : IDepartmentService
    {
        private IPersistenceContext _persistenceContext;

        public DepartmentService(IPersistenceContext persistenceContext)
        {
            PersistenceContext = persistenceContext;
        }

        public IPersistenceContext PersistenceContext { get => _persistenceContext; set => _persistenceContext = value; }

        public void AddDepartment(Department department)
        {
            PersistenceContext.EmployeeRepository.AddDepartment(department);
            PersistenceContext.Complete();
        }

        public void AddEmployeeToDepartment(Employee employee, Department department)
        {
            employee.Department = department;
            department.Employees.Add(employee);
            employee.Team = null;
            employee.Position = PersistenceContext.EmployeeRepository.GetDeveloperPosition();
            PersistenceContext.Complete();
        }

        public void AddTeamToDepartment(Department department, Team team)
        {
            var exDepartment = team.Department;
            if (exDepartment != null)
            {
                exDepartment.Teams.Remove(team);
            }
            department.Teams.Add(team);
            team.Department = department;
            foreach (var employee in team.Employees)
            {
                employee.Department = department;
            }
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
                if (department.Teams.Count != 0)
                {
                    foreach (var team in department.Teams)
                    {
                        team.Department = null;
                    }
                }

                if (department.Employees.Count != 0)
                {
                    foreach (var employee in department.Employees)
                    {
                        employee.Department = null;
                    }
                }

                PersistenceContext.EmployeeRepository.DeleteDepartment(department);
                PersistenceContext.Complete();
            }
        }

        public IEnumerable<Department> GetAllDepartments()
        {
            if (PersistenceContext.EmployeeRepository.GetAllPositions().Count() != 0)
                return PersistenceContext.EmployeeRepository.GetAllDepartments().ToList();
            return new List<Department>();
        }

        public List<Employee> GetAllEmployeesFromDepartment(Department department)
        {
            return PersistenceContext.EmployeeRepository.GetAllEmployeesFromDepartment(department);
        }

        public List<Team> GetAllTeamsFromDepartment(Department department)
        {
            return PersistenceContext.EmployeeRepository.GetAllTeamsFromDepartment(department);
        }

        public Department GetDepartment(int idDepartment)
        {
            return PersistenceContext.EmployeeRepository.GetDepartmentById(idDepartment);
        }

        public Department GetDepartmentByName(string department)
        {
            return PersistenceContext.EmployeeRepository.GetDepartmentByName(department);
        }

        public Employee GetDepartmentManager(Department department)
        {
            foreach (var employee in department.Employees)
            {
                if (employee.Position.RoleName.Equals(PersistenceContext.EmployeeRepository.GetDepartmentManagerPosition().RoleName))
                    return employee;
            }
            return null;
        }

        public Position GetDepartmentManagerPosition()
        {
            return PersistenceContext.EmployeeRepository.GetDepartmentManagerPosition();
        }

        public void RemoveEmployeeFromDepartment(int idEmployee)
        {
            var user = PersistenceContext.EmployeeRepository.GetEmployeeById(idEmployee);
            var userTeam = user.Team;
            var userDepartment = user.Department;
            if (userTeam != null)
            {
                user.Team = null;
                userTeam.Employees.Remove(user);
            }

            user.Department = null;
            userDepartment.Employees.Remove(user);
            PersistenceContext.Complete();
        }

        public void RemoveTeamFromDepartment(int idTeam)
        {
            var team = PersistenceContext.EmployeeRepository.GetTeamById(idTeam);
            var department = team.Department;
            if (department != null)
            {
                foreach (var employee in team.Employees)
                {
                    employee.Department = null;
                    department.Employees.Remove(employee);
                }
                department.Teams.Remove(team);
                team.Department = null;
            }
        }

        public void UpdateDepartment(Department department)
        {
            var newDepartment = PersistenceContext.EmployeeRepository.GetDepartmentById(department.Id);
            newDepartment.Name = department.Name;
            newDepartment.Description = department.Description;
            PersistenceContext.Complete();
        }

        public void UpdateDepartmentManager(Department department, Employee employee)
        {
            var exDepartmentManager = GetDepartmentManager(department);
            var currentEmployee = department.Employees.Where(e => employee.Username.Equals(employee.Username)).FirstOrDefault();

            if (currentEmployee != null)
            {
                if (exDepartmentManager != null)
                {
                    exDepartmentManager.Position = PersistenceContext.EmployeeRepository.GetDeveloperPosition();
                }
                currentEmployee.Position = PersistenceContext.EmployeeRepository.GetDepartmentManagerPosition();
            }
            else
            {
                if (exDepartmentManager != null)
                {
                    exDepartmentManager.Position = PersistenceContext.EmployeeRepository.GetDeveloperPosition();
                }                
                employee.Department = department;
                employee.Position = PersistenceContext.EmployeeRepository.GetDepartmentManagerPosition();
            }
            PersistenceContext.Complete();
        }
    }
}
