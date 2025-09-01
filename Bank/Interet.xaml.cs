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
    /// Interaction logic for Interet.xaml
    /// </summary>
    public partial class Interet : Window
    {
        BankEntities2 mybdd = new BankEntities2();
        public Interet()
        {
            InitializeComponent();
            dtgInteret.DataContext = (from c in mybdd.Compte_Bancaire
                                      where c.Type_de_Compte == "Epargne"
                                      select c).ToList();
        }

        private void btnPayer_Click(object sender, RoutedEventArgs e)
        {
            var query = from c in mybdd.Compte_Bancaire
                         where c.Type_de_Compte == "Epargne"
                         select c;

            foreach(var c in query)
            {
                c.Montant += c.Montant * (decimal?)0.01;
            }
            try
            {
                mybdd.SaveChanges();
                MessageBox.Show("Transaction effuctuer avec succes");
                dtgInteret.DataContext = (from c in mybdd.Compte_Bancaire
                                          where c.Type_de_Compte == "Epargne"
                                          select c).ToList();
            }
            catch (Exception ex) {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
