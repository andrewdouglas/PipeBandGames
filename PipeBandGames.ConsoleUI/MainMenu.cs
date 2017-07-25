using PipeBandGames.ConsoleUI.Commands;

namespace PipeBandGames.ConsoleUI
{
    public sealed class MainMenu : Menu
    {
        public MainMenu()
        {
            this.Description = "Main menu";
            this.MenuItems.Add(new MenuItem { Command = 'd', Text = "Data entry", Action = NoopCommand.Noop });
            this.MenuItems.Add(new MenuItem { Command = 's', Text = "Create schedule", Action = NoopCommand.Noop });
            this.MenuItems.Add(new MenuItem { Command = 'p', Text = "Print solo event schedule", Action = NoopCommand.Noop });
            this.MenuItems.Add(new MenuItem { Command = 'q', Text = "Quit this application", Action = QuitCommand.Quit });
        }
    }
}
