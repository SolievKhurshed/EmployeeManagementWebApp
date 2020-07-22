using EmployeeManagementWebApp.Models;
using EmployeeManagementWebApp.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManagementWebApp.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly EmployeeManagementContext context;

        public EmployeeController(EmployeeManagementContext _context)
        {
            context = _context;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var empList = await context.Employees
                .Include(c => c.Group)
                .ToListAsync();

            if (empList != null)
            {
                var model = empList.Select(x =>
                    new EmployeeViewModel
                    {
                        Id = x.Id,
                        FullName = x.LastName + " " + x.FirstName + " " + x.MiddleName,
                        Group = x.Group.Name,
                        Email = x.Email
                    }).OrderBy(c => c.FullName).ToList();

                return View(model);
            }
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> EmployeeDetails(string id)
        {
            bool result = int.TryParse(id, out int appId);
            if (result)
            {
                var emp = await context.Employees
                    .Where(s => s.Id == Convert.ToInt32(id))
                    .Include(s => s.Group).ThenInclude(s => s.Department)
                    .Include(s => s.EmployeePositions)
                    .Include(s => s.MobilePhones)
                    .FirstOrDefaultAsync();

                if (emp == null)
                {
                    Response.StatusCode = 404;
                    return View("EmployeeNotFound", id);
                }
                else
                {
                    EmployeeDetailsViewModel model = new EmployeeDetailsViewModel()
                    {
                        Id = emp.Id,
                        FirstName = emp.FirstName,
                        LastName = emp.LastName,
                        MiddleName = emp.MiddleName,
                        Department = emp.Group.Department.Name,
                        Group = emp.Group.Name,
                        Login = emp.Login,
                        Email = emp.Email,
                        ExtensionPhone = emp.ExtensionPhone,
                        BithDate = emp.BirthDate,
                        EmploymentDate = emp.EmploymentDate,
                        MobilePhone = emp.MobilePhones.Count == 0 ? "Номер отсутствует" : string.Join(", ", emp.MobilePhones.Select(s => s.Number)),
                        IsActive = emp.IsActive
                    };

                    var position = await context.EmployeePositions
                                            .Where(s => s.EmployeeId == Convert.ToInt32(id))
                                            .Include(s => s.Position)
                                            .ToListAsync();

                    if (position != null)
                    {
                        model.Position = string.Join(", ", position.Select(s => s.Position.Name));
                    }

                    return View(model);
                }
            }
            Response.StatusCode = 404;
            return View("EmployeeNotFound", id);
        }

        [HttpGet]
        public async Task<IActionResult> EmployeesByDepartment(string id)
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
                    var empList = await context.Employees
                        .Where(c => c.Group.Department.Id == dep.Id)
                        .Include(c => c.Group.Department)
                        .ToListAsync();

                    ViewBag.DepartmentName = empList.Select(c => c.Group.Department.Name).FirstOrDefault();

                    if (empList != null)
                    {
                        var model = empList.Select(x =>
                            new EmployeeViewModel
                            {
                                Id = x.Id,
                                FullName = x.LastName + " " + x.FirstName + " " + x.MiddleName,
                                Group = x.Group.Name,
                                Email = x.Email
                            }).OrderBy(c => c.FullName).ToList();

                        return View(model);
                    }
                }
            }
            Response.StatusCode = 404;
            return View("DepartmentNotFound", id);
        }

        [HttpGet]
        public async Task<IActionResult> EmployeesByGroup(string id)
        {
            bool result = int.TryParse(id, out int appId);
            if (result)
            {
                var group = await context.Groups
                    .Where(c => c.Id == Convert.ToInt32(id))
                    .FirstOrDefaultAsync();

                if (group == null)
                {
                    Response.StatusCode = 404;
                    return View("DepartmentNotFound", id);
                }
                else
                {
                    var empList = await context.Employees
                        .Where(c => c.Group.Id == group.Id)
                        .ToListAsync();

                    ViewBag.GroupName = empList.Select(c => c.Group.Name).FirstOrDefault();

                    if (empList != null)
                    {
                        var model = empList.Select(x =>
                            new EmployeeViewModel
                            {
                                Id = x.Id,
                                FullName = x.LastName + " " + x.FirstName + " " + x.MiddleName,
                                Group = x.Group.Name,
                                Email = x.Email
                            }).OrderBy(c => c.FullName).ToList();

                        return View(model);
                    }
                }
            }
            Response.StatusCode = 404;
            return View("DepartmentNotFound", id);
        }

        [HttpGet]
        public async Task<IActionResult> EmployeesByPosition(string id)
        {
            bool result = int.TryParse(id, out int appId);
            if (result)
            {
                var empPos = await context.EmployeePositions
                    .Where(c => c.PositionId == Convert.ToInt32(id))
                    .ToListAsync();

                if (empPos == null)
                {
                    Response.StatusCode = 404;
                    return View("EmployeesByPositionNotFound", id);
                }
                else
                {
                    var empList = await context.Employees
                        .Where(c => empPos.Any(x => x.EmployeeId == c.Id))
                        .Include(c => c.Group)
                        .ToListAsync();

                    var pos = await context.Positions
                        .Where(c => c.Id == Convert.ToInt32(id))
                        .FirstOrDefaultAsync();

                    ViewBag.PositionName = pos.Name;

                    if (empList != null)
                    {
                        var model = empList.Select(x =>
                            new EmployeeViewModel
                            {
                                Id = x.Id,
                                FullName = x.LastName + " " + x.FirstName + " " + x.MiddleName,
                                Group = x.Group.Name,
                                Email = x.Email
                            }).OrderBy(c => c.FullName).ToList();

                        return View(model);
                    }
                }
            }
            Response.StatusCode = 404;
            return View("EmployeesByPositionNotFound", id);
        }

        [HttpGet]
        public IActionResult AddEmployee()
        {
            ViewBag.Group = context.Groups.Select(c => new SelectListItem
            {
                Text = c.Name,
                Value = c.Name
            });

            ViewBag.Position = context.Positions.Select(c => new SelectListItem
            {
                Text = c.Name,
                Value = c.Name
            });

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddEmployee(AddEmployeeViewModel model)
        {
            ViewBag.Group = context.Groups.Select(c => new SelectListItem
            {
                Text = c.Name,
                Value = c.Name
            });

            ViewBag.Position = context.Positions.Select(c => new SelectListItem
            {
                Text = c.Name,
                Value = c.Name
            });

            if (ModelState.IsValid)
            {
                var group = await context.Groups.Where(c => c.Name == model.GroupValue).FirstOrDefaultAsync();
                var position = await context.Positions.Where(c => c.Name == model.PositionValue).FirstOrDefaultAsync();

                var employee = new Employee()
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    MiddleName = model.MiddleName,
                    Login = model.Login,
                    Email = model.Email,
                    ExtensionPhone = model.ExtensionPhone,
                    BirthDate = model.BirthDate,
                    EmploymentDate = model.EmploymentDate,
                    IsActive = true,
                    Group = group
                };

                var mobile = new MobilePhone()
                {
                    Employee = employee,
                    Number = model.MobilePhone
                };

                var emplPosition = new EmployeePosition()
                {
                    Position = position,
                    Employee = employee
                };

                await context.Employees.AddAsync(employee);
                await context.MobilePhones.AddAsync(mobile);
                await context.EmployeePositions.AddAsync(emplPosition);

                int changesCount = await context.SaveChangesAsync();

                if (changesCount > 0)
                {
                    var appLogType = await context.AppLogTypes.Where(c => c.TypeName == "EmployeeAdded").FirstOrDefaultAsync();

                    var appLog = new AppLog()
                    {
                        Created = DateTime.Now,
                        AppLogType = appLogType,
                        Message = $"Добавлен новый пользователь. ID: {employee.Id}, ФИО: {employee.LastName} {employee.FirstName} {employee.MiddleName}"
                    };

                    await context.AppLogs.AddAsync(appLog);
                    await context.SaveChangesAsync();
                }
                return RedirectToAction("EmployeeDetails", "Employee", new { id = employee.Id });
            }
            return View();
        }


        [HttpGet]
        public async Task<IActionResult> AddMobilePhone(string id)
        {
            ViewBag.EmpId = id;
            var emp = await context.Employees
                                .Where(c => c.Id == Convert.ToInt32(id))
                                .FirstOrDefaultAsync();

            ViewBag.EmpFullName = emp.LastName + " " + emp.FirstName + " " + emp.MiddleName;

            return View();
        }


        [HttpPost]
        public async Task<IActionResult> AddMobilePhone(AddMobilePhoneViewModel model)
        {
            ViewBag.EmpId = model.EmployeeId;

            if (ModelState.IsValid)
            {
                var emp = await context.Employees
                                .Where(c => c.Id == Convert.ToInt32(model.EmployeeId))                                
                                .FirstOrDefaultAsync();

                var mobPhone = new MobilePhone()
                {
                    Employee = emp,
                    Number = model.PhoneNumber
                };

                await context.MobilePhones.AddAsync(mobPhone);

                int changesCount = await context.SaveChangesAsync();

                if (changesCount > 0)
                {
                    var appLogType = await context.AppLogTypes.Where(c => c.TypeName == "MobilePhoneAdded").FirstOrDefaultAsync();

                    var appLog = new AppLog()
                    {
                        Created = DateTime.Now,
                        AppLogType = appLogType,
                        Message = $"Для пользователя {mobPhone.Employee.Id} добавлен номер мобильного телефона. ID: {mobPhone.Id}, номер: {mobPhone.Number}"
                    };

                    await context.AppLogs.AddAsync(appLog);
                    await context.SaveChangesAsync();
                }
                return RedirectToAction("EmployeeDetails", "Employee", new { id = mobPhone.Employee.Id });
            }
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> AddPositionToEmployee(string id)
        {
            bool result = int.TryParse(id, out int appId);
            if (result)
            {
                ViewBag.EmpId = id;

                var emp = await context.Employees
                                    .Where(c => c.Id == Convert.ToInt32(id))
                                    .Include(s => s.EmployeePositions)
                                    .FirstOrDefaultAsync();

                var empPosition = await context.EmployeePositions
                                            .Where(c => c.EmployeeId == Convert.ToInt32(id))
                                            .ToListAsync();

                var positions = await context.Positions.Where(p => empPosition.All(s => s.PositionId != p.Id)).ToListAsync();

                ViewBag.Position = positions.Select(c => new SelectListItem
                {
                    Text = c.Name,
                    Value = c.Name
                });

                ViewBag.EmpFullName = emp.LastName + " " + emp.FirstName + " " + emp.MiddleName;

                return View();
            }
            return View("PositionNotFound");
        }


        [HttpPost]
        public async Task<IActionResult> AddPositionToEmployee(AddPositionToEmployeeViewModel model)
        {
            ViewBag.EmpId = model.EmployeeId;
            ViewBag.Position = context.Positions.Select(c => new SelectListItem
            {
                Text = c.Name,
                Value = c.Name
            });

            if (ModelState.IsValid)
            {
                var emp = await context.Employees
                                .Where(c => c.Id == Convert.ToInt32(model.EmployeeId))
                                .FirstOrDefaultAsync();

                var position = await context.Positions.Where(c => c.Name == model.PositionValue).FirstOrDefaultAsync();

                if (position != null)
                {
                    var emplPosition = new EmployeePosition()
                    {
                        Position = position,
                        Employee = emp
                    };

                    await context.EmployeePositions.AddAsync(emplPosition);

                    int changesCount = await context.SaveChangesAsync();

                    if (changesCount > 0)
                    {
                        var appLogType = await context.AppLogTypes.Where(c => c.TypeName == "PositionToEmployeeAdded").FirstOrDefaultAsync();

                        var appLog = new AppLog()
                        {
                            Created = DateTime.Now,
                            AppLogType = appLogType,
                            Message = $"Для пользователя {emplPosition.Employee.Id} добавлена должность. ID: {emplPosition.PositionId}, наименование: {emplPosition.Position.Name}"
                        };

                        await context.AppLogs.AddAsync(appLog);
                        await context.SaveChangesAsync();
                    }
                    return RedirectToAction("EmployeeDetails", "Employee", new { id = emplPosition.EmployeeId });
                }
                else
                {
                    ModelState.AddModelError("", "Выберите должность");
                }
            }
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> EditEmployee(string id)
        {
            ViewBag.Group = context.Groups.Select(c => new SelectListItem
            {
                Text = c.Name,
                Value = c.Name
            });            

            bool result = int.TryParse(id, out int appId);
            if (result)
            {
                var employee = await context.Employees
                    .Where(e => e.Id == Convert.ToInt32(id))
                    .Include(m => m.MobilePhones)
                    .Include(g => g.Group)
                    .FirstOrDefaultAsync();

                ViewBag.CurrentGroup = employee.Group.Name;

                if (employee == null)
                {
                    Response.StatusCode = 404;
                    return View("EmployeeNotFound", id);
                }
                else
                {
                    EditEmployeeViewModel model = new EditEmployeeViewModel()
                    {
                        Id = employee.Id,
                        FirstName = employee.FirstName,
                        LastName = employee.LastName,
                        MiddleName = employee.MiddleName,
                        Login = employee.Login,
                        Email = employee.Email,
                        ExtensionPhone = employee.ExtensionPhone,
                        BirthDate = employee.BirthDate,
                        EmploymentDate = employee.EmploymentDate,
                        IsActive = employee.IsActive,
                        MobilePhone = employee.MobilePhones.Count == 0 ? "Номер отсутствует" : string.Join(", ", employee.MobilePhones.Select(s => s.Number)),                        
                    };

                    var position = await context.EmployeePositions
                                            .Where(s => s.EmployeeId == Convert.ToInt32(id))
                                            .Include(s => s.Position)
                                            .ToListAsync();

                    if (position != null)
                    {
                        model.PositionReadOnly = string.Join(", ", position.Select(s => s.Position.Name));
                    }

                    if (model != null)
                    {
                        return View(model);
                    }
                }
            }
            Response.StatusCode = 404;
            return View("EmployeeNotFound", id);
        }

        [HttpPost]
        public async Task<IActionResult> EditEmployee(EditEmployeeViewModel model, string id)
        {
            ViewBag.Group = context.Groups.Select(c => new SelectListItem
            {
                Text = c.Name,
                Value = c.Name
            });

            bool result = int.TryParse(id, out int appId);
            if (result)
            {
                if (ModelState.IsValid)
                {
                    var emp = await context.Employees
                        .Where(e => e.Id == Convert.ToInt32(id))
                        .Include(g => g.Group)
                        .FirstOrDefaultAsync();

                    if (emp != null)
                    {
                        string changeLog = $"Было - ID: {emp.Id}, Фамилия: {emp.LastName}, Имя: {emp.FirstName}, Отчество: {emp.MiddleName}, " +
                                           $"Учетка: {emp.Login}, Электронный адрес: {emp.Email}, Добавочный номер: {emp.ExtensionPhone}, " +
                                           $"Дата рождения: {emp.BirthDate}, Дата устройства: {emp.EmploymentDate}, Статус: {emp.IsActive}, " +
                                           $"Группа: {emp.Group.Name}";
                        
                        var group = await context.Groups.Where(d => d.Name == model.GroupValue).FirstOrDefaultAsync();

                        Group newGroup = emp.Group;

                        if (group != null)
                        {
                            newGroup = group;
                        }

                        emp.FirstName = model.FirstName;
                        emp.LastName = model.LastName;
                        emp.MiddleName = model.MiddleName;
                        emp.Login = model.Login;
                        emp.Email = model.Email;
                        emp.ExtensionPhone = model.ExtensionPhone;
                        emp.BirthDate = model.BirthDate;
                        emp.EmploymentDate = model.EmploymentDate;
                        emp.Group = newGroup;

                        int changesCount = await context.SaveChangesAsync();

                        if (changesCount > 0)
                        {
                            var appLogType = context.AppLogTypes.Where(c => c.TypeName == "EmployeeWasEdited").FirstOrDefault();

                            var appLog = new AppLog()
                            {
                                Created = DateTime.Now,
                                AppLogType = appLogType,
                                Message = $"Изменены реквизиты сотрудника. {changeLog}. " +
                                          $"Стало - ID: {emp.Id}, Фамилия: {emp.LastName}, Имя: {emp.FirstName}, Отчество: {emp.MiddleName}, " +
                                          $"Учетка: {emp.Login}, Электронный адрес: {emp.Email}, Добавочный номер: {emp.ExtensionPhone}, " +
                                          $"Дата рождения: {emp.BirthDate}, Дата устройства: {emp.EmploymentDate}, Статус: {emp.IsActive}, " +
                                          $"Группа: {emp.Group.Name}"
                            };

                            await context.AppLogs.AddAsync(appLog);
                            await context.SaveChangesAsync();
                        }
                        return RedirectToAction("Index", "Employee");
                    }
                }
                return View();
            }
            Response.StatusCode = 404;
            return View("EmployeeNotFound", id);
        }
    }
}