//using iText.Html2pdf;
using iText.Kernel.Pdf;
using iText.Signatures;
using SERVICEAPP.Utility;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;
using iText.Kernel.Geom;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.X509;
using System.IO;
using CRUDENTITY.Domain;

namespace SERVICEAPP.ServiceLayer
{
    public class PDFSignerandPdfGenerate : IPDFSignerandPdfGenerate
    {
        public byte[] GeneratePdf(string html)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                //ConverterProperties properties = new ConverterProperties();
                //properties.SetBaseUri(Directory.GetCurrentDirectory());
                //HtmlConverter.ConvertToPdf(html, stream, properties);

                return stream.ToArray();
            }
        }

        public AppointmentLetterDM GetAppointmentLetterData()
        {
            var domainmodel = new AppointmentLetterDM
            {
                Date = DateTime.Now,

                RollNo = "202345",
                CandidateName = "Vivek Kumar",
                IdNo = "EMP4567",

                State = "Delhi",
                District = "New Delhi",

                PostName = "PGT Computer Science",

                SchoolName = "EMRS Rampur",
                PostingDistrict = "Rampur",
                PostingState = "Uttar Pradesh",

                PayLevel = "Level 7",

                ReportingEmail = "info@nests.gov.in",

                JoiningPlace = "EMRS Rampur",

                ReservedCategory = "OBC",

                AppointmentPostName = "PGT Computer Science",

                OfficerName = "Director NESTS",
                OfficerDesignation = "National Education Society for Tribal Students"
            };

            return domainmodel;
        }

        //public void Sign(string reason, string contact, string location, bool visible)
        //{
        //    using (PdfReader reader = new PdfReader(inputPDF))
        //    using (FileStream os = new FileStream(outputPDF, FileMode.Create))
        //    {
        //        PdfSigner signer = new PdfSigner(reader, os, new StampingProperties());

        //        // Signature appearance
        //        PdfSignatureAppearance appearance = signer.GetSignatureAppearance();
        //        appearance
        //            .SetReason(reason)
        //            .SetContact(contact)
        //            .SetLocation(location);

        //        if (visible)
        //        {
        //            appearance
        //                .SetPageRect(new Rectangle(100, 100, 250, 150))
        //                .SetPageNumber(1);
        //        }

        //        signer.SetFieldName("Signature1");

        //        // BouncyCastle signing
        //        IExternalSignature pks =
        //            new PrivateKeySignature(cert.PrivateKey, DigestAlgorithms.SHA256);


        //        signer.SignDetached(
        //            pks,
        //            cert.Chain,
        //            null,
        //            null,
        //            null,
        //            0,
        //            PdfSigner.CryptoStandard.CADES
        //        );
        //    }
        //}
    }
}
