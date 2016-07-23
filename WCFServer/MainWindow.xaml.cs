using Microsoft.AspNet.SignalR;
using Microsoft.Owin.Cors;
using Microsoft.Owin.Hosting;
using Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WCFLib;

namespace WCFServer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private WCFLib.WCF service;
        private ServiceHost WCFHostService;

        public MainWindow()
        {
            InitializeComponent();
        }

        /*private void StartWCFServer2()
        {
            WCF.CertificateWork(8080);
            Uri baseAddress = new Uri("https://localhost:8080");

            // Create the ServiceHost.
            ServiceHost host = new ServiceHost(typeof(WebSocketsServer), baseAddress);
            // Enable metadata publishing.
            ServiceMetadataBehavior smb = new ServiceMetadataBehavior();
            smb.HttpGetEnabled = false;
            smb.MetadataExporter.PolicyVersion = PolicyVersion.Policy15;
            host.Description.Behaviors.Add(smb);

            CustomBinding binding = new CustomBinding();
            binding.Elements.Add(new ByteStreamMessageEncodingBindingElement());
            HttpsTransportBindingElement transport = new HttpsTransportBindingElement();
            //transport.WebSocketSettings = new WebSocketTransportSettings();
            transport.WebSocketSettings.TransportUsage = WebSocketTransportUsage.Always;
            transport.WebSocketSettings.CreateNotificationOnConnection = true;
            binding.Elements.Add(transport);

            host.AddServiceEndpoint(typeof(IWebSocketsServer), binding, "");

            host.Open();

            // Close the ServiceHost.
            //host.Close();
        }*/

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            StartWCFServer1();
            //StartWCFServer2();
            StartSignalRServer();
        }

        private void StartSignalRServer()
        {
            string url = "https://*:8080";
            WCF.CertificateWork(8080);
            IDisposable disposable = WebApp.Start(url);
        }

        private void StartWCFServer1()
        {
            try
            {
                //System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                service = new WCF();
                WCFHostService = new ServiceHost(service);
                WCFHostService.Open();
            }
            catch (Exception er)
            {
                MessageBox.Show(er.ToString());
            }
        }
    }

    class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.UseCors(CorsOptions.AllowAll);
            app.MapSignalR();
        }
    }

    public class MyHub : Hub
    {
        private static Thread clockThread=null;
        public override Task OnConnected()
        {
            //string name = Context.User.Identity.Name;
            if (clockThread == null)
            {
                clockThread = new Thread(Write);
                clockThread.Start();
            }

            return base.OnConnected();
        }

        public void Write()
        {
            while (true)
            {
                Clients.All.setTime(DateTime.Now.ToString());
                Thread.Sleep(1);
            }
        }

        public void Send(string name, string message)
        {
            Clients.All.addMessage(name, message);
        }
    }

    /*[ServiceContract(CallbackContract = typeof(IProgressContext))]
    public interface IWebSocketsServer
    {
        [OperationContract(IsOneWay = true, Action = "*")]
        void SendMessageToServer(Message msg);
    }

    [ServiceContract]
    interface IProgressContext
    {
        [OperationContract(IsOneWay = true, Action = "*")]
        void ReportProgress(Message msg);
    }

    public class WebSocketsServer : IWebSocketsServer
    {
        public void SendMessageToServer(Message msg)
        {
            var callback = OperationContext.Current.GetCallbackChannel<IProgressContext>();
            if (msg.IsEmpty || ((IChannel)callback).State != CommunicationState.Opened)
            {
                return;
            }

            byte[] body = msg.GetBody<byte[]>();
            string msgTextFromClient = Encoding.UTF8.GetString(body);

            string msgTextToClient = string.Format(
                "Got message {0} at {1}",
                msgTextFromClient,
                DateTime.Now.ToLongTimeString());

            for(int i=0;i<100000; i++)
            {
                callback.ReportProgress(CreateMessage(msgTextToClient+i));
                Thread.Sleep(1);
            }
        }

        private Message CreateMessage(string msgText)
        {
            Message msg = ByteStreamMessage.CreateMessage(
                new ArraySegment<byte>(Encoding.UTF8.GetBytes(msgText)));

            msg.Properties["WebSocketMessageProperty"] =
                new WebSocketMessageProperty
                {
                    MessageType = WebSocketMessageType.Text
                };

            return msg;
        }
    }*/
}
