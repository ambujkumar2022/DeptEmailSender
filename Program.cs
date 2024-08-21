using System.Net;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Mail;
using System.Net;
public class Program {

    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddControllersWithViews();

        var app = builder.Build();

		var client = new SmtpClient("sandbox.smtp.mailtrap.io", 2525)
		{
			Credentials = new NetworkCredential("your_username", "your_password"),
			EnableSsl = true
		};
		client.Send("from@example.com", "to@example.com", "Hello world", "testbody");
		Console.WriteLine("Sent");
		Console.ReadLine();
		// Configure the HTTP request pipeline.
		if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Home/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();

        app.UseRouting();

        app.UseAuthorization();

        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}");

        app.Run();
    }
}