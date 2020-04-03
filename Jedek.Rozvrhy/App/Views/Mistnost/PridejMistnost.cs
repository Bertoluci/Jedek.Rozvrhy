using System;
using System.Collections.Generic;
using Jedek.Rozvrhy.Libs;
using Jedek.Rozvrhy.App.Forms;

namespace Jedek.Rozvrhy.App.Views.Mistnost
{
    class PridejMistnost : View
    {
        public PridejMistnost(Dictionary<string, Object> context)
            : base(context)
        {
        }

        public override void Render()
        {
            OnFormSend(new MistnostForm());
        }

        protected override void OnFormSend(Form form)
        {
            base.OnFormSend(form);
        }
    }
}
