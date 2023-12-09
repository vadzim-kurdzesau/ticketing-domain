using System.IO;

namespace IWent.Notifications.Email.Builders;

internal static class Constants
{
    private static string _resourceFolderPath = Path.Combine(Directory.GetCurrentDirectory(), "Resources");

    public static string ResourceFolderPath => _resourceFolderPath;
}
