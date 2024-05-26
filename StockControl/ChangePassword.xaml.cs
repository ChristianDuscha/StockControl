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
        StockControlContext ctx;
        public ChangePassword(Benutzer benutzerNeuesPW, StockControlContext context)
        {
            InitializeComponent();
            user = benutzerNeuesPW;
            this.ctx = context;
        }

        private void Button_ClickChangePasswort(object sender, RoutedEventArgs e)
        {
            if(PwCurr.Password == "" || PwN1.Password == "" || PwN2.Password == "")
            {
                MessageBox.Show("Kein Feld darf leer sein. Bitte erneut versuchen", "Empty Field Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            Benutzer? currentUser = ctx.Benutzers.FirstOrDefault(x => x.Email == user.Email);
            if(currentUser == null)
            {
                MessageBox.Show("Fehler beim Suchen Ihres Benutzers. Bitte erneut versuchen", "Database Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (currentUser.Passwort == PwCurr.Password)
            {
                if (PwN1.Password == PwN2.Password) 
                {
                    currentUser.Passwort = PwN1.Password;
                    MessageBox.Show("Passwort erfolgreich geändert.", "Password successfully changed", MessageBoxButton.OK, MessageBoxImage.Information);
                    ctx.SaveChanges();
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
