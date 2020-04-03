using System;
using Jedek.Rozvrhy.App.Models;

namespace Jedek.Rozvrhy.Libs
{
    enum Role
    {
        host = 1, student, vyucujici, admin
    }
    class Uzivatel
    {
        /// <summary>
        /// Objekt pro správu uživatelů
        /// </summary>
        public UzivatelManager UzivatelManager { get; private set; }

        // data uživatele
        public int Id { get; set; }
        public string OsobniCislo { get; set; }
        public string Jmeno { get; set; }
        public string Prijmeni { get; set; }
        public string UzivatelskeJmeno { get; set; }
        public string Heslo { get; set; }
        public Role Role { get; set; }
        public bool JePrihlasen { get; private set; }


        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="uzivatelManager"></param>
        public Uzivatel(UzivatelManager uzivatelManager)
        {
            UzivatelManager = uzivatelManager;
            Id = 0;
            OsobniCislo = string.Empty;
            Jmeno = string.Empty;
            Prijmeni = string.Empty;
            UzivatelskeJmeno = string.Empty;
            Heslo = string.Empty;
            JePrihlasen = false;
            Role = Role.host;
        }


        /// <summary>
        /// Přihlásí uživatele proti přihlašujícím údajům
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public bool Prihlaseni(string username, string password)
        {
            if(UzivatelManager.NastavUzivatele(username, password, this))
            {
                JePrihlasen = true;
                return true;
            }
            return false;
        }


        /// <summary>
        /// Odhlásí uživatele
        /// </summary>
        public void Odhlaseni()
        {
            Role = Role.host;
            JePrihlasen = false;
        }


        public override string ToString()
        {
            return String.Format("{0} {1}", Jmeno, Prijmeni);
        }
    }
}
