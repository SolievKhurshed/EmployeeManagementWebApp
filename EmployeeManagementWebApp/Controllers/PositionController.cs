using EmployeeManagementWebApp.Models;
using EmployeeManagementWebApp.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManagementWebApp.Controllers
{
    public class PositionController : Controller
    {
        private readonly EmployeeManagementContext context;

        public PositionController(EmployeeManagementContext _context)
        {
            context = _context;
        }

        public IActionResult Index()
        {
            var positionList = context.Positions.ToList();

            if (positionList != null)
            {
                var model = positionList.Select(x =>
                    new PositionViewModel
                    {
                        Id = x.Id,
                        Name = x.Name
                    }).OrderBy(c => c.Name).ToList();

                return View(model);
            }

            return View();
        }

        [HttpGet]
        public IActionResult AddPosition() => View();

        [HttpPost]
        public async Task<IActionResult> AddPosition(AddPositionViewModel model)
        {
            if (ModelState.IsValid)
            {
                var posFromDb = await context.Positions.Where(p => p.Name == model.Name).FirstOrDefaultAsync();

                if (posFromDb == null)
                {

                    var position = new Position()
                    {
                        Name = model.Name
                    };

                    await context.Positions.AddAsync(position);

                    int changesCount = await context.SaveChangesAsync();

                    if (changesCount > 0)
                    {
                        var appLogType = context.AppLogTypes.Where(c => c.TypeName == "PositionAdded").FirstOrDefault();

                        var appLog = new AppLog()
                        {
                            Created = DateTime.Now,
                            AppLogType = appLogType,
                            Message = $"Добавлена новая должность. ID: {position.Id}, Наименование: {position.Name}"
                        };

                        await context.AppLogs.AddAsync(appLog);
                        await context.SaveChangesAsync();
                    }
                    return RedirectToAction("Index", "Position");
                }
                else
                {
                    ModelState.AddModelError("", "Данная должность уже присутствует в базе данных. Укажите другую.");
                }
            }
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> EditPosition(string id)
        {
            bool result = int.TryParse(id, out int appId);
            if (result)
            {
                var position = await context.Positions.Where(p => p.Id == Convert.ToInt32(id)).FirstOrDefaultAsync();

                if (position == null)
                {
                    Response.StatusCode = 404;
                    return View("PositionNotFound", id);
                }
                else
                {
                    PositionViewModel model = new PositionViewModel()
                    {
                        Id = position.Id,
                        Name = position.Name
                    };

                    if (model != null)
                    {
                        return View(model);
                    }
                }
            }
            Response.StatusCode = 404;
            return View("PositionNotFound", id);
        }

        [HttpPost]
        public async Task<IActionResult> EditPosition(PositionViewModel model, string id)
        {
            bool result = int.TryParse(id, out int appId);
            if (result)
            {
                if (ModelState.IsValid)
                {
                    var position = await context.Positions.Where(p => p.Id == Convert.ToInt32(id)).FirstOrDefaultAsync();

                    if (position != null)
                    {
                        string changeLog = $"Было - ID: {position.Id}, Наименование: {position.Name}";

                        position.Name = model.Name;

                        int changesCount = await context.SaveChangesAsync();

                        if (changesCount > 0)
                        {
                            var appLogType = context.AppLogTypes.Where(c => c.TypeName == "PositionWasEdited").FirstOrDefault();

                            var appLog = new AppLog()
                            {
                                Created = DateTime.Now,
                                AppLogType = appLogType,
                                Message = $"Изменена должность. {changeLog}. Стало - ID: {position.Id}, Наименование: {position.Name}"
                            };

                            await context.AppLogs.AddAsync(appLog);
                            await context.SaveChangesAsync();
                        }
                        return RedirectToAction("Index", "Position");
                    }
                }
                return View();
            }
            Response.StatusCode = 404;
            return View("PositionNotFound", id);
        }
    }
}