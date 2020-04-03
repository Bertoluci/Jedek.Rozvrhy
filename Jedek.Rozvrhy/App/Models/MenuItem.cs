using Jedek.Rozvrhy.Libs;

namespace Jedek.Rozvrhy.App.Models
{
    class MenuItem : Model
    {
        public int Id { get; private set; }
        public string Name { get; private set; }
        public string Controller { get; private set; }
        public string Action { get; private set; }


        public MenuItem(int id, string name, string controller, string action = "Default")
        {
            Id = id;
            Name = name;
            Controller = controller;
            Action = action;
        }
    }
}
