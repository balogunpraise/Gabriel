using Gabriel.Data;
using Gabriel.Models;
using Microsoft.AspNetCore.Mvc;
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


        [HttpPost]
        public async Task<IActionResult> Payment(PaymentViewModel model)
        {

            TransactionInitializeRequest request = new()
            {
                AmountInKobo = model.Price * 100,
                Email = model.Email,
                Reference = Generate().ToString(),
                Currency = "NGN",
                CallbackUrl = "http://localhost:26283/works/verify",
            };

            TransactionInitializeResponse response = PayStack.Transactions.Initialize(request);
            if (response.Status)
            {
                var transaction = new TransactionModel
                {
                    Name = model.Name,
                    Amount = model.Price,
                    Email = model.Email,
                    TrxRef = request.Reference

                };

                await _context.Transactions.AddAsync(transaction);
                await _context.SaveChangesAsync();
                return Redirect(response.Data.AuthorizationUrl);
            }
            ViewData["error"] = response.Message;

            return RedirectToAction("Payment");
        }




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
                    return Redirect("https://drive.google.com/file/d/1qALHqXVEt2MTjB0XbJy5-B0wq3-RsT1C/view?ts=61a99582");
                }
            }
            ViewData["error"] = response.Data.GatewayResponse;
            return View("");
        }




        private static int Generate()
        {
            Random rand = new ((int)DateTime.Now.Ticks);
            return rand.Next(100000000, 999999999);
        }
    }
}
