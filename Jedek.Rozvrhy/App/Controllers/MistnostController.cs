using Jedek.Rozvrhy.Libs;
using Jedek.Rozvrhy.App.Models;

namespace Jedek.Rozvrhy.App.Controllers
{
    class MistnostController : Controller
    {
        /// <summary>
        /// Objekt zabezpečující správu místností (model)
        /// </summary>
        public MistnostManager MistnostManager { get; private set; }


        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="mistnostManager"></param>
        public MistnostController(MistnostManager mistnostManager)
            : base()
        {
            MistnostManager = mistnostManager;
        }


        /// <summary>
        /// Defaultní akce Controlleru (rozcestník)
        /// </summary>
        public void Default()
        {
            View.Render();
        }


        /// <summary>
        /// Akce Controlleru pro výpis, editaci a mazání místností
        /// </summary>
        public void VypisMistnosti()
        {
            View.Context.Add("mistnosti", MistnostManager.Mistnosti);
            View.DeleteEvent += this.DeleteMistnost;
            View.EditEvent += this.EditMistnost;
            View.Render();
        }


        /// <summary>
        /// Akce Controlleru pro přidání nové místnosti
        /// </summary>
        public void PridejMistnost()
        {
            View.SendEvent += this.SaveMistnost;
            View.Render();
        }


        /// <summary>
        /// Předá MistnostManagerovi položku, která se má smazat
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="mistnost"></param>
        private void DeleteMistnost(object sender, Model mistnost)
        {
            Mistnost m = (Mistnost)mistnost;
            MistnostManager.Delete(m);

        }


        /// <summary>
        /// Zpracuje data z formuláře a předá je MistnostManagerovi
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void SaveMistnost(object sender, Forms.Form args)
        {
            Forms.MistnostForm mistnost = (Forms.MistnostForm)args;
            int newId = MistnostManager.FindTopId() + 1;
            if(mistnost.GetTypMistnosti() == TypMistnosti.Počítačová)
            {
                MistnostManager.Save(new PocitacovaMistnost(newId, mistnost.GetBudova(), mistnost.GetCislo(), mistnost.GetKapacita()));
            }
            else if (mistnost.GetTypMistnosti() == TypMistnosti.Posluchárna)
            {
                MistnostManager.Save(new PrednaskovaMistnost(newId, mistnost.GetBudova(), mistnost.GetCislo(), mistnost.GetKapacita()));
            }
            else
            {
                MistnostManager.Save(new SeminarniMistnost(newId, mistnost.GetBudova(), mistnost.GetCislo(), mistnost.GetKapacita()));
            }
        }


        /// <summary>
        /// Zpracuje data s editačního formuláře a předá je MistnostManagerovi
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void EditMistnost(object sender, Forms.Form args)
        {
            Forms.EditMistnostForm emf = (Forms.EditMistnostForm)args;

            if(emf.GetKapacita() != 0)
            {
                emf.Mistnost.Kapacita = emf.GetKapacita();
            }
            if(emf.GetBudova() != 0)
            {
                emf.Mistnost.Budova = emf.GetBudova();
            }
            if(emf.GetCislo() != 0)
            {
                emf.Mistnost.Cislo = emf.GetCislo();
            }

            MistnostManager.Save(emf.Mistnost);
        }
    }
}
