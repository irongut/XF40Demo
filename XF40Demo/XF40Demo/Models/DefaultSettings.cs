namespace XF40Demo.Models
{
    public static class DefaultSettings
    {
        public static bool WifiOnly()
        {
            return false;
        }

        public static int NewsCacheTime()
        {
            return 6;
        }

        public static int PrioritiesCacheTime()
        {
            return 5;
        }

        public static bool DarkTheme()
        {
            return true;
        }

        public static bool OnlyShowNextCycleWhenImminent()
        {
            return false;
        }

        public static bool ShowEliteStatusOnMenu()
        {
            return true;
        }

        public static bool CopySystemName()
        {
            return true;
        }

        public static bool EnableDiagnostics()
        {
            return true;
        }
    }
}
