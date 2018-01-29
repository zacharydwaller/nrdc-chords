using System.Reflection;

namespace NCInterface.Utilities
{
    public static class Version
    {
        public static string GetString()
        {
            var version = Assembly.GetExecutingAssembly().GetName().Version;
            return string.Format("Version: {0}.{1}.{2} Rev {3}", version.Major, version.Minor, version.Build, version.MinorRevision);
        }
    }
}