using System.ComponentModel;
using System.Runtime.CompilerServices;
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
using ClosedXML.Excel;
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

            if (now.Hour >= 6 && now.Hour < 12)
            {
                greeting = "Guten Morgen, ";
            }
            if (now.Hour >= 12 && now.Hour < 18)
            {
                greeting = "Guten Mittag, ";
            }
            if (now.Hour >= 18 || now.Hour < 4)
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
            if (currentUser.Rolle != "Admin")
            {
                DgNutzer.IsReadOnly = true;
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
            else
            {
                DisplayViewNutzer.Filter = x =>
                {
                    Benutzer? benutzer = x as Benutzer;
                    return benutzer != null && benutzer.Rolle != "Admin";
                };
            }
        }

        private void Profile_Click(object sender, MouseButtonEventArgs e)
        {
            UserInformation userInformation = new UserInformation(currentUser, this, ctx);

            userInformation.Show();

            InitGreeting();
        }

        private void Button_ClickSave(object sender, RoutedEventArgs e)
        {
            try
            {
                ctx.SaveChanges();
                MessageBox.Show("Änderungen erfolgreich gespeichert", "Save successful", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch
            {
                MessageBox.Show("Fehler beim Speichern. Bitte Ihre Daten überprüfen und erneut versuchen.", "Save Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Button_ClickDelSelectedItem(object sender, RoutedEventArgs e)
        {
            foreach (TabItem TabItem in Tabs.Items)
            {
                if (TabItem.IsSelected)
                {
                    if (TabItem.Name.Contains("Lager"))
                    {
                        try
                        {
                            ctx.Remove(DgLager.SelectedItem);
                        }
                        catch
                        {
                            MessageBox.Show("Bitte wählen Sie einen gültigen Datensatz an","Data Error",MessageBoxButton.OK, MessageBoxImage.Error);
                            return;
                        }
                    }
                    else if (TabItem.Name.Contains("Waren"))
                    {
                        try
                        {
                            ctx.Remove(DgWaren.SelectedItem);
                        }
                        catch
                        {
                            MessageBox.Show("Bitte wählen Sie einen gültigen Datensatz an", "Data Error", MessageBoxButton.OK, MessageBoxImage.Error);
                            return;
                        }
                    }
                    else if (TabItem.Name.Contains("Nutzer"))
                    {
                        try
                        {
                            Benutzer b = (Benutzer)DgNutzer.SelectedItem;
                            if (currentUser.Email == b.Email)
                            {
                                MessageBox.Show("Bitte versuchen Sie nicht, Ihren eigenen Benutzer zu löschen", "Del Own User Error", MessageBoxButton.OK, MessageBoxImage.Error);
                                return;

                            }
                            ctx.Remove(DgNutzer.SelectedItem);
                        }
                        catch
                        {
                            MessageBox.Show("Bitte wählen Sie einen gültigen Datensatz an", "Data Error", MessageBoxButton.OK, MessageBoxImage.Error);
                            return;
                        }                    
                    }
                    else if (TabItem.Name.Contains("Liefer"))
                    {
                        try
                        {
                            ctx.Remove(DgLiefer.SelectedItem);
                        }
                        catch
                        {
                            MessageBox.Show("Bitte wählen Sie einen gültigen Datensatz an", "Data Error", MessageBoxButton.OK, MessageBoxImage.Error);
                            return;
                        }
                    }
                    MessageBox.Show("Löschen des ausgewählten Datensatzes erfolgreich", "Deletion successful", MessageBoxButton.OK, MessageBoxImage.Error);
                    ctx.SaveChanges();
                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ExportDataToExcel();
        }

        private void ExportDataToExcel()
        {
            var datei = new XLWorkbook();
            IXLWorksheet blatt;
            string dateiName = "";

            if (TabLager.IsSelected)
            {
                dateiName = "Lagerdaten";

                blatt = datei.Worksheets.Add("Lager");

                blatt.Cell(1, 1).Value = "Lagername";
                blatt.Cell(1, 2).Value = "Standort";
                blatt.Cell(1, 3).Value = "Bestand";

                var lagerZeilen = ctx.Lagers.Local.ToObservableCollection();

                if (lagerZeilen.Count<= 0) 
                {
                    MessageBox.Show("Keine Daten zum Exportieren vorhanden.", "Export Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                for (int i = 0; i < lagerZeilen.Count; i++)
                {
                    blatt.Cell(i + 2, 1).Value = lagerZeilen[i].Lagername;
                    blatt.Cell(i + 2, 2).Value = lagerZeilen[i].Standort;
                    blatt.Cell(i + 2, 3).Value = lagerZeilen[i].Bestand;
                }
            }
            else if (TabWaren.IsSelected)
            {
                dateiName = "Warendaten";

                blatt = datei.Worksheets.Add("Waren");

                blatt.Cell(1, 1).Value = "Warennamen";
                blatt.Cell(1, 2).Value = "Warentyp";

                var warenZeilen = ctx.Warens.Local.ToObservableCollection();

                if (warenZeilen.Count <= 0)
                {
                    MessageBox.Show("Keine Daten zum Exportieren vorhanden.", "Export Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                for (int i = 0; i < warenZeilen.Count; i++)
                {
                    blatt.Cell(i + 2, 1).Value = warenZeilen[i].Warennamen;
                    blatt.Cell(i + 2, 2).Value = warenZeilen[i].Warentyp;
                }
            }
            else if (TabNutzer.IsSelected)
            {
                dateiName = "Nutzerdaten";

                blatt = datei.Worksheets.Add("Nutzer");

                blatt.Cell(1, 1).Value = "Rolle";
                blatt.Cell(1, 2).Value = "Name";
                blatt.Cell(1, 3).Value = "Adresse";
                blatt.Cell(1, 4).Value = "Telefon";
                blatt.Cell(1, 5).Value = "Email";

                var nutzerZeilen = ctx.Benutzers.Local.ToObservableCollection();

                if (nutzerZeilen.Count <= 0)
                {
                    MessageBox.Show("Keine Daten zum Exportieren vorhanden.", "Export Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                for (int i = 0; i < nutzerZeilen.Count; i++)
                {
                    blatt.Cell(i + 2, 1).Value = nutzerZeilen[i].Rolle;
                    blatt.Cell(i + 2, 2).Value = nutzerZeilen[i].Name;
                    blatt.Cell(i + 2, 3).Value = nutzerZeilen[i].Adresse;
                    blatt.Cell(i + 2, 4).Value = nutzerZeilen[i].Telefon;
                    blatt.Cell(i + 2, 5).Value = nutzerZeilen[i].Email;
                }
            }
            else if (TabLiefer.IsSelected)
            {
                dateiName = "Lieferantendaten";

                blatt = datei.Worksheets.Add("Lieferanten");

                blatt.Cell(1, 1).Value = "Name";
                blatt.Cell(1, 2).Value = "Adresse";
                blatt.Cell(1, 3).Value = "Telefon";

                var lieferentenZeilen = ctx.Lieferants.Local.ToObservableCollection();

                if (lieferentenZeilen.Count <= 0)
                {
                    MessageBox.Show("Keine Daten zum Exportieren vorhanden.", "Export Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                for (int i = 0; i < lieferentenZeilen.Count; i++)
                {
                    blatt.Cell(i + 2, 1).Value = lieferentenZeilen[i].Name;
                    blatt.Cell(i + 2, 2).Value = lieferentenZeilen[i].Adresse;
                    blatt.Cell(i + 2, 3).Value = lieferentenZeilen[i].Telefon;
                }
            }

            DateTime now = DateTime.Now;
            dateiName += $"_{now.Day}-{now.Month}-{now.Year}_{now.Hour}-{now.Minute}-{now.Second}";

            try
            {
                datei.SaveAs($"../../../../{dateiName}.xlsx");
            }
            catch (Exception e)
            {
                MessageBox.Show($"Fehler beim Exportieren der Daten: " + e.ToString().Substring(0, 50), "Export Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            MessageBox.Show($"Daten erfolgreich als \"{dateiName}.xlsx\" gespeichert!", "Export successful", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}