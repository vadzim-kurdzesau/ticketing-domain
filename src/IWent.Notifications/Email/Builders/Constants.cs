using System;
using System.IO;

namespace IWent.Notifications.Email.Builders;

internal static class Constants
{
    private static string _resourceFolderPath = Path.Combine(AppDirectoryPath, "Resources");

    public static string AppDirectoryPath { get; set; }

    public static string ResourceFolderPath => _resourceFolderPath;
}
