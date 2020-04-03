using Jedek.Rozvrhy.Libs;
using Jedek.Rozvrhy.App.Models;

namespace Jedek.Rozvrhy.App.Controllers
{
    class StudijniSkupinaController : Controller
    {
        /// <summary>
        /// Objekt zabezpečující správu studijních skupin (model)
        /// </summary>
        public StudijniSkupinaManager StudijniSkupinaManager { get; private set; }


        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="studijniSkupinaManager"></param>
        public StudijniSkupinaController(StudijniSkupinaManager studijniSkupinaManager)
            : base()
        {
            StudijniSkupinaManager = studijniSkupinaManager;
        }


        /// <summary>
        /// Instance aktuální skupiny pro správu zapsaných studentů
        /// </summary>
        public StudijniSkupina Skupina { get; private set; }


        /// <summary>
        /// Defaultní akce Controlleru
        /// </summary>
        public void Default()
        {
            View.Render();
        }


        /// <summary>
        /// Akce Controlleru pro výpis, editaci a mazání studijních skupin
        /// </summary>
        public void VypisStudijniSkupinyPredmetu()
        {
            View.Context.Add("skupiny", StudijniSkupinaManager.StudijniSkupiny);
            View.DeleteEvent += this.DeleteStudijniSkupinu;
            View.Render();
        }


        /// <summary>
        /// Předá StudijniSkupinaManagerovi skupinu, která má být odstraněna
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="studijniSkupina"></param>
        private void DeleteStudijniSkupinu(object sender, Model studijniSkupina)
        {
            StudijniSkupina ss = (StudijniSkupina)studijniSkupina;
            StudijniSkupinaManager.Delete(ss);
        }


        /// <summary>
        /// Akce Controlleru pro výpis, editaci a mazání studentů studijní skupiny
        /// </summary>
        /// <param name="skupina"></param>
        public void StudentiSkupiny(StudijniSkupina skupina)
        {
            Skupina = skupina;
            View.Context.Add("skupina", skupina);
            View.Context.Add("studenti", StudijniSkupinaManager.Databaze.Studenti);
            View.AddEvent += AddStudentaSkupiny;
            View.DeleteEvent += RemoveStudentaSkupiny;
            View.Render();
        }


        /// <summary>
        /// Předá StudijniSkupinaManagerovi studenta, který má být skupině odebrán
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="student"></param>
        private void RemoveStudentaSkupiny(object sender, Model student)
        {
            Student s = (Student)student;
            StudijniSkupinaManager.RemoveStudenta(Skupina, s);
        }


        /// <summary>
        /// Předá StudijniSkupinaManagerovi studenta, který má být skupině přidán
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="student"></param>
        private void AddStudentaSkupiny(object sender, Model student)
        {
            Student s = (Student)student;
            StudijniSkupinaManager.AddStudenta(Skupina, s);
        }


        /// <summary>
        /// Akce Controlleru pro vytvoření studijních skupin
        /// </summary>
        public void VytvorStudijniSkupiny()
        {
            if (StudijniSkupinaManager.StudijniSkupiny.Count != 0)
            {
                View.Context.Add("message", "Studijní skupiny jsou již vytvořeny.");
            }
            else
            {
                StudijniSkupinaManager.VytvorSkupinyDlePredmetu();
                if (StudijniSkupinaManager.StudijniSkupiny.Count == 0)
                {
                    View.Context.Add("message", "Nelze vytvořit Studijní skupiny, pokud neexistuje alespoň jeden zapsaný předmět.");
                }
                else
                {
                    View.Context["studijniSkupiny"] = StudijniSkupinaManager.StudijniSkupiny;
                }
            }
            View.Render();
        }

    }
}
