using Gabriel.Data;
using Gabriel.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Gabriel.Controllers
{
    public class MusicController : Controller
    {
        private readonly DatabaseContext context;
        public MusicController(DatabaseContext context)
        {
            this.context = context;
        }

        [HttpGet]
        public async Task<ActionResult<Sheet>> Compositions()
        {
            var sheets = await context.Sheets.ToListAsync();
            ViewData["Sheets"] = sheets;
            return View();
        }
    }
}
