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
    /// Interaction logic for Afficher_Compte.xaml
    /// </summary>
    public partial class Afficher_Compte : Window
    {
        BankEntities2 mybdd = new BankEntities2();
        public Afficher_Compte()
        {
            InitializeComponent();
            dtgAfficher.DataContext = (from c in mybdd.Transactions
                                       select c).ToList();
        }

        private void btnQuitter_Click(object sender, RoutedEventArgs e)
        {
            Close();

        }

       
    }
}
