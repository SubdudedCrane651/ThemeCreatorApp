using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using Microsoft.Win32;

public class SystemIconChanger
{
    [DllImport("user32.dll", CharSet = CharSet.Auto)]
    private static extern int SystemParametersInfo(int uAction, int uParam, string lpvParam, int fuWinIni);

    [DllImport("shell32.dll", CharSet = CharSet.Auto)]
    private static extern void SHChangeNotify(long wEventId, uint uFlags, IntPtr dwItem1, IntPtr dwItem2);

    const int SPI_SETDESKWALLPAPER = 20;
    const int SPIF_UPDATEINIFILE = 0x01;
    const int SPIF_SENDCHANGE = 0x02;
    const int SHCNE_ASSOCCHANGED = 0x08000000;
    const int SHCNF_IDLIST = 0x0000;

    public static void SetDesktopWallpaper(string filePath)
    {
        SystemParametersInfo(SPI_SETDESKWALLPAPER, 0, filePath, SPIF_UPDATEINIFILE | SPIF_SENDCHANGE);
    }

    public static void ChangeSystemIcon(string iconPath, string registryKey, string description, string type = null)
    {
        string keyPath = $"Software\\Microsoft\\Windows\\CurrentVersion\\Explorer\\CLSID\\{registryKey}";
        if (!string.IsNullOrEmpty(type))
        {
            keyPath += $"\\DefaultIcon";
        }

        using (RegistryKey key = Registry.CurrentUser.OpenSubKey(keyPath, true))
        {
            if (key != null)
            {
                key.SetValue("", iconPath);
                key.Close();
            }
        }

        using (RegistryKey key = Registry.CurrentUser.OpenSubKey($"Software\\Microsoft\\Windows\\CurrentVersion\\Explorer\\CLSID\\{registryKey}", true))
        {
            if (key != null)
            {
                key.SetValue("", description);
                key.Close();
            }
        }
    }

    public static void RefreshDesktop()
    {
        SHChangeNotify(SHCNE_ASSOCCHANGED, SHCNF_IDLIST, IntPtr.Zero, IntPtr.Zero);
    }
}
