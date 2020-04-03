using System;
using Jedek.Rozvrhy.Libs;
using Jedek.Rozvrhy.App.Models;


namespace Jedek.Rozvrhy.App.Controllers
{
    class StudijniOborController : Controller
    {
        /// <summary>
        /// Objekt zabezpečující správu studijních oborů (model)
        /// </summary>
        public StudijniOborManager StudijniOborManager { get; private set; }


        /// <summary>
        /// Objekt zabezpečující správu předmětů (model)
        /// </summary>
        public PredmetManager PredmetManager { get; private set; }


        /// <summary>
        /// Instance aktuálního oboru pro správu předmětů
        /// </summary>
        public StudijniObor StudijniObor { get; private set; }


        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="studijniOborManager"></param>
        /// <param name="predmetManager"></param>
        public StudijniOborController(StudijniOborManager studijniOborManager, PredmetManager predmetManager)
            : base()
        {
            StudijniOborManager = studijniOborManager;
            PredmetManager = predmetManager;
        }


        /// <summary>
        /// Defaultní akce Controlleru (rozcestník)
        /// </summary>
        public void Default()
        {
            View.Render();
        }

        /// <summary>
        /// Akce Controlleru pro výpis, editaci a mazání studijních oborů
        /// </summary>
        public void VypisObory()
        {
            View.Context.Add("obory", StudijniOborManager.StudijniObory);
            View.DeleteEvent += this.DeleteStudijniObor;
            View.EditEvent += this.EditStudijniObor;
            View.Render();
        }


        /// <summary>
        /// Akce Controlleru pro přidání nového studijního oboru
        /// </summary>
        public void PridejStudijniObor()
        {
            View.SendEvent += this.SaveStudijniObor;
            View.Render();
            View.Request("StudijniObor", "Default", null);
        }


        /// <summary>
        /// Akce Controlleru pro správu předmětů sdudijního oboru
        /// </summary>
        public void PredmetyOboru(StudijniObor obor)
        {
            StudijniObor = obor;
            View.Context.Add("obor", obor);
            View.Context.Add("predmety", PredmetManager.Predmety);
            View.AddEvent += AddPredmetOboru;
            View.DeleteEvent += RemovePredmetOboru;
            View.Render();
        }


        /// <summary>
        /// Předá StudijniOborManagerovi předmět, který má být odebrán oboru
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="predmet"></param>
        private void RemovePredmetOboru(object sender, Model predmet)
        {
            Predmet p = (Predmet)predmet;
            StudijniOborManager.RemovePredmet(StudijniObor, p);
        }


        /// <summary>
        /// Předá StudijniOborManagerovi předmět, který má být přiřazen oboru
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="predmet"></param>
        private void AddPredmetOboru(object sender, Model predmet)
        {
            Predmet p = (Predmet)predmet;
            StudijniOborManager.AddPredmet(StudijniObor, p);
        }


        /// <summary>
        /// Předá StudijniOborManagerovi položku, která se má smazat
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="studijniObor"></param>
        private void DeleteStudijniObor(object sender, Model studijniObor)
        {
            StudijniObor so = (StudijniObor)studijniObor;
            StudijniOborManager.Delete(so);
        }


        /// <summary>
        /// Zpracuje data z formuláře a předá je StudijniOborManagerovi
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void SaveStudijniObor(object sender, Forms.Form args)
        {
            Forms.StudijniOborForm sof = (Forms.StudijniOborForm)args;
            int newId = StudijniOborManager.FindTopId() + 1;
            StudijniOborManager.Save(new StudijniObor(newId, sof.GetNazev(), sof.GetZkratka()));
        }


        /// <summary>
        /// Zpracuje data s editačního formuláře a předá je StudijniOborManagerovi
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void EditStudijniObor(object sender, Forms.Form args)
        {
            Forms.EditStudijniOborForm esof = (Forms.EditStudijniOborForm)args;

            if (esof.GetNazev() != String.Empty)
            {
                esof.StudijniObor.Nazev = esof.GetNazev();
            }

            if (esof.GetZkratka() != String.Empty)
            {
                esof.StudijniObor.Zkratka = esof.GetZkratka();
            }

            StudijniOborManager.Save(esof.StudijniObor);

        }
    }
}
