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
using StockControl.Models;

namespace StockControl
{
    /// <summary>
    /// Interaction logic for LogIn.xaml
    /// </summary>
    public partial class LogIn : Window
    {
        StockControlContext ctx = new StockControlContext();
        public LogIn()
        {
            InitializeComponent();

            if (!DatabaseIsConnected())
            {
                MessageBox.Show("Fehler bei der Datenbankverbindung. Eventuell müssen Sie den Connection-String überprüfen.", "Database Error", MessageBoxButton.OK, MessageBoxImage.Error);
                this.Close();
                return;
            }
        }

        private bool DatabaseIsConnected()
        {
            try
            {
                Benutzer b = ctx.Benutzers.First();
            }
            catch
            {
                return false;
            }
            return true;
        }

        private void Register_Click(object sender, RoutedEventArgs e)
        {
            if (TbName.Text == "" || TbAd.Text == "" || TbTel.Text == "" || TbMail.Text == "" || TbPw.Password == "")
            {
                MessageBox.Show("Es dürfen keine Felder leer sind, um sich zu registrieren. Bitte erneut versuchen.", "Register Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            //Neue Nutzer werden immer der Rolle "Mitarbeiter" zugewiesen, ein Admin kann die Rollen in der Nutzerverwaltungsballe ändern
            Benutzer b = new()
            {
                Rolle = "Mitarbeiter",
                Name = TbName.Text,
                Adresse = TbAd.Text,
                Telefon = TbTel.Text,
                Email = TbMail.Text,
                Passwort = TbPw.Password
            };

            Benutzer? testUser = ctx.Benutzers.FirstOrDefault(b => b.Email == TbMail.Text);

            //Überprüfung, ob der testUser nach dem filtern der E-Mail existert. 
            if (testUser == null) 
            {
                ctx.Benutzers.Add(b);
                ctx.SaveChanges();

                MainWindow mw = new(b);

                MessageBox.Show("Registrierung erfolgreich. StockControl wird nun gestartet.", "Register successful", MessageBoxButton.OK, MessageBoxImage.Information);

                mw.Show();
                this.Close();
                return;
            }
            //Sollte er existieren, so ist ein Nutzer mit dieser E-Mail bereits vorhanden und es wird eine Fehlermeldung ausgegeben
            MessageBox.Show("User existiert bereits. Bitte einloggen.", "Login Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void Login_Click(object sender, RoutedEventArgs e)
        {
            if (TbMail.Text == "")
            {
                MessageBox.Show("E-Mail Feld ist leer. Bitte eine E-Mail angeben.", "Login Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            Benutzer? b = ctx.Benutzers.FirstOrDefault(x => x.Email == TbMail.Text);

            if(b == null)
            {
                MessageBox.Show("Login fehlgeschlagen. Bitte erneut versuchen und bei mehrmaligem Misslingen neu registrieren", "Login Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (b.Passwort == TbPw.Password) 
            {
                MainWindow mw = new(b);

                MessageBox.Show("Login erfolgreich. StockControl wird nun gestartet.", "Login successful", MessageBoxButton.OK, MessageBoxImage.Information);
                mw.Show();
                this.Close();
                return;
            }
            else
            {
                MessageBox.Show("Login fehlgeschlagen. Falsches Passwort.", "Login Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void KeyDown_Enter(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                Login_Click(null, null);
            }
        }
    }
}
