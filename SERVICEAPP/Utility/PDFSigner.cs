using iText.Bouncycastle.Crypto;
using iText.Bouncycastle.X509;
using iText.Commons.Bouncycastle.Cert;
//using iText.Html2pdf;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Signatures;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.X509;
using System.IO;

namespace SERVICEAPP.Utility
{
    public class PDFSigner
    {
        private readonly string inputPDF;
        private readonly string outputPDF;
        private readonly Cert cert;

        public PDFSigner(string input, string output, Cert certificate)
        {
            inputPDF = input;
            outputPDF = output;
            cert = certificate;
        }

        public void Sign(string reason, string contact, string location, bool visible)
        {
            using (PdfReader reader = new PdfReader(inputPDF))
            using (FileStream os = new FileStream(outputPDF, FileMode.Create))
            {
                PdfSigner signer = new PdfSigner(reader, os, new StampingProperties());

                // Signature appearance
                PdfSignatureAppearance appearance = signer.GetSignatureAppearance();
                appearance
                    .SetReason(reason)
                    .SetContact(contact)
                    .SetLocation(location);

                if (visible)
                {
                    appearance
                        .SetPageRect(new Rectangle(100, 100, 250, 150))
                        .SetPageNumber(1);
                }

                signer.SetFieldName("Signature1");

                // BouncyCastle signing
                IExternalSignature pks =
                    new PrivateKeySignature(new PrivateKeyBC(cert.PrivateKey), DigestAlgorithms.SHA256);

                IX509Certificate[] chain = cert.Chain
                            .Select(x => new X509CertificateBC(x))
                            .ToArray();

                signer.SignDetached(                   
                    pks,
                    chain,
                    null,
                    null,
                    null,
                    0,
                    PdfSigner.CryptoStandard.CADES
                );
            }
        }

       
    }
}
