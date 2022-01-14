using Gabriel.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gabriel.Data.Repository
{
    public class SheetRepository
    {
        private readonly DatabaseContext _context;


        public SheetRepository(DatabaseContext context)
        {
            _context = context;
        }



        public async Task<ICollection<Sheet>> GetAllSheets()
        {
            return await _context.Sheets.ToListAsync();
        }



        public async Task<Sheet> GetBookByName(string name)
        {
            var modifiedName = name.ToUpper();
            var book = await _context.Sheets.Where(x => x.Name == modifiedName).FirstOrDefaultAsync();
            return book;
        }
    }
}
