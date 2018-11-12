using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using SentoniServiceLib;

namespace SentioniHostLite
{
    //  The only purpose of this program is to run SentoniService on the local computer (without the overhead of starting SentoniHost)
    //      so that the service reference can be updated in SentoniClient
    //  
    class Program
    {
        static void Main(string[] args)
        {
            using (var serviceHost = new ServiceHost(typeof(SentoniService)))
            {
                serviceHost.Open();
                Console.WriteLine("Host started");
                Console.WriteLine("Press any key to end");
                Console.ReadKey();
                serviceHost.Close();
                Console.WriteLine("Host closed");
            }
        }
    }
}

