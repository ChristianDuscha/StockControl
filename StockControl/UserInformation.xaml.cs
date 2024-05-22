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
        StockControlContext context;
        public UserInformation(Benutzer selectedUser, MainWindow mw, StockControlContext ctx)
        {
            InitializeComponent();
            currentUser = selectedUser;
            DataContext = currentUser;
            this.mw = mw;
            context = ctx;
        }

        private void Button_ClickChangePassword(object sender, RoutedEventArgs e)
        {
            ChangePassword cp = new ChangePassword(currentUser, context);
            cp.ShowDialog();
            context.SaveChanges();
        }

        private void Button_ClickLogout(object sender, RoutedEventArgs e)
        {
            LogIn logIn = new LogIn();

            this.Close();
            mw.Close();
            logIn.Show();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            context.SaveChanges();
        }
    }
}
