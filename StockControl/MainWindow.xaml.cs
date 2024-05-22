using System.ComponentModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.EntityFrameworkCore;
using StockControl.Models;

namespace StockControl
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ICollectionView DisplayViewLager;
        private ICollectionView DisplayViewWare;
        private ICollectionView DisplayViewNutzer;
        private ICollectionView DisplayViewLiefer;
        StockControlContext ctx = new StockControlContext();
        Benutzer currentUser;
        public MainWindow(Benutzer user)
        {
            InitializeComponent();
            currentUser = user;
            InitDataContexts();
        }

        private void InitDataContexts()
        {
            ctx.Lagers.Load();
            DisplayViewLager = CollectionViewSource.GetDefaultView(ctx.Lagers.Local.ToObservableCollection());
            DgLager.DataContext = DisplayViewLager;

            ctx.Warens.Load();
            DisplayViewWare = CollectionViewSource.GetDefaultView(ctx.Warens.Local.ToObservableCollection());
            DgWaren.DataContext = DisplayViewWare;

            ctx.Benutzers.Load();
            DisplayViewNutzer = CollectionViewSource.GetDefaultView(ctx.Benutzers.Local.ToObservableCollection());
            DgNutzer.DataContext = DisplayViewNutzer;

            ctx.Lieferants.Load();
            DisplayViewLiefer = CollectionViewSource.GetDefaultView(ctx.Lieferants.Local.ToObservableCollection());
            DgLiefer.DataContext = DisplayViewLiefer;

            FilterUsersByRole();
        }

        private void FilterUsersByRole()
        {
            if (currentUser.Rolle == "Admin")
            {
                DisplayViewNutzer.Filter = null;
            }
            else if (currentUser.Rolle == "Mitarbeiter")
            {
                DisplayViewNutzer.Filter = x =>
                {
                    Benutzer? benutzer = x as Benutzer;
                    return benutzer != null && benutzer.Rolle == "Mitarbeiter";
                };
            }
        }

        private void Profile_Click(object sender, MouseButtonEventArgs e)
        {
            UserInformation userInformation = new UserInformation(currentUser, this, ctx);

            userInformation.Show();
        }

        private void Button_ClickSave(object sender, RoutedEventArgs e)
        {
            ctx.SaveChanges();
        }
    }
}