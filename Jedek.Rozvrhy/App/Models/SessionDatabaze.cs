using Jedek.Rozvrhy.Libs;

namespace Jedek.Rozvrhy.App.Models
{
    class SessionDatabaze : Databaze
    {
        public SessionDatabaze()
        {
            Load();
        }

        public override void Load()
        {
            base.Load();
            LoadVyucujici();
        }

        public override void LoadVyucujici()
        {
            Vyucujici.Add(1, new Vyucujici(1, "Franta", "Opršálek", "Ing.", "00001", "f_oprsalek", vratHash("00001"), "admin"));
        }

    }
}
