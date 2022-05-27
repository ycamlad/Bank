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
    /// Interaction logic for Compte.xaml
    /// </summary>
    public partial class Compte : Window
    {
        BankEntities2 mybdd = new BankEntities2();
        decimal? comptant;
        string _compte;

        public Compte()
        {
            InitializeComponent();
            string code = Application.Current.Properties["code"].ToString();

            cboCompte.DataContext = (from g in mybdd.Compte_Bancaire
                                     join m in mybdd.Clients on g.CClient equals m.Code_Client
                                     where m.Code_Client == code
                                     select m).ToList();

            dtgAfficher.DataContext = (from g in mybdd.Compte_Bancaire
                                       join m in mybdd.Clients on g.CClient equals m.Code_Client
                                       where m.Code_Client == code
                                       select m).ToList();
            cboCompte.DisplayMemberPath = "Code_Client";
            dtgAfficher.DisplayMemberPath = "Montant";



        }

        // ComboBox cboCompte qui se synchronise avec la table client 
        private void cboCompte_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Client client = cboCompte.SelectedItem as Client;
            //dtgAfficher.DataContext = client.Compte_Bancaire.ToList();
            dtgAfficher.DataContext = (from c in mybdd.Compte_Bancaire
                                       where c.CClient == client.Code_Client
                                       select c).ToList();

        }



        // bouton pour faire un depot d'argent dans les comptes concernes
        private void btnfdepot_Click(object sender, RoutedEventArgs e)
        {
            // Instanciation de l'objet Transaction pour enregistrer les transactions
            Transaction tr = new Transaction();
            // connexion entre classe Compte_bancaire et dtgAfficher pour afficher les comptes bancaires
            Compte_Bancaire _Bancaire = dtgAfficher.SelectedItem as Compte_Bancaire;
            if (txtMontant.Text == string.Empty|| decimal.Parse(txtMontant.Text) < 0)
            {
                MessageBox.Show("Veuillez saisir un montant");
            }
            else
            {
                // condition poiur empecher de faire un depot dans le compte Marge de credit
                if (_Bancaire.Type_de_Compte != "Marge de credit")
                {

                    // requete pour que le compte selectionner recoive l'argent deposer 
                    var query = (from c in mybdd.Compte_Bancaire
                                 where c.No_Compte == _Bancaire.No_Compte
                                 select c);
                 
                    // remplissage de tr pour pouvoir faire l'enregistrement de la transaction
                    tr.Depot = decimal.Parse(txtMontant.Text);
                    tr.No_Compte = _Bancaire.No_Compte;
                    tr.CCLIENT = _Bancaire.CClient;
     
                    foreach (Compte_Bancaire c in query)
                    {

                        c.Montant += decimal.Parse(txtMontant.Text);
                     
                    }

                    mybdd.Transactions.Add(tr);
                    try
                    {
                        mybdd.SaveChanges();
                        MessageBox.Show("Transaction effctué avec succes");

                        Client client = cboCompte.SelectedItem as Client;
                        //  dtgAfficher.DataContext = client.Compte_Bancaire.ToList();
                        dtgAfficher.DataContext = (from c in mybdd.Compte_Bancaire
                                                   where c.CClient == client.Code_Client
                                                   select c).ToList();


                    }
                    catch (Exception ex)
                    {

                        MessageBox.Show(ex.Message);
                    }
                }
                else
                {
                    MessageBox.Show("Transaction Refusé", "Erreur", button: MessageBoxButton.OK);

                }
            }
        }

        // bouton pour faire un Retrait
        private void btnfretrait_Click(object sender, RoutedEventArgs e)
        {

            // variable verif qui verifie si le compte selectionner n'est pas le compte Marge de credit 
            Compte_Bancaire verfif = mybdd.Compte_Bancaire.FirstOrDefault(u => u.Type_de_Compte == "Marge de credit" && u.CClient == _compte);
            Transaction tr = new Transaction();
            Compte_Bancaire _Bancaire = dtgAfficher.SelectedItem as Compte_Bancaire;
            Argent_Comptant ar = new Argent_Comptant();

            //requete pour selectionner Argent Comptant
            var query = (from c in mybdd.Argent_Comptant
                         where c.IDComptant == "1"
                         select c);

            var query2 = (from d in mybdd.Compte_Bancaire
                          where d.No_Compte == _Bancaire.No_Compte
                          select d);


            //requete pour selectionner le compte Marge de credit 
            var query3 = (from l in mybdd.Compte_Bancaire
                          where l.Type_de_Compte == "Marge de credit"
                          select l);

            // condition pout empecher de d'entrer un montant vide ou negatif 
            if (txtMontant.Text == string.Empty|| decimal.Parse(txtMontant.Text) < 0)
            {
                MessageBox.Show("Veuillez saisir un montant");
            }
            else
            {
               

                foreach (Argent_Comptant c in query)
                {
                    decimal? diff2 = c.Comptant - decimal.Parse(txtMontant.Text);
                    // condition qui verifie si il y a assez d'argent comptant dans la machine  
                    if (diff2<=0  )
                    {
                        MessageBox.Show(" Argent comptant insuffisant ","Erreur");
                    }

                    comptant = diff2;
                }
                // condition qui empeche de prendre de l'argent comptant si la quantite  est plus petite que la difference entre argent comptant et la quantiter demander 
                if (comptant > 0)
                {
                    // condition pour empecher de faire un retrait de  plus de 1000$
                    if (int.Parse(txtMontant.Text) > 1000)
                    {

                        MessageBox.Show("Auncun Retrait ne peux exceder 1000$");

                    }
                    else
                    {
                        // condition qui empeche de faire un retrait qui n'est pas un multiple de 10
                        if (int.Parse(txtMontant.Text) % 10 == 0)
                        {
                            //condition qui empeche de faire un retrait dans les compte Hypothecaire et Marge de credit
                            if (_Bancaire.Type_de_Compte != "Hypothecaire" && _Bancaire.Type_de_Compte != "Marge de credit")
                            {
                                //condition qui empeche de faire un retrait si le solde est insuffisant
                                if (_Bancaire.Montant <= 0)
                                {
                                    MessageBox.Show("Transaction refuser");
                                }
                                else
                                {

                                    tr.CCLIENT = _Bancaire.CClient;
                                    tr.Retrait = decimal.Parse(txtMontant.Text);
                                    tr.No_Compte = _Bancaire.No_Compte;

                                    if (tr.Retrait > _Bancaire.Montant)
                                    {
                                        decimal? diff = tr.Retrait - _Bancaire.Montant;

                                        // condition qui empeche de faire un retrait du compte Marge de credit
                                        if (verfif == null)
                                        {
                                            MessageBox.Show("Transaction refuse");
                                        }
                                        else
                                        {
                                            // fait un retrait du compte Marge de credit la premiere fois que si le retait est plus  eleve que l'argent dans le compte 
                                            MessageBox.Show("Retrait marge de credit");
                                            foreach (Compte_Bancaire l in query3)
                                            {


                                                _Bancaire.Montant = _Bancaire.Montant - tr.Retrait;
                                                l.Montant += diff;



                                            }
                                            try
                                            {
                                                mybdd.SaveChanges();
                                                MessageBox.Show("Transaction effectué avec succes");
                                                // _Bancaire.Montant -= diff;
                                                Client client = cboCompte.SelectedItem as Client;
                                                dtgAfficher.DataContext = (from k in mybdd.Compte_Bancaire
                                                                           where k.CClient == client.Code_Client
                                                                           select k).ToList();



                                            }
                                            catch (Exception ex)
                                            {

                                                MessageBox.Show(ex.Message);
                                            }
                                        }


                                    }
                                    else
                                    {
                                        foreach (Argent_Comptant a in query)
                                        {
                                            if (a.Comptant < tr.Retrait)                 // Condition qui empeche de faire un retrait plus eleve que le montant disponible dans argent comptant
                                            {
                                                MessageBox.Show("Fond insuffisant", "ERREUR");
                                            }

                                        }

                                        {
                                            foreach (Argent_Comptant c in query)
                                            {
                                                c.Comptant -= tr.Retrait;                                    // Retrait du montant dans Argent comptant
                                                
                                            }
                                            foreach (Compte_Bancaire d in query2)
                                            {        // Retrait du montant dans le compte selectionner
                                                d.Montant -= tr.Retrait;
                                            }

                                            mybdd.Transactions.Add(tr);


                                            try
                                            {
                                                mybdd.SaveChanges();
                                                MessageBox.Show("Transaction effctué avec succes");

                                                Client client = cboCompte.SelectedItem as Client;
                                                dtgAfficher.DataContext = (from k in mybdd.Compte_Bancaire
                                                                           where k.CClient == client.Code_Client
                                                                           select k).ToList();


                                            }
                                            catch (Exception ex)
                                            {

                                                MessageBox.Show(ex.Message);
                                            }
                                        }


                                    }
                                }
                            }
                            else
                            {

                                MessageBox.Show("Transaction Refusé", "Erreur", button: MessageBoxButton.OK);

                            }
                        }

                        else
                        {
                            MessageBox.Show("Seul des multiples de 10 sont accepte");

                        }

                    }

                }
                
            }


        }


        //public string un_test(string x)
        //{
        //    string le_test = x;

        //    return _compte = le_test;
        //}

        //Bouton transfert d'argent

        private void btnfTrasnfert_Click(object sender, RoutedEventArgs e)
        {

            
            Transfert transfert = new Transfert();
            transfert.Show();
            
           

            //transfert.cboTransfert.DataContext = (from g in mybdd.Compte_Bancaire
            //                                      join m in mybdd.Clients on g.CClient equals m.Code_Client
            //                                      where m.Code_Client == code
            //                                      select m).ToList();
        }

        // bouton pour payer les factures
        private void btnfFacture_Click(object sender, RoutedEventArgs e)
        {
            //connexion entre la classe Compte_Bancaire et le dtgAfficher pour que tout les item selectionner sois synchroniser avec la table
            Compte_Bancaire _Bancaire = dtgAfficher.SelectedItem as Compte_Bancaire;
            // Instanciation de l'objet Transaction pour enregistrer les transactions
            Transaction tr = new Transaction();

            // requete pour selectionne le compte cheque
            var query = (from d in mybdd.Compte_Bancaire
                         where d.Type_de_Compte == "Cheque" && d.CClient == _compte
                         select d);
            //requete pour selectionne le compte marge de credit 
            var query3 = (from l in mybdd.Compte_Bancaire
                          where l.Type_de_Compte == "Marge de credit" && l.CClient == _compte
                          select l);
            if (txtMontant.Text == string.Empty||decimal.Parse(txtMontant.Text)<0)
            {
                MessageBox.Show("Veuiller saisir un montant");

            }
            else
            {
               // condition pour empecher un paiment de facture en selectionnant un autre compte
                if (_Bancaire.Montant < 0 || _Bancaire.Type_de_Compte != "Cheque         ")

                {
                    MessageBox.Show("Transaction refuser");
                }
                else
                {

                    // remplissage des donnees pour une transaction dans la table transaction
                    tr.CCLIENT = _Bancaire.CClient;
                    tr.Retrait = decimal.Parse(txtMontant.Text);
                    tr.No_Compte = _Bancaire.No_Compte;

                    // condition pour que si le montant qu'il y a dans le compte cheque est plus petit que le montant demander le compte Marge de credit prend la difference  
                    if (tr.Retrait > _Bancaire.Montant)
                    {
                        decimal? diff = tr.Retrait - _Bancaire.Montant;
                        _Bancaire.Montant -= tr.Retrait;


                            MessageBox.Show("Retrait marge de credit");
                            foreach (Compte_Bancaire l in query3)
                            {


                                _Bancaire.Montant = _Bancaire.Montant - decimal.Parse(txtMontant.Text);

                                l.Montant += diff;



                            }
                            try
                            {
                                mybdd.SaveChanges();
                                MessageBox.Show("Transaction effectué avec succes");

                                Client client = cboCompte.SelectedItem as Client;
                                dtgAfficher.DataContext = (from k in mybdd.Compte_Bancaire
                                                           where k.CClient == _compte
                                                           select k).ToList();



                            }
                            catch (Exception ex)
                            {

                                MessageBox.Show(ex.Message);
                            }
                       

                    }


                    else
                    {
                        //condition qui selectionne le compte cheque concerner et fais la soustraction du montant demande
                        foreach (Compte_Bancaire c in query)
                        {
                            c.Montant -= decimal.Parse(txtMontant.Text) + (decimal?)1.25;


                        }
                        try
                        {
                            mybdd.SaveChanges();
                            MessageBox.Show("Transaction effctué avec succes");

                            Client client = cboCompte.SelectedItem as Client;
                            dtgAfficher.DataContext = (from h in mybdd.Compte_Bancaire
                                                       where h.CClient == _compte
                                                       select h).ToList();


                        }
                        catch (Exception ex)
                        {

                            MessageBox.Show(ex.Message);
                        }
                    }
                }
            }

        }

        // bouton pour revenir a la page de connexion
        private void btnQuitter_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Visibility = Visibility.Visible;
            Application.Current.Properties.Remove("code");
            this.Close();


        }
    }
  }
