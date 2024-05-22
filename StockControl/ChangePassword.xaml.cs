using StockControl.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace StockControl
{
    /// <summary>
    /// Interaction logic for ChangePassword.xaml
    /// </summary>
    public partial class ChangePassword : Window
    {
        Benutzer user;
        StockControlContext context;
        public ChangePassword(Benutzer benutzerNeuesPW, StockControlContext ctx)
        {
            InitializeComponent();
            user = benutzerNeuesPW;
            context = ctx;
        }

        private void Button_ClickChangePasswort(object sender, RoutedEventArgs e)
        {
            if (user.Passwort == PwCurr.Password)
            {
                if (PwN1.Password == PWN2.Password) 
                {
                    user.Passwort = PwN1.Password;
                    MessageBox.Show("Passwort erfolgreich geändert.", "Password successfully changed", MessageBoxButton.OK, MessageBoxImage.Information);
                    context.SaveChanges();
                }
                else
                {
                    MessageBox.Show("Die neuen Passwörter unterscheiden sich. Bitte erneut versuchen", "Password Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("Das angegebene derzeitige Passwort ist falsch. Bitte erneut versuchen.", "Password Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
