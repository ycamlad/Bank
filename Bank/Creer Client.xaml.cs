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

                Client client = new Client();
                client.Code_Client = txtCodeClient.Text;
                client.Nom = txtNom.Text;
                client.Prenom = txtPrenom.Text;
                client.Telephone = txtTelephone.Text;
                client.Courriel = txtCourriel.Text;
                client.Nip = txtNip.Text;
                client.Acces = "debloquer";
                mybdd.Clients.Add(client);
                try
                {

                    mybdd.SaveChanges();


                    lstAfficher.ItemsSource = mybdd.Clients.ToList();

                    MessageBox.Show("succes!");

                }

                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);


                }
            }

        }

    }
}
