using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Jedek.Rozvrhy.Libs;

namespace Jedek.Rozvrhy.App.Models
{
    class UzivatelManager : Model
    {
        /// <summary>
        /// Databáze aplikace
        /// </summary>
        public Databaze Databaze { get; private set; }


        public UzivatelManager(Databaze databaze)
        {
            Databaze = databaze;
        }


        /// <summary>
        /// Nastaví identitu uživateli, nebo vrací false (předá konkrétní databázi)
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <param name="uzivatel"></param>
        /// <returns></returns>
        public bool NastavUzivatele(string username, string password, Uzivatel uzivatel)
        {
            return Databaze.NajdiUzivatele(username, password, uzivatel) ? true : false;
        }

    }
}
