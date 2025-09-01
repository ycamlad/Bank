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
    /// Interaction logic for Creer_Compte.xaml
    /// </summary>
    public partial class Creer_Compte : Window
    {
        BankEntities2 mybdd = new BankEntities2();
        public Creer_Compte()
        {
            InitializeComponent();
            dtgafficher.DataContext = (from g in mybdd.Compte_Bancaire
                                       join m in mybdd.Clients on g.CClient equals m.Code_Client

                                       select new
                                       {
                                           type = g.Type_de_Compte,
                                           Code = g.CClient,
                                           No_compte = g.No_Compte,
                                           Montant = g.Montant,
                                       }).ToList();
        }

        private void btnCreer_Click(object sender, RoutedEventArgs e)
        {
            var test2 = mybdd.Compte_Bancaire.FirstOrDefault(u => u.Type_de_Compte == "Marge de credit"&&u.CClient==txtCode.Text );
            
            if (txtnoCompte.Text == string.Empty || txtCode.Text == string.Empty || cboCompte.Text == string.Empty)
            {
                MessageBox.Show("Remplir tous les champs, merci");
            }

            else if (test2!=null)
            {
                MessageBox.Show("Le client a deja un Compte Marge de credit");
            }
            else
            {
                var compte = new Compte_Bancaire();
                compte.CClient = txtCode.Text;
                compte.No_Compte = txtnoCompte.Text;
                compte.Type_de_Compte = cboCompte.Text;
                compte.Montant = 0;
                mybdd.Compte_Bancaire.Add(compte);
                try
                {
                    mybdd.SaveChanges();

                    dtgafficher.ItemsSource = (from g in mybdd.Compte_Bancaire
                                               join m in mybdd.Clients on g.CClient equals m.Code_Client

                                               select new
                                               {
                                                   Client = g.CClient,
                                                   type = g.Type_de_Compte,
                                                   No_compte = g.No_Compte,
                                                   Montant = g.Montant,
                                               }).ToList();

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

