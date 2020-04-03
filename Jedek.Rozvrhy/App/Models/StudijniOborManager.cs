using System;
using System.Collections.Generic;
using Jedek.Rozvrhy.Libs;

namespace Jedek.Rozvrhy.App.Models
{
    class StudijniOborManager
    {
        /// <summary>
        /// Databáze aplikace
        /// </summary>
        public Databaze Databaze { get; private set; }
        
        
        /// <summary>
        /// Seznam studijnícj oborů
        /// </summary>
        public Dictionary<int, StudijniObor> StudijniObory { get; private set; }


        /// <summary>
        /// Constructor
        /// </summary>
        public StudijniOborManager(Databaze databaze)
        {
            StudijniObory = databaze.StudijniObory;
            Databaze = databaze;
            // testovací
            //LoadDataFromCSV();
            //PrintCSV();
        }


        /// <summary>
        /// Uloží nového, nebo editovaného studijního oboru
        /// </summary>
        /// <param name="obor"></param>
        public void Save(StudijniObor obor)
        {
            if (StudijniObory.ContainsKey(obor.Id))
            {
                // editace stávajícího
                StudijniObory[obor.Id] = obor;
            }
            else
            {
                // ukládání nového
                StudijniObory.Add(obor.Id, obor);
            }
        }


        /// <summary>
        /// Vymaže záznam studijního oboru
        /// </summary>
        /// <param name="obor"></param>
        public void Delete(StudijniObor obor)
        {
            foreach (var student in Databaze.Studenti)
            {
                if(student.Value.StudijniObor.Id == obor.Id)
                {
                    throw new InvalidOperationException("Obor nelze smazat, je studován");
                }
            }
            StudijniObory.Remove(obor.Id);
        }


        /// <summary>
        /// Prohledá záznamy a vrátí nejvyšší Id
        /// </summary>
        /// <returns>int</returns>
        public int FindTopId()
        {
            int topId = 0;
            foreach (KeyValuePair<int, StudijniObor> obor in StudijniObory)
            {
                int cislo = obor.Key;
                if (obor.Key > topId)
                {
                    topId = obor.Key; // obor.key == obor.Value.Id
                }
            }
            return topId;
        }


        /// <summary>
        /// Přidá předmět do studijního oboru
        /// </summary>
        /// <param name="obor"></param>
        /// <param name="predmet"></param>
        public void AddPredmet(StudijniObor obor, Predmet predmet)
        {
            StudijniObory[obor.Id].PredmetyOboru.Add(predmet.Id, predmet);
        }


        /// <summary>
        /// Odebere předmět ze studijního oboru
        /// </summary>
        /// <param name="obor"></param>
        /// <param name="predmet"></param>
        public void RemovePredmet(StudijniObor obor, Predmet predmet)
        {
            StudijniObory[obor.Id].PredmetyOboru.Remove(predmet.Id);
        }

    }
}
