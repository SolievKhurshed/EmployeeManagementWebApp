using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EmployeeManagementWebApp.Models;
using EmployeeManagementWebApp.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagementWebApp.Controllers
{
    public class DepartmentController : Controller
    {
        private readonly EmployeeManagementContext context;

        public DepartmentController(EmployeeManagementContext _context)
        {
            context = _context;
        }

        public IActionResult Index()
        {
            var depList = context.Departments
                .Include(c => c.Groups)
                .ToList();

            if (depList != null)
            {
                var depForView = depList.Select(x =>
                    new DepartmentViewModel
                    {
                        Id = x.Id,
                        Name = x.Name,                        
                        Email = x.Email,
                        Description = x.Description,
                        Groups = x.Groups.Count == 0 ? "Группы отсутствуют" : string.Join(", ", x.Groups.Select(s => s.Name))
                    }).OrderBy(c => c.Name).ToList();
                return View(depForView);
            }

            return View();
        }

        [HttpGet]
        public IActionResult AddDepartment() => View();

        [HttpPost]
        public async Task<IActionResult> AddDepartment(AddDepartmentViewModel model)
        {
            if (ModelState.IsValid)
            {
                var depFromDb = await context.Departments.Where(d => d.Name == model.Name).FirstOrDefaultAsync();

                if (depFromDb == null)
                {
                    var dep = new Department()
                    {
                        Name = model.Name,
                        Email = model.Email,
                        Description = model.Description
                    };

                    await context.Departments.AddAsync(dep);

                    int changesCount = await context.SaveChangesAsync();

                    if (changesCount > 0)
                    {
                        var appLogType = context.AppLogTypes.Where(c => c.TypeName == "DepartmentAdded").FirstOrDefault();

                        var appLog = new AppLog()
                        {
                            Created = DateTime.Now,
                            AppLogType = appLogType,
                            Message = $"Добавлен новый отдел. ID: {dep.Id}, Наименование: {dep.Name}"
                        };

                        await context.AppLogs.AddAsync(appLog);
                        await context.SaveChangesAsync();
                    }
                    return RedirectToAction("Index", "Department");
                }
                else
                {
                    ModelState.AddModelError("", "Указанный отдел уже присутствует в базе данных. Укажите другой.");
                }
            }
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> EditDepartment(string id)
        {
            bool result = int.TryParse(id, out int appId);
            if (result)
            {
                var dep = await context.Departments
                    .Where(c => c.Id == Convert.ToInt32(id))
                    .FirstOrDefaultAsync();

                if (dep == null)
                {
                    Response.StatusCode = 404;
                    return View("DepartmentNotFound", id);
                }
                else
                {
                    DepartmentViewModel model = new DepartmentViewModel()
                    {
                        Id = dep.Id,
                        Name = dep.Name,
                        Email = dep.Email,
                        Description = dep.Description
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
        public async Task<IActionResult> EditDepartment(DepartmentViewModel model, string id)
        {
            bool result = int.TryParse(id, out int appId);
            if (result)
            {
                if (ModelState.IsValid)
                {
                    var dep = context.Departments
                        .Where(c => c.Id == Convert.ToInt32(id))
                        .FirstOrDefault();

                    if (dep != null)
                    {
                        string changeLog = $"Было - ID: {dep.Id}, Наименование: {dep.Name}, Почта: {dep.Email}, Описание: {dep.Description}";

                        dep.Name = model.Name;
                        dep.Email = model.Email;
                        dep.Description = model.Description;

                        int changesCount = await context.SaveChangesAsync();

                        if (changesCount > 0)
                        {
                            var appLogType = context.AppLogTypes.Where(c => c.TypeName == "DepartmentWasEdited").FirstOrDefault();

                            var appLog = new AppLog()
                            {
                                Created = DateTime.Now,
                                AppLogType = appLogType,
                                Message = $"Изменен отдел. {changeLog}. Стало - ID: {dep.Id}, Наименование: {dep.Name}, Почта: {dep.Email}, Описание: {dep.Description}"
                            };

                            await context.AppLogs.AddAsync(appLog);
                            await context.SaveChangesAsync();
                        }
                        return RedirectToAction("Index", "Department");
                    }
                }
                return View();
            }
            Response.StatusCode = 404;
            return View("DepartmentNotFound", id);
        }
    }
}