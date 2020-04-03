using System;
using Jedek.Rozvrhy.Libs;
using Jedek.Rozvrhy.App.Models;

namespace Jedek.Rozvrhy.App.Controllers
{
    class PredmetController : Controller
    {
        /// <summary>
        /// Objekt zabezpečující správu předmětů (model)
        /// </summary>
        public PredmetManager PredmetManager { get; private set; }


        /// <summary>
        /// Objekt zabezpečující správu vyučujících (model)
        /// </summary>
        public VyucujiciManager VyucujiciManager { get; private set; }


        /// <summary>
        /// Instance aktuálního předmětu pro správu vyučujících
        /// </summary>
        public Predmet Predmet { get; private set; }


        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="predmetManager"></param>
        public PredmetController(PredmetManager predmetManager, VyucujiciManager vyucujiciManager)
            : base()
        {
            PredmetManager = predmetManager;
            VyucujiciManager = vyucujiciManager;
        }


        /// <summary>
        /// Defaultní akce Controlleru (rozcestník)
        /// </summary>
        public void Default()
        {
            View.Render();
        }


        /// <summary>
        /// Akce Controlleru pro výpis, editaci a mazání předmětů
        /// </summary>
        public void VypisPredmety()
        {
            View.Context.Add("predmety", PredmetManager.Predmety);
            View.DeleteEvent += this.DeletePredmet;
            View.EditEvent += this.EditPredmet;
            View.Render();
        }


        /// <summary>
        /// Akce Controlleru pro přidání nového předmětu
        /// </summary>
        public void PridejPredmet()
        {
            View.SendEvent += this.SavePredmet;
            View.Render();
        }


        /// <summary>
        /// Akce Controlleru pro správu přednášejících předmět
        /// </summary>
        /// <param name="predmet"></param>
        public void PrednasejiciPredmetu(Predmet predmet)
        {
            Predmet = predmet;
            View.Context.Add("predmet", Predmet);
            View.Context.Add("vyucujici", VyucujiciManager.Vyucujici);
            View.AddEvent += AddPrednasejicihoPredmetu;
            View.DeleteEvent += RemovePrednasejicihoPredmetu;
            View.Render();
        }


        /// <summary>
        /// Předá PredmetManagerovi vyučujícího, který má být odebrán předmětu jako přednášející
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="vyucujici"></param>
        private void RemovePrednasejicihoPredmetu(object sender, Model vyucujici)
        {
            Vyucujici v = (Vyucujici)vyucujici;
            PredmetManager.RemovePrednasejiciho(Predmet, v);
        }


        /// <summary>
        /// Předá PredmetManagerovi vyučujícího, který má být přiřazen předmětu jako přednášející
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="vyucujici"></param>
        private void AddPrednasejicihoPredmetu(object sender, Model vyucujici)
        {
            Vyucujici v = (Vyucujici)vyucujici;
            PredmetManager.AddPrednasejiciho(Predmet, v);
        }


        /// <summary>
        /// Akce Controlleru pro správu cvičících předmět
        /// </summary>
        /// <param name="predmet"></param>
        public void CviciciPredmetu(Predmet predmet)
        {
            Predmet = predmet;
            View.Context.Add("predmet", Predmet);
            View.Context.Add("vyucujici", VyucujiciManager.Vyucujici);
            View.AddEvent += AddCvicicihoPredmetu;
            View.DeleteEvent += RemoveCvicicihoPredmetu;
            View.Render();
        }


        /// <summary>
        /// Předá PredmetManagerovi vyučujícího, který má být odebrán předmětu jako cvičící
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="vyucujici"></param>
        private void RemoveCvicicihoPredmetu(object sender, Model vyucujici)
        {
            Vyucujici v = (Vyucujici)vyucujici;
            PredmetManager.RemoveCviciciho(Predmet, v);
        }


        /// <summary>
        /// Předá PredmetManagerovi vyučujícího, který má být přiřazen předmětu jako cvičící
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="vyucujici"></param>
        private void AddCvicicihoPredmetu(object sender, Model vyucujici)
        {
            Vyucujici v = (Vyucujici)vyucujici;
            PredmetManager.AddCviciciho(Predmet, v);
        }


        /// <summary>
        /// Akce Controlleru pro správu vedoucích seminářů předmětu
        /// </summary>
        /// <param name="predmet"></param>
        public void VedouciSeminarePredmetu(Predmet predmet)
        {
            Predmet = predmet;
            View.Context.Add("predmet", Predmet);
            View.Context.Add("vyucujici", VyucujiciManager.Vyucujici);
            View.AddEvent += AddVedoucihoSeminarPredmetu;
            View.DeleteEvent += RemoveVedoucihoSeminarPredmetu;
            View.Render();
        }


        /// <summary>
        /// Předá PredmetManagerovi vyučujícího, který má být odebrán předmětu jako vedoucího seminář
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="vyucujici"></param>
        private void RemoveVedoucihoSeminarPredmetu(object sender, Model vyucujici)
        {
            Vyucujici v = (Vyucujici)vyucujici;
            PredmetManager.RemoveVedeSeminar(Predmet, v);
        }


        /// <summary>
        /// Předá PredmetManagerovi vyučujícího, který má být přiřazen předmětu jako vedoucího seminář
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="vyucujici"></param>
        private void AddVedoucihoSeminarPredmetu(object sender, Model vyucujici)
        {
            Vyucujici v = (Vyucujici)vyucujici;
            PredmetManager.AddVedeSeminar(Predmet, v);
        }


        /// <summary>
        /// Předá PredmetManagerovi položku, která se má smazat
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="predmet"></param>
        private void DeletePredmet(object sender, Model predmet)
        {
            Predmet s = (Predmet)predmet;
            PredmetManager.Delete(s);
        }


        /// <summary>
        /// Zpracuje data z formuláře a předá je PredmetManagerovi
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void SavePredmet(object sender, Forms.Form args)
        {
            Forms.PredmetForm pf = (Forms.PredmetForm)args;
            int newId = PredmetManager.FindTopId() + 1;
            PredmetManager.Save(new Predmet(newId, pf.GetZkratka(), pf.GetNazev(), pf.GetHodinPrednasek(), pf.GetHodinCviceni(), pf.GetHodinSeminaru()));
        }


        /// <summary>
        /// Zpracuje data s editačního formuláře a předá je PredmetManagerovi
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void EditPredmet(object sender, Forms.Form args)
        {
            Forms.EditPredmetForm epf = (Forms.EditPredmetForm)args;

            if (epf.GetZkratka() != String.Empty)
            {
                epf.Predmet.Zkratka = epf.GetZkratka();
            }

            if (epf.GetNazev() != String.Empty)
            {
                epf.Predmet.Nazev = epf.GetNazev();
            }

            epf.Predmet.HodinPrednasek = epf.GetHodinPrednasek();
            epf.Predmet.HodinCviceni = epf.GetHodinCviceni();
            epf.Predmet.HodinSeminaru = epf.GetHodinSeminaru();

            PredmetManager.Save(epf.Predmet);
        }
    }
}