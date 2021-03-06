using System;
using System.Collections.Generic;
using System.Data.Entity;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Bank
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        BankEntities2 mybdd = new BankEntities2();
        public MainWindow()
        {
            InitializeComponent();

        }


        
        int counter = 3;


        private void Button_Click(object sender, RoutedEventArgs e)
        {

            Admin admin = new Admin();
            Client client = new Client();
            string code = txtCodeClient.Text;
            string nip = txtNip.Password;
            Application.Current.Properties["code"] = code;
            // requete pour savoir si le compte est debloquer 
            Admin verif = mybdd.Admins.FirstOrDefault(u => u.ID_Admin == code && u.Pass_Admin == nip);
            Client Verification = mybdd.Clients.FirstOrDefault(u => u.Code_Client == code && u.Nip == nip && u.Acces == "Debloquer");
            Client Verification2 = mybdd.Clients.FirstOrDefault(u => u.Code_Client == code);

            // requete pour savoir si le compte qui se connecte est bloquer 
            Client Verification3 = mybdd.Clients.FirstOrDefault(u => u.Code_Client == code && u.Acces == "Bloquer");
           
                
                if (verif!=null)
            {
                counter = 3;
             

                admin.Show();
                Hide();
                txtCodeClient.Text = string.Empty;
                txtNip.Password = string.Empty;

            }
                //condition qui empeche un utilisateur bloquer de pouvoir acceder a son compte
          else  if (Verification3 != null) {
                txtCodeClient.Text = string.Empty;
                txtNip.Password = string.Empty;
                MessageBox.Show("veuillez contacter un representant");
            }

            //
            else if (counter == 0 && Verification3 != null)
            {
                MessageBox.Show("veuillez contacter un representant");
            }
            else
            {
                // probleme avec le compteur essayer de reparer !!!!!!!!!

                if ((Verification2 == null && (txtCodeClient.Text != string.Empty || txtNip.Password != string.Empty)))
                {
                    MessageBox.Show("votre code client ou votre ou votre nip n'est pas bon");
                }


                // condition qui renvoie une informartion si les informations rentrer son correcte 
                else if (Verification != null)
                {
                    counter = 3;
                    
                   

                    Transfert transfert = new Transfert();
                    Compte compte = new Compte();
                    compte.ShowDialog();
                    Hide();


                    //compte.cboCompte.DataContext = (from g in mybdd.Compte_Bancaire
                    //                                join m in mybdd.Clients on g.CClient equals m.Code_Client
                    //                                where m.Code_Client == code
                    //                                select m).ToList();

                    //compte.dtgAfficher.DataContext = (from g in mybdd.Compte_Bancaire
                    //                                  join m in mybdd.Clients on g.CClient equals m.Code_Client
                    //                                  where m.Code_Client == code
                    //                                  select m).ToList();

                  //  compte.un_test(code);

                }
                
                //condition qui bloque un utilisateur s'il rentre trois fois le mauvais mots de passe 
                else if (txtCodeClient.Text != string.Empty || txtNip.Password != string.Empty)
                {
                    
                    MessageBox.Show("il vous reste: " + counter.ToString() + " essaie(s)");
                   
                    counter--;
                    txtCodeClient.Text = string.Empty;
                    txtCodeClient.Focus();
                    txtNip.Password = string.Empty;

                    if (counter == 0)
                    {


                        var query = (from c in mybdd.Clients
                                     where c.Code_Client == code
                                     select c);
                        foreach (Client c in query)
                        {

                            c.Acces = "Bloquer";
                        }


                        try
                        {

                            mybdd.SaveChanges();

                            MessageBox.Show("Votre Compte a ete suspendue veuiller contacter un representant");

                        }

                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message);

                        }
                    }
                }

                else if (txtCodeClient.Text == string.Empty || txtNip.Password == string.Empty)
                {

                    MessageBox.Show("Veuillez rentrer votre Code et votre Nip ");
                }



            }
        }
    }   }
    


