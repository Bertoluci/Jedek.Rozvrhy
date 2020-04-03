using System.Collections.Generic;
using Jedek.Rozvrhy.Libs;

namespace Jedek.Rozvrhy.App.Models
{
    class MenuManager : Model
    {
        public Dictionary<string, MenuItem> Items { get; private set; }

        public MenuManager()
        {
            Items = new Dictionary<string, MenuItem>();
            Init();
        }

        private void Init()
        {
            Items.Add("1", new MenuItem(1, "Studenti", "Student", "Default"));
            Items.Add("2", new MenuItem(2, "Studijni skupiny", "StudijniSkupina", "Default"));
            Items.Add("3", new MenuItem(3, "Vyučující", "Vyucujici", "Default"));
            Items.Add("4", new MenuItem(4, "Předměty", "Predmet", "Default"));
            Items.Add("5", new MenuItem(5, "Studijní Obory", "StudijniObor", "Default"));
            Items.Add("6", new MenuItem(6, "Místnosti", "Mistnost", "Default"));
            Items.Add("7", new MenuItem(7, "Rozvrhové akce", "RozvrhovaAkce", "Default"));
            Items.Add("8", new MenuItem(8, "Můj Rozvrh", "Uzivatel", "Default"));
            Items.Add("0", new MenuItem(9, "Konec s uložením změn", "Shutdown", "Default"));
            Items.Add("O", new MenuItem(10, "Odhlášení", "Prihlaseni", "OdhlasUzivatele"));
            //Items.Add(9, new MenuItem(9, "Blbost"));

        }
    }
}
