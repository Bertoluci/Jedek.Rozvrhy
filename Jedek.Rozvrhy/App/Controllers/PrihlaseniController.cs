using Jedek.Rozvrhy.Libs;
using Jedek.Rozvrhy.App.Forms;

namespace Jedek.Rozvrhy.App.Controllers
{
    class PrihlaseniController : Controller
    {
        /// <summary>
        /// Akce Controleru pro přihlášení uživatele
        /// </summary>
        public void Default()
        {
            View.SendEvent += this.PrihlasUzivatele;
            View.Render();
        }


        /// <summary>
        /// Odhlášení uživatele
        /// </summary>
        public void OdhlasUzivatele()
        {
            Uzivatel.Odhlaseni();
        }


        /// <summary>
        /// Zpracovaní dat z přihlašovacího formuláře
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void PrihlasUzivatele(object sender, Form args)
        {
            PrihlaseniForm pf = (PrihlaseniForm)args;
            if(!Uzivatel.Prihlaseni(pf.GetUzivatelskeJmeno(), pf.GetHeslo()))
            {
                if (!View.Context.ContainsKey("chybaPrihlaseni"))
                {
                    this.View.Context.Add("chybaPrihlaseni", true);
                }
                this.Default();
            }
        }

    }
}
