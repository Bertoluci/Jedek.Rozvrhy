using System.Collections.Generic;
using Jedek.Rozvrhy.Libs;
using Jedek.Rozvrhy.App.Models;

namespace Jedek.Rozvrhy.App.Controllers
{
    class MenuController : Controller
    {
        /// <summary>
        /// Objekt zabezpečující správu menu (model)
        /// </summary>
        public MenuManager MenuManager { get; private set; }


        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="menuManager"></param>
        public MenuController(MenuManager menuManager)
            : base()
        {
            MenuManager = menuManager;
        }


        /// <summary>
        /// Akce Controlleru pro vypsání menu
        /// </summary>
        public void Default()
        {
            Dictionary<string, MenuItem> items = MenuManager.Items;
            View.Context.Add("items", items);
            View.Render();
        }
    }
}
