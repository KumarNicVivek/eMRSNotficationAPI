using iText.Bouncycastle.Crypto;
using iText.Bouncycastle.X509;
using iText.Commons.Bouncycastle.Cert;
using iText.Commons.Bouncycastle.Crypto;
//using iText.Html2pdf;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Signatures;
//using Org.BouncyCastle.Crypto;
//using Org.BouncyCastle.X509;
using System.IO;

namespace SERVICEAPP.Utility
{
    public class PDfSignerWithMetaData
    {
            private readonly string inputPDF;
            private readonly string outputPDF;
            private readonly Cert cert;
            private readonly MetaData metadata;

            public PDfSignerWithMetaData(string input, string output, Cert certificate, MetaData metaData)
            {
                inputPDF = input;
                outputPDF = output;
                cert = certificate;
                metadata = metaData;
            }

        public void Sign(string reason, string contact, string location, bool visible)
        {
            using (PdfReader reader = new PdfReader(inputPDF))
            using (FileStream os = new FileStream(outputPDF, FileMode.Create))
            {
                PdfSigner signer = new PdfSigner(reader, os, new StampingProperties());


                PdfDocument pdfDoc = signer.GetDocument();

                // ✅ Get LAST PAGE
                int lastPage = pdfDoc.GetNumberOfPages();

                // Signature appearance
                PdfSignatureAppearance appearance = signer.GetSignatureAppearance();
                appearance
                    .SetReason(reason)
                    .SetContact(contact)
                    .SetLocation(location);

                if (visible)
                {
                    var pageSize = pdfDoc.GetPage(lastPage).GetPageSize();

                    float width = 200;
                    float height = 80;
                    
                    float x = pageSize.GetWidth() - width - 36; // right margin
                    float y = 36;

                    appearance
                        .SetPageRect(new Rectangle(x, y, width, height))
                        .SetPageNumber(lastPage);
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

