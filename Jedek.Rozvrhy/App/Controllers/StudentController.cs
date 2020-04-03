using System;
using System.Collections.Generic;
using System.Text;
using Jedek.Rozvrhy.Libs;
using Jedek.Rozvrhy.App.Models;
using Jedek.Rozvrhy.App.Forms;
using System.Security.Cryptography;

namespace Jedek.Rozvrhy.App.Controllers
{
    class StudentController : Controller
    {
        /// <summary>
        /// Objekt zabezpečující správu studentů (model)
        /// </summary>
        public StudentManager StudentManager { get; private set; }


        /// <summary>
        /// Objekt zabezpečující správu studijních oborů (model)
        /// </summary>
        public StudijniOborManager StudijniOborManager { get; private set; }


        /// <summary>
        /// Instance aktuálního studenta pro správu zapsaných předmětů
        /// </summary>
        public Student Student { get; private set; }


        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="studentManager"></param>
        public StudentController(StudentManager studentManager, StudijniOborManager studijniOborManager)
            : base()
        {
            StudentManager = studentManager;
            StudijniOborManager = studijniOborManager;
        }


        /// <summary>
        /// Defaultní akce Controlleru (rozcestník)
        /// </summary>
        public void Default()
        {
            View.Render();
        }


        /// <summary>
        /// Akce Controlleru pro výpis, editaci a mazání studentů
        /// </summary>
        public void VypisStudenty()
        {
            View.Context.Add("studenti", StudentManager.Studenti);
            View.Context.Add("studijniOborManager", StudijniOborManager);
            View.DeleteEvent += this.DeleteStudent;
            View.EditEvent += this.EditStudent;
            View.Render();
        }


        /// <summary>
        /// Akce Controlleru pro přidání nového studenta
        /// </summary>
        public void PridejStudenta()
        {
            if (StudijniOborManager.StudijniObory.Count == 0)
            {
                View.Context.Add("message", "Nelze vytvořit studenta, pokud neexistuje alespoň jeden studijní obor.");
                View.Render();
            }
            else
            {
                View.Context["studentForm"] = new StudentForm(StudijniOborManager);
                View.SendEvent += this.SaveStudent;
                View.Render();
            }

        }


        /// <summary>
        /// Akce Controlleru pro správu zapsaných předmětů studenta
        /// </summary>
        /// <param name="student"></param>
        public void PredmetyStudenta(Student student)
        {
            Student = student;
            View.Context.Add("student", Student);
            View.AddEvent += AddPredmetStudenta;
            View.DeleteEvent += RemovePredmetStudenta;
            View.Render();
        }


        /// <summary>
        /// Předá StudentManagerovi predmet, který má být studentovi odebrán
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="predmet"></param>
        private void RemovePredmetStudenta(object sender, Model predmet)
        {
            Predmet p = (Predmet)predmet;
            StudentManager.RemovePredmet(Student, p);
        }


        /// <summary>
        /// Předá StudentManagerovi predmet, který má být studentovi přidán
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="predmet"></param>
        private void AddPredmetStudenta(object sender, Model predmet)
        {
            Predmet p = (Predmet)predmet;
            StudentManager.AddPredmet(Student, p);
        }


        /// <summary>
        /// Předá StudentManagerovi položku, která se má smazat
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="student"></param>
        private void DeleteStudent(object sender, Model student)
        {
            Student s = (Student)student;
            StudentManager.Delete(s);
        }


        /// <summary>
        /// Zpracuje data z formuláře a předá je StudentManagerovi
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void SaveStudent(object sender, Forms.Form args)
        {
            Forms.StudentForm sf = (Forms.StudentForm)args;
            int newId = StudentManager.FindTopId() + 1;

            List<string> uJmena = new List<string>();
            foreach (KeyValuePair<int, Vyucujici> v in StudentManager.Databaze.Vyucujici)
            {
                uJmena.Add(v.Value.UzivatelskeJmeno);
            }
            foreach (KeyValuePair<int, Student> s in StudentManager.Studenti)
            {
                uJmena.Add(s.Value.UzivatelskeJmeno);
            }
            string os_cislo = sf.GetOsobniCislo();

            StudentManager.Save(new Student(newId, sf.GetJmeno(), sf.GetPrijmeni(), sf.GetRocnik(), sf.GetStudijniObor(), os_cislo, VratUzivatelskeJmeno(sf.GetJmeno(), sf.GetPrijmeni(), uJmena), VratHash(os_cislo), "student"));
            // ctor: Student(int id, string jmeno, string prijmeni, int rocnik, StudijniObor studijniObor, string osobniCislo, string uzivatelskeJmeno, string heslo, string role)
        }


        /// <summary>
        /// Zpracuje data s editačního formuláře a předá je StudentManagerovi
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void EditStudent(object sender, Forms.Form args)
        {
            Forms.EditStudentForm esf = (Forms.EditStudentForm)args;

            if (esf.GetJmeno() != String.Empty)
            {
                esf.Student.Jmeno = esf.GetJmeno();
            }

            if (esf.GetPrijmeni() != String.Empty)
            {
                esf.Student.Prijmeni = esf.GetPrijmeni();
            }

            if (esf.GetOsobniCislo() != String.Empty)
            {
                esf.Student.OsobniCislo = esf.GetOsobniCislo();
            }

            if (esf.GetStudijniObor() != null)
            {
                esf.Student.StudijniObor = esf.GetStudijniObor();
            }

            if (esf.GetRocnik() != 0)
            {
                esf.Student.Rocnik = esf.GetRocnik();
            }

            StudentManager.Save(esf.Student);

        }

        /************************************************************************************************************ start helperů */

        /// <summary>
        /// Vytvoří unikátní uživatelské jméno
        /// </summary>
        /// <param name="jmeno"></param>
        /// <param name="prijmeni"></param>
        /// <param name="uJmena"></param>
        /// <returns></returns>
        private string VratUzivatelskeJmeno(string jmeno, string prijmeni, List<string> uJmena)
        {
            string uzivatelskeJmeno = String.Format("{0}_{1}", jmeno[0].ToString().ToLower(), prijmeni.ToLower());
            uzivatelskeJmeno = OdstranDiakritiku(uzivatelskeJmeno);
            while (uJmena.Contains(uzivatelskeJmeno))
            {
                uzivatelskeJmeno = PridejPoradoveCislo(uzivatelskeJmeno);
            }
            return uzivatelskeJmeno;
        }


        /// <summary>
        /// Vrátí hash ze stringu
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        private string VratHash(string text)
        {
            using (SHA1Managed sha1 = new SHA1Managed())
            {
                var hash = sha1.ComputeHash(Encoding.UTF8.GetBytes(text));
                var sb = new StringBuilder(hash.Length * 2);

                foreach (byte b in hash)
                {
                    // "x2" = lowercase
                    sb.Append(b.ToString("x2"));
                }
                return sb.ToString();
            }
        }


        /// <summary>
        /// Odstraní diakritiku ze stringu
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string OdstranDiakritiku(string text)
        {
            string stringFormD = text.Normalize(System.Text.NormalizationForm.FormD);
            System.Text.StringBuilder retVal = new System.Text.StringBuilder();
            for (int index = 0; index < stringFormD.Length; index++)
            {
                if (System.Globalization.CharUnicodeInfo.GetUnicodeCategory(stringFormD[index]) != System.Globalization.UnicodeCategory.NonSpacingMark)
                    retVal.Append(stringFormD[index]);
            }
            return retVal.ToString().Normalize(System.Text.NormalizationForm.FormC);
        }
        

        /// <summary>
        /// Přidává k uživatelskému jménu pořadové číslo pro potlačení duplicit
        /// </summary>
        /// <param name="ujmeno"></param>
        /// <returns></returns>
        private string PridejPoradoveCislo(string ujmeno)
        {
            string zaklad = String.Empty;
            string textCisla = String.Empty;
            int val = 0;

            for (int i = 0; i < ujmeno.Length; i++)
            {
                if (Char.IsDigit(ujmeno[i]))
                {
                    textCisla += ujmeno[i];
                }
                else
                {
                    zaklad += ujmeno[i];
                }
            }

            if (textCisla.Length > 0)
            {
                val = int.Parse(textCisla);
                val++;
            }
            else
            {
                val = 2;
            }

            return zaklad.Trim() + val.ToString();
        }
        /********************************************************************************************************** konec helperů **/
    }
}
