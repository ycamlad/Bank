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
    /// Interaction logic for Acces.xaml
    /// </summary>
    public partial class Acces : Window
    {
        BankEntities2 mybdd = new BankEntities2();
        public Acces()
        {
            InitializeComponent();
            dtgAcces.DisplayMemberPath = "Code_Client";
           
            cboAcces.DataContext = mybdd.Clients.ToList();
        }

        private void cboAcces_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Client client = cboAcces.SelectedItem as Client;
            dtgAcces.DataContext = (from p in mybdd.Clients
                                    select p).ToList();
        }

        private void btnBloquer_Click(object sender, RoutedEventArgs e)
        {
            Client client2 = dtgAcces.SelectedItem as Client;
            client2.Acces = "Bloquer";
            
            try
            {

                mybdd.SaveChanges();
                Client client = cboAcces.SelectedItem as Client;
                 dtgAcces.DataContext = (from p in mybdd.Clients
                                                               select p).ToList();

            }
            catch (Exception ex) {

                MessageBox.Show(ex.Message);
            
            }
        }

        private void btnDebloquer_Click(object sender, RoutedEventArgs e)
        {

            Client client2 = dtgAcces.SelectedItem as Client;
            client2.Acces = "Debloquer";
            
            try
            {

                mybdd.SaveChanges();
                Client client = cboAcces.SelectedItem as Client;
                dtgAcces.DataContext = (from p in mybdd.Clients
                                        select p).ToList();

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);

            }
        }
    }
}
