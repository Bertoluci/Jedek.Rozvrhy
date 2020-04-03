using Jedek.Rozvrhy.Libs;

namespace Jedek.Rozvrhy.App.Controllers
{
    class SettingController : Controller
    {
        /// <summary>
        /// Objekt zajištující tvorbu objektů (DI)
        /// </summary>
        public Container Container { get; private set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="container"></param>
        public SettingController(Container container)
            : base()
        {
            Container = container;
        }

        /// <summary>
        /// Defaultní akce Controlleru
        /// </summary>
        public void Default()
        {
            View.Context.Add("parameters", Container.Parameters);
            View.Render();
        }
    }
}
