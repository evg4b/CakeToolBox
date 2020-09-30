namespace CakeToolBox.Environment.Aliases
{
    using System.Linq;
    using System.Net;
    using Cake.Core;
    using System.Net.Sockets;
    using Cake.Core.Annotations;

    public static class GetHostIPsAliases
    {
        [CakeMethodAlias]
        public static IPAddress[] GetHostIps(this ICakeContext context) =>
            GetHostIpsInternal();
        
        [CakeMethodAlias]
        public static IPAddress[] GetHostV4Ips(this ICakeContext context) =>
            GetHostIpsInternal()
                .Where(ip => ip.AddressFamily == AddressFamily.InterNetwork)
                .ToArray();
        
        [CakeMethodAlias]
        public static IPAddress[] GetHostV6Ips(this ICakeContext context) => 
            GetHostIpsInternal()
                .Where(ip => ip.AddressFamily == AddressFamily.InterNetworkV6)
                .ToArray();

        private static IPAddress[] GetHostIpsInternal()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            return host.AddressList;
        }
    }
}