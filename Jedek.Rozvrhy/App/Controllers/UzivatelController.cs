using Jedek.Rozvrhy.Libs;
using Jedek.Rozvrhy.App.Models;

namespace Jedek.Rozvrhy.App.Controllers
{
    class UzivatelController : Controller
    {
        /// <summary>
        /// Objekt zabezpečující správu uživatelů (model)
        /// </summary>
        public UzivatelManager UzivatelManager { get; private set; }


        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="uzivatelManager"></param>
        public UzivatelController(UzivatelManager uzivatelManager)
            : base()
        {
            UzivatelManager = uzivatelManager;
        }


        /// <summary>
        /// Defaultní akce Controlleru (rozcestník)
        /// </summary>
        public void Default()
        {

            if(Uzivatel.Role == Role.student)
            {
                View.Context.Add("rozvrh", UzivatelManager.Databaze.Studenti[Uzivatel.Id].Rozvrh);
                View.Context.Add("student", UzivatelManager.Databaze.Studenti[Uzivatel.Id]);
            }
            else
            {
                View.Context.Add("rozvrh", UzivatelManager.Databaze.Vyucujici[Uzivatel.Id].Rozvrh);
                View.Context.Add("vyucujici", UzivatelManager.Databaze.Vyucujici[Uzivatel.Id]);
            }
            View.Render();
        }
    }
}
