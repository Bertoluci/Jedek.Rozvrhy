using System;
using System.Collections.Generic;
using Jedek.Rozvrhy.Libs;
using Jedek.Rozvrhy.App.Forms;

namespace Jedek.Rozvrhy.App.Views.Vyucujici
{
    class PridejVyucujiciho : View
    {
        public PridejVyucujiciho(Dictionary<string, Object> context)
            : base(context)
        {
        }

        public override void Render()
        {
            OnFormSend(new VyucujiciForm());
        }

        protected override void OnFormSend(Form form)
        {
            base.OnFormSend(form);
        }
    }
}
