using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SubnauticaMPMod
{
    internal class Utils
    {

        public static string GenerateSFID()
        {
            var snowflake = new StringBuilder();
            snowflake.Append(Process.GetCurrentProcess().Id);
            snowflake.Append((int)DateTime.Now.Subtract(new DateTime(1970, 1, 1)).TotalSeconds);
            return snowflake.ToString();
        }
    }
}
