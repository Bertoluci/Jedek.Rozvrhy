using System;
using System.Collections.Generic;
using System.Collections;
using System.Reflection;
using Jedek.Rozvrhy.Libs;

namespace Jedek.Rozvrhy.App
{

    class Application
    {
        public Container Container { get; private set; }
        public SortedList Parameters { get; private set; }
        public Controller Controller { get; private set; }
        public ControllerFactory ControllerFactory { get; private set; }

        public Application()
        {
            Parameters = new SortedList();
            Container = new Container(Parameters);
            ControllerFactory = new ControllerFactory(Container);
        }


        public void CreateRequest(string controller, string action, Object[] args)
        {

            // Todo nějaký config
            const string ViewNamespace = "Jedek.Rozvrhy.App.Views";

            // Získání controlleru
            Controller = ControllerFactory.GetController(controller);

            // Přidání uživatele do Controlleru
            if (controller != "Setting")
            {
                Controller.Uzivatel = Container.GetUzivatel();
                if (controller != "Prihlaseni" && !Controller.Uzivatel.JePrihlasen)
                {
                    Controller ctr = Controller;
                    CreateRequest("Prihlaseni", "Default", null);
                    Controller = ctr;
                }
            }

            // vytvoření View
            string TypeFullName = string.Format("{0}.{1}.{2}", ViewNamespace, controller, action);
            Type type = Type.GetType(TypeFullName, true);
            Controller.View = (View)Activator.CreateInstance(type, new Dictionary<string, Object>());

            // Přidání uživatele do View
            if (controller != "Setting")
            {
                Controller.View.Uzivatel = Container.GetUzivatel();
            }

            // přidání aktuálního Controlleru do Contextu 
            //Controller.View.Context.Add("Controller", Controller);

            Controller.View.Request = new View.RequestHandle(this.CreateRequest);

            //Získá informace o metodě pomocí třídy MethodInfo
            MethodInfo methodInfo = Controller.GetType().GetMethod(action);

            // volání metody Controlleru s / bez parametrů
            if (methodInfo != null)
            {
                ParameterInfo[] parameters = methodInfo.GetParameters();
                methodInfo.Invoke(Controller, parameters.Length == 0 ? null : args);
            }
            else
            {
                throw new MissingMethodException(String.Format("Volání neznámé metody {0}.{1}", controller + "Controller", action));
            }

            CreateRequest("Menu", "Default", null);
        }

    }
}
