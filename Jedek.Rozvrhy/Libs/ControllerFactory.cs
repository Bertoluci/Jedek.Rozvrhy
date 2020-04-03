using System;
using System.Collections.Generic;
using System.Reflection;

namespace Jedek.Rozvrhy.Libs
{
    class ControllerFactory
    {
        // Skladiště instancí Controllerů
        public Dictionary<string, Controller> Controllers { get; private set; }
        public Container Container { get; set; }
        public string Namespace { get; private set; }

        public ControllerFactory(Container container)
        {
            Container = container;
            Controllers = new Dictionary<string, Controller>();
            Namespace = "Jedek.Rozvrhy.App.Controllers";
        }
        public Controller GetController(string name)
        {
            // Pokud je Controller ve skladu vrátí se tato
            if (Controllers.ContainsKey(name))
            {
                // je na skladě
                return Controllers[name];
            }

            // Získání objektu Type daného Controlleru
            string TypeFullName = string.Format("{0}.{1}", Namespace, name + "Controller");
            Type type = Type.GetType(TypeFullName, true);

            // Získání informací o konstruktorech
            ConstructorInfo[] constructorsInfo = type.GetConstructors();

            // Controller smí mít pouze jeden konstruktor
            if (constructorsInfo.Length > 1)
            {
                throw new AmbiguousMatchException("Controller smí mít pouze jeden konstruktor.");
            }

            // Získání informací o argumentech konstruktoru
            ParameterInfo[] parameters = constructorsInfo[0].GetParameters();

            // Vytvoření služeb požadovaných konstruktorem
            List<Object> arg = new List<Object>();
            if (parameters.Length != 0)
            {
                string className;
                foreach (ParameterInfo pi in parameters)
                {
                    className = pi.ParameterType.Name;
                    MethodInfo mi = Container.GetType().GetMethod("Get" + className);
                    if (mi == null)
                    {
                        mi = Container.GetType().GetMethod("Create" + className);
                    }
                    arg.Add(mi.Invoke(Container, null));
                }
            }

            // Vytvoření Controlleru
            Controller ctr = (Controller)Activator.CreateInstance(type, arg.ToArray());

            // Přidání controlleru do skladu
            Controllers.Add(name, ctr);

            // vrácení instance
            return ctr;
        }
    }
}
