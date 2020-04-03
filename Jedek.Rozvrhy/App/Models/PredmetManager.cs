using System.Collections.Generic;
using Jedek.Rozvrhy.Libs;

namespace Jedek.Rozvrhy.App.Models
{
    class PredmetManager
    {
        /// <summary>
        /// Databáze aplikace
        /// </summary>
        public Databaze Databaze { get; private set; }


        /// <summary>
        /// Seznam předmětů
        /// </summary>
        public Dictionary<int, Predmet> Predmety { get; private set; }

        
        /// <summary>
        /// Constructor
        /// </summary>
        public PredmetManager(Databaze databaze)
        {
            Predmety = databaze.Predmety;
            Databaze = databaze;
            // testovací
            //LoadDataFromCSV();
            //PrintCSV();
        }


        /// <summary>
        /// Uloží nového, nebo editovaného předmětu
        /// </summary>
        /// <param name="predmet"></param>
        public void Save(Predmet predmet)
        {
            if (Predmety.ContainsKey(predmet.Id))
            {
                // editace stávajícího
                Predmety[predmet.Id] = predmet;
            }
            else
            {
                // ukládání nového
                Predmety.Add(predmet.Id, predmet);
            }
        }


        /// <summary>
        /// Vymaže záznam o předmětu
        /// </summary>
        /// <param name="predmet"></param>
        public void Delete(Predmet predmet)
        {
            Predmety.Remove(predmet.Id);
        }


        /// <summary>
        /// Prohledá záznamy a vrátí nejvyšší Id
        /// </summary>
        /// <returns>int</returns>
        public int FindTopId()
        {
            int topId = 0;
            foreach (KeyValuePair<int, Predmet> predmet in Predmety)
            {
                int cislo = predmet.Key;
                if (predmet.Key > topId)
                {
                    topId = predmet.Key; // predmet.key == predmet.Value.Id
                }
            }
            return topId;
        }


        /// <summary>
        /// Přidá přednášejícího k danému předmětu
        /// </summary>
        /// <param name="predmet"></param>
        /// <param name="vyucujici"></param>
        public void AddPrednasejiciho(Predmet predmet, Vyucujici vyucujici)
        {
            Predmety[predmet.Id].Prednasejici.Add(vyucujici.Id, vyucujici);
            Databaze.Vyucujici[vyucujici.Id].Prednasky.Add(predmet.Id, predmet);
        }


        /// <summary>
        /// Odebere přednášejícího z předmětu
        /// </summary>
        /// <param name="predmet"></param>
        /// <param name="vyucujici"></param>
        public void RemovePrednasejiciho(Predmet predmet, Vyucujici vyucujici)
        {
            Predmety[predmet.Id].Prednasejici.Remove(vyucujici.Id);
            Databaze.Vyucujici[vyucujici.Id].Prednasky.Remove(predmet.Id);
        }


        /// <summary>
        /// Přidá cvičícího k danému předmětu
        /// </summary>
        /// <param name="predmet"></param>
        /// <param name="vyucujici"></param>
        public void AddCviciciho(Predmet predmet, Vyucujici vyucujici)
        {
            Predmety[predmet.Id].Cvicici.Add(vyucujici.Id, vyucujici);
            Databaze.Vyucujici[vyucujici.Id].Cviceni.Add(predmet.Id, predmet);
        }


        /// <summary>
        /// Odebere cvičícího z předmětu
        /// </summary>
        /// <param name="predmet"></param>
        /// <param name="vyucujici"></param>
        public void RemoveCviciciho(Predmet predmet, Vyucujici vyucujici)
        {
            Predmety[predmet.Id].Cvicici.Remove(vyucujici.Id);
            Databaze.Vyucujici[vyucujici.Id].Cviceni.Remove(predmet.Id);
        }


        /// <summary>
        /// Přidá vedoucího semináře k danému předmětu
        /// </summary>
        /// <param name="predmet"></param>
        /// <param name="vyucujici"></param>
        public void AddVedeSeminar(Predmet predmet, Vyucujici vyucujici)
        {
            Predmety[predmet.Id].VedeSeminar.Add(vyucujici.Id, vyucujici);
            Databaze.Vyucujici[vyucujici.Id].Seminare.Add(predmet.Id, predmet);
        }


        /// <summary>
        /// Odebere vedoucího semináře z předmětu
        /// </summary>
        /// <param name="predmet"></param>
        /// <param name="vyucujici"></param>
        public void RemoveVedeSeminar(Predmet predmet, Vyucujici vyucujici)
        {
            Predmety[predmet.Id].VedeSeminar.Remove(vyucujici.Id);
            Databaze.Vyucujici[vyucujici.Id].Seminare.Remove(predmet.Id);
        }

    }
}
