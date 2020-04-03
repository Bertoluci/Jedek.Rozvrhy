using System.Collections.Generic;
using Jedek.Rozvrhy.Libs;

namespace Jedek.Rozvrhy.App.Models
{
    class MistnostManager : Model
    {
        /// <summary>
        /// Databáze aplikace
        /// </summary>
        public Databaze Databaze { get; private set; }


        /// <summary>
        /// Seznam místností
        /// </summary>
        public Dictionary<int, Mistnost> Mistnosti { get; private set; }


        /// <summary>
        /// Constructor
        /// </summary>
        public MistnostManager(Databaze databaze)
        {
            Mistnosti = databaze.Mistnosti;
            Databaze = databaze;
        }

        
        /// <summary>
        /// Uloží novou, nebo editovanou místnost
        /// </summary>
        /// <param name="mistnost"></param>
        public void Save(Mistnost mistnost)
        {
            if(Mistnosti.ContainsKey(mistnost.Id))
            {
                // editace stávajícího
                Mistnosti[mistnost.Id] = mistnost;
            }
            else
            {
                // ukládání nového
                Mistnosti.Add(mistnost.Id, mistnost);
            }  
        }


        /// <summary>
        /// Vymaže záznam o místnosti
        /// </summary>
        /// <param name="mistnost"></param>
        public void Delete(Mistnost mistnost)
        {
            Mistnosti.Remove(mistnost.Id);
        }


        /// <summary>
        /// Prohledá záznamy a vrátí nejvyšší Id
        /// </summary>
        /// <returns></returns>
        public int FindTopId()
        {
            int topId = 0;
            foreach (KeyValuePair<int, Mistnost> mistnost in Mistnosti)
            {
                int cislo = mistnost.Key;
                if (mistnost.Key > topId)
                {
                    topId = mistnost.Key; // mistnost.key == mistnost.Value.Id
                }
            }
            return topId;
        }

    }
}
