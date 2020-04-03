using System;
using System.Collections.Generic;
using System.Text;
using Jedek.Rozvrhy.Libs;
using System.Xml;
using System.IO;

namespace Jedek.Rozvrhy.App.Models
{
    class XMLDatabaze : Databaze
    {
        public XmlWriterSettings Settings { get; private set; }

        public XMLDatabaze()
        {
            Load();
            Settings = new XmlWriterSettings();
            Settings.Indent = true;
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


            //LoadVyucujiciFromCSV();
            //LoadPredmetyFromCSV();
            //LoadOboryFromCSV();
            //LoadStudentyFromCSV();
            //LoadMistnostiFromCSV();
            //LoadPredmetyOboruFromCSV();
            //LoadZapsanePredmetyFromCSV();
            //LoadVyucujiciPredmetuFromCSV();
            //// Studijní skupiny
            //LoadStudijniSkupinyFromCSV();
            //LoadStudentySkupinFromCSV();
            //// Rozvrhové akce
            //LoadRozvrhoveAkceFromCSV();
            //LoadStudijniSkupinyRozvrhovychAkciFromCSV();
        }


        public override void Save()
        {
            SaveStudenty();
            SaveZapsanePredmety();
            SaveVyucujici();
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


        public override void SaveStudenty() // id, osobni_cislo, prijmeni, jmeno, obor_id, rocnik, uzivatelske_jmeno, heslo, role
        {
            using (XmlWriter xw = XmlWriter.Create(@".\xml\studenti.xml", Settings))
            {
                xw.WriteStartDocument();
                xw.WriteStartElement("studenti");

                foreach (KeyValuePair<int, Student> student in Studenti)
                {
                    xw.WriteStartElement("student");
                    xw.WriteAttributeString("id", student.Key.ToString());

                    xw.WriteElementString("osobni_cislo", student.Value.OsobniCislo);
                    xw.WriteElementString("prijmeni", student.Value.Prijmeni);
                    xw.WriteElementString("jmeno", student.Value.Jmeno);
                    xw.WriteElementString("obor_id", student.Value.StudijniObor.Id.ToString());
                    xw.WriteElementString("rocnik", student.Value.Rocnik.ToString());
                    xw.WriteElementString("uzivatelske_jmeno", student.Value.UzivatelskeJmeno);
                    xw.WriteElementString("heslo", student.Value.Heslo);
                    xw.WriteElementString("role", student.Value.Role);

                    xw.WriteEndElement();
                }
                xw.WriteEndElement();
                xw.WriteEndDocument();
                xw.Flush();
            }
        }


        public override void SaveZapsanePredmety() // studentId, predmetId
        {
            using (XmlWriter xw = XmlWriter.Create(@".\xml\zapsane_predmety.xml", Settings))
            {
                xw.WriteStartDocument();
                xw.WriteStartElement("zapsane_predmety");

                foreach (KeyValuePair<int, Student> student in Studenti)
                {
                    foreach (KeyValuePair<int, Predmet> predmet in student.Value.ZapsanePredmety)
                    {
                        xw.WriteStartElement("student_predmet");
                        xw.WriteElementString("studentId", student.Value.Id.ToString());
                        xw.WriteElementString("predmetId", predmet.Value.Id.ToString());
                        xw.WriteEndElement();
                    }
                }
                xw.WriteEndElement();
                xw.WriteEndDocument();
                xw.Flush();
            }
        }


        public override void SaveVyucujici() // id, prijmeni, jmeno, tituly, osobni_cislo, uzivatelske_jmeno, heslo, role
        {
            using (XmlWriter xw = XmlWriter.Create(@".\xml\vyucujici.xml", Settings))
            {
                xw.WriteStartDocument();
                xw.WriteStartElement("vyucujici");

                foreach (KeyValuePair<int, Vyucujici> ucitel in Vyucujici)
                {
                    xw.WriteStartElement("ucitel");
                    xw.WriteAttributeString("id", ucitel.Key.ToString());

                    xw.WriteElementString("prijmeni", ucitel.Value.Prijmeni);
                    xw.WriteElementString("jmeno", ucitel.Value.Jmeno);
                    xw.WriteElementString("tituly", ucitel.Value.Tituly);
                    xw.WriteElementString("osobni_cislo", ucitel.Value.OsobniCislo);
                    xw.WriteElementString("uzivatelske_jmeno", ucitel.Value.UzivatelskeJmeno);
                    xw.WriteElementString("heslo", ucitel.Value.Heslo);
                    xw.WriteElementString("role", ucitel.Value.Role);

                    xw.WriteEndElement();
                }
                xw.WriteEndElement();
                xw.WriteEndDocument();
                xw.Flush();
            }
        }


        public override void SavePredmety() // id, zkratka, nazev, prednasky, cviceni, seminare
        {
            using (XmlWriter xw = XmlWriter.Create(@".\xml\predmety.xml", Settings))
            {
                xw.WriteStartDocument();
                xw.WriteStartElement("predmety");

                foreach (KeyValuePair<int, Predmet> predmet in Predmety)
                {
                    xw.WriteStartElement("predmet");
                    xw.WriteAttributeString("id", predmet.Key.ToString());

                    xw.WriteElementString("zkratka", predmet.Value.Zkratka);
                    xw.WriteElementString("nazev", predmet.Value.Nazev);
                    xw.WriteElementString("prednasky", predmet.Value.HodinPrednasek.ToString());
                    xw.WriteElementString("cviceni", predmet.Value.HodinCviceni.ToString());
                    xw.WriteElementString("seminare", predmet.Value.HodinSeminaru.ToString());

                    xw.WriteEndElement();
                }
                xw.WriteEndElement();
                xw.WriteEndDocument();
                xw.Flush();
            }
        }


        public override void SaveObory() // id, nazev, zkratka
        {
            using (XmlWriter xw = XmlWriter.Create(@".\xml\obory.xml", Settings))
            {
                xw.WriteStartDocument();
                xw.WriteStartElement("obory");

                foreach (KeyValuePair<int, StudijniObor> obor in StudijniObory)
                {
                    xw.WriteStartElement("obor");
                    xw.WriteAttributeString("id", obor.Key.ToString());

                    xw.WriteElementString("nazev", obor.Value.Nazev);
                    xw.WriteElementString("zkratka", obor.Value.Zkratka);

                    xw.WriteEndElement();
                }
                xw.WriteEndElement();
                xw.WriteEndDocument();
                xw.Flush();
            }
        }


        public override void SaveMistnosti() // id, budova, cislo, typ, kapacita
        {
            using (XmlWriter xw = XmlWriter.Create(@".\xml\mistnosti.xml", Settings))
            {
                xw.WriteStartDocument();
                xw.WriteStartElement("mistnosti");

                foreach (KeyValuePair<int, Mistnost> mistnost in Mistnosti)
                {
                    xw.WriteStartElement("mistnost");
                    xw.WriteAttributeString("id", mistnost.Key.ToString());

                    xw.WriteElementString("budova", mistnost.Value.Budova.ToString());
                    xw.WriteElementString("cislo", mistnost.Value.Cislo.ToString());
                    xw.WriteElementString("typ", mistnost.Value.GetType().Name);
                    xw.WriteElementString("kapacita", mistnost.Value.Kapacita.ToString());

                    xw.WriteEndElement();
                }
                xw.WriteEndElement();
                xw.WriteEndDocument();
                xw.Flush();
            }
        }


        public override void SavePredmetyOboru() // idOboru, idPredmetu
        {
            using (XmlWriter xw = XmlWriter.Create(@".\xml\predmety_oboru.xml", Settings))
            {
                xw.WriteStartDocument();
                xw.WriteStartElement("predmety_oboru");

                foreach (KeyValuePair<int, StudijniObor> obor in StudijniObory)
                {
                    foreach (KeyValuePair<int, Predmet> predmet in obor.Value.PredmetyOboru)
                    {
                        xw.WriteStartElement("obor_predmet");
                        xw.WriteElementString("oborId", obor.Value.Id.ToString());
                        xw.WriteElementString("predmetId", predmet.Value.Id.ToString());
                        xw.WriteEndElement();
                    }
                }
                xw.WriteEndElement();
                xw.WriteEndDocument();
                xw.Flush();
            }
        }


        public override void SaveVyucujiciPredmetu() // idVyucujici, idPredmet, forma
        {
            using (XmlWriter xw = XmlWriter.Create(@".\xml\vyucujici_predmety.xml", Settings))
            {
                xw.WriteStartDocument();
                xw.WriteStartElement("vyucujici_predmety");

                foreach (KeyValuePair<int, Vyucujici> ucitel in Vyucujici)
                {
                    // Přednášky
                    foreach (KeyValuePair<int, Predmet> predmet in ucitel.Value.Prednasky)
                    {
                        xw.WriteStartElement("vyucujici_predmet_forma");

                        xw.WriteElementString("vyucujiciId", ucitel.Value.Id.ToString());
                        xw.WriteElementString("predmetId", predmet.Value.Id.ToString());
                        xw.WriteElementString("forma", "Prednaska");

                        xw.WriteEndElement();
                    }
                    // Cvičení
                    foreach (KeyValuePair<int, Predmet> predmet in ucitel.Value.Cviceni)
                    {
                        xw.WriteStartElement("vyucujici_predmet_forma");

                        xw.WriteElementString("vyucujiciId", ucitel.Value.Id.ToString());
                        xw.WriteElementString("predmetId", predmet.Value.Id.ToString());
                        xw.WriteElementString("forma", "Cviceni");

                        xw.WriteEndElement();
                    }
                    // Přednášky
                    foreach (KeyValuePair<int, Predmet> predmet in ucitel.Value.Seminare)
                    {
                        xw.WriteStartElement("vyucujici_predmet_forma");

                        xw.WriteElementString("vyucujiciId", ucitel.Value.Id.ToString());
                        xw.WriteElementString("predmetId", predmet.Value.Id.ToString());
                        xw.WriteElementString("forma", "Seminar");

                        xw.WriteEndElement();
                    }
                }
                xw.WriteEndElement();
                xw.WriteEndDocument();
                xw.Flush();
            }
        }


        public override void SaveStudijniSkupiny() // skupinaId, predmetId
        {
            using (XmlWriter xw = XmlWriter.Create(@".\xml\studijni_skupiny.xml", Settings))
            {
                xw.WriteStartDocument();
                xw.WriteStartElement("studijni_skupiny");

                foreach (KeyValuePair<int, StudijniSkupina> skupina in StudijniSkupiny)
                {
                    xw.WriteStartElement("skupina_predmet");

                    xw.WriteElementString("skupinaId", skupina.Value.Id.ToString());
                    xw.WriteElementString("predmetId", skupina.Value.Predmet.Id.ToString());

                    xw.WriteEndElement();
                }
                xw.WriteEndElement();
                xw.WriteEndDocument();
                xw.Flush();
            }
        }


        public override void SaveStudentySkupin() // skupinaId, studentId
        {
            using (XmlWriter xw = XmlWriter.Create(@".\xml\studenti_skupiny.xml", Settings))
            {
                xw.WriteStartDocument();
                xw.WriteStartElement("studenti_skupiny");

                foreach (KeyValuePair<int, StudijniSkupina> skupina in StudijniSkupiny)
                {
                    foreach (KeyValuePair<int, Student> student in skupina.Value.StudentiSkupiny)
                    {
                        xw.WriteStartElement("skupina_student");
                        xw.WriteElementString("skupinaId", skupina.Value.Id.ToString());
                        xw.WriteElementString("studentId", student.Value.Id.ToString());
                        xw.WriteEndElement();
                    }
                }
                xw.WriteEndElement();
                xw.WriteEndDocument();
                xw.Flush();
            }
        }


        public override void SaveRozvrhoveAkce() // id, predmetId, typVyuky, vyucujiciId, mistnostId, den, zacatek, delka
        {
            using (XmlWriter xw = XmlWriter.Create(@".\xml\rozvrhove_akce.xml", Settings))
            {
                xw.WriteStartDocument();
                xw.WriteStartElement("rozvrhove_akce");

                foreach (KeyValuePair<int, RozvrhovaAkce> akce in RozvrhoveAkce)
                {
                    xw.WriteStartElement("akce");
                    xw.WriteAttributeString("id", akce.Key.ToString());

                    xw.WriteElementString("predmetId", akce.Value.Predmet.Id.ToString());
                    xw.WriteElementString("typ", akce.Value.TypVyuky.ToString());
                    xw.WriteElementString("vyucujiciId", akce.Value.Vyucujici.Id.ToString());
                    xw.WriteElementString("mistnostId", akce.Value.Mistnost.Id.ToString());
                    xw.WriteElementString("den", akce.Value.Den.ToString());
                    xw.WriteElementString("zacatek", akce.Value.Zacatek.ToString());
                    xw.WriteElementString("delka", akce.Value.Delka.ToString());

                    xw.WriteEndElement();
                }
                xw.WriteEndElement();
                xw.WriteEndDocument();
                xw.Flush();
            }
        }


        public override void SaveStudijniSkupinyRozvrhovychAkci() // rozvrhovaAkceId, studijniSkupinaId
        {
            using (XmlWriter xw = XmlWriter.Create(@".\xml\skupiny_akci.xml", Settings))
            {
                xw.WriteStartDocument();
                xw.WriteStartElement("skupiny_akci");

                foreach (KeyValuePair<int, RozvrhovaAkce> akce in RozvrhoveAkce)
                {
                    foreach (KeyValuePair<int, StudijniSkupina> skupina in akce.Value.StudijniSkupiny)
                    {
                        xw.WriteStartElement("akce_skupina");
                        xw.WriteElementString("akceId", akce.Value.Id.ToString());
                        xw.WriteElementString("skupinaId", skupina.Value.Id.ToString());
                        xw.WriteEndElement();
                    }
                }
                xw.WriteEndElement();
                xw.WriteEndDocument();
                xw.Flush();
            }
        }


        /******************************************************************************** load xml */

        public override void LoadVyucujici()
        {
            using (XmlReader xr = XmlReader.Create(@".\xml\vyucujici.xml"))
            {
                int id = 0;
                string jmeno = string.Empty;
                string prijmeni = string.Empty;
                string tituly = String.Empty;
                string osobni_cislo = string.Empty;
                string uzivatelske_jmeno = string.Empty;
                string heslo = string.Empty;
                string role = string.Empty;

                string element = String.Empty;

                while (xr.Read())
                {
                    if (xr.NodeType == XmlNodeType.Element)
                    {
                        element = xr.Name;
                        if (element == "ucitel")
                        {
                            id = int.Parse(xr.GetAttribute("id"));
                        }
                    }
                    else if (xr.NodeType == XmlNodeType.Text)
                    {
                        switch (element)
                        {
                            case "jmeno":
                                jmeno = xr.Value;
                                break;
                            case "prijmeni":
                                prijmeni = xr.Value;
                                break;
                            case "tituly":
                                tituly = xr.Value;
                                break;
                            case "osobni_cislo":
                                osobni_cislo = xr.Value;
                                break;
                            case "uzivatelske_jmeno":
                                uzivatelske_jmeno = xr.Value;
                                break;
                            case "heslo":
                                heslo = xr.Value;
                                break;
                            case "role":
                                role = xr.Value;
                                break;
                        }
                    }
                    else if (xr.NodeType == XmlNodeType.EndElement && xr.Name == "ucitel")
                    {
                        Vyucujici.Add(id, new Vyucujici(id, jmeno, prijmeni, tituly, osobni_cislo, uzivatelske_jmeno, heslo, role));
                        // ctor: Vyucujici(int id, string jmeno, string prijmeni, string tituly, string osobniCislo, string uzivatelskeJmeno, string heslo, string role)
                    }

                }
            }
        } // LoadVyucujici


        public override void LoadPredmety()
        {
            using (XmlReader xr = XmlReader.Create(@".\xml\predmety.xml"))
            {
                int id = 0;
                string zkratka = String.Empty;
                string nazev = String.Empty;
                int hodPred = 0;
                int hodCvic = 0;
                int hodSem = 0;
                string element = String.Empty;

                while (xr.Read())
                {
                    if (xr.NodeType == XmlNodeType.Element)
                    {
                        element = xr.Name;
                        if (element == "predmet")
                        {
                            id = int.Parse(xr.GetAttribute("id"));
                        }
                    }
                    else if (xr.NodeType == XmlNodeType.Text)
                    {
                        switch (element)
                        {
                            case "zkratka":
                                zkratka = xr.Value;
                                break;
                            case "nazev":
                                nazev = xr.Value;
                                break;
                            case "prednasky":
                                hodPred = int.Parse(xr.Value);
                                break;
                            case "cviceni":
                                hodCvic = int.Parse(xr.Value);
                                break;
                            case "seminare":
                                hodSem = int.Parse(xr.Value);
                                break;
                        }
                    }
                    else if (xr.NodeType == XmlNodeType.EndElement && xr.Name == "predmet")
                    {
                        Predmety.Add(id, new Predmet(id, zkratka, nazev, hodPred, hodCvic, hodSem));
                    }
                }

            }
        } // LoadPredmety


        public override void LoadObory()
        {
            using (XmlReader xr = XmlReader.Create(@".\xml\obory.xml"))
            {
                int id = 0;
                string nazev = String.Empty;
                string zkratka = String.Empty;
                string element = String.Empty;

                while (xr.Read())
                {
                    if (xr.NodeType == XmlNodeType.Element)
                    {
                        element = xr.Name;
                        if (element == "obor")
                        {
                            id = int.Parse(xr.GetAttribute("id"));
                        }
                    }
                    else if (xr.NodeType == XmlNodeType.Text)
                    {
                        switch (element)
                        {
                            case "nazev":
                                nazev = xr.Value;
                                break;
                            case "zkratka":
                                zkratka = xr.Value;
                                break;
                        }
                    }
                    else if (xr.NodeType == XmlNodeType.EndElement && xr.Name == "obor")
                    {
                        StudijniObory.Add(id, new StudijniObor(id, nazev, zkratka));
                    }
                }

            }
        } // LoadObory


        public override void LoadStudenty()
        {
            using (XmlReader xr = XmlReader.Create(@".\xml\studenti.xml"))
            {
                int id = 0;
                string jmeno = string.Empty;
                string prijmeni = string.Empty;
                int rocnik = 0;
                int obor_id = 0;
                string osobni_cislo = string.Empty;
                string uzivatelske_jmeno = string.Empty;
                string heslo = string.Empty;
                string role = string.Empty;

                string element = string.Empty;

                while (xr.Read())
                {
                    if (xr.NodeType == XmlNodeType.Element)
                    {
                        element = xr.Name;
                        if (element == "student")
                        {
                            id = int.Parse(xr.GetAttribute("id"));
                        }
                    }
                    else if (xr.NodeType == XmlNodeType.Text)
                    {
                        switch (element)
                        {
                            case "jmeno":
                                jmeno = xr.Value;
                                break;
                            case "prijmeni":
                                prijmeni = xr.Value;
                                break;
                            case "rocnik":
                                rocnik = int.Parse(xr.Value);
                                break;
                            case "obor_id":
                                obor_id = int.Parse(xr.Value);
                                break;
                            case "osobni_cislo":
                                osobni_cislo = xr.Value;
                                break;
                            case "uzivatelske_jmeno":
                                uzivatelske_jmeno = xr.Value;
                                break;
                            case "heslo":
                                heslo = xr.Value;
                                break;
                            case "role":
                                role = xr.Value;
                                break;
                        }
                    }
                    else if (xr.NodeType == XmlNodeType.EndElement && xr.Name == "student")
                    {
                        Studenti.Add(id, new Student(id, jmeno, prijmeni, rocnik, StudijniObory[obor_id], osobni_cislo, uzivatelske_jmeno, heslo, role));
                        // ctor: Student(int id, string jmeno, string prijmeni, int rocnik, StudijniObor studijniObor, string osobniCislo, string uzivatelskeJmeno, string heslo, string role)
                    }
                }

            }
        } // LoadStudenty


        public override void LoadMistnosti()
        {
            using (XmlReader xr = XmlReader.Create(@".\xml\mistnosti.xml"))
            {
                int id = 0;
                int budova = 0;
                int cislo = 0;
                string typ = String.Empty;
                int kapacita = 0;
                string element = String.Empty;

                while (xr.Read())
                {
                    if (xr.NodeType == XmlNodeType.Element)
                    {
                        element = xr.Name;
                        if (element == "mistnost")
                        {
                            id = int.Parse(xr.GetAttribute("id"));
                        }
                    }
                    else if (xr.NodeType == XmlNodeType.Text)
                    {
                        switch (element)
                        {
                            case "budova":
                                budova = int.Parse(xr.Value);
                                break;
                            case "cislo":
                                cislo = int.Parse(xr.Value);
                                break;
                            case "typ":
                                typ = xr.Value;
                                break;
                            case "kapacita":
                                kapacita = int.Parse(xr.Value);
                                break;
                        }
                    }
                    else if (xr.NodeType == XmlNodeType.EndElement && xr.Name == "mistnost")
                    {
                        if (typ == "PrednaskovaMistnost")
                        {
                            Mistnosti.Add(id, new PrednaskovaMistnost(id, budova, cislo, kapacita));
                        }
                        else if (typ == "SeminarniMistnost")
                        {
                            Mistnosti.Add(id, new SeminarniMistnost(id, budova, cislo, kapacita));
                        }
                        else if (typ == "PocitacovaMistnost")
                        {
                            Mistnosti.Add(id, new PocitacovaMistnost(id, budova, cislo, kapacita));
                        }
                    }
                }

            }
        } // LoadMistnosti


        public override void LoadPredmetyOboru()
        {
            using (XmlReader xr = XmlReader.Create(@".\xml\predmety_oboru.xml"))
            {
                int oborId = 0;
                int predmetId = 0;
                string element = String.Empty;

                while (xr.Read())
                {
                    if (xr.NodeType == XmlNodeType.Element)
                    {
                        element = xr.Name;
                    }
                    else if (xr.NodeType == XmlNodeType.Text)
                    {
                        switch (element)
                        {
                            case "oborId":
                                oborId = int.Parse(xr.Value);
                                break;
                            case "predmetId":
                                predmetId = int.Parse(xr.Value);
                                break;
                        }
                    }
                    else if (xr.NodeType == XmlNodeType.EndElement && xr.Name == "obor_predmet")
                    {
                        StudijniObory[oborId].PredmetyOboru.Add(predmetId, Predmety[predmetId]);
                    }
                }
            }
        } // LoadPredmetyOboru


        public override void LoadZapsanePredmety()
        {
            using (XmlReader xr = XmlReader.Create(@".\xml\zapsane_predmety.xml"))
            {
                int studentId = 0;
                int predmetId = 0;
                string element = String.Empty;

                while (xr.Read())
                {
                    if (xr.NodeType == XmlNodeType.Element)
                    {
                        element = xr.Name;
                    }
                    else if (xr.NodeType == XmlNodeType.Text)
                    {
                        switch (element)
                        {
                            case "studentId":
                                studentId = int.Parse(xr.Value);
                                break;
                            case "predmetId":
                                predmetId = int.Parse(xr.Value);
                                break;
                        }
                    }
                    else if (xr.NodeType == XmlNodeType.EndElement && xr.Name == "student_predmet")
                    {
                        Studenti[studentId].ZapsanePredmety.Add(predmetId, Predmety[predmetId]);
                        Predmety[predmetId].Studujici.Add(studentId, Studenti[studentId]);
                    }
                }
            }
        } // LoadZapsanePredmety


        public override void LoadVyucujiciPredmetu()
        {
            using (XmlReader xr = XmlReader.Create(@".\xml\vyucujici_predmety.xml"))
            {
                int vyucujiciId = 0;
                int predmetId = 0;
                string forma = String.Empty;
                string element = String.Empty;

                while (xr.Read())
                {
                    if (xr.NodeType == XmlNodeType.Element)
                    {
                        element = xr.Name;
                    }
                    else if (xr.NodeType == XmlNodeType.Text)
                    {
                        switch (element)
                        {
                            case "vyucujiciId":
                                vyucujiciId = int.Parse(xr.Value);
                                break;
                            case "predmetId":
                                predmetId = int.Parse(xr.Value);
                                break;
                            case "forma":
                                forma = xr.Value;
                                break;
                        }
                    }
                    else if (xr.NodeType == XmlNodeType.EndElement && xr.Name == "vyucujici_predmet_forma")
                    {
                        if (forma == "Prednaska")
                        {
                            Vyucujici[vyucujiciId].Prednasky.Add(predmetId, Predmety[predmetId]);
                            Predmety[predmetId].Prednasejici.Add(vyucujiciId, Vyucujici[vyucujiciId]);
                        }
                        else if (forma == "Cviceni")
                        {
                            Vyucujici[vyucujiciId].Cviceni.Add(predmetId, Predmety[predmetId]);
                            Predmety[predmetId].Cvicici.Add(vyucujiciId, Vyucujici[vyucujiciId]);
                        }
                        else if (forma == "Seminar")
                        {
                            Vyucujici[vyucujiciId].Seminare.Add(predmetId, Predmety[predmetId]);
                            Predmety[predmetId].VedeSeminar.Add(vyucujiciId, Vyucujici[vyucujiciId]);
                        }
                    }
                }
            }
        } // LoadVyucujiciPredmetu


        public override void LoadStudijniSkupiny()
        {
            using (XmlReader xr = XmlReader.Create(@".\xml\studijni_skupiny.xml"))
            {
                int skupinaId = 0;
                int predmetId = 0;
                string element = String.Empty;

                while (xr.Read())
                {
                    if (xr.NodeType == XmlNodeType.Element)
                    {
                        element = xr.Name;
                    }
                    else if (xr.NodeType == XmlNodeType.Text)
                    {
                        switch (element)
                        {
                            case "skupinaId":
                                skupinaId = int.Parse(xr.Value);
                                break;
                            case "predmetId":
                                predmetId = int.Parse(xr.Value);
                                break;
                        }
                    }
                    else if (xr.NodeType == XmlNodeType.EndElement && xr.Name == "skupina_predmet")
                    {
                        StudijniSkupiny.Add(skupinaId, new StudijniSkupina(skupinaId, Predmety[predmetId], new Dictionary<int, Student>()));
                        Predmety[predmetId].StudijniSkupiny.Add(skupinaId, StudijniSkupiny[skupinaId]);
                    }
                }
            }
        } // LoadStudijniSkupiny


        public override void LoadStudentySkupin()
        {
            using (XmlReader xr = XmlReader.Create(@".\xml\studenti_skupiny.xml"))
            {
                int skupinaId = 0;
                int studentId = 0;
                string element = String.Empty;

                while (xr.Read())
                {
                    if (xr.NodeType == XmlNodeType.Element)
                    {
                        element = xr.Name;
                    }
                    else if (xr.NodeType == XmlNodeType.Text)
                    {
                        switch (element)
                        {
                            case "skupinaId":
                                skupinaId = int.Parse(xr.Value);
                                break;
                            case "studentId":
                                studentId = int.Parse(xr.Value);
                                break;
                        }
                    }
                    else if (xr.NodeType == XmlNodeType.EndElement && xr.Name == "skupina_student")
                    {
                        StudijniSkupiny[skupinaId].StudentiSkupiny.Add(studentId, Studenti[studentId]);
                        Studenti[studentId].StudijniSkupiny.Add(skupinaId, StudijniSkupiny[skupinaId]);
                    }
                }
            }
        } // LoadStudentySkupin


        public override void LoadRozvrhoveAkce()
        {
            using (XmlReader xr = XmlReader.Create(@".\xml\rozvrhove_akce.xml"))
            {

                int id = 0;
                int predmetId = 0;
                TypyVyuky typ = 0;
                int vyucujiciId = 0;
                int mistnostId = 0;
                Dny den = 0;
                int zacatek = 0;
                int delka = 0;
                string element = String.Empty;

                while (xr.Read())
                {
                    if (xr.NodeType == XmlNodeType.Element)
                    {
                        element = xr.Name;
                        if (element == "akce")
                        {
                            id = int.Parse(xr.GetAttribute("id"));
                        }
                    }
                    else if (xr.NodeType == XmlNodeType.Text)
                    {
                        switch (element)
                        {
                            case "predmetId":
                                predmetId = int.Parse(xr.Value);
                                break;
                            case "typ":
                                typ = (TypyVyuky)Enum.Parse(typeof(TypyVyuky), xr.Value);
                                break;
                            case "vyucujiciId":
                                vyucujiciId = int.Parse(xr.Value);
                                break;
                            case "mistnostId":
                                mistnostId = int.Parse(xr.Value);
                                break;
                            case "den":
                                den = (Dny)Enum.Parse(typeof(Dny), xr.Value);
                                break;
                            case "zacatek":
                                zacatek = int.Parse(xr.Value);
                                break;
                            case "delka":
                                delka = int.Parse(xr.Value);
                                break;
                        }
                    }
                    else if (xr.NodeType == XmlNodeType.EndElement && xr.Name == "akce")
                    {
                        RozvrhovaAkce ra = new RozvrhovaAkce(id, Predmety[predmetId], typ, Vyucujici[vyucujiciId], Mistnosti[mistnostId], den, zacatek, delka);
                        RozvrhoveAkce.Add(id, ra);
                        Predmety[ra.Predmet.Id].RozvrhoveAkce.Add(ra.Id, ra);
                        for (int i = 0; i < delka; i++)
                        {
                            Vyucujici[vyucujiciId].Rozvrh[den].Add(zacatek + i, ra);
                            Mistnosti[mistnostId].Rozvrh[den].Add(zacatek + i, ra);
                        }
                    }
                }
            }
        } // LoadRozvrhoveAkce


        public override void LoadStudijniSkupinyRozvrhovychAkci()
        {
            using (XmlReader xr = XmlReader.Create(@".\xml\skupiny_akci.xml"))
            {
                int akceId = 0;
                int skupinaId = 0;
                string element = String.Empty;

                while (xr.Read())
                {
                    if (xr.NodeType == XmlNodeType.Element)
                    {
                        element = xr.Name;
                    }
                    else if (xr.NodeType == XmlNodeType.Text)
                    {
                        switch (element)
                        {
                            case "akceId":
                                akceId = int.Parse(xr.Value);
                                break;
                            case "skupinaId":
                                skupinaId = int.Parse(xr.Value);
                                break;
                        }
                    }
                    else if (xr.NodeType == XmlNodeType.EndElement && xr.Name == "akce_skupina")
                    {
                        RozvrhoveAkce[akceId].StudijniSkupiny.Add(skupinaId, StudijniSkupiny[skupinaId]);
                        for (int i = 0; i < RozvrhoveAkce[akceId].Delka; i++)
                        {
                            StudijniSkupiny[skupinaId].Rozvrh[RozvrhoveAkce[akceId].Den].Add(RozvrhoveAkce[akceId].Zacatek + i, RozvrhoveAkce[akceId]);
                            foreach (KeyValuePair<int, Student> student in StudijniSkupiny[skupinaId].StudentiSkupiny)
                            {
                                student.Value.Rozvrh[RozvrhoveAkce[akceId].Den].Add(RozvrhoveAkce[akceId].Zacatek + i, RozvrhoveAkce[akceId]);
                            }
                        }
                    }
                }
            }
        } // LoadStudijniSkupinyRozvrhovychAkci

        /******************************************************************************** load xml **/


        /***************************************************************************************************  start CSV plnění (pro přesun dat csv->xml)*/
        /// <summary>
        /// Naplní seznam vyučujících ze souboru CSV
        /// </summary>
        private void LoadVyucujiciFromCSV()
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
        private void LoadPredmetyFromCSV()
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
        private void LoadOboryFromCSV()
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
        private void LoadStudentyFromCSV()
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
        private void LoadMistnostiFromCSV()
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
        private void LoadPredmetyOboruFromCSV()
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
        private void LoadZapsanePredmetyFromCSV()
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
        private void LoadVyucujiciPredmetuFromCSV()
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
        private void LoadStudijniSkupinyFromCSV()
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
        private void LoadStudentySkupinFromCSV()
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
        private void LoadRozvrhoveAkceFromCSV()
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
                    for (int i = 0; i < delka; i++)
                    {
                        Vyucujici[idVyucujiciho].Rozvrh[den].Add(zacatek + i, ra);
                        Mistnosti[idMistnosti].Rozvrh[den].Add(zacatek + i, ra);
                    }
                }
            }
        }



        private void LoadStudijniSkupinyRozvrhovychAkciFromCSV()
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
        /********************************************************************************************************************  konec CSV plnění **/
    }
}
