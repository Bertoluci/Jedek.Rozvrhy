using System;
using System.Collections.Generic;
using System.Text;
using Jedek.Rozvrhy.Libs;
using Jedek.Rozvrhy.App.Models;
using System.Security.Cryptography;

namespace Jedek.Rozvrhy.App.Controllers
{
    class VyucujiciController : Controller
    {
        /// <summary>
        /// Objekt zabezpečující správu vyučujících (model)
        /// </summary>
        public VyucujiciManager VyucujiciManager { get; private set; }


        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="vyucujiciManager"></param>
        public VyucujiciController(VyucujiciManager vyucujiciManager)
            : base()
        {
            VyucujiciManager = vyucujiciManager;
        }


        /// <summary>
        /// Defaultní akce Controlleru (rozcestník)
        /// </summary>
        public void Default()
        {
            View.Render();
        }


        /// <summary>
        /// Akce Controlleru pro výpis, editaci a mazání vyučujících
        /// </summary>
        public void VypisVyucujici()
        {
            View.Context.Add("vyucujici", VyucujiciManager.Vyucujici);
            View.DeleteEvent += this.DeleteVyucujici;
            View.EditEvent += this.EditVyucujici;
            View.Render();
        }

        
        /// <summary>
        /// Akce Controlleru pro přidání nového vyučujícího
        /// </summary>
        public void PridejVyucujiciho()
        {
            View.SendEvent += this.SaveVyucujici;
            View.Render();
        }


        /// <summary>
        /// Předá VyucujiciManagerovi položku, která se má smazat
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="vyucujici"></param>
        private void DeleteVyucujici(object sender, Model vyucujici)
        {
            Vyucujici s = (Vyucujici)vyucujici;
            VyucujiciManager.Delete(s);
        }


        /// <summary>
        /// Zpracuje data z formuláře a předá je VyucujiciManagerovi
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void SaveVyucujici(object sender, Forms.Form args)
        {
            Forms.VyucujiciForm vf = (Forms.VyucujiciForm)args;
            int newId = VyucujiciManager.FindTopId() + 1;

            List<string> uJmena = new List<string>();
            foreach (KeyValuePair<int, Vyucujici> v in VyucujiciManager.Vyucujici)
            {
                uJmena.Add(v.Value.UzivatelskeJmeno);
            }
            foreach (KeyValuePair<int, Student> s in VyucujiciManager.Databaze.Studenti)
            {
                uJmena.Add(s.Value.UzivatelskeJmeno);
            }
            string os_cislo = VratOsobniCislo(newId.ToString());

            VyucujiciManager.Save(new Vyucujici(newId, vf.GetJmeno(), vf.GetPrijmeni(), vf.GetTituly(), os_cislo, VratUzivatelskeJmeno(vf.GetJmeno(), vf.GetPrijmeni(), uJmena), VratHash(os_cislo), "vyucujici"));
            // ctor: Vyucujici(int id, string jmeno, string prijmeni, string tituly, string osobniCislo, string uzivatelskeJmeno, string heslo, string role)
        }


        /// <summary>
        /// Zpracuje data s editačního formuláře a předá je VyucujiciManagerovi
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void EditVyucujici(object sender, Forms.Form args)
        {
            Forms.EditVyucujiciForm evf = (Forms.EditVyucujiciForm)args;

            if (evf.GetJmeno() != String.Empty)
            {
                evf.Vyucujici.Jmeno = evf.GetJmeno();
            }

            if (evf.GetPrijmeni() != String.Empty)
            {
                evf.Vyucujici.Prijmeni = evf.GetPrijmeni();
            }

            if (evf.GetTituly() != String.Empty)
            {
                evf.Vyucujici.Tituly = evf.GetTituly();
            }

            VyucujiciManager.Save(evf.Vyucujici);

        }

        /************************************************************************************************************ start helperů */

        /// <summary>
        /// Vytvoří osobní číslo vyučujícího z jeho Id
        /// </summary>
        /// <param name="idVyucujiciho"></param>
        /// <returns></returns>
        private string VratOsobniCislo(string idVyucujiciho)
        {
            string osCislo = idVyucujiciho;
            char pad = '0';
            osCislo = osCislo.PadLeft(5, pad);
            return osCislo;
        }


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