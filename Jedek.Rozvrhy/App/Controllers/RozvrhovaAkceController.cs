using System.Collections.Generic;
using Jedek.Rozvrhy.Libs;
using Jedek.Rozvrhy.App.Models;
using Jedek.Rozvrhy.App.Forms;

namespace Jedek.Rozvrhy.App.Controllers
{
    class RozvrhovaAkceController : Controller
    {
        /// <summary>
        /// Objekt zabezpečující správu rozvrhových akcí (model)
        /// </summary>
        public RozvrhovaAkceManager RozvrhovaAkceManager { get; private set; }

        /// <summary>
        /// Objekt zabezpečující správu předmětů (model)
        /// </summary>
        public PredmetManager PredmetManager { get; private set; }

        /// <summary>
        /// Objekt zabezpečující správu vyučujících (model)
        /// </summary>
        public VyucujiciManager VyucujiciManager { get; private set; }

        /// <summary>
        /// Objekt zabezpečující správu místností (model)
        /// </summary>
        public MistnostManager MistnostManager { get; private set; }


        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="ram"></param>
        /// <param name="pm"></param>
        /// <param name="vm"></param>
        /// <param name="mm"></param>
        public RozvrhovaAkceController(RozvrhovaAkceManager ram, PredmetManager pm, VyucujiciManager vm, MistnostManager mm)
            : base()
        {
            RozvrhovaAkceManager = ram;
            PredmetManager = pm;
            VyucujiciManager = vm;
            MistnostManager = mm;
        }


        /// <summary>
        /// Defaultní akce Controlleru (rozcestník)
        /// </summary>
        public void Default()
        {
            View.Render();
        }


        /// <summary>
        /// Akce Controlleru pro výpis rozvrhových akcí
        /// </summary>
        public void VypisRozvrhoveAkce()
        {
            View.Context.Add("rozvrhoveAkce", RozvrhovaAkceManager.RozvrhoveAkce);
            View.Context.Add("rozvrhovaAkceManager", RozvrhovaAkceManager);
            View.DeleteEvent += this.DeleteRozvrhovaAkce;
            //View.EditEvent += this.EditRozvrhovaAkce;
            View.Render();
        }


        /// <summary>
        /// Předá managerovi položku k odstranění
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="rozvrhovaAkce"></param>
        private void DeleteRozvrhovaAkce(object sender, Model rozvrhovaAkce)
        {
            RozvrhovaAkce ra = (RozvrhovaAkce)rozvrhovaAkce;
            RozvrhovaAkceManager.Delete(ra);
        }


        /// <summary>
        /// Akce Controlleru pro přidání rozvrhové akce
        /// </summary>
        public void PridejRozvrhovouAkci()
        {

            View.Context["rozvrhovaAkceForm"] = new RozvrhovaAkceForm(PredmetManager, VyucujiciManager, MistnostManager);
            View.SendEvent += this.SaveRozvrhovaAkce;
            View.Render();
        }


        /// <summary>
        /// Spracování formuláře pro tvorbu rozvrhové akce
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void SaveRozvrhovaAkce(object sender, Form args)
        {
            RozvrhovaAkceForm raf = (RozvrhovaAkceForm)args;
            if (raf.GetExit() == false)
            {
                int newId = RozvrhovaAkceManager.FindTopId() + 1;
                RozvrhovaAkce akce = new RozvrhovaAkce(newId, raf.GetPredmet(), raf.GetTypVyuky(), raf.GetVyucujici(), raf.GetMistnost(), raf.GetDen(), raf.GetZacatek(), raf.GetDelka());
                RozvrhovaAkceManager.Save(akce);
                foreach (KeyValuePair<int, StudijniSkupina> skupina in raf.GetStudijniSkupiny())
                {
                    RozvrhovaAkceManager.AddStudijniSkupina(akce, skupina.Value);
                }
            }
            else
            {
                // Formulář přerušen
            }

        }
    }
}