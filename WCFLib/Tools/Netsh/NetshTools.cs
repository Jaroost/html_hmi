using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace WCFLib
{
    public class NetshTools
    {
        public static void ExecuteNetShCommand(string arguments)
        {

            Process bindPortToCertificate = new Process();
            bindPortToCertificate.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            bindPortToCertificate.StartInfo.FileName = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.SystemX86), "netsh.exe");
            bindPortToCertificate.StartInfo.Arguments = arguments;
            bindPortToCertificate.Start();
            bindPortToCertificate.WaitForExit();

            /*string filename = FileTools.GetUniqueFilename(ApplicationTools.StartupPath, ".bat");
            File.WriteAllText(filename, System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.SystemX86), "netsh.exe") + " " + arguments + Environment.NewLine + "PAUSE");
            Process proc = Process.Start(filename);
            proc.WaitForExit();
            File.Delete(filename);*/
        }

        public static void AddCertificateToApplication(int port, string cerFilename, Guid appid)
        {
            string certPath = System.IO.Path.Combine(ApplicationTools.StartupPath, cerFilename);
            X509Certificate2 certificate = new X509Certificate2(certPath);
            // netsh http add sslcert ipport=0.0.0.0:<port> certhash={<thumbprint>} appid={<some GUID>}
            File.AppendAllText("cert.txt", string.Format("http add sslcert ipport=0.0.0.0:{0} certhash={1} appid={{{2}}}", port, certificate.Thumbprint, appid));
            ExecuteNetShCommand(string.Format("http add sslcert ipport=0.0.0.0:{0} certhash={1} appid={{{2}}}", port, certificate.Thumbprint, appid));
        }

        public static void AddCertificateToApplication(int port, X509Certificate2 certificate, Guid appid)
        {
            File.AppendAllText("cert.txt", string.Format("http add sslcert ipport=0.0.0.0:{0} certhash={1} appid={{{2}}}", port, certificate.Thumbprint, appid));
            // netsh http add sslcert ipport=0.0.0.0:<port> certhash={<thumbprint>} appid={<some GUID>}
            ExecuteNetShCommand(string.Format("http add sslcert ipport=0.0.0.0:{0} certhash={1} appid={{{2}}}", port, certificate.Thumbprint, appid));
        }
    }
}
