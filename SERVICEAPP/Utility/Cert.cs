using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Pkcs;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.X509;
using System.IO;
using System.Security.Cryptography.X509Certificates;

namespace SERVICEAPP.Utility
{
    public class Cert
    {
        public AsymmetricKeyParameter PrivateKey { get; private set; }
        public Org.BouncyCastle.X509.X509Certificate[] Chain { get; private set; }

        public Cert(string path, string password)
        {
            var cert = new X509Certificate2(
                path,
                password,
                X509KeyStorageFlags.Exportable | X509KeyStorageFlags.MachineKeySet
            );

            PrivateKey = DotNetUtilities.GetKeyPair(cert.GetRSAPrivateKey()).Private;

            Chain = new Org.BouncyCastle.X509.X509Certificate[]
            {
            DotNetUtilities.FromX509Certificate(cert)
            };
        }
    }
}
