using Auto_Parts_Store.Repositories;
using Auto_Parts_Store.Services;
using System;
using System.Windows.Forms;
namespace Auto_Parts_Store
{
    internal static class Program
    {

        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            var repo     = new PartRepository();
            var settings = new Auto_Parts_Store.Repositories.SettingsRepository();
            var authService = new AuthService(repo, settings);
            var loginForm   = new frmLogin(authService);

            Application.Run(loginForm);
        }
    }
}
