using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Identity;
using StudentsManagement.Domain;
using StudentsManagement.Persistence;

namespace StudentsManagement.Core.Shared
{
    public class BusinessLogic : IBusinessLayer
    {
        //List<IInitializer> initList;
        
        private IStudentServices _studentServices;
        private ITeacherServices _teacherServices;


        public BusinessLogic(IPersistenceContext persistenceContext)
        {            
            _studentServices = new StudentServices(persistenceContext);
            _teacherServices = new TeacherServices(persistenceContext);

        }

        

        public IStudentServices GetStudentOperationService()
        {
            return _studentServices;
        }

        public ITeacherServices GetTeacherOperationService()
        {
            return _teacherServices;
        }
    }
}
