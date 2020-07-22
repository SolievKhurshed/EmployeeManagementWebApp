using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EmployeeManagementWebApp.Models;
using EmployeeManagementWebApp.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagementWebApp.Controllers
{
    public class GroupController : Controller
    {
        private readonly EmployeeManagementContext context;

        public GroupController(EmployeeManagementContext _context)
        {
            context = _context;
        }

        public IActionResult Index()
        {
            var groupList = context.Groups
                .Include(c => c.Department)
                .ToList();

            if (groupList != null)
            {
                var model = groupList.Select(x =>
                    new GroupViewModel
                    {
                        Id = x.Id,
                        Name = x.Name,
                        Email = x.Email,
                        Description = x.Description,
                        Department = x.Department.Name
                    }).OrderBy(c => c.Name).ToList();

                return View(model);
            }

            return View();
        }



        [HttpGet]
        public IActionResult AddGroup()
        {
            ViewBag.Department = context.Departments.Select(c => new SelectListItem
            {
                Text = c.Name,
                Value = c.Name
            });

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddGroup(AddGroupViewModel model)
        {
            ViewBag.Department = context.Departments.Select(c => new SelectListItem
            {
                Text = c.Name,
                Value = c.Name
            });

            if (ModelState.IsValid)
            {
                var dep = context.Departments.Where(s => s.Name == model.DepartmentValue).FirstOrDefault();
                var group = new Group()
                {
                    Name = model.Name,
                    Email = model.Email,
                    Description = model.Description,
                    Department = dep
                };

                await context.Groups.AddAsync(group);

                int changesCount = await context.SaveChangesAsync();

                if (changesCount > 0)
                {
                    var appLogType = context.AppLogTypes.Where(c => c.TypeName == "GroupAdded").FirstOrDefault();

                    var appLog = new AppLog()
                    {
                        Created = DateTime.Now,
                        AppLogType = appLogType,
                        Message = $"Добавлена новая группа. ID: {group.Id}, Наименование: {group.Name}"
                    };

                    await context.AppLogs.AddAsync(appLog);
                    await context.SaveChangesAsync();
                }
                return RedirectToAction("Index", "Group");
            }

            return View();
        }

        [HttpGet]
        public async Task<IActionResult> EditGroup(string id)
        {
            bool result = int.TryParse(id, out int appId);
            if (result)
            {
                var group = await context.Groups
                    .Where(c => c.Id == Convert.ToInt32(id))
                    .Include(d => d.Department)
                    .FirstOrDefaultAsync();

                if (group == null)
                {
                    Response.StatusCode = 404;
                    return View("DepartmentNotFound", id);
                }
                else
                {
                    ViewBag.Department = context.Departments.Select(c => new SelectListItem
                    {
                        Text = c.Name,
                        Value = c.Name
                    });

                    ViewBag.CurrentDepartment = group.Department.Name;

                    EditGroupViewModel model = new EditGroupViewModel()
                    {
                        Id = group.Id,
                        Name = group.Name,
                        Email = group.Email,
                        Description = group.Description
                    };

                    if (model != null)
                    {
                        return View(model);
                    }
                }
            }
            Response.StatusCode = 404;
            return View("DepartmentNotFound", id);
        }

        [HttpPost]
        public async Task<IActionResult> EditGroup(EditGroupViewModel model, string id)
        {
            ViewBag.Department = context.Departments.Select(c => new SelectListItem
            {
                Text = c.Name,
                Value = c.Name
            });

            bool result = int.TryParse(id, out int appId);
            if (result)
            {
                if (ModelState.IsValid)
                {
                    var group = await context.Groups
                        .Where(g => g.Id == Convert.ToInt32(id))
                        .Include(d => d.Department)
                        .FirstOrDefaultAsync();                    

                    if (group != null)
                    {
                        string changeLog = $"Было - ID: {group.Id}, Наименование: {group.Name}, Почта: {group.Email}, Описание: {group.Description}, " +
                                           $"Отдел: {group.Department.Name}";

                        var dep = await context.Departments.Where(d => d.Name == model.DepartmentValue).FirstOrDefaultAsync();

                        group.Name = model.Name;
                        group.Email = model.Email;
                        group.Description = model.Description;
                        group.Department = dep;

                        int changesCount = await context.SaveChangesAsync();

                        if (changesCount > 0)
                        {
                            var appLogType = context.AppLogTypes.Where(c => c.TypeName == "GroupWasEdited").FirstOrDefault();

                            var appLog = new AppLog()
                            {
                                Created = DateTime.Now,
                                AppLogType = appLogType,
                                Message = $"Изменена группа. {changeLog}. Стало - ID: {group.Id}, Наименование: {group.Name}, Почта: {group.Email}, Описание: {group.Description}, " +
                                          $"Отдел: {group.Department.Name}"
                            };

                            await context.AppLogs.AddAsync(appLog);
                            await context.SaveChangesAsync();
                        }
                        return RedirectToAction("Index", "Group");
                    }
                }
                return View();
            }
            Response.StatusCode = 404;
            return View("DepartmentNotFound", id);
        }
    }
}