using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Identity;
using IdentityServer.Core.Shared;
using IdentityServer.Persistence;

namespace IdentityServer.Core
{
    public class BusinessLogic : IBusinessLayer
    {
        IEmployeeService employeeService;
        ITeamService teamService;
        IDepartmentService departmentService;

        public BusinessLogic(IPersistenceContext persistenceContext)
        {
            employeeService = new EmployeeService(persistenceContext);
            teamService = new TeamService(persistenceContext);
            departmentService = new DepartmentService(persistenceContext);
        }

        public IDepartmentService GetDepartmentService()
        {
            return departmentService;
        }

        public IEmployeeService GetEmployeeService()
        {
            return employeeService;
        }

        public ITeamService GetTeamService()
        {
            return teamService;
        }
    }
}
