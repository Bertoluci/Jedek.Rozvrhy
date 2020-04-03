using System.Collections.Generic;
using Jedek.Rozvrhy.Libs;

namespace Jedek.Rozvrhy.App.Models
{
    class RozvrhovaAkceManager
    {
        /// <summary>
        /// Databáze aplikace
        /// </summary>
        public Databaze Databaze { get; private set; }


        /// <summary>
        /// Seznam rozvrhových akcí
        /// </summary>
        public Dictionary<int, RozvrhovaAkce> RozvrhoveAkce { get; private set; }


        /// <summary>
        /// Connstructor
        /// </summary>
        /// <param name="databaze"></param>
        public RozvrhovaAkceManager(Databaze databaze)
        {
            RozvrhoveAkce = databaze.RozvrhoveAkce;
            Databaze = databaze;
        }


        /// <summary>
        /// Uloží novou, nebo edituje stávající rozvrhovou akci
        /// </summary>
        /// <param name="rozvrhovaAkce"></param>
        public void Save(RozvrhovaAkce rozvrhovaAkce)
        {
            if (RozvrhoveAkce.ContainsKey(rozvrhovaAkce.Id))
            {
                // editace stávající, neimplementováno
                RozvrhoveAkce[rozvrhovaAkce.Id] = rozvrhovaAkce;
            }
            else
            {
                // ukládání nové
                RozvrhoveAkce.Add(rozvrhovaAkce.Id, rozvrhovaAkce);
                // registrace u předmětu
                Databaze.Predmety[rozvrhovaAkce.Predmet.Id].RozvrhoveAkce.Add(rozvrhovaAkce.Id, rozvrhovaAkce);
                for (int i = 0; i < rozvrhovaAkce.Delka; i++)
                {
                    Databaze.Vyucujici[rozvrhovaAkce.Vyucujici.Id].Rozvrh[rozvrhovaAkce.Den].Add(rozvrhovaAkce.Zacatek + i, rozvrhovaAkce);
                    Databaze.Mistnosti[rozvrhovaAkce.Mistnost.Id].Rozvrh[rozvrhovaAkce.Den].Add(rozvrhovaAkce.Zacatek + i, rozvrhovaAkce);
                }
            }
        }


        /// <summary>
        /// Vymaže záznam rozvrhové akce
        /// </summary>
        /// <param name="rozvrhovaAkce"></param>
        public void Delete(RozvrhovaAkce rozvrhovaAkce)
        {

            foreach (KeyValuePair<int, StudijniSkupina> skupina in rozvrhovaAkce.StudijniSkupiny)
            {
                for (int i = 0; i < rozvrhovaAkce.Delka; i++)
                {
                    foreach (KeyValuePair<int, Student> student in skupina.Value.StudentiSkupiny)
                    {
                        student.Value.Rozvrh[rozvrhovaAkce.Den].Remove(rozvrhovaAkce.Zacatek + i);
                    }
                    Databaze.StudijniSkupiny[skupina.Value.Id].Rozvrh[rozvrhovaAkce.Den].Remove(rozvrhovaAkce.Zacatek + i);
                }
            }
            RozvrhoveAkce[rozvrhovaAkce.Id].StudijniSkupiny.Clear();


            for (int i = 0; i < rozvrhovaAkce.Delka; i++)
            {
                Databaze.Vyucujici[rozvrhovaAkce.Vyucujici.Id].Rozvrh[rozvrhovaAkce.Den].Remove(rozvrhovaAkce.Zacatek + i);
                Databaze.Mistnosti[rozvrhovaAkce.Mistnost.Id].Rozvrh[rozvrhovaAkce.Den].Remove(rozvrhovaAkce.Zacatek + i);
            }
            Databaze.Predmety[rozvrhovaAkce.Predmet.Id].RozvrhoveAkce.Remove(rozvrhovaAkce.Id);
            RozvrhoveAkce.Remove(rozvrhovaAkce.Id);
        }


        /// <summary>
        /// Prohledá záznamy a vrátí nejvyšší Id
        /// </summary>
        /// <returns>int</returns>
        public int FindTopId()
        {
            int topId = 0;
            foreach (KeyValuePair<int, RozvrhovaAkce> akce in RozvrhoveAkce)
            {
                //int cislo = akce.Key;
                if (akce.Key > topId)
                {
                    topId = akce.Key; // akce.key == akce.Value.Id
                }
            }
            return topId;
        }


        /// <summary>
        /// Přidá do RA studijní skupinu, skupině a jejim studentům přidá RA do rozvrhu
        /// </summary>
        /// <param name="akce"></param>
        /// <param name="skupina"></param>
        public void AddStudijniSkupina(RozvrhovaAkce akce, StudijniSkupina skupina)
        {
            RozvrhoveAkce[akce.Id].StudijniSkupiny.Add(skupina.Id, skupina);
            for (int i = 0; i < akce.Delka; i++)
            {
                Databaze.StudijniSkupiny[skupina.Id].Rozvrh[akce.Den].Add(akce.Zacatek + i, akce);
                foreach (KeyValuePair<int, Student> student in skupina.StudentiSkupiny)
                {
                    student.Value.Rozvrh[akce.Den].Add(akce.Zacatek + i, akce);
                }
            }
                
        }

    }
}
