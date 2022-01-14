using Gabriel.Data;
using Gabriel.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gabriel.Controllers
{
    public class DashboardController : Controller
    {
        private readonly DatabaseContext context;
        public DashboardController(DatabaseContext context)
        {
            this.context = context;
        }

        
        public IActionResult AddBook()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddBook(SheetViewModel model)
        {
            var compare = context.Sheets.Where(x => x.Name == model.Name).FirstOrDefault();
            if(compare != null)
            {
                ViewData["Error"] = "The music already exists";
                return View();
            }
            Sheet sheet = new()
            {
                Name = model.Name,
                Composer = model.Composer,
                Description = model.Description,
                Price = model.Price,
                Url = model.Url
            };
            await context.Sheets.AddAsync(sheet);
            await context.SaveChangesAsync();
            return RedirectToAction("Compositions", "Music");
        }


        [HttpDelete]
        public async Task<IActionResult> DeleteSheet(int id)
        {
            var sheet = context.Sheets.Where(x => x.Id == id).FirstOrDefault();
            context.Sheets.Remove(sheet);
            await context.SaveChangesAsync();
            return View();
        }

        [HttpGet("{id}")]
        public ActionResult<Sheet> GetSheet(int id) 
        {
            var sheet = context.Sheets.Where(x => x.Id == id).FirstOrDefault();
            if(sheet is not null)
            {

            }
            return View();
        }


        [HttpGet]
        public async Task<ActionResult<ICollection<Sheet>>> GetAllSheets()
        {
            var sheets = await context.Sheets.ToListAsync();
            return View(sheets);
        }
    }
}
