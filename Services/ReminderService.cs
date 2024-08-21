using Microsoft.Extensions.Hosting;
using System;
using System.Threading;
using Microsoft.Extensions.DependencyInjection;
using DeptEmailSender.Data;
using Microsoft.EntityFrameworkCore;

namespace DeptEmailSender.Services
{
	public class ReminderService : IHostedService, IDisposable
	{
		private Timer _timer;
		private readonly IServiceProvider _serviceProvider;

		public ReminderService(IServiceProvider serviceProvider)
		{
			_serviceProvider = serviceProvider;
		}
		public Task StopAsync(CancellationToken cancellationToken)
		{
			_timer?.Change(Timeout.Infinite, 0);
			return Task.CompletedTask;
		}

		public void Dispose()
		{
			_timer?.Dispose();
		}
		public Task StartAsync(CancellationToken cancellationToken)
		{
			_timer = new Timer(SendReminders, null, TimeSpan.Zero, TimeSpan.FromMinutes(1));
			return Task.CompletedTask;
		}
		public async void SendReminders(object state)
		{
			using (var scope = _serviceProvider.CreateScope())
			{
				var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
				var emailSender = scope.ServiceProvider.GetRequiredService<IEmailSender>();

				var reminders = await context.Reminders
						 .Where(r => r.ReminderDateTime <= DateTime.Now && !r.IsEmailSent)
						 .ToListAsync();

                foreach (var reminder in reminders)
                {
					await emailSender.SendEmailAsync("recipient@example.com", reminder.RemTitle, "This is your reminder.");
					reminder.IsEmailSent = true;
					context.Update(reminder);
                }

				await context.SaveChangesAsync();
            }
		}
	}
}
