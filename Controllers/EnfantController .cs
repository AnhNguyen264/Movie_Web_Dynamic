using Microsoft.AspNetCore.Mvc;
using TP2.Models;
using TP2.ViewModels;

namespace TP2.Controllers
{
    public class EnfantController : Controller
    {
        private BaseDonnees _baseDonnees { get; set; }

        public EnfantController(BaseDonnees donnees)
        {
            _baseDonnees = donnees;
        }


        [Route("Enfant/Recherche")]
        [Route("Recherche")]
        public IActionResult Recherche()
        {
            //var donnees = _baseDonnees.Enfants.ToList();
            //return View(donnees);
            var model = new PageRechercheViewModel();
            model.Criteres = new CritereRechercheViewModel();
            model.Criteres.statut1 = true;
            model.Criteres.statut2 = true;
            model.Criteres.statut3 = true;
            model.Resultat = _baseDonnees.Enfants.ToList();
            return View(model);


        }

       
        [Route("Enfant/Recherche")]
        [HttpPost()]
        public IActionResult Recherche(CritereRechercheViewModel pCriteres)
        {
            var filtrer = new PageRechercheViewModel();
            filtrer.Criteres = pCriteres;
            filtrer.Resultat = _baseDonnees.Enfants.ToList();

            //statut
            if (!filtrer.Criteres.statut1)
            {
                filtrer.Resultat = filtrer.Resultat.Where(x => x.Parent.Id != 1).ToList(); 

            } 
            if (!filtrer.Criteres.statut2)
            {
                filtrer.Resultat = filtrer.Resultat.Where(x => x.Parent.Id != 2).ToList();

            } 
            if (!filtrer.Criteres.statut3)
            {
                filtrer.Resultat = filtrer.Resultat.Where(x => x.Parent.Id != 3).ToList();

            }

            //motscle
            if (filtrer.Criteres.search_byword != null)
            {
                filtrer.Resultat = filtrer.Resultat.Where(x => x.Nom.ToUpper().Contains(filtrer.Criteres.search_byword.ToUpper())).ToList();
            }
            
            //min - max
            if(filtrer.Criteres.vus_min.HasValue)
            {
                filtrer.Resultat = filtrer.Resultat.Where(x => x.Vus >= filtrer.Criteres.vus_min).ToList();
            }

            if (filtrer.Criteres.vus_max.HasValue)
            {
                filtrer.Resultat = filtrer.Resultat.Where(x => x.Vus <= filtrer.Criteres.vus_max).ToList();
            }

            //annee
            //if (filtrer.Criteres.selectAnnee == 2023)
            //{
            //    filtrer.Resultat = filtrer.Resultat.Where(x => x.Date == 2023).ToList();
            //}
            //else if (filtrer.Criteres.selectAnnee == 2022)
            //{
            //    filtrer.Resultat = filtrer.Resultat.Where(x => x.Date == 2022).ToList();
            //}
            //else if (filtrer.Criteres.selectAnnee == 2021)
            //{
            //    filtrer.Resultat = filtrer.Resultat.Where(x => x.Date == 2021).ToList();
            //}

            if (filtrer.Criteres.selectAnnee != null)
            {
             filtrer.Resultat = filtrer.Resultat.Where(x => x.Date == filtrer.Criteres.selectAnnee).ToList(); 
            }

            //message
            if(filtrer.Resultat.Count == 0)
            {
                return View("NonTrouve", "Le film demandé n'a pas été trouvé!");
            }


            return View( filtrer);
        }







        [Route("enfant/detail/{id:int}")]
        [Route("enfant/{id:int}")]
        [Route("e/{id:int}")]
        public IActionResult Detail(int id)
        {
            var filmRecherche = _baseDonnees.Enfants.Where(e => e.Id == id).SingleOrDefault();

            if (filmRecherche == null)
            {
                return View("NonTrouve", "Le film demandé n'a pas été trouvé!");

            } else
            {
                return View(filmRecherche);
            }
        }


        [Route("enfant/detail/{nom}")]
        [Route("enfant/{nom}")]
        //[Route("/{nom}")]
        public IActionResult Detail(string nom)

        {
            //var filmRecherche = _baseDonnees.Enfants.Where(p=> p.Id == id).SingleOrDefault();
            var filmRecherche = _baseDonnees.Enfants.Where(e => e.Nom.ToUpper() == nom.ToUpper()).SingleOrDefault();

            if (filmRecherche == null)
            {
                return View("NonTrouve", "Le film demandé n'a pas été trouvé!");

            } else
            {
                return View(filmRecherche);
            }
            //ViewData["titre"] = filmRecherche.Nom;

           
        }

    }
}