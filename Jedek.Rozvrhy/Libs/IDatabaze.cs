namespace Jedek.Rozvrhy.Libs
{
    interface IDatabaze
    {
        void Load();
        void LoadVyucujici();
        void LoadPredmety();
        void LoadObory();
        void LoadStudenty();
        void LoadMistnosti();
        void LoadPredmetyOboru();
        void LoadZapsanePredmety();
        void LoadVyucujiciPredmetu();
        void LoadStudijniSkupiny();
        void LoadStudentySkupin();
        void LoadRozvrhoveAkce();
        void LoadStudijniSkupinyRozvrhovychAkci();

        void Save();
        void SaveVyucujici();
        void SaveStudenty();
        void SaveZapsanePredmety();
        void SavePredmety();
        void SaveObory();
        void SaveMistnosti();
        void SavePredmetyOboru();
        void SaveVyucujiciPredmetu();
        void SaveStudijniSkupiny();
        void SaveStudentySkupin();
        void SaveRozvrhoveAkce();
        void SaveStudijniSkupinyRozvrhovychAkci();

        bool NajdiUzivatele(string username, string password, Uzivatel uzivatel);
    }
}
