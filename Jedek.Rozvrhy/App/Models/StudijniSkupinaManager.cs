using System.Collections.Generic;
using System.Linq;
using Jedek.Rozvrhy.Libs;

namespace Jedek.Rozvrhy.App.Models
{
    class StudijniSkupinaManager
    {
        /// <summary>
        /// Databáze aplikace
        /// </summary>
        public Databaze Databaze { get; private set; }


        /// <summary>
        /// Seznam studijních skupin
        /// </summary>
        public Dictionary<int, StudijniSkupina> StudijniSkupiny { get; private set; }


        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="databaze"></param>
        public StudijniSkupinaManager(Databaze databaze)
        {
            StudijniSkupiny = databaze.StudijniSkupiny;
            Databaze = databaze;
        }


        /// <summary>
        /// Metoda pro tvorbu studijních skupin na základě zapsaných studentů na předměty
        /// </summary>
        public void VytvorSkupinyDlePredmetu()
        {
            int idSkupiny = 0;
            // čitač skupin, slouží i pro id skupiny
            int skupina = 1;
            int size = 12;
            var predmety = Databaze.Predmety.ToList();

            for (int i = 0; i < predmety.Count; i++)
            {
                var studenti = predmety[i].Value.Studujici.Values.ToList();
                var serazeniStudenti = studenti.OrderBy(s => s.Rocnik).ThenBy(s => s.StudijniObor.Id).ToList();
                int iterator = 0;
                // algoritmus pro výpočet optimálního počtu skupin a studentů v ní obsažených
                int count = serazeniStudenti.Count;
                int pocetSkupin = (count / size) + 1;
                if (count % size == 0)
                {
                    pocetSkupin--;
                }
                pocetSkupin = pocetSkupin == 0 ? 1 : pocetSkupin;
                int zaklad = count / pocetSkupin;
                int pocetNavyseni = count % pocetSkupin;
                int navyseni = pocetNavyseni-- > 0 ? 1 : 0;

                for (int j = 0; j < count; j++)
                {
                    if (iterator == 0)
                    {
                        idSkupiny = skupina++;
                        // vytvoření nové skupiny
                        StudijniSkupiny.Add(idSkupiny, new StudijniSkupina(idSkupiny, predmety[i].Value, new Dictionary<int, Student>()));
                        // zaregistrování skupiny v předmětu, pro který byla vytvořena
                        Databaze.Predmety[predmety[i].Key].StudijniSkupiny.Add(idSkupiny, StudijniSkupiny[idSkupiny]);
                    }
                    // cyklické přidávání studentů skupině dle optimálního počtu / skupina u studenta
                    StudijniSkupiny[idSkupiny].StudentiSkupiny.Add(serazeniStudenti[j].Id, serazeniStudenti[j]);
                    serazeniStudenti[j].StudijniSkupiny.Add(idSkupiny, StudijniSkupiny[idSkupiny]);
                    iterator++;

                    // skupina naplněna požadovaným počtem studentů
                    if (iterator == zaklad + navyseni)
                    {
                        navyseni = pocetNavyseni > 0 ? 1 : 0;
                        pocetNavyseni--;
                        iterator = 0;
                    }
                }
            }
        }


        /// <summary>
        /// Odstraní studijní skupinu
        /// </summary>
        /// <param name="studijniSkupina"></param>
        public void Delete(StudijniSkupina studijniSkupina)
        {
            foreach (KeyValuePair<int, Student> student in studijniSkupina.StudentiSkupiny)
            {
                student.Value.StudijniSkupiny.Remove(studijniSkupina.Id);
            }
            Databaze.Predmety[studijniSkupina.Predmet.Id].StudijniSkupiny.Remove(studijniSkupina.Id);
            StudijniSkupiny.Remove(studijniSkupina.Id);
        }


        /// <summary>
        /// Odebere studijní skupině studenta / studentovi skupinu
        /// </summary>
        /// <param name="skupina"></param>
        /// <param name="student"></param>
        public void RemoveStudenta(StudijniSkupina skupina, Student student)
        {
            StudijniSkupiny[skupina.Id].StudentiSkupiny.Remove(student.Id);
            Databaze.Studenti[student.Id].StudijniSkupiny.Remove(skupina.Id);
        }


        /// <summary>
        /// Přidá do studijní skupiny studenta / studentovi skupinu
        /// </summary>
        /// <param name="skupina"></param>
        /// <param name="student"></param>
        public void AddStudenta(StudijniSkupina skupina, Student student)
        {
            StudijniSkupiny[skupina.Id].StudentiSkupiny.Add(student.Id, student);
            Databaze.Studenti[student.Id].StudijniSkupiny.Add(skupina.Id, skupina);
        }

    }
}
