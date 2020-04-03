using System;
using Jedek.Rozvrhy.Libs;

namespace Jedek.Rozvrhy.App.Models
{
    class Osoba : Model
    {
        public int Id { get; private set; }
        public string OsobniCislo { get; set; }
        public string Jmeno { get; set; }
        public string Prijmeni { get; set; }
        public string UzivatelskeJmeno { get; set; }
        public string Heslo { get; set; }
        public string Role { get; set; }

        public Osoba(int id, string jmeno, string prijmeni)
        {
            Id = id;
            Jmeno = jmeno;
            Prijmeni = prijmeni;
        }
 

        public Osoba(int id, string jmeno, string prijmeni, string osobniCislo, string uzivatelskeJmeno, string heslo, string role)
            : this(id, jmeno, prijmeni)
        {
            OsobniCislo = osobniCislo;
            UzivatelskeJmeno = uzivatelskeJmeno;
            Heslo = heslo;
            Role = role;
        }

        public override string ToString()
        {
            return String.Format("{0} {1}", Prijmeni, Jmeno);
        }
    }
}

