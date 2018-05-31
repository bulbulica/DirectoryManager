// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityServer.Core.Shared;
using IdentityServer.Domain;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using System.IO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;

namespace IdentityServer
{
    [Route("[controller]/[action]")]
    [SecurityHeaders]
    public class DeveloperController : Controller
    {
        private readonly IAuthentication _auth;
        private readonly IBusinessLayer _businessLogic;
        private readonly IEmployeeService _employeeService;
        private readonly IDepartmentService _departmentService;

        public DeveloperController(IAuthentication auth,
            IBusinessLayer businessLayer)
        {
            _auth = auth;
            _businessLogic = businessLayer;
            _employeeService = _businessLogic.GetEmployeeService();
            _departmentService = _businessLogic.GetDepartmentService();
        }

        [HttpGet]
        public IActionResult Add()
        {
            var user = _employeeService.GetEmployeeByName(User.Identity.Name);

            var allPositions = _employeeService.GetAllPositions();
            List<Position> displayedPositions = new List<Position>();

            foreach (var position in allPositions)
            {
                if (position.RoleName != Constants.TeamLeaderRole)
                {
                    displayedPositions.Add(position);
                }
            }
            var username = User.Identity.Name;

            var model = new AddEmployee()
            {
                Active = true,
                AllPositions = displayedPositions,
                AllDepartments = _departmentService.GetAllDepartments()
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult>Add(AddEmployee model)
        {
            var user = _employeeService.GetEmployeeByName(User.Identity.Name);

            if (ModelState.IsValid)
            {
                var position = _employeeService.GetPositionByName(model.Position);
                var department = _departmentService.GetDepartmentByName(model.Department);

                if (position.AccessLevel <= Constants.GeneralManagerAccessLevel
                    || position.AccessLevel == Constants.OfficeManagerAccessLevel)
                {
                    department = null;
                }
                if (position != null)
                {
                    var User = new ApplicationUser { UserName = model.Username, FullName = model.Name, Email = model.Username };
                    var result = await _auth.RegisterProcess(User, model.Password, position);

                    if (result)
                    {
                        var employee = new Employee
                        {
                            Name = model.Name,
                            Username = model.Username,
                            Active = true,
                            Position = position,
                            Department = department,
                        };
                        _employeeService.AddEmployee(employee);

                        if (employee.Position == _departmentService.GetDepartmentManagerPosition() && department != null)
                        {
                            var exDepartmentManager = _departmentService.GetDepartmentManager(department);

                            if (exDepartmentManager != null)
                            {
                                await _auth.UpdateRoleAsync(exDepartmentManager.Username, Constants.DeveloperRole);
                            }
                            _departmentService.UpdateDepartmentManager(employee.Department, employee);
                        }
                    }
                    else
                    {
                        var ErrorMessage = $"the password does not meet the password policy requirements.";
                        var policyRequirements = $"* At least an uppercase and a special character";

                        ViewBag.Error = ErrorMessage;
                        ViewBag.policyRequirments = policyRequirements;

                        var returnModel = new AddEmployee()
                        {
                            Active = true,
                            AllPositions = model.AllPositions,
                            AllDepartments = _departmentService.GetAllDepartments()
                        };

                        return View(returnModel);
                    }
                }
                return RedirectToAction("Index", "Home");
            }
            else
            {
                var ErrorMessage = $"the password does not meet the password policy requirements.";
                var policyRequirements = $"* At least an uppercase and a special character";

                ViewBag.Error = ErrorMessage;
                ViewBag.policyRequirments = policyRequirements;

                var returnModel = new AddEmployee()
                {
                    Name = model.Name,
                    Username = model.Username,
                    Active = true,
                    AllPositions = model.AllPositions,
                    AllDepartments = _departmentService.GetAllDepartments()
                };

                //return View("Add", returnModel);
                return RedirectToAction("Add", "Developer", returnModel);
            }
        }
    }
}