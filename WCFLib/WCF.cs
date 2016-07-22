using System;
using System.IO;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Web;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WCFLib
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, UseSynchronizationContext = false, ConcurrencyMode = ConcurrencyMode.Multiple)]
    public class WCF :IWCF
    {
        public static string STATIC_ASSETS = Application.StartupPath + "\\static_assets";
        public static string CERTIFIATES = Application.StartupPath + "\\certificates";

        public WCF()
        {
            CreateFolders();
            WCF.CertificateWork(8701);
        }

        private void CreateFolders()
        {
            FileTools.CreateFolderIfNotExists(STATIC_ASSETS);
            FileTools.CreateFolderIfNotExists(CERTIFIATES);
        }

        public static void CertificateWork(int port)
        {
            try
            {
                if (Directory.GetFiles(CERTIFIATES).Length <= 0)
                {
                    var cert = CertificatesTools.CreateSelfSignedCertificate("localhost");
                    CertificatesTools.SaveCertificate(cert, FileTools.GetUniqueFilename(CERTIFIATES, ".pfx"));
                }
                else
                {
                    string certFile = Directory.GetFiles(CERTIFIATES)[0];
                    CertificatesTools.AddCertToLocal(certFile);
                }
                //supression de la réservation du port pour l'application
                NetshTools.ExecuteNetShCommand(string.Format("http delete sslcert ipport=0.0.0.0:{0}", port));
                //Ajout de la réservation du port pou rl'application
                NetshTools.AddCertificateToApplication(port, Directory.GetFiles(CERTIFIATES)[0], GuidTools.NewGuid());
            }
            catch (Exception)
            {
            }
        }

        public Stream ServeFile(string filename)
        {
            return StreamFile(filename);
        }

        public Stream StreamFile(string filename)
        {
            string fullFilename = filename;
            if(filename!=null || !File.Exists(filename))
            {
                fullFilename = FileTools.FindFile(filename, STATIC_ASSETS);                
            }
            if (File.Exists(fullFilename))
            {
                WebOperationContext.Current.OutgoingResponse.ContentType = FileTools.GetFileContentType(fullFilename); 
                WebOperationContext.Current.OutgoingResponse.StatusCode = System.Net.HttpStatusCode.OK;
                return FileTools.StreamFromFile(fullFilename);
            }
            else
            {
                WebOperationContext.Current.OutgoingResponse.StatusCode = System.Net.HttpStatusCode.NotFound;
                return Stream.Null;
            }
        }

        public Stream AppFile()
        {
            return StreamFile("index.html");
        }

        public string Test()
        {
            return "Hello!";
        }
    }
}
