using Gabriel.Data;
using Gabriel.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using PayStack.Net;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Gabriel.Controllers
{
    public class WorksController : Controller
    {

        private readonly IConfiguration _configuration;
        private readonly DatabaseContext _context;
        private readonly string Token;
        private PayStackApi PayStack { get; set; }

        private static string _mod;


        public WorksController(IConfiguration configuraion, DatabaseContext context)
        {
            _configuration = configuraion;
            Token = _configuration["Payment:PaystackSK"];
            _context = context;
            PayStack = new PayStackApi(Token);
        }

        public IActionResult DisplayItem()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Payment()
        {
            return View();
        }


        [HttpGet("buy-work/{id}")]
        public async Task<IActionResult> Payment(int id)
        {
            var work = await _context.Sheets.Where(x => x.Id == id).FirstOrDefaultAsync();
            _mod = work.Url;
            TransactionInitializeRequest request = new()
            {
                AmountInKobo = work.Price * 100,
                Email = "gab@gmail.com",
                Reference = Generate().ToString(),
                Currency = "NGN",
                CallbackUrl = "http://localhost:26283/works/verify",
            };

            TransactionInitializeResponse response = PayStack.Transactions.Initialize(request);
            if (response.Status)
            {
                var transaction = new TransactionModel
                {
                    Name = work.Name,
                    Amount = work.Price,
                    //Email = work.Email
                    TrxRef = request.Reference
                };

                await _context.Transactions.AddAsync(transaction);
                await _context.SaveChangesAsync();
                return Redirect(response.Data.AuthorizationUrl);
            }
            ViewData["error"] = response.Message;
            return RedirectToAction("Payment");
        }



        [HttpGet]
        public async Task<IActionResult> Verify(string reference)
        {
            TransactionVerifyResponse response = PayStack.Transactions.Verify(reference);
            if(response.Data.Status == "success")
            {
                var transaction = _context.Transactions.Where(x => x.TrxRef == reference).FirstOrDefault();
                if (transaction != null)
                {
                    transaction.Status = true;
                    _context.Transactions.Update(transaction);
                    await _context.SaveChangesAsync();
                //return Redirect("https://drive.google.com/file/d/1qALHqXVEt2MTjB0XbJy5-B0wq3-RsT1C/view?ts=61a99582");
                //https://drive.google.com/u/0/uc?id=1qALHqXVEt2MTjB0XbJy5-B0wq3-RsT1C&export=download
                    return Redirect(_mod);
                }
            }
            ViewData["error"] = response.Data.GatewayResponse;
            return View();
        }




        private static int Generate()
        {
            Random rand = new ((int)DateTime.Now.Ticks);
            return rand.Next(100000000, 999999999);
        }
    }
}
