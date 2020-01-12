using SAIM.Models.Classe;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SAIM.Controllers
{
    public class ClientController : Controller
    {
        // GET: Client
        public ActionResult Index()
        {
            ViewBag.title = "Accueil";
            Client client = new Client();
            List<Client> liste = client.GetListeClient();
            ViewData["liste"] = liste;
            ViewBag.download = "no";
            return View("Accueil");
        }
        [HttpPost]
        public ActionResult Importer(HttpPostedFileBase files)
        {
            Client client = new Client();
            try
            {
                client.Import(files);
                List<Client> liste = client.GetListeClient();
                ViewData["liste"] = liste;
                ViewBag.title = "Accueil";
                ViewBag.message = "Le fichier a été importé dans la base de donnée";
                return View("Accueil");
            }
            catch (Exception exception)
            {
                ViewBag.message = exception.Message;
                List<Client> liste = client.GetListeClient();
                ViewData["liste"] = liste;
                ViewBag.title = "Accueil";
                return View("Accueil");
            }
        }
        public ActionResult Exporter()
        {
            Client clientExport = new Client();
            Client client = new Client();
            try
            {
                string path = Server.MapPath("~/")+"Content/Files/export.txt";
                clientExport.Export(path);
                List<Client> liste = client.GetListeClient();
                ViewData["liste"] = liste;
                ViewBag.title = "Accueil";
                ViewBag.download = "ok";
                return View("Accueil");
            }
            catch(Exception exception)
            {
                ViewBag.message = exception.Message;
                List<Client> liste = client.GetListeClient();
                ViewData["liste"] = liste;
                ViewBag.title = "Accueil";
                return View("Accueil");
            }
        }
        public ActionResult Modifier()
        {
            try
            {
                string idsociete = Request.Form["idSociete"];
                string societe = Request.Form["societe"];
                string nif = Request.Form["nif"];
                string stat = Request.Form["stat"];
                string adresse = Request.Form["adresse"];
                string mail = Request.Form["mail"];
                string telephone = Request.Form["telephone"];
                string activite = Request.Form["activite"];
                Client client = new Client(int.Parse(idsociete), societe, nif, stat, activite, adresse, mail, telephone);
                client.ModifierClient();
                List<Client> liste = client.GetListeClient();
                ViewData["liste"] = liste;
                ViewBag.message = "Modification d' un client avec succès";
                ViewBag.title = "Accueil";
                return View("Accueil");
            }
            catch (Exception exception)
            {
                ViewBag.message = exception.Message;
                return View("Accueil");
            }
        }
        public ActionResult Supprimer(string id)
        {
            try
            {
                Client client = new Client();
                client.SupprimerClient(int.Parse(id));
                List<Client> liste = client.GetListeClient();
                ViewData["liste"] = liste;
                ViewBag.message = "Suppression d' un client avec succès";
                ViewBag.title = "Accueil";
                return View("Accueil");
            }
            catch (Exception exception)
            {
                ViewBag.message = exception.Message;
                return View("Accueil");
            }
        }
        public ActionResult AjoutClient()
        {
            string societe = Request.Form["societe"];
            string nif = Request.Form["nif"];
            string stat = Request.Form["stat"];
            string adresse = Request.Form["adresse"];
            string mail = Request.Form["mail"];
            string telephone = Request.Form["telephone"];
            string activite = Request.Form["activite"];
            try {
                Client client = new Client(0, societe, nif, stat, activite, adresse, mail, telephone);
                List<Client> liste = client.GetListeClient();
                client.Ajout();
                ViewData["liste"] = liste;
                ViewBag.message = "Ajout d' un client avec succès";
                ViewBag.title = "Accueil";
                return View("Accueil");
            }
            catch(Exception exception)
            {
                Client client = new Client();
                ViewData["liste"] = client.GetListeClient();
                ViewBag.title = "Accueil";
                ViewBag.message = exception.Message;
                return View("Accueil");
            }
        }
    }
}