using System;
using System.Collections.Generic;
using Jedek.Rozvrhy.App.Forms;
using Jedek.Rozvrhy.Libs;

namespace Jedek.Rozvrhy.App.Views.Prihlaseni
{
    class Default : View
    {
        public Default(Dictionary<string, Object> context)
            : base(context)
        {
        }

        public override void Render()
        {
            bool chybaPrihlaseni = false;
            if(Context.ContainsKey("chybaPrihlaseni"))
            {
                chybaPrihlaseni = true;
            }
            OnFormSend(new PrihlaseniForm(chybaPrihlaseni));
        }

        protected override void OnFormSend(Form form)
        {
            base.OnFormSend(form);
        }
    }
}