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
    /// Interaction logic for Hypotheque.xaml
    /// </summary>
    public partial class Hypotheque : Window
    {
        BankEntities2 mybdd = new BankEntities2();
        public Hypotheque()
        {
            InitializeComponent();
            cboHypotheque.DataContext = mybdd.Compte_Bancaire.ToList();
            cboHypotheque.DisplayMemberPath = "CClient";
            dtgMarge.DisplayMemberPath = "CClient";
            dtgHypotheque.DisplayMemberPath = "CClient";

        }

        private void cboHypotheque_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var _Bancaire = cboHypotheque.SelectedItem as Compte_Bancaire;
            dtgHypotheque.DataContext = (from c in mybdd.Compte_Bancaire
                                         where c.Type_de_Compte == "Hypotheque" 
                                         select c).ToList();

            dtgMarge.DataContext = (from c in mybdd.Compte_Bancaire
                                    where c.Type_de_Compte == "Marge de credit" 
                                    select c).ToList();
        }

        private void btnPrelever_Click(object sender, RoutedEventArgs e)
        {
            var Bancaire = dtgHypotheque.SelectedItem as Compte_Bancaire;
            var verfif = mybdd.Compte_Bancaire.FirstOrDefault(u => u.Type_de_Compte == "Marge de credit" && u.CClient==Bancaire.CClient);
            var query = from c in mybdd.Compte_Bancaire
                        where c.Type_de_Compte == "Marge de credit" && c.CClient==Bancaire.CClient
                        select c;

            if (txtMontant.Text == string.Empty)
            {
                MessageBox.Show("Veuillez saisir un montant");
            }
            else
            {
                if (decimal.Parse(txtMontant.Text) > Bancaire.Montant)
                {
                    if (verfif == null)
                    {
                        MessageBox.Show("Transaction refuse", "Erreur");
                    }
                    else
                    {
                        var diff = decimal.Parse(txtMontant.Text) - Bancaire.Montant;
                        foreach (var c in query)
                        {
                            Bancaire.Montant = Bancaire.Montant - decimal.Parse(txtMontant.Text);
                            c.Montant += diff;
                        }
                        try
                        {
                            mybdd.SaveChanges();
                            MessageBox.Show("Transaction effctuer avec succes");
                            var _Bancaire = cboHypotheque.SelectedItem as Compte_Bancaire;
                            dtgHypotheque.DataContext = (from c in mybdd.Compte_Bancaire
                                                         where c.Type_de_Compte == "Hypotheque"
                                                         select c).ToList();
                            dtgMarge.DataContext = (from c in mybdd.Compte_Bancaire
                                                    where c.Type_de_Compte == "Marge de credit"
                                                    select c).ToList();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message);
                        }
                    }
                }
                else
                {
                    Bancaire.Montant -= decimal.Parse(txtMontant.Text);
                    try
                    {
                        mybdd.SaveChanges();
                        MessageBox.Show("Transaction effctuer avec succes");
                        var _Bancaire = cboHypotheque.SelectedItem as Compte_Bancaire;
                        dtgHypotheque.DataContext = (from c in mybdd.Compte_Bancaire
                                                     where c.Type_de_Compte == "Hypotheque" 
                                                     select c).ToList();

                        dtgMarge.DataContext = (from c in mybdd.Compte_Bancaire
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
    }
}
