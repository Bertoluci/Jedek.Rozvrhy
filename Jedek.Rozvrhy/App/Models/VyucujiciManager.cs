using System.Collections.Generic;
using Jedek.Rozvrhy.Libs;

namespace Jedek.Rozvrhy.App.Models
{
    class VyucujiciManager
    {
        /// <summary>
        /// Databáze aplikace
        /// </summary>
        public Databaze Databaze { get; private set; }


        /// <summary>
        /// Seznam vyučujících
        /// </summary>
        public Dictionary<int, Vyucujici> Vyucujici { get; private set; }

        
        /// <summary>
        /// Constructor
        /// </summary>
        public VyucujiciManager(Databaze databaze)
        {
            Vyucujici = databaze.Vyucujici;
            Databaze = databaze;
            // testovací
            //LoadDataFromCSV();  
            //PrintCSV();
        }


        /// <summary>
        /// Uloží nového, nebo editovaného vyučujícího
        /// </summary>
        /// <param name="vyucujici"></param>
        public void Save(Vyucujici vyucujici)
        {
            if (Vyucujici.ContainsKey(vyucujici.Id))
            {
                // editace stávajícího
                Vyucujici[vyucujici.Id] = vyucujici;
            }
            else
            {
                // ukládání nového
                Vyucujici.Add(vyucujici.Id, vyucujici);
            }
        }


        /// <summary>
        /// Vymaže záznam vyučujícího
        /// </summary>
        /// <param name="vyucujici"></param>
        public void Delete(Vyucujici vyucujici)
        {
            Vyucujici.Remove(vyucujici.Id);
        }


        /// <summary>
        /// Prohledá záznamy a vrátí nejvyšší Id
        /// </summary>
        /// <returns></returns>
        public int FindTopId()
        {
            int topId = 0;
            foreach (KeyValuePair<int, Vyucujici> vyucujici in Vyucujici)
            {
                int cislo = vyucujici.Key;
                if (vyucujici.Key > topId)
                {
                    topId = vyucujici.Key; // vyucujici.key == vyucujici.Value.Id
                }
            }
            return topId;
        }

    }
}