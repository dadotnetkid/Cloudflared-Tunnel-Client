using Microsoft.Extensions.Configuration;
using System.Diagnostics;

namespace Client.WinApp
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.


            var builder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            Configuration = builder.Build();


            ApplicationConfiguration.Initialize();

            Application.ApplicationExit += Application_ApplicationExit;

            Application.Run(new Form1());
        }

        private static void Application_ApplicationExit(object? sender, EventArgs e)
        {
            foreach (var process in Process.GetProcessesByName("cloudflared"))
            {
                process.Kill();
            }
        }

        public static IConfigurationRoot Configuration { get; set; }
    }
}