using System;
using System.Collections.Generic;
using System.Text;

namespace IdentityServer.Core.Shared
{
    public interface IBusinessLayer 
    {
       
        IEmployeeService GetEmployeeService();


    }
}
