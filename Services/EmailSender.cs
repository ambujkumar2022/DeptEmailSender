using System.Net;
using System.Net.Mail;
using System.Reflection;
using System.Threading.Tasks;

namespace DeptEmailSender.Services
{
	public class EmailSender :IEmailSender
	{
		public async Task SendEmailAsync(string email, string subject, string message)
		{
			var client = new SmtpClient("smtp.example.com")
			{
				UseDefaultCredentials = false,
			    Credentials = new NetworkCredential("ambujkumar@gmail.com","Ambuj@8380"),
				EnableSsl = true,
			    Port = 587
			};

			var mailMessag = new MailMessage
			{
				From = new MailAddress("ambujkumar@gmail.com"),
				Subject = subject,
				Body = message,
				IsBodyHtml = true,
			};

			mailMessag.To.Add(email);

			await client.SendMailAsync(mailMessag);
			
		}
		public bool SendMailWithAttachment(MailDataWithAttachment mailData)
		{
			try
			{
				using (MimeMessage emailMessage = new MimeMessage())
				{
					MailboxAddress emailFrom = new MailboxAddress(_mailSettings.SenderName, _mailSettings.SenderEmail);
					emailMessage.From.Add(emailFrom);
					MailboxAddress emailTo = new MailboxAddress(mailData.EmailToName, mailData.EmailToId);
					emailMessage.To.Add(emailTo);

					// you can add the CCs and BCCs here.
					//emailMessage.Cc.Add(new MailboxAddress("Cc Receiver", "cc@example.com"));
					//emailMessage.Bcc.Add(new MailboxAddress("Bcc Receiver", "bcc@example.com"));

					emailMessage.Subject = mailData.EmailSubject;

					BodyBuilder emailBodyBuilder = new BodyBuilder();
					emailBodyBuilder.TextBody = mailData.EmailBody;

					if (mailData.EmailAttachments != null)
					{
						foreach (var attachmentFile in mailData.EmailAttachments)
						{
							if (attachmentFile.Length == 0)
							{
								continue;
							}

							using (MemoryStream memoryStream = new MemoryStream())
							{
								attachmentFile.CopyTo(memoryStream);
								var attachmentFileByteArray = memoryStream.ToArray();

								emailBodyBuilder.Attachments.Add(attachmentFile.FileName, attachmentFileByteArray, ContentType.Parse(attachmentFile.ContentType));
							}
						}
					}

					emailMessage.Body = emailBodyBuilder.ToMessageBody();
					//this is the SmtpClient from the Mailkit.Net.Smtp namespace, not the System.Net.Mail one
					using (SmtpClient mailClient = new SmtpClient())
					{
						mailClient.Connect(_mailSettings.Server, _mailSettings.Port, MailKit.Security.SecureSocketOptions.StartTls);
						mailClient.Authenticate(_mailSettings.UserName, _mailSettings.Password);
						mailClient.Send(emailMessage);
						mailClient.Disconnect(true);
					}
				}

				return true;
			}
			catch (Exception ex)
			{
				// Exception Details
				return false;
			}
		}
	}
}
