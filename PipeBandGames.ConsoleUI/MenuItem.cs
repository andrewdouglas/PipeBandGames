using System;

namespace PipeBandGames.ConsoleUI
{
    public class MenuItem
    {
        // Text to show for this menu item
        public string Text { get; set; }

        // The key that will trigger this menu's action
        public char Command { get; set; }

        // Optional sub-menu
        public Menu SubMenu { get; set; }

        // The action to be performed by this menu item
        public Action Action { get; internal set; }

        // Override of System.Object's ToString method in order to describe this menu item's function
        public override string ToString()
        {
            return $"{Command.ToString()}: {Text}";
        }
    }
}
