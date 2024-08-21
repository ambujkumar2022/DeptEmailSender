using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DeptEmailSender.Data;
using DeptEmailSender.Models;
using DeptEmailSender.Services;
using System.Threading.Tasks;
using System.Linq;

namespace DeptEmailSender.Controllers
{
    public class ReminderController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IEmailSender _emailSender;
        public ReminderController(ApplicationDbContext context, IEmailSender emailSender )
        {
            _context = context;
            _emailSender = emailSender;
        }


        //Get -Reminders
        public async Task<IActionResult> Index()
        {
            return View(await _context.Reminders.ToListAsync());
        }
        //Get -Reminders/Create
        public IActionResult Create()
        {
            return View();
        }

        //POST :Reminders/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Title, ReminderDateTime")] Reminder reminder)
        {
            if(ModelState.IsValid)
            {
                _context.Add(reminder);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(reminder);
        }

        //Method to send emailNotifications
        public async Task<IActionResult> SendReminders()
        {
            var reminders = await _context.Reminders
                .Where(r => r.ReminderDateTime <= DateTime.Now && !r.IsEmailSent)
                .ToListAsync();

            foreach (var item in reminders)
            {
                await _emailSender.SendEmailAsync("recipient@example.com", item.RemTitle, "This is your reminder.");
                item.IsEmailSent = true;
                _context.Update(item);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
