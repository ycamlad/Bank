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
    /// Interaction logic for Creer_Client.xaml
    /// </summary>
    public partial class Creer_Client : Window
    {
        BankEntities2 mybdd = new BankEntities2();
        public Creer_Client()
        {
            InitializeComponent();
            lstAfficher.ItemsSource = mybdd.Clients.ToList();

        }

        private void btnSauvegarder_Click(object sender, RoutedEventArgs e)
        {
            if (txtCodeClient.Text == string.Empty || txtNom.Text == string.Empty || txtPrenom.Text == string.Empty || txtTelephone.Text == string.Empty || txtCourriel.Text == string.Empty || txtNip.Text == string.Empty)
            {
                MessageBox.Show("Remplir tous les champs, Merci");
            }
            else
            {
                var client = new Client
                {
                    Code_Client = txtCodeClient.Text,
                    Nom = txtNom.Text,
                    Prenom = txtPrenom.Text,
                    Telephone = txtTelephone.Text,
                    Courriel = txtCourriel.Text,
                    Nip = txtNip.Text,
                    Acces = "debloquer"
                };
                mybdd.Clients.Add(client);

                try
                {
                    mybdd.SaveChanges();

                    lstAfficher.ItemsSource = mybdd.Clients.ToList();

                    MessageBox.Show("succès!");
                }

                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }

        }

    }
}
