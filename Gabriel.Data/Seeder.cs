using Gabriel.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Gabriel.Data
{
    public class Seeder
    {


        public static async Task DbInitializer(DatabaseContext context, ILoggerFactory loggerFactory)
        {
            try
            {
                context.Database.EnsureCreated();
                if (!context.Sheets.Any())
                {
                    var sheet = File.ReadAllText("");
                    var serializedSheet = JsonSerializer.Deserialize<List<Sheet>>(sheet);

                    foreach(var item in serializedSheet)
                    {
                        context.Sheets.Add(item);
                    }
                    await context.SaveChangesAsync();
                }
            }
            catch(Exception ex)
            {
                var logger = loggerFactory.CreateLogger<Seeder>();
                logger.LogError(ex.Message);
            }
        }
    }
}
