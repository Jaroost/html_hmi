using CERTENROLLLib;
using System;
using System.IO;
using System.Security.Cryptography.X509Certificates;

namespace WCFLib
{
    public class CertificatesTools
    {
        public static X509Certificate2 CreateSelfSignedCertificate(string subjectName)
        {
            // create DN for subject and issuer
            var dn = new CX500DistinguishedName();
            dn.Encode("CN=" + subjectName, X500NameFlags.XCN_CERT_NAME_STR_NONE);

            // create a new private key for the certificate
            CX509PrivateKey privateKey = new CX509PrivateKey();
            privateKey.ProviderName = "Microsoft Base Cryptographic Provider v1.0";
            privateKey.MachineContext = true;
            privateKey.Length = 4096;
            privateKey.KeySpec = X509KeySpec.XCN_AT_SIGNATURE; // use is not limited
            privateKey.ExportPolicy = X509PrivateKeyExportFlags.XCN_NCRYPT_ALLOW_PLAINTEXT_EXPORT_FLAG;
            privateKey.Create();

            // Use the stronger SHA512 hashing algorithm
            var hashobj = new CObjectId();
            hashobj.InitializeFromAlgorithmName(ObjectIdGroupId.XCN_CRYPT_HASH_ALG_OID_GROUP_ID,
                ObjectIdPublicKeyFlags.XCN_CRYPT_OID_INFO_PUBKEY_ANY,
                AlgorithmFlags.AlgorithmFlagsNone, "SHA512");

            // add extended key usage if you want - look at MSDN for a list of possible OIDs
            var oid = new CObjectId();
            oid.InitializeFromValue("1.3.6.1.5.5.7.3.1"); // SSL server
            var oidlist = new CObjectIds();
            oidlist.Add(oid);
            var eku = new CX509ExtensionEnhancedKeyUsage();
            eku.InitializeEncode(oidlist);

            // Create the self signing request
            var cert = new CX509CertificateRequestCertificate();
            cert.InitializeFromPrivateKey(X509CertificateEnrollmentContext.ContextMachine, privateKey, "");
            cert.Subject = dn;
            cert.Issuer = dn; // the issuer and the subject are the same
            cert.NotBefore = DateTime.Now;
            // this cert expires immediately. Change to whatever makes sense for you
            cert.NotAfter = DateTime.Now.AddYears(10);
            cert.X509Extensions.Add((CX509Extension)eku); // add the EKU
            cert.HashAlgorithm = hashobj; // Specify the hashing algorithm
            cert.Encode(); // encode the certificate

            // Do the final enrollment process
            var enroll = new CX509Enrollment();
            enroll.InitializeFromRequest(cert); // load the certificate
            enroll.CertificateFriendlyName = subjectName; // Optional: add a friendly name
            string csr = enroll.CreateRequest(); // Output the request in base64
            // and install it back as the response
            enroll.InstallResponse(InstallResponseRestrictionFlags.AllowUntrustedCertificate,
                csr, EncodingType.XCN_CRYPT_STRING_BASE64, ""); // no password
            // output a base64 encoded PKCS#12 so we can import it back to the .Net security classes
            var base64encoded = enroll.CreatePFX("", // no password, this is for internal consumption
                PFXExportOptions.PFXExportChainWithRoot);

            // instantiate the target class with the PKCS#12 data (and the empty password)
            return new System.Security.Cryptography.X509Certificates.X509Certificate2(
                System.Convert.FromBase64String(base64encoded), "",
                // mark the private key as exportable (this is usually what you want to do)
                System.Security.Cryptography.X509Certificates.X509KeyStorageFlags.Exportable
            );
        }

        public static bool AddCertToLocal(string filename)
        {
            return AddCertToLocal(new X509Certificate2(filename));
        }

        public static bool AddCertToLocal(X509Certificate2 cert)
        {
            return AddCertToStore(cert, StoreName.My, StoreLocation.LocalMachine);
        }
        
        public static bool AddCertToStore(X509Certificate2 cert, StoreName sn, StoreLocation sl)
        {
            bool bRet = false;

            try
            {
                X509Store store = new X509Store(sn, sl);
                store.Open(OpenFlags.MaxAllowed);
                store.Add(cert);

                store.Close();
            }
            catch
            {

            }

            return bRet;
        }

        public static bool RemoveCertFromLocal(string filename)
        {
            return RemoveCertFromStore(filename, StoreName.My, StoreLocation.LocalMachine);
        }

        public static bool RemoveCertFromStore(string filename, StoreName st, StoreLocation sl)
        {
            try
            {
                X509Certificate2 cert = new X509Certificate2(filename);
                X509Store certStore = new X509Store(st, sl);
                // Try to open the store.
                certStore.Open(OpenFlags.ReadWrite);

                // Find the certificate that matches the thumbprint.
                X509Certificate2Collection certCollection = certStore.Certificates.Find(X509FindType.FindByThumbprint, cert.Thumbprint, false);
                certStore.RemoveRange(certCollection);
                certStore.Close();
                return true;
            }
            catch(Exception)
            {
                return false;
            }            
        }

        /// <summary>
        /// Sauvegarde le certificate sous forme de fichier (filename)
        /// </summary>
        public static void SaveCertificate(X509Certificate2 cert, string filename)
        {
            File.WriteAllBytes(filename, cert.Export(X509ContentType.Pkcs12));
            /*StringBuilder builder = new StringBuilder();

            builder.AppendLine("-----BEGIN CERTIFICATE-----");
            builder.AppendLine(Convert.ToBase64String(cert.Export(X509ContentType.Cert), Base64FormattingOptions.InsertLineBreaks));
            builder.AppendLine("-----END CERTIFICATE-----");

            File.WriteAllText(filename, builder.ToString());*/
        }
    }
}
