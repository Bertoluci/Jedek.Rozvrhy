using System;
using System.Collections;
using System.Collections.Generic;
using Jedek.Rozvrhy.App.Models;

namespace Jedek.Rozvrhy.Libs
{
    enum Repository
    {
        Session = 1, CSV, XML
    }

    class Container
    {
        public SortedList Parameters { get; set; }
        public Dictionary<string, Object> Services { get; set; }

        public Container(SortedList parameters)
        {
            Parameters = parameters;
            Services = new Dictionary<string, object>();
        }


        /// <summary>
        /// Vrací sám sebe, pro potřeby nastavení parametrů
        /// </summary>
        /// <returns>Container</returns>
        public Container GetContainer()
        {
            return this;
        }


        /// <summary>
        /// Vytváří instanci zvolené databáze
        /// </summary>
        /// <returns></returns>
        public Databaze CreateDatabaze()
        {
            if (Parameters["repository"].Equals(Repository.XML))
            {
                return new XMLDatabaze();
            }
            else if (Parameters["repository"].Equals(Repository.CSV))
            {
                return new CSVDatabaze();
            }
            else
            {
                return new SessionDatabaze();
            }
        }


        /// <summary>
        /// Vrátí singleton instance zvolené databáze
        /// </summary>
        /// <returns>Databaze</returns>
        public Databaze GetDatabaze()
        {
            if (!Services.ContainsKey("databaze"))
            {
                Services.Add("databaze", CreateDatabaze());
            }
            return (Databaze)Services["databaze"];
        }


        /// <summary>
        /// Vrátí singleton instance třídy Uzivatel
        /// </summary>
        /// <returns>UzivatelManager</returns>
        public Uzivatel GetUzivatel()
        {
            if (!Services.ContainsKey("uzivatel"))
            {
                Services.Add("uzivatel", new Uzivatel(GetUzivatelManager()));
            }
            return (Uzivatel)Services["uzivatel"];
        }


        /// <summary>
        /// Vrátí singleton instance třídy UzivatelManager
        /// </summary>
        /// <returns>UzivatelManager</returns>
        public UzivatelManager GetUzivatelManager()
        {
            if (!Services.ContainsKey("uzivatelManager"))
            {
                Services.Add("uzivatelManager", new UzivatelManager(GetDatabaze()));
            }
            return (UzivatelManager)Services["uzivatelManager"];
        }


        public MenuManager CreateMenuManager()
        {
            return new MenuManager();
        }


        /// <summary>
        /// Vrátí singleton instance třídy MistnostManager
        /// </summary>
        /// <returns>MistnostManager</returns>
        public MistnostManager GetMistnostManager()
        {
            if (!Services.ContainsKey("mistnostManager"))
            {
                Services.Add("mistnostManager", new MistnostManager(GetDatabaze()));
            }
            return (MistnostManager)Services["mistnostManager"];
        }


        /// <summary>
        /// Vrátí singleton instance třídy StudentManager
        /// </summary>
        /// <returns>StudentManager</returns>
        public StudentManager GetStudentManager()
        {
            if (!Services.ContainsKey("studentManager"))
            {
                Services.Add("studentManager", new StudentManager(GetDatabaze()));
            }
            return (StudentManager)Services["studentManager"];
        }


        /// <summary>
        /// Vrátí singleton instance třídy StudijniOborManager
        /// </summary>
        /// <returns>StudijniOborManager</returns>
        public StudijniOborManager GetStudijniOborManager()
        {
            if (!Services.ContainsKey("studijniOborManager"))
            {
                Services.Add("studijniOborManager", new StudijniOborManager(GetDatabaze()));
            }
            return (StudijniOborManager)Services["studijniOborManager"];
        }


        /// <summary>
        /// Vrátí singleton instance třídy RozvrhovaAkceManager
        /// </summary>
        /// <returns>RozvrhovaAkceManager</returns>
        public RozvrhovaAkceManager GetRozvrhovaAkceManager()
        {
            if (!Services.ContainsKey("rozvrhovaAkceManager"))
            {
                Services.Add("rozvrhovaAkceManager", new RozvrhovaAkceManager(GetDatabaze()));
            }
            return (RozvrhovaAkceManager)Services["rozvrhovaAkceManager"];
        }


        /// <summary>
        /// Vrátí singleton instance třídy VyucujiciManager
        /// </summary>
        /// <returns>VyucujiciManager</returns>
        public VyucujiciManager GetVyucujiciManager()
        {
            if (!Services.ContainsKey("vyucujiciManager"))
            {
                Services.Add("vyucujiciManager", new VyucujiciManager(GetDatabaze()));
            }
            return (VyucujiciManager)Services["vyucujiciManager"];
        }


        /// <summary>
        /// Vrátí singleton instance třídy StudijniSkupinaManager
        /// </summary>
        /// <returns>StudijniSkupinaManager</returns>
        public StudijniSkupinaManager GetStudijniSkupinaManager()
        {
            if (!Services.ContainsKey("studijniSkupinaManager"))
            {
                Services.Add("studijniSkupinaManager", new StudijniSkupinaManager(GetDatabaze()));
            }
            return (StudijniSkupinaManager)Services["studijniSkupinaManager"];
        }


        /// <summary>
        /// Vrátí singleton instance třídy PredmetManager
        /// </summary>
        /// <returns>PredmetManager</returns>
        public PredmetManager GetPredmetManager()
        {
            if (!Services.ContainsKey("predmetManager"))
            {
                Services.Add("predmetManager", new PredmetManager(GetDatabaze()));
            }
            return (PredmetManager)Services["predmetManager"];
        }


    }
}
