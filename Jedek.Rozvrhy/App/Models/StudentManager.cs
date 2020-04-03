using System.Collections.Generic;
using Jedek.Rozvrhy.Libs;

namespace Jedek.Rozvrhy.App.Models
{
    class StudentManager
    {
        /// <summary>
        /// Databáze aplikace
        /// </summary>
        public Databaze Databaze { get; private set; }


        /// <summary>
        /// Seznam studentů
        /// </summary>
        public Dictionary<int, Student> Studenti { get; private set; }


        /// <summary>
        /// Constructor
        /// </summary>
        public StudentManager(Databaze databaze)
        {
            Studenti = databaze.Studenti;
            Databaze = databaze;
            // testovací
            //LoadDataFromCSV();
            //PrintCSV();
        }


        /// <summary>
        /// Uloží nového, nebo editovaného studenta
        /// </summary>
        /// <param name="student"></param>
        public void Save(Student student)
        {
            if (Studenti.ContainsKey(student.Id))
            {
                // editace stávajícího
                Studenti[student.Id] = student;
            }
            else
            {
                // ukládání nového
                Studenti.Add(student.Id, student);
            }
        }


        /// <summary>
        /// Vymaže záznam studenta
        /// </summary>
        /// <param name="student"></param>
        public void Delete(Student student)
        {
            Studenti.Remove(student.Id);
        }


        /// <summary>
        /// Prohledá záznamy a vrátí nejvyšší Id
        /// </summary>
        /// <returns>int</returns>
        public int FindTopId()
        {
            int topId = 0;
            foreach (KeyValuePair<int, Student> student in Studenti)
            {
                //int cislo = student.Key;
                if (student.Key > topId)
                {
                    topId = student.Key; // student.key == student.Value.Id
                }
            }
            return topId;
        }

        
        /// <summary>
        /// Zapíše studentovi předmět
        /// </summary>
        /// <param name="student"></param>
        /// <param name="predmet"></param>
        public void AddPredmet(Student student, Predmet predmet)
        {
            Studenti[student.Id].ZapsanePredmety.Add(predmet.Id, predmet);
            Databaze.Predmety[predmet.Id].Studujici.Add(student.Id, student);
        }


        /// <summary>
        /// Odebere studentovi zapsaný předmět
        /// </summary>
        /// <param name="student"></param>
        /// <param name="predmet"></param>
        public void RemovePredmet(Student student, Predmet predmet)
        {
            Studenti[student.Id].ZapsanePredmety.Remove(predmet.Id);
            Databaze.Predmety[predmet.Id].Studujici.Remove(student.Id);
        }

    }
}
