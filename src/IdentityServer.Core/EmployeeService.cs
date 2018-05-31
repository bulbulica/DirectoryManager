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

        public IEnumerable<Employee> GetAllUnassignedEmployees()
        {
            return PersistenceContext.EmployeeRepository.GetAllUnassignedEmployees();
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
    }
}
