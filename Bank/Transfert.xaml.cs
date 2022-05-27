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
    /// Interaction logic for Transfert.xaml
    /// </summary>
    public partial class Transfert : Window
    {
        BankEntities2 mybdd = new BankEntities2();
        
        public Transfert()
        {
            InitializeComponent();
            string code = Application.Current.Properties["code"].ToString();

            cboTransfert.DataContext = (from g in mybdd.Compte_Bancaire
                                        join m in mybdd.Clients on g.CClient equals m.Code_Client
                                        where m.Code_Client == code
                                        select m).ToList();


            dtgCheque.DisplayMemberPath = "Compte";
            dtgComptes.DisplayMemberPath = "Compte";
            cboTransfert.DisplayMemberPath = "Code_Client";

           ;
        }

        

        private void cboTransfert_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Client client = cboTransfert.SelectedItem as Client;
            dtgCheque.DataContext = (from a in mybdd.Compte_Bancaire
                                     where a.CClient == client.Code_Client && a.Type_de_Compte == "Cheque"
                                     select a).ToList();
             dtgComptes.DataContext = (from a in mybdd.Compte_Bancaire
                                       where a.CClient == client.Code_Client && a.Type_de_Compte != "Cheque"
                                       select a).ToList();
        }
        

        private void btnTransfert_Click(object sender, RoutedEventArgs e)
        {
            if (txtTransfert.Text == string.Empty|| decimal.Parse(txtTransfert.Text) < 0)
            {
                MessageBox.Show("Veuiller saisir un montant");
            }
            else
            {
                Compte_Bancaire _cheque = dtgCheque.SelectedItem as Compte_Bancaire;
            Compte_Bancaire _compte = dtgComptes.SelectedItem as Compte_Bancaire;
            Transaction tr = new Transaction();
            decimal? transfert = decimal.Parse(txtTransfert.Text);

                decimal? diff = _cheque.Montant - transfert;

                if (diff < 0 && _compte.Type_de_Compte == "Marge de credit")
                {
                    MessageBox.Show("Transaction refusée");
                }

                else if (_compte.Type_de_Compte == "Marge de credit")
                {

                    tr.Depot = transfert;
                    tr.No_Compte = _compte.No_Compte;
                    tr.CCLIENT = _compte.CClient;
                    _cheque.Montant -= transfert;
                    _compte.Montant -= transfert;

                    mybdd.Transactions.Add(tr);
                    try
                    {
                        mybdd.SaveChanges();
                        MessageBox.Show("Transaction effctué avec succes");

                        Client client = cboTransfert.SelectedItem as Client;
                        dtgCheque.DataContext = (from a in mybdd.Compte_Bancaire
                                                 where a.CClient == client.Code_Client && a.Type_de_Compte == "Cheque"
                                                 select a).ToList();

                        dtgComptes.DataContext = (from a in mybdd.Compte_Bancaire
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
                        tr.No_Compte = _compte.No_Compte;
                        tr.CCLIENT = _compte.CClient;
                        _cheque.Montant -= transfert;
                        _compte.Montant += transfert;

                        if (_cheque.Montant < transfert)
                        {
                            MessageBox.Show("Fond insuffisant");

                        }
                        else
                        {
                            mybdd.Transactions.Add(tr);

                            try
                            {
                                mybdd.SaveChanges();
                                MessageBox.Show("Transaction effctué avec succes");

                                Client client = cboTransfert.SelectedItem as Client;
                                dtgCheque.DataContext = (from a in mybdd.Compte_Bancaire
                                                         where a.CClient == client.Code_Client && a.Type_de_Compte == "Cheque"
                                                         select a).ToList();

                                dtgComptes.DataContext = (from a in mybdd.Compte_Bancaire
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
            Compte compte = new Compte();
            compte.dtgAfficher.Items.Refresh();
            
            this.Close();
        }
    }
}

