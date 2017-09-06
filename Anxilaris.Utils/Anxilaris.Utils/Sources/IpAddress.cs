//-----------------------------------------------------------------------
// <copyright file="Ip.cs" company="BestDay">
//     Copyright (c) Sprocket Enterprises. All rights reserved.
// </copyright>
// <author>Jose Martín Trejo Pancardo</author>
//----------------------------------------------------------------------

namespace Anxilaris.Utils
{
    using System.Net;
    using System.Net.Sockets;

    public class IpAddress
    {
        public static string GetPublic()
        {
            string external = string.Empty;
            try
            {
                external = new WebClient().DownloadString("https://ipinfo.io/ip").Replace("\n", "");
            }
            catch
            {
                external = string.Empty;
            }
            return external;
        }

        public static string GetLocal()
        {
            string local = string.Empty;
            try
            {
                IPHostEntry host = Dns.GetHostEntry(Dns.GetHostName());
                foreach (var ip in host.AddressList)
                {
                    if (ip.AddressFamily == AddressFamily.InterNetwork)
                    {
                        local = ip.ToString();
                    }
                }
            }
            catch
            {
                local = string.Empty;
            }
            return local;
        }
    }
}
