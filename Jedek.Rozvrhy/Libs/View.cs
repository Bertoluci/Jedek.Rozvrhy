using System;
using System.Collections.Generic;
using Jedek.Rozvrhy.App.Forms;

namespace Jedek.Rozvrhy.Libs
{
    abstract class View : IView 
    {
        /// <summary>
        /// Prostředek předávání dat controlleru pro view
        /// </summary>
        public Dictionary<string, Object> Context { get; set; }

        public Uzivatel Uzivatel { get; set; }


        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="context"></param>
        public View(Dictionary<string, Object> context)
        {
            Context = context;
        }
        


        // delegát přesměrování z menu
        public delegate void RequestHandle(string controller, string action, params Object[] args);
        public RequestHandle Request { get; set; }



        // Definice události odeslání formuláře
        public event EventHandler<Form> SendEvent;

        protected virtual void OnFormSend(Form form)
        {
            EventHandler<Form> localHandler = SendEvent;

            if (localHandler != null)
            {
                localHandler(this, form);
            }
        }



        // Definice události odstranění položky
        public event EventHandler<Model> DeleteEvent;

        protected virtual void OnDeleteItem(Model item)
        {
            EventHandler<Model> deleteHandler = DeleteEvent;

            if (deleteHandler != null)
            {
                deleteHandler(this, item);
            }
        }



        // Definice události editace položky
        public event EventHandler<Form> EditEvent;

        protected virtual void OnEditItem(Form form)
        {
            EventHandler<Form> editHandler = EditEvent;

            if (editHandler != null)
            {
                editHandler(this, form);
            }
        }



        // Definice události přidání položky
        public event EventHandler<Model> AddEvent;

        protected virtual void OnAddItem(Model item)
        {
            EventHandler<Model> addHandler = AddEvent;

            if (addHandler != null)
            {
                addHandler(this, item);
            }
        }



        /// <summary>
        /// Vykreslení view
        /// </summary>
        public abstract void Render();

    }
}
