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
    /// Interaction logic for UserInformation.xaml
    /// </summary>
    public partial class UserInformation : Window
    {
        MainWindow mw;
        Benutzer currentUser;
        StockControlContext ctx;
        public UserInformation(Benutzer selectedUser, MainWindow mw, StockControlContext context)
        {
            InitializeComponent();
            currentUser = selectedUser;
            DataContext = currentUser;
            this.mw = mw;
            this.ctx = context;
        }

        private void Button_ClickChangePassword(object sender, RoutedEventArgs e)
        {
            ChangePassword cp = new ChangePassword(currentUser, ctx);
            cp.ShowDialog();
            ctx.SaveChanges();
        }

        private void Button_ClickLogout(object sender, RoutedEventArgs e)
        {
            LogIn logIn = new LogIn();

            this.Close();
            mw.Close();
            logIn.Show();
        }

        private void Button_ClickSave(object sender, RoutedEventArgs e)
        {
            if(TbName.Text == "" || TbAd.Text == "" || TbTel.Text == "")
            {
                MessageBox.Show("Kein Feld darf leer sein. Bitte erneut versuchen", "Empty Field Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            Benutzer? b = ctx.Benutzers.FirstOrDefault(x => x.Email == TbMail.Text);
            b.Name = TbName.Text;
            b.Adresse = TbAd.Text;
            b.Telefon = TbTel.Text;

            MessageBox.Show("Daten erfolgreich geändert.", "Data successfully changed", MessageBoxButton.OK, MessageBoxImage.Information);

            ctx.SaveChanges();
        }
    }
}
