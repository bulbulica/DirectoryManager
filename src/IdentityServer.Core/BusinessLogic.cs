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
        //List<IInitializer> initList;
        


        public BusinessLogic(IPersistenceContext persistenceContext)
        {            

        }

        public IEmployeeService GetEmployeeService()
        {
            throw new NotImplementedException();
        }
    }
}
