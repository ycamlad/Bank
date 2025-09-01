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

namespace Bank
{
    /// <summary>
    /// Interaction logic for Admin.xaml
    /// </summary>
    public partial class Admin : Window
    {
        BankEntities2 mybdd = new BankEntities2();
        public Admin()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var creer_Client = new Creer_Client();
            creer_Client.ShowDialog();
        }

        private void btnCreerCompte_Click(object sender, RoutedEventArgs e)
        {
            var creer_ = new Creer_Compte();
            creer_.ShowDialog();
        }

        private void btnFermerGuichet_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void btnAjouterComptant_Click(object sender, RoutedEventArgs e)
        {
            var ajouter_Comptant = new ajouter_comptant();

            ajouter_Comptant.ShowDialog(); ;
        }

        private void btnAfficherCompte_Click(object sender, RoutedEventArgs e)
        {
           var afficher_Compte = new Afficher_Compte();
             afficher_Compte.ShowDialog();                       
        }

        private void btnAcces_Click(object sender, RoutedEventArgs e)
        {
            var acces = new Acces();
            acces.ShowDialog();
        }

        private void btnPayerInteret_Click(object sender, RoutedEventArgs e)
        {
            var interet = new Interet();
            interet.ShowDialog();
        }

        private void btnPrelevement_Click(object sender, RoutedEventArgs e)
        {
            var hypotheque = new Hypotheque();
            hypotheque.ShowDialog();
        }

        private void btnAugmenter_Click(object sender, RoutedEventArgs e)
        {
            var query = from c in mybdd.Compte_Bancaire
                        where c.Type_de_Compte == "Marge de credit"
                        select c;

            foreach (var c in query)
            {
                if (c.Montant < 0)
                {
                    c.Montant -= c.Montant * (decimal?)0.05;
                }
                else
                {
                    c.Montant += c.Montant * (decimal?)0.05;
                }

            }
            try
            {
                mybdd.SaveChanges();
                MessageBox.Show("Augmenter avec succès");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnQuitter_Click(object sender, RoutedEventArgs e)
        {
            var mainWindow = new MainWindow
            {
                Visibility = Visibility.Visible
            };
            Application.Current.Properties.Remove("code");
            this.Close();
        }
    }
}
