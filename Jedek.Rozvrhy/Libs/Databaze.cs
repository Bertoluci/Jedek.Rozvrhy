using System;
using System.Collections.Generic;
using System.Text;
using Jedek.Rozvrhy.App.Models;
using System.Security.Cryptography;

namespace Jedek.Rozvrhy.Libs
{
    abstract class Databaze : IDatabaze
    {
        /// <summary>
        /// Databázová kolekce vyučujících
        /// </summary>
        public Dictionary<int, Vyucujici> Vyucujici { get; protected set; }


        /// <summary>
        /// Databázová kolekce předmětů
        /// </summary>
        public Dictionary<int, Predmet> Predmety { get; protected set; }


        /// <summary>
        /// Databázová kolekce studijních oborů
        /// </summary>
        public Dictionary<int, StudijniObor> StudijniObory { get; protected set; }


        /// <summary>
        /// Databázová kolekce studentů
        /// </summary>
        public Dictionary<int, Student> Studenti { get; protected set; }


        /// <summary>
        /// Databázová kolekce studijních skupin
        /// </summary>
        public Dictionary<int, StudijniSkupina> StudijniSkupiny { get; protected set; }


        /// <summary>
        /// Databázová kolekce rozvrhových akcí
        /// </summary>
        public Dictionary<int, RozvrhovaAkce> RozvrhoveAkce { get; protected set; }


        /// <summary>
        /// Databázová kolekce místností
        /// </summary>
        public Dictionary<int, Mistnost> Mistnosti { get; protected set; }


        public virtual void Load()
        {
            Vyucujici = new Dictionary<int, Vyucujici>();
            Predmety = new Dictionary<int, Predmet>();
            StudijniObory = new Dictionary<int, StudijniObor>();
            Studenti = new Dictionary<int, Student>();
            StudijniSkupiny = new Dictionary<int, StudijniSkupina>();
            RozvrhoveAkce = new Dictionary<int, RozvrhovaAkce>();
            Mistnosti = new Dictionary<int, Mistnost>();
        }


        /******************************************************************************************************************* Autentizace */

        /// <summary>
        /// Pokusí se najít uživatele a nastavit mu identitu
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <param name="uzivatel"></param>
        /// <returns></returns>
        public virtual bool NajdiUzivatele(string username, string password, Uzivatel uzivatel)
        {
            string pass = vratHash(password);
            // mezi studenty
            foreach (KeyValuePair<int, Student> s in Studenti)
            {
                
                if (s.Value.UzivatelskeJmeno == username && s.Value.Heslo == pass)
                {
                    uzivatel.Id = s.Value.Id;
                    uzivatel.OsobniCislo = s.Value.OsobniCislo;
                    uzivatel.Jmeno = s.Value.Jmeno;
                    uzivatel.Prijmeni = s.Value.Prijmeni;
                    uzivatel.UzivatelskeJmeno = s.Value.UzivatelskeJmeno;
                    uzivatel.Heslo = s.Value.Heslo;
                    Role role = Role.host;
                    if (Enum.IsDefined(typeof(Role), s.Value.Role))
                    {
                        bool parse = Enum.TryParse(s.Value.Role, out role);
                    }
                    uzivatel.Role = role;
                    return true;
                }
            }
            // mezi vyučujícími
            foreach (KeyValuePair<int, Vyucujici> v in Vyucujici)
            {
                if (v.Value.UzivatelskeJmeno == username && v.Value.Heslo == pass)
                {
                    uzivatel.Id = v.Value.Id;
                    uzivatel.OsobniCislo = v.Value.OsobniCislo;
                    uzivatel.Jmeno = v.Value.Jmeno;
                    uzivatel.Prijmeni = v.Value.Prijmeni;
                    uzivatel.UzivatelskeJmeno = v.Value.UzivatelskeJmeno;
                    uzivatel.Heslo = v.Value.Heslo;
                    Role role = Role.host;
                    if (Enum.IsDefined(typeof(Role), v.Value.Role))
                    {
                        bool parse = Enum.TryParse(v.Value.Role, out role);
                    }
                    uzivatel.Role = role;
                    return true;
                }
            }
            return false;
        }


        // hash
        public string vratHash(string text)
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

        /****************************************************************************************************************** Autentizace **/

        public virtual void LoadVyucujici() { }
        public virtual void LoadPredmety() { }
        public virtual void LoadObory() { }
        public virtual void LoadStudenty() { }
        public virtual void LoadMistnosti() { }
        public virtual void LoadPredmetyOboru() { }
        public virtual void LoadZapsanePredmety() { }
        public virtual void LoadVyucujiciPredmetu() { }
        public virtual void LoadStudijniSkupiny() { }
        public virtual void LoadStudentySkupin() { }
        public virtual void LoadRozvrhoveAkce() { }
        public virtual void LoadStudijniSkupinyRozvrhovychAkci() { }

        public virtual void Save() { }
        public virtual void SaveVyucujici() { }
        public virtual void SaveStudenty() { }
        public virtual void SaveZapsanePredmety() { }
        public virtual void SavePredmety() { }
        public virtual void SaveObory() { }
        public virtual void SaveMistnosti() { }
        public virtual void SavePredmetyOboru() { }
        public virtual void SaveVyucujiciPredmetu() { }
        public virtual void SaveStudijniSkupiny() { }
        public virtual void SaveStudentySkupin() { }
        public virtual void SaveRozvrhoveAkce() { }
        public virtual void SaveStudijniSkupinyRozvrhovychAkci() { }
    }
}
