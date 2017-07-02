namespace PipeBandGames.DataLayer.Constants
{
    // TODO: Replace this class with proper configuration via appsettings.json
    public static class Config
    {
        // default number of minutes to plan for each piobaireachd event
        public static int PiobaireachdEventDuration = 15;

        // default number of minutes to plan for each light music event
        public static int LightMusicEventDuration = 5;

        // default number of minutes after DoorsOpen time that the Registration table should open
        public static int RegistrationOpenDoorsOpenOffset = 30;

        // default number of minutes after RegistrationOpen time that the first SoloEvent should start
        public static int FirstEventRegistrationOpenOffset = 30;

        public static int BreakBetweenEvents = 10;
    }
}
