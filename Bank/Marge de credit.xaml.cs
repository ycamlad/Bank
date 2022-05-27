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
    /// Interaction logic for Marge_de_credit.xaml
    /// </summary>
    public partial class Marge_de_credit : Window
    {
        BankEntities2 mybdd = new BankEntities2();
        public Marge_de_credit()
        {
            InitializeComponent();
            dtgAugmenter.DataContext = (from c in mybdd.Compte_Bancaire
                                      where c.Type_de_Compte == "Marge de credit"
                                      select c).ToList();
        }

        private void btnAugmenter_Click(object sender, RoutedEventArgs e)
        {
            var query = from c in mybdd.Compte_Bancaire
                        where c.Type_de_Compte == "Marge de credit"
                        select c;

            foreach (Compte_Bancaire c in query)
            {

                c.Montant += c.Montant * (decimal?)0.05;

            }
            try
            {

                mybdd.SaveChanges();
                MessageBox.Show("Transaction effuctuer avec succes");
                dtgAugmenter.DataContext = (from c in mybdd.Compte_Bancaire
                                          where c.Type_de_Compte == "Marge de credit"
                                          select c).ToList();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }
    }
}
