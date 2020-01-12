using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace SAIM.Models.Classe
{
    public class Client
    {
        private int idSociete;
        private string nomSociete;
        private string nif;
        private string stat;
        private string activite;
        private string adresse;
        private string mail;
        private string telephone;

        public Client() { }

        public Client(int idSociete, string nomSociete, string nif, string stat, string activite, string adresse, string mail, string telephone)
        {
            this.IdSociete = idSociete;
            this.NomSociete = nomSociete;
            this.Nif = nif;
            this.Stat = stat;
            this.Activite = activite;
            this.Adresse = adresse;
            this.Mail = mail;
            this.Telephone = telephone;
        }

        public void Import(HttpPostedFileBase files)
        {
            Connexion connexion = new Connexion();
            MySqlConnection connection = connexion.GetConnection();
            connection.Open();
            MySqlCommand command = connection.CreateCommand();
            MySqlTransaction transaction;
            transaction = connection.BeginTransaction();
            command.Connection = connection;
            command.Transaction = transaction;
            try
            {
                if (files.FileName == "")
                {
                    throw new Exception("Veuillez choisir un fichier");
                }
         
                List<Client> clientImporte = ClientImport(files);

                for(int i = 0; i<clientImporte.Count; i++)
                {
                    clientImporte[i].AjoutClient(command);
                }
                if (transaction != null) { 
                    transaction.Commit();
                }

            }
            catch(Exception exception)
            {
                transaction.Rollback();
                throw exception;
            }
            finally
            {
                if (command != null)
                {
                    command.Dispose();
                }
                if (transaction != null)
                {
                    transaction.Dispose();
                }
                if (connection != null)
                {
                    connection.Close();
                }
            }
        }
        private List<Client> ClientImport(HttpPostedFileBase files)
        {
            StreamReader lecture = new StreamReader(files.InputStream);
            try
            {
                List<Client> listeClients = new List<Client>();
                while (!lecture.EndOfStream)
                {
                    string ligne = lecture.ReadLine();
                    string[] ligneSplite= ligne.Split(';');
                    List<string> valeurAttribut = new List<string>();
                    for(int i = 0; i<ligneSplite.Length; i++)
                    {
                        string[] valeur = ligneSplite[i].Split(':');
                        valeurAttribut.Add(valeur[1]);
                    }
                    Client client = new Client(0, valeurAttribut.ElementAt(0), valeurAttribut.ElementAt(1), valeurAttribut.ElementAt(2), valeurAttribut.ElementAt(3), valeurAttribut.ElementAt(4), valeurAttribut.ElementAt(5),valeurAttribut.ElementAt(6));
                    listeClients.Add(client);
                }
                return listeClients;
            }
            catch (Exception exception)
            {
                throw new Exception("Une erreur s'est produite lors de l'importation du fichier, vérifiez bien les données insérées");
            }
            finally
            {
                lecture.Close();
            }
        }
        public void Export(string path)
        {
            try
            {
                //Passe le chemin dans un streamWriter
                //StreamWriter sw = new StreamWriter("E:/fianarana/auto_didact/c#mvc/Projet final SAIM/SAIM/Models/Classe/tes.txt");
                StreamWriter sw = new StreamWriter(path);
                List<Client> liste = this.GetListeClient();
                for(int i = 0; i<liste.Count; i++)
                {
                    Client client = liste[i];
                    sw.WriteLine("nom:"+client.NomSociete+ ";nif:" + client.Nif + ";stat:" + client.Stat + ";activite:" + client.Activite + ";adresse:" + client.Adresse + ";mail:" + client.Mail + ";telephone:" + client.Telephone);
                }
                //Fermeture du fichier
                sw.Close();
            }
            catch(Exception ex)
            {
                throw new Exception("Une erreur s'est produite lors de l' exportation: "+ex.Message);
            }
        }
        public void ModifierClient()
        {
            Connexion connexion = new Connexion();
            try
            {
                string query = "UPDATE societe set nom_societe='"+this.NomSociete+"', nif='"+this.Nif+"',stat='"+this.Stat+"',adresse='"+this.Adresse+"',activite='"+this.Activite+"',mail='"+this.Mail+"',num_telephone='"+this.Telephone+"' " +
                    "where id_societe='"+this.IdSociete+"'";

                //ouverture de la connexion
                if (connexion.OpenConnection() == true)
                {
                    //create command and assign the query and connection from the constructor
                    MySqlCommand cmd = new MySqlCommand(query, connexion.GetConnection());

                    //Execute command
                    cmd.ExecuteNonQuery();

                    //close connection
                    connexion.CloseConnection();
                }
            }
            catch (Exception exception)
            {
                throw new Exception("Une erreur s'est produite lors de la modification d'une société");
            }
        }
        public void SupprimerClient(int idclient)
        {
            Connexion connexion = new Connexion();
            try
            {
                string query = "DELETE FROM societe where id_societe='"+idclient+"'";

                //ouverture de la connexion
                if (connexion.OpenConnection() == true)
                {
                    //create command and assign the query and connection from the constructor
                    MySqlCommand cmd = new MySqlCommand(query, connexion.GetConnection());

                    //Execute command
                    cmd.ExecuteNonQuery();

                    //close connection
                    connexion.CloseConnection();
                }
            }
            catch (Exception exception)
            {
                throw new Exception("Une erreur s'est produite lors de la supression d'une société");
            }
        }

        public void Ajout()
        {
            Connexion connexion = new Connexion();
            MySqlConnection connection = connexion.GetConnection();
            connection.Open();
            MySqlCommand command = connection.CreateCommand();
            MySqlTransaction transaction;
            transaction = connection.BeginTransaction();
            command.Connection = connection;
            command.Transaction = transaction;
            try
            {
                this.AjoutClient(command);
                if (transaction != null)
                {
                    transaction.Commit();
                }

            }
            catch (Exception exception)
            {
                transaction.Rollback();
                throw exception;
            }
            finally
            {
                if (command != null)
                {
                    command.Dispose();
                }
                if (transaction != null)
                {
                    transaction.Dispose();
                }
                if (connection != null)
                {
                    connection.Close();
                }
            }
        }
        public void AjoutClient(MySqlCommand cmd)
        {
            Connexion connexion = new Connexion();
            try { 
                string query = "INSERT INTO societe (nom_societe,nif,stat,adresse,activite,mail,num_telephone) " +
                    "VALUES('"+this.NomSociete+"', '"+this.Nif+"','"+this.Stat+"','"+this.Adresse+"','"+this.Activite+"','"+this.Mail+"','"+this.Telephone+"')";
                    //create command and assign the query and connection from the constructor
                    cmd.CommandText = query;
                    //Execute command
                    cmd.ExecuteNonQuery();
            }
            catch(Exception exception)
            {
                throw new Exception("Une erreur s'est produite lors de l ajout d'une société");
            }
        }
        public List<Client> GetListeClient() {
            Connexion connexion = new Connexion();
            string query = "SELECT * FROM societe";

            //Creation d'une liste
            List<Client> list = new List<Client>();

            //Ouverture connexion
            if (connexion.OpenConnection() == true)
            {
                //Creation d'une commande
                MySqlCommand cmd = new MySqlCommand(query, connexion.GetConnection());
                //Excecution de la commande
                MySqlDataReader dataReader = cmd.ExecuteReader();

                //Lecture des donnees et stockage dans la liste
                while (dataReader.Read())
                {
                    //int idSociete, string nomSociete, string nif, string stat, string activite, string adresse, string mail, string telephone
                    Client clientTemp = new Client(int.Parse(dataReader["id_societe"].ToString()),dataReader["nom_societe"].ToString(),dataReader["nif"].ToString(),dataReader["stat"].ToString(),dataReader["activite"].ToString(),dataReader["adresse"].ToString(),dataReader["mail"].ToString(),dataReader["num_telephone"].ToString());
                    list.Add(clientTemp);
                }

                //fermeture du Data Reader
                dataReader.Close();

                //fermeture de la connexion
                connexion.CloseConnection();

                //return
                return list;
            }
            else
            {
                return list;
            }
        }
        

        
        public string NomSociete {
            get => nomSociete;
            set
            {
                if (value == "")
                {
                    throw new Exception("Veuillez entrez un societe valide");
                }
                else { 
                    nomSociete = value;
                }
            }
        }
        public string Nif { 
            get => nif;
            set {
                if (value == "")
                {
                    throw new Exception("Veuillez entrez un NIF valide");
                }
                else { 
                    nif = value;
                }
            }
        }
        public string Stat { 
            get => stat;
            set
            {
                if (value == "")
                {
                    throw new Exception("Veuillez entrez un STAT valide");
                }
                else
                {
                    stat = value;
                }
            }
        }
        public string Activite { 
            get => activite;
            set
            {
                if (value == "")
                {
                    throw new Exception("Veuillez entrez la description de la société");
                }
                else
                {
                    activite = value;
                }
            }
        }
        public string Adresse { 
            get => adresse;
            set
            {
                if (value == "")
                {
                    throw new Exception("Veuillez entrez une adresse valide");
                }
                else
                {
                    adresse = value;
                }
            }
        }
        public string Mail { 
            get => mail;
            set
            {
                if (value == "")
                {
                    throw new Exception("Veuillez entrez un mail valide");
                }
                else
                {
                    mail = value;
                }
            }
        }
        public string Telephone { 
            get => telephone;
            set
            {
                if (value == "")
                {
                    throw new Exception("Veuillez entrez un numéro valide");
                }
                else
                {
                    telephone = value;
                }
            }
        }
        public int IdSociete { 
            get => idSociete;
            set
            {
                if (value==null)
                {
                    throw new Exception("Veuillez entrez un ID valide");
                }
                else
                {
                    idSociete = value;
                }
            }
        }
    }
}
