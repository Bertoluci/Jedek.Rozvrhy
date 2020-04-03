using System;
using System.Collections.Generic;
using Jedek.Rozvrhy.Libs;
using Jedek.Rozvrhy.App.Forms;

namespace Jedek.Rozvrhy.App.Views.Student
{
    class PridejStudenta : View
    {
        public PridejStudenta(Dictionary<string, Object> context)
            : base(context)
        {
        }

        public override void Render()
        {
            if(Context.ContainsKey("message"))
            {
                // chybové hlášení
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\r\n\r\n\t{0}", Context["message"]);
                Console.ForegroundColor = ConsoleColor.Black;
                Console.Write("\r\n\t\tStiskněte jakoukoli klávesu.");
                Console.ReadKey();
                Request("Student", "Default", null);
            }
            else
            {
                OnFormSend((StudentForm)Context["studentForm"]);
            }
            
        }


        protected override void OnFormSend(Form form)
        {
            base.OnFormSend(form);
        }
    }
}
