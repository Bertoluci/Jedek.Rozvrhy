using Jedek.Rozvrhy.Libs;

namespace Jedek.Rozvrhy.App.Controllers
{
    class ShutdownController : Controller
    {
        /// <summary>
        /// Objekt databáze
        /// </summary>
        public Databaze Databaze { get; private set; }


        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="databaze"></param>
        public ShutdownController(Databaze databaze)
            : base()
        {
            Databaze = databaze;
        }


        /// <summary>
        /// Uložení zmen a ukončení aplikace
        /// </summary>
        public void Default()
        {
            Databaze.Save();
            View.Render();
        }

    }
}
