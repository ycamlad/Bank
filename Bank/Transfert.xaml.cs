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
using JetBrains.Annotations;

namespace Bank
{
    /// <summary>
    /// Interaction logic for Transfert.xaml
    /// </summary>
    public partial class Transfert : Window
    {
        [UsedImplicitly] BankEntities2 _myBankEntities2 = new BankEntities2();
        
        public Transfert()
        {
            InitializeComponent();
            var code = Application.Current.Properties["code"].ToString();

            cboTransfert.DataContext = (from g in _myBankEntities2.Compte_Bancaire
                                        join m in _myBankEntities2.Clients on g.CClient equals m.Code_Client
                                        where m.Code_Client == code
                                        select m).ToList();


            dtgCheque.DisplayMemberPath = "Compte";
            dtgComptes.DisplayMemberPath = "Compte";
            cboTransfert.DisplayMemberPath = "Code_Client";

           ;
        }

        

        private void cboTransfert_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var client = cboTransfert.SelectedItem as Client;
            dtgCheque.DataContext = (from a in _myBankEntities2.Compte_Bancaire
                                     where a.CClient == client.Code_Client && a.Type_de_Compte == "Cheque"
                                     select a).ToList();
             dtgComptes.DataContext = (from a in _myBankEntities2.Compte_Bancaire
                                       where a.CClient == client.Code_Client && a.Type_de_Compte != "Cheque"
                                       select a).ToList();
        }
        

        private void btnTransfert_Click(object sender, RoutedEventArgs e)
        {
            if (txtTransfert.Text == string.Empty|| decimal.Parse(txtTransfert.Text) < 0)
            {
                MessageBox.Show("Veuillez saisir un montant");
            }
            else
            {
                var cheque = dtgCheque.SelectedItem as Compte_Bancaire;
            var compte = dtgComptes.SelectedItem as Compte_Bancaire;
            var tr = new Transaction();
            decimal? transfert = decimal.Parse(txtTransfert.Text);

                var diff = cheque.Montant - transfert;

                if (diff < 0 && compte != null && compte.Type_de_Compte == "Marge de credit")
                {
                    MessageBox.Show("Transaction refusée");
                }

                else if (compte.Type_de_Compte == "Marge de credit")
                {

                    tr.Depot = transfert;
                    tr.No_Compte = compte.No_Compte;
                    tr.CCLIENT = compte.CClient;
                    cheque.Montant -= transfert;
                    compte.Montant -= transfert;

                    _myBankEntities2.Transactions.Add(tr);
                    try
                    {
                        _myBankEntities2.SaveChanges();
                        MessageBox.Show("Transaction effectué avec succès");

                        var client = cboTransfert.SelectedItem as Client;
                        dtgCheque.DataContext = (from a in _myBankEntities2.Compte_Bancaire
                                                 where a.CClient == client.Code_Client && a.Type_de_Compte == "Cheque"
                                                 select a).ToList();

                        dtgComptes.DataContext = (from a in _myBankEntities2.Compte_Bancaire
                                                  where a.CClient == client.Code_Client && a.Type_de_Compte != "Cheque"
                                                  select a).ToList();


                    }
                    catch (Exception ex)
                    {

                        MessageBox.Show(ex.Message);
                    }
                }
                else
                {
                    if (diff < 0)
                    {
                        MessageBox.Show("Transaction refusée");
                    }
                    else
                    {
                        tr.Depot = transfert;
                        tr.No_Compte = compte.No_Compte;
                        tr.CCLIENT = compte.CClient;
                        cheque.Montant -= transfert;
                        compte.Montant += transfert;

                        if (cheque.Montant < transfert)
                        {
                            MessageBox.Show("Fond insuffisant");

                        }
                        else
                        {
                            _myBankEntities2.Transactions.Add(tr);

                            try
                            {
                                _myBankEntities2.SaveChanges();
                                MessageBox.Show("Transaction effectué avec succès");

                                var client = cboTransfert.SelectedItem as Client;
                                dtgCheque.DataContext = (from a in _myBankEntities2.Compte_Bancaire
                                                         where a.CClient == client.Code_Client && a.Type_de_Compte == "Cheque"
                                                         select a).ToList();

                                dtgComptes.DataContext = (from a in _myBankEntities2.Compte_Bancaire
                                                          where a.CClient == client.Code_Client && a.Type_de_Compte != "Cheque"
                                                          select a).ToList();


                            }
                            catch (Exception ex)
                            {

                                MessageBox.Show(ex.Message);
                            }
                        }
                    }
                }
            }    }

        private void btnQuitter_Click(object sender, RoutedEventArgs e)
        {
            var compte = new Compte();
            compte.dtgAfficher.Items.Refresh();
            
            this.Close();
        }
    }
}

