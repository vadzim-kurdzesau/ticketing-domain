using System;
using System.IO;

namespace IWent.Notifications.Email.Builders;

internal static class Constants
{
    private static string _resourceFolderPath = Path.Combine(Environment.CurrentDirectory, "Resources");

    public static string ResourceFolderPath => _resourceFolderPath;
}
