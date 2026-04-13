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

            PartRepository repo = new PartRepository();
            AuthService authService = new AuthService(repo);
            frmLogin loginForm = new frmLogin(authService);

            Application.Run(loginForm);
        }
    }
}
