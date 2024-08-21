using DeptEmailSender.Models;
using DeptEmailSender.Services;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace DeptEmailSender.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IEmailSender emailSender;

        public HomeController(ILogger<HomeController> logger, IEmailSender emailSender)
        {
            _logger = logger;
            this.emailSender = emailSender;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

		[HttpPost]
		[Route("SendMailWithAttachment")]
		public bool SendMailWithAttachment([FromForm] MailDataWithAttachment mailDataWithAttachment)
		{
			return _mailService.SendMailWithAttachment(mailDataWithAttachment);
		}
	}
}