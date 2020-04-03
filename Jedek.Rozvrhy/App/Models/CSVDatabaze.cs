using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Jedek.Rozvrhy.Libs;
using System.IO;

namespace Jedek.Rozvrhy.App.Models
{
    class CSVDatabaze : Databaze
    {
        public CSVDatabaze()
        {
            Load();
        }

        public override void Load()
        {
            base.Load();
            LoadVyucujici();
            LoadPredmety();
            LoadObory();
            LoadStudenty();
            LoadMistnosti();
            LoadPredmetyOboru();
            LoadZapsanePredmety();
            LoadVyucujiciPredmetu();
            // Studijní skupiny
            LoadStudijniSkupiny();
            LoadStudentySkupin();
            // Rozvrhové akce
            LoadRozvrhoveAkce();
            LoadStudijniSkupinyRozvrhovychAkci();

            // helper pro první plnění, poté LoadZapsanePredmety
            //PrvnichX(7, 14);
        }


        /// <summary>
        /// Testování ukládání vyčlenit ze save do jednotlivých metod
        /// </summary>
        public override void Save()
        {
            SaveVyucujici();
            SaveStudenty();
            SaveZapsanePredmety();
            SavePredmety();
            SaveObory();
            SaveMistnosti();
            SavePredmetyOboru();
            SaveVyucujiciPredmetu();
            SaveStudijniSkupiny();
            SaveStudentySkupin();
            SaveRozvrhoveAkce();
            SaveStudijniSkupinyRozvrhovychAkci();
        }

        /***************************************************************************************************************************** Loading **/


        /// <summary>
        /// Naplní seznam vyučujících ze souboru CSV
        /// </summary>
        public override void LoadVyucujici()
        {
            string CSVfile = (@".\csv\vyucujici.csv");
            using (StreamReader sr = new StreamReader(CSVfile, Encoding.UTF8))
            {
                string line;
                int it = 1;
                while ((line = sr.ReadLine()) != null)
                {
                    if (it++ == 1) continue;
                    // id;prijmeni;jmeno;tituly;osobni_cislo;uzivatelske_jmeno;heslo;role
                    string[] explode = line.Split(';');
                    int id = int.Parse(explode[0]);
                    Vyucujici.Add(id, new Vyucujici(id, explode[2], explode[1], explode[3], explode[4], explode[5], explode[6], explode[7]));
                    //id, jmeno, prijmeni, tituly, osobniCislo, uzivatelskeJmeno, heslo, role
                }
            }
        }


        /// <summary>
        /// Naplní seznam předmětů ze souboru CSV
        /// </summary>
        public override void LoadPredmety()
        {
            string CSVfile = (@".\csv\predmety.csv");
            using (StreamReader sr = new StreamReader(CSVfile, Encoding.UTF8))
            {
                string line;
                int it = 1;
                while ((line = sr.ReadLine()) != null)
                {
                    if (it++ == 1) continue;
                    // id;zkratka;nazev;prednasky;cviceni;seminare
                    string[] explode = line.Split(';');
                    int id = int.Parse(explode[0]);
                    int hodPred = int.Parse(explode[3]);
                    int hodCvic = int.Parse(explode[4]);
                    int hodSem = int.Parse(explode[5]);
                    Predmety.Add(id, new Predmet(id, explode[1], explode[2], hodPred, hodCvic, hodSem));
                }
            }
        }


        /// <summary>
        /// Naplní seznam studijních oborů ze souboru CSV
        /// </summary>
        public override void LoadObory()
        {
            string CSVfile = (@".\csv\obory.csv");
            using (StreamReader sr = new StreamReader(CSVfile, Encoding.UTF8))
            {
                string line;
                int it = 1;
                while ((line = sr.ReadLine()) != null)
                {
                    if (it++ == 1) continue;
                    // id;nazev;zkratka
                    string[] explode = line.Split(';');
                    int id = int.Parse(explode[0]);
                    StudijniObory.Add(id, new StudijniObor(id, explode[1], explode[2]));
                }
            }
        }




        /// <summary>
        /// Naplní seznam studentů ze souboru CSV
        /// </summary>
        public override void LoadStudenty()
        {
            string CSVfile = (@".\csv\studenti.csv");
            using (StreamReader sr = new StreamReader(CSVfile, Encoding.UTF8))
            {
                string line;
                int it = 1;
                while ((line = sr.ReadLine()) != null)
                {
                    if (it++ == 1) continue;
                    // id; osobni_cislo; prijmeni; jmeno; obor_id; rocnik; uzivatelske_jmeno; heslo
                    string[] explode = line.Split(';');
                    int id = int.Parse(explode[0]);
                    string osobni_cislo = explode[1];
                    string prijmeni = explode[2];
                    string jmeno = explode[3];
                    int oborId = int.Parse(explode[4]);
                    int rocnik = int.Parse(explode[5]);
                    string uzivatelske_jmeno = explode[6];
                    string heslo = explode[7];
                    string role = explode[8];
                    Studenti.Add(id, new Student(id, jmeno, prijmeni, rocnik, StudijniObory[oborId], osobni_cislo, uzivatelske_jmeno, heslo, role));
                }
            }
        }


        /// <summary>
        /// Naplní seznam místností ze souboru CSV
        /// </summary>
        public override void LoadMistnosti()
        {
            string CSVfile = (@".\csv\mistnosti.csv");
            using (StreamReader sr = new StreamReader(CSVfile, Encoding.UTF8))
            {
                string line;
                int it = 1;
                while ((line = sr.ReadLine()) != null)
                {
                    if (it++ == 1) continue;
                    // id;budova;cislo;typ;kapacita
                    string[] explode = line.Split(';');
                    int id = int.Parse(explode[0]);
                    int budova = int.Parse(explode[1]);
                    int cislo = int.Parse(explode[2]);
                    int kapacita = int.Parse(explode[4]);

                    if (explode[3] == "PrednaskovaMistnost")
                    {
                        Mistnosti.Add(id, new PrednaskovaMistnost(id, budova, cislo, kapacita));
                    }
                    else if (explode[3] == "SeminarniMistnost")
                    {
                        Mistnosti.Add(id, new SeminarniMistnost(id, budova, cislo, kapacita));
                    }
                    else if (explode[3] == "PocitacovaMistnost")
                    {
                        Mistnosti.Add(id, new PocitacovaMistnost(id, budova, cislo, kapacita));
                    }
                }
            }
        }


        /// <summary>
        /// Naplní studijní obory předměty
        /// </summary>
        public override void LoadPredmetyOboru()
        {
            string CSVfile = (@".\csv\predmety_oboru.csv");
            using (StreamReader sr = new StreamReader(CSVfile, Encoding.UTF8))
            {
                string line;
                int it = 1;
                while ((line = sr.ReadLine()) != null)
                {
                    if (it++ == 1) continue;
                    // idOboru; idPredmetu
                    string[] explode = line.Split(';');
                    int idOboru = int.Parse(explode[0]);
                    int idPredmetu = int.Parse(explode[1]);
                    //var foo = this.Predmety.First(x => x.Value.Zkratka == zkratkaPredmetu);
                    StudijniObory[idOboru].PredmetyOboru.Add(idPredmetu, Predmety[idPredmetu]);
                }
            }
        }


        /// <summary>
        /// Zapíše studentům předměty, a naopak
        /// </summary>
        public override void LoadZapsanePredmety()
        {
            string CSVfile = (@".\csv\zapsane_predmety.csv");
            using (StreamReader sr = new StreamReader(CSVfile, Encoding.UTF8))
            {
                string line;
                int it = 1;
                while ((line = sr.ReadLine()) != null)
                {
                    if (it++ == 1) continue;
                    // idStudenta, idPredmetu
                    string[] explode = line.Split(';');
                    int idStudenta = int.Parse(explode[0]);
                    int idPredmetu = int.Parse(explode[1]);

                    Studenti[idStudenta].ZapsanePredmety.Add(idPredmetu, Predmety[idPredmetu]);
                    Predmety[idPredmetu].Studujici.Add(idStudenta, Studenti[idStudenta]);
                }
            }
        }


        /// <summary>
        /// Naplní vyučujícím jejich předměty a naopak
        /// </summary>
        public override void LoadVyucujiciPredmetu()
        {
            string CSVfile = (@".\csv\vyucujici_predmety.csv");
            using (StreamReader sr = new StreamReader(CSVfile, Encoding.UTF8))
            {
                string line;
                int it = 1;
                while ((line = sr.ReadLine()) != null)
                {
                    if (it++ == 1) continue;
                    // idVyucujici;idPredmet;forma
                    string[] explode = line.Split(';');
                    int idVyucujici = int.Parse(explode[0]);
                    int idPredmet = int.Parse(explode[1]);
                    string forma = explode[2];

                    if (forma == "Prednaska")
                    {
                        Vyucujici[idVyucujici].Prednasky.Add(idPredmet, Predmety[idPredmet]);
                        Predmety[idPredmet].Prednasejici.Add(idVyucujici, Vyucujici[idVyucujici]);
                    }
                    else if (forma == "Cviceni")
                    {
                        Vyucujici[idVyucujici].Cviceni.Add(idPredmet, Predmety[idPredmet]);
                        Predmety[idPredmet].Cvicici.Add(idVyucujici, Vyucujici[idVyucujici]);
                    }
                    else if (forma == "Seminar")
                    {
                        Vyucujici[idVyucujici].Seminare.Add(idPredmet, Predmety[idPredmet]);
                        Predmety[idPredmet].VedeSeminar.Add(idVyucujici, Vyucujici[idVyucujici]);
                    }
                }
            }
        }


        /// <summary>
        /// Vytvoří studijní skupiny pro předměty a v predmetech je zaregistruje
        /// </summary>
        public override void LoadStudijniSkupiny()
        {
            string CSVfile = (@".\csv\studijni_skupiny.csv");
            using (StreamReader sr = new StreamReader(CSVfile, Encoding.UTF8))
            {
                string line;
                int it = 1;
                while ((line = sr.ReadLine()) != null)
                {
                    if (it++ == 1) continue;
                    // skupinaId; predmetId
                    string[] explode = line.Split(';');
                    int idSkupiny = int.Parse(explode[0]);
                    int idPredmetu = int.Parse(explode[1]);

                    StudijniSkupiny.Add(idSkupiny, new StudijniSkupina(idSkupiny, Predmety[idPredmetu], new Dictionary<int, Student>()));
                    Predmety[idPredmetu].StudijniSkupiny.Add(idSkupiny, StudijniSkupiny[idSkupiny]);
                }
            }
        }


        /// <summary>
        /// Naplní studijní skupiny studenty a u studentů zaregistruje skupinu
        /// </summary>
        public override void LoadStudentySkupin()
        {
            string CSVfile = (@".\csv\studenti_skupiny.csv");
            using (StreamReader sr = new StreamReader(CSVfile, Encoding.UTF8))
            {
                string line;
                int it = 1;
                while ((line = sr.ReadLine()) != null)
                {
                    if (it++ == 1) continue;
                    // skupinaId; studentId
                    string[] explode = line.Split(';');
                    int idSkupiny = int.Parse(explode[0]);
                    int idStudenta = int.Parse(explode[1]);

                    StudijniSkupiny[idSkupiny].StudentiSkupiny.Add(idStudenta, Studenti[idStudenta]);
                    Studenti[idStudenta].StudijniSkupiny.Add(idSkupiny, StudijniSkupiny[idSkupiny]);
                }
            }
        }


        /// <summary>
        /// Vytvoří rozvrhové akce a zaregistruje je u vyučujících a místností
        /// </summary>
        public override void LoadRozvrhoveAkce()
        {
            string CSVfile = (@".\csv\rozvrhove_akce.csv");
            using (StreamReader sr = new StreamReader(CSVfile, Encoding.UTF8))
            {
                string line;
                int it = 1;
                while ((line = sr.ReadLine()) != null)
                {
                    if (it++ == 1) continue;
                    // id; predmetId; typVyuky; vyucujiciId; mistnostId; den; zacatek; delka
                    string[] explode = line.Split(';');
                    int id = int.Parse(explode[0]);
                    int idPredmetu = int.Parse(explode[1]);
                    TypyVyuky typ = (TypyVyuky)Enum.Parse(typeof(TypyVyuky), explode[2]);
                    int idVyucujiciho = int.Parse(explode[3]);
                    int idMistnosti = int.Parse(explode[4]);
                    Dny den = (Dny)Enum.Parse(typeof(Dny), explode[5]);
                    int zacatek = int.Parse(explode[6]);
                    int delka = int.Parse(explode[7]);

                    RozvrhovaAkce ra = new RozvrhovaAkce(id, Predmety[idPredmetu], typ, Vyucujici[idVyucujiciho], Mistnosti[idMistnosti], den, zacatek, delka);
                    RozvrhoveAkce.Add(id, ra);
                    Predmety[ra.Predmet.Id].RozvrhoveAkce.Add(ra.Id, ra);
                    for (int i = 0; i < delka; i++)
                    {
                        Vyucujici[idVyucujiciho].Rozvrh[den].Add(zacatek + i, ra);
                        Mistnosti[idMistnosti].Rozvrh[den].Add(zacatek + i, ra);
                    }
                }
            }
        }


        /// <summary>
        /// naplní rozvrhové akce studijními skupinami, a registrují se u skupin a jejich studentů
        /// </summary>
        public override void LoadStudijniSkupinyRozvrhovychAkci()
        {
            string CSVfile = (@".\csv\skupiny_akci.csv");
            using (StreamReader sr = new StreamReader(CSVfile, Encoding.UTF8))
            {
                string line;
                int it = 1;
                while ((line = sr.ReadLine()) != null)
                {
                    if (it++ == 1) continue;
                    // rozvrhovaAkceId; studijniSkupinaId
                    string[] explode = line.Split(';');
                    int idAkce = int.Parse(explode[0]);
                    int idSkupiny = int.Parse(explode[1]);

                    RozvrhoveAkce[idAkce].StudijniSkupiny.Add(idSkupiny, StudijniSkupiny[idSkupiny]);
                    for (int i = 0; i < RozvrhoveAkce[idAkce].Delka; i++)
                    {
                        StudijniSkupiny[idSkupiny].Rozvrh[RozvrhoveAkce[idAkce].Den].Add(RozvrhoveAkce[idAkce].Zacatek + i, RozvrhoveAkce[idAkce]);
                        foreach (KeyValuePair<int, Student> student in StudijniSkupiny[idSkupiny].StudentiSkupiny)
                        {
                            student.Value.Rozvrh[RozvrhoveAkce[idAkce].Den].Add(RozvrhoveAkce[idAkce].Zacatek + i, RozvrhoveAkce[idAkce]);
                        }
                    }
                }
            }
        }


        /***************************************************************************************************************************** Testing **/


        /// <summary>
        /// Testovací (plnící) metoda, zapíše každému studentovi prvních x předmětů vedených pro jeho obor
        /// Závisí na znalosti hodnot ID jednotlivých oborů
        /// </summary>
        public void PrvnichX(int x, int plusZaRocnik)
        {
            int jPlus = 0;
            var foo = Studenti.ToList();
            var bt = StudijniObory[1].PredmetyOboru.ToList();
            var irt = StudijniObory[2].PredmetyOboru.ToList();
            var ita = StudijniObory[3].PredmetyOboru.ToList();

            for (int i = 0; i < foo.Count; i++)
            {

                if (foo[i].Value.Rocnik == 1)
                {
                    // prvák
                    jPlus = 0;
                }

                if (foo[i].Value.Rocnik == 2)
                {
                    // druhák
                    jPlus = plusZaRocnik;
                }

                if (foo[i].Value.Rocnik == 3)
                {
                    // třeťák
                    jPlus = 2 * plusZaRocnik;
                }

                // BT
                if (foo[i].Value.StudijniObor.Id == 1)
                {
                    for (int j = 0; j < x; j++)
                    {
                        foo[i].Value.ZapsanePredmety.Add(bt[j + jPlus].Key, bt[j + jPlus].Value);
                        bt[j + jPlus].Value.Studujici.Add(foo[i].Key, foo[i].Value);
                    }
                }
                // IŘT
                if (foo[i].Value.StudijniObor.Id == 2)
                {
                    for (int j = 0; j < x; j++)
                    {
                        foo[i].Value.ZapsanePredmety.Add(irt[j + jPlus].Key, irt[j + jPlus].Value);
                        irt[j + jPlus].Value.Studujici.Add(foo[i].Key, foo[i].Value);
                    }
                }
                // ITA
                if (foo[i].Value.StudijniObor.Id == 3)
                {
                    for (int j = 0; j < x; j++)
                    {
                        foo[i].Value.ZapsanePredmety.Add(ita[j + jPlus].Key, ita[j + jPlus].Value);
                        ita[j + jPlus].Value.Studujici.Add(foo[i].Key, foo[i].Value);
                    }
                }
            }
        }

        /***************************************************************************************************************************** Saving **/


        /// <summary>
        /// Ukládání vyučujících do vyucujici.csv
        /// </summary>
        public override void SaveVyucujici()
        {
            //List<string> uJmena = new List<string>();
            //string uJmeno = String.Empty;

            using (StreamWriter sw = new StreamWriter(@".\csv\vyucujici.csv"))
            {
                sw.WriteLine("id;prijmeni;jmeno;tituly;osobni_cislo;uzivatelske_jmeno;heslo;role");
                foreach (KeyValuePair<int, Vyucujici> v in Vyucujici)
                {
                    // id; prijmeni; jmeno; tituly; osobni_cislo; uzivatelske_jmeno; heslo; role
                    string[] hodnoty = {
                                        v.Value.Id.ToString()
                                        , v.Value.Prijmeni.Trim()
                                        , v.Value.Jmeno
                                        , v.Value.Tituly
                                        , v.Value.OsobniCislo
                                        , v.Value.UzivatelskeJmeno
                                        , v.Value.Heslo
                                        , v.Value.Role
                                        };
                    string radek = String.Join(";", hodnoty);
                    sw.WriteLine(radek);
                }
                sw.Flush();
            }
        }


        /// <summary>
        /// Ukládání studentů do studenti.csv
        /// </summary>
        public override void SaveStudenty()
        {
            using (StreamWriter sw = new StreamWriter(@".\csv\studenti.csv")) // přidání na konec => druhý parametr true
            {
                sw.WriteLine("id;osobni_cislo;prijmeni;jmeno;obor_id;rocnik;uzivatelske_jmeno;heslo;role");
                foreach (KeyValuePair<int, Student> s in Studenti)
                {
                    string[] hodnoty = {
                                        s.Value.Id.ToString()
                                        , s.Value.OsobniCislo
                                        , s.Value.Prijmeni
                                        , s.Value.Jmeno
                                        , s.Value.StudijniObor.Id.ToString()
                                        , s.Value.Rocnik.ToString()
                                        , s.Value.UzivatelskeJmeno
                                        , s.Value.Heslo
                                        , s.Value.Role
                                        };
                    string radek = String.Join(";", hodnoty);
                    sw.WriteLine(radek);
                }
                sw.Flush();
            }
        }


        /// <summary>
        /// Ukládání předmětů do predmety.csv
        /// </summary>
        public override void SavePredmety()
        {
            using (StreamWriter sw = new StreamWriter(@".\csv\predmety.csv"))
            {
                sw.WriteLine("id;zkratka;nazev;prednasky;cviceni;seminare");
                foreach (KeyValuePair<int, Predmet> p in Predmety)
                {
                    // id; zkratka; nazev; prednasky; cviceni; seminare
                    string[] hodnoty = { p.Value.Id.ToString(), p.Value.Zkratka, p.Value.Nazev, p.Value.HodinPrednasek.ToString(), p.Value.HodinCviceni.ToString(), p.Value.HodinSeminaru.ToString() };
                    string radek = String.Join(";", hodnoty);
                    sw.WriteLine(radek);
                }
                sw.Flush();
            }
        }


        /// <summary>
        /// Ukládání oborů do obory.csv
        /// </summary>
        public override void SaveObory()
        {
            using (StreamWriter sw = new StreamWriter(@".\csv\obory.csv"))
            {
                sw.WriteLine("id;nazev;zkratka");
                foreach (KeyValuePair<int, StudijniObor> so in StudijniObory)
                {
                    // id; nazev; zkratka
                    string[] hodnoty = { so.Value.Id.ToString(), so.Value.Nazev, so.Value.Zkratka };
                    string radek = String.Join(";", hodnoty);
                    sw.WriteLine(radek);
                }
                sw.Flush();
            }
        }


        /// <summary>
        /// Ukládání místností do mistnosti.csv
        /// </summary>
        public override void SaveMistnosti()
        {
            using (StreamWriter sw = new StreamWriter(@".\csv\mistnosti.csv"))
            {
                sw.WriteLine("id;budova;cislo;typ;kapacita");
                foreach (KeyValuePair<int, Mistnost> m in Mistnosti)
                {
                    // id; budova; cislo; typ; kapacita
                    string[] hodnoty = { m.Value.Id.ToString(), m.Value.Budova.ToString(), m.Value.Cislo.ToString(), m.Value.GetType().Name, m.Value.Kapacita.ToString() };
                    string radek = String.Join(";", hodnoty);
                    sw.WriteLine(radek);
                }
                sw.Flush();
            }
        }


        /// <summary>
        /// Ukládání předmětů oboru do predmety_oboru.csv
        /// </summary>
        public override void SavePredmetyOboru()
        {
            using (StreamWriter sw = new StreamWriter(@".\csv\predmety_oboru.csv"))
            {
                sw.WriteLine("idOboru; idPredmetu");
                foreach (KeyValuePair<int, StudijniObor> so in StudijniObory)
                {
                    foreach (KeyValuePair<int, Predmet> p in so.Value.PredmetyOboru)
                    {
                        // idOboru; idPredmetu
                        string[] hodnoty = { so.Value.Id.ToString(), p.Value.Id.ToString() };
                        string radek = String.Join(";", hodnoty);
                        sw.WriteLine(radek);
                    }
                }
                sw.Flush();
            }
        }


        /// <summary>
        /// Ukládání zapsaných předmětů do zapsanePredmety.csv
        /// </summary>
        public override void SaveZapsanePredmety()
        {
            using (StreamWriter sw = new StreamWriter(@".\csv\zapsane_predmety.csv"))
            {
                sw.WriteLine("studentId;predmetId");
                foreach (KeyValuePair<int, Student> s in Studenti)
                {
                    foreach (KeyValuePair<int, Predmet> p in s.Value.ZapsanePredmety)
                    {
                        // studentId; predmetId
                        string[] hodnoty = { s.Value.Id.ToString(), p.Value.Id.ToString() };
                        string radek = String.Join(";", hodnoty);
                        sw.WriteLine(radek);
                    }
                }
                sw.Flush();
            }
        }


        /// <summary>
        /// Ukládání vyučujících předmětů do vyucujici_predmety.csv
        /// </summary>
        public override void SaveVyucujiciPredmetu()
        {
            using (StreamWriter sw = new StreamWriter(@".\csv\vyucujici_predmety.csv"))
            {
                sw.WriteLine("idVyucujici;idPredmet;forma");
                foreach (KeyValuePair<int, Vyucujici> v in Vyucujici)
                {
                    // přednášky
                    foreach (KeyValuePair<int, Predmet> p in v.Value.Prednasky)
                    {
                        // idVyucujici; idPredmet; forma
                        string[] hodnoty = { v.Value.Id.ToString(), p.Value.Id.ToString(), "Prednaska" };
                        string radek = String.Join(";", hodnoty);
                        sw.WriteLine(radek);
                    }
                    // cvičení
                    foreach (KeyValuePair<int, Predmet> p in v.Value.Cviceni)
                    {
                        // idVyucujici; idPredmet; forma
                        string[] hodnoty = { v.Value.Id.ToString(), p.Value.Id.ToString(), "Cviceni" };
                        string radek = String.Join(";", hodnoty);
                        sw.WriteLine(radek);
                    }
                    // semináře
                    foreach (KeyValuePair<int, Predmet> p in v.Value.Seminare)
                    {
                        // idVyucujici; idPredmet; forma
                        string[] hodnoty = { v.Value.Id.ToString(), p.Value.Id.ToString(), "Seminar" };
                        string radek = String.Join(";", hodnoty);
                        sw.WriteLine(radek);
                    }
                }
                sw.Flush();
            }
        }


        /// <summary>
        /// Ukládání studijních skupin do studijni_skupiny.csv
        /// </summary>
        public override void SaveStudijniSkupiny()
        {
            using (StreamWriter sw = new StreamWriter(@".\csv\studijni_skupiny.csv"))
            {
                sw.WriteLine("skupinaId;predmetId");
                foreach (KeyValuePair<int, StudijniSkupina> ss in StudijniSkupiny)
                {
                    // skupinaId; predmetId
                    string[] hodnoty = { ss.Value.Id.ToString(), ss.Value.Predmet.Id.ToString() };
                    string radek = String.Join(";", hodnoty);
                    sw.WriteLine(radek);
                }
                sw.Flush();
            }
        }


        /// <summary>
        /// Ukládání studentů skupin do studenti_skupiny.csv
        /// </summary>
        public override void SaveStudentySkupin()
        {
            using (StreamWriter sw = new StreamWriter(@".\csv\studenti_skupiny.csv"))
            {
                sw.WriteLine("skupinaId;studentId");
                foreach (KeyValuePair<int, StudijniSkupina> ss in StudijniSkupiny)
                {
                    foreach (KeyValuePair<int, Student> s in ss.Value.StudentiSkupiny)
                    {
                        // skupinaId; studentId
                        string[] hodnoty = { ss.Value.Id.ToString(), s.Value.Id.ToString() };
                        string radek = String.Join(";", hodnoty);
                        sw.WriteLine(radek);
                    }
                }
                sw.Flush();
            }
        }


        /// <summary>
        /// Ukládání rozvrhových akcí do rozvrhove_akce.csv
        /// </summary>
        public override void SaveRozvrhoveAkce()
        {
            using (StreamWriter sw = new StreamWriter(@".\csv\rozvrhove_akce.csv"))
            {
                sw.WriteLine("id;predmetId;typVyuky;vyucujiciId;mistnostId;den;zacatek;delka");
                foreach (KeyValuePair<int, RozvrhovaAkce> ra in RozvrhoveAkce)
                {
                    string[] hodnoty = { ra.Value.Id.ToString(), ra.Value.Predmet.Id.ToString(), ra.Value.TypVyuky.ToString(), ra.Value.Vyucujici.Id.ToString()
                                           , ra.Value.Mistnost.Id.ToString(), ra.Value.Den.ToString(), ra.Value.Zacatek.ToString(), ra.Value.Delka.ToString()};
                    string radek = String.Join(";", hodnoty);
                    sw.WriteLine(radek);
                }
                sw.Flush();
            }
        }


        /// <summary>
        /// Ukládání párů studijní skupia / rozvrhová akce
        /// </summary>
        public override void SaveStudijniSkupinyRozvrhovychAkci()
        {
            using (StreamWriter sw = new StreamWriter(@".\csv\skupiny_akci.csv"))
            {
                sw.WriteLine("rozvrhovaAkceId;studijniSkupinaId");
                foreach (KeyValuePair<int, RozvrhovaAkce> ra in RozvrhoveAkce)
                {
                    foreach (KeyValuePair<int, StudijniSkupina> ss in ra.Value.StudijniSkupiny)
                    {
                        string[] hodnoty = { ra.Value.Id.ToString(), ss.Value.Id.ToString() };
                        string radek = String.Join(";", hodnoty);
                        sw.WriteLine(radek);
                    }
                }
                sw.Flush();
            }
        }

    }
}
