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
    /// Interaction logic for ajouter_comptant.xaml
    /// </summary>
    public partial class ajouter_comptant : Window
    {
        decimal? _Montant;
        BankEntities2 mybdd = new BankEntities2();
        public ajouter_comptant()
        {
            InitializeComponent();
            lblComptant.DataContext = mybdd.Argent_Comptant.ToList();
        }

        private void btnAjouter_Click(object sender, RoutedEventArgs e)
        {
            //var argent = new Argent_Comptant();
            var query = (from c in mybdd.Argent_Comptant
                         select c);

            if (txtComptant.Text == string.Empty||decimal.Parse(txtComptant.Text)<=0)
            { MessageBox.Show("Veuillez saisir un montant"); }
            else { 
                foreach (var c in query)
                {
                    _Montant = c.Comptant;
                }

                if (_Montant == 20000)
                {
                    MessageBox.Show("Guichet remplit ");
                }
                else
                {
                    var limite = _Montant + decimal.Parse(txtComptant.Text);
                    //condition qui empeche de mettre plus de 20 000$ dans la machine
                    if (limite > 20000)
                    {
                        MessageBox.Show("Veuillez ajuster le montant");
                    }

                    else
                    {
                        foreach (var c in query)
                        {
                            c.Comptant += decimal.Parse(txtComptant.Text);
                        }


                        try
                        {
                            mybdd.SaveChanges();
                            lblComptant.DataContext = mybdd.Argent_Comptant.ToList();
                            MessageBox.Show("Montant ajouter avec succès");
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message);
                        }
                    }
                }
            }

        }
    }
} 

