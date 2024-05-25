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
            InitAdminView();
            InitGreeting();
        }

        private void InitGreeting()
        {
            DateTime now = DateTime.Now;

            string greeting = "";

            if (now.Hour >= 5 && now.Hour < 12)
            {
                greeting = "Guten Morgen, ";
            }
            if (now.Hour >= 12 && now.Hour < 18)
            {
                greeting = "Guten Mittag, ";
            }
            if (now.Hour >= 18 && now.Hour < 5)
            {
                greeting = "Guten Abend, ";
            }

            greeting += currentUser.Name;

            LbGreetingLager.Content = greeting;
            LbGreetingWaren.Content = greeting;
            LbGreetingNutzer.Content = greeting;
            LbGreetingLiefer.Content = greeting;
        }

        private void InitAdminView()
        {
            if(currentUser.Rolle != "Admin")
            {
                DgLager.IsReadOnly = true;
                DgWaren.IsReadOnly = true;
                DgNutzer.IsReadOnly = true;
                DgLiefer.IsReadOnly = true;
                return;
            }

            GridLager.RowDefinitions.Add(new RowDefinition());
            GridWaren.RowDefinitions.Add(new RowDefinition());
            GridNutzer.RowDefinitions.Add(new RowDefinition());
            GridLiefer.RowDefinitions.Add(new RowDefinition());
        }

        private void InitDataContexts()
        {
            ctx.Lagers.Load();
            DisplayViewLager = CollectionViewSource.GetDefaultView(ctx.Lagers.Local.ToObservableCollection());
            DgLager.DataContext = DisplayViewLager;

            ctx.Warens.Include(x=> x.LieferantenWares).Load();
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

        private void Button_ClickDelSelectedItem(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(this.Height.ToString());
            foreach (TabItem TabItem in Tabs.Items)
            {
                if (TabItem.IsSelected)
                {
                    if (TabItem.Name.Contains("Lager"))
                    {
                        ctx.Remove(DgLager.SelectedItem);
                    }
                    else if (TabItem.Name.Contains("Waren"))
                    {
                        ctx.Remove(DgWaren.SelectedItem);
                    }
                    else if (TabItem.Name.Contains("Nutzer"))
                    {
                        ctx.Remove(DgNutzer.SelectedItem);
                    }
                    else  if (TabItem.Name.Contains("Liefer"))
                    {
                        ctx.Remove(DgLiefer.SelectedItem);
                    }

                    ctx.SaveChanges();
                    return;
                }
            }
        }
    }
}