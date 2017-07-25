using System;
using System.Collections.Generic;

namespace PipeBandGames.ConsoleUI
{
    public class Menu
    {
        // This Description is shown at the top of the menu before the menu items
        public string Description { get; set; }

        // The menu items that make up this menu (example of a property initializer)
        public List<MenuItem> MenuItems { get; set; } = new List<MenuItem>();

        // Override the ToString method from the System.Object object in order to facilitate displaying the menu's contents
        public override string ToString()
        {
            // First show the description
            string text = this.Description;

            // Next call the ToString overridden method on each of the MenuItem instances
            this.MenuItems.ForEach(x =>
            {
                text += Environment.NewLine;
                text += x.ToString();
            });

            return text;
        }
    }
}
