using System;
using System.Collections.Generic;
using Jedek.Rozvrhy.Libs;
using Jedek.Rozvrhy.App.Forms;

namespace Jedek.Rozvrhy.App.Views.StudijniObor
{
    class PridejStudijniObor : View
    {
        public PridejStudijniObor(Dictionary<string, Object> context)
            : base(context)
        {
        }

        public override void Render()
        {
            OnFormSend(new StudijniOborForm());
        }

        protected override void OnFormSend(Form form)
        {
            base.OnFormSend(form);
        }

    }
}
