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
        


        public BusinessLogic(IPersistenceContext persistenceContext)
        {
            employeeService = new EmployeeService(persistenceContext);
        }

        public IEmployeeService GetEmployeeService()
        {
            return employeeService;
        }
    }
}
