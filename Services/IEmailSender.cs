using System.Threading.Tasks;

namespace DeptEmailSender.Services
{
	public interface IEmailSender
	{
		public Task SendEmailAsync(string email, string subject, string message);
		bool SendMail(MailData mailData);
		bool SendHTMLMail(HTMLMailData htmlMailData);
		bool SendMailWithAttachment(MailDataWithAttachment mailData);

	}
}
