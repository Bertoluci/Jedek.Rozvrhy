using System;
using System.Collections.Generic;
using Jedek.Rozvrhy.Libs;
using Jedek.Rozvrhy.App.Forms;

namespace Jedek.Rozvrhy.App.Views.Predmet
{
    class PridejPredmet : View
    {
        public PridejPredmet(Dictionary<string, Object> context)
            : base(context)
        {
        }

        public override void Render()
        {
            OnFormSend(new PredmetForm());
        }

        protected override void OnFormSend(Form form)
        {
            base.OnFormSend(form);
        }
    }
}
