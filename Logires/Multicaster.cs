using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Net.NetworkInformation;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Runtime.Serialization.Formatters.Binary;

namespace Logires
{
		[Serializable]
		public sealed class Message
		{
		    public string ID { get; set; }
		    public object Value { get; set; }
		}

    public sealed class Multicaster
    {
        public static readonly int port = 43662;
        public static readonly IPEndPoint localEndPoint = new IPEndPoint(IPAddress.Any, port);
        public static readonly IPAddress multicastAddress = IPAddress.Parse("224.0.0.1");
        public static readonly IPEndPoint remoteEndPoint = new IPEndPoint(multicastAddress, port);

        public event Action<Message> MessageReceived;

        private UdpClient _client = null;
        private IPEndPoint _lastReceivedIP = new IPEndPoint(0, 0);
        private static Multicaster _instance = null;

        public static Multicaster Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new Multicaster();
                }

                return _instance;
            }
        }

        private Multicaster()
        {
            _client = new UdpClient();

						_client.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
						_client.ExclusiveAddressUse = false;

            _client.Client.Bind(localEndPoint);
            _client.JoinMulticastGroup(multicastAddress);

            Task.Run(() =>
            {
            		while (true)
            	  {
            	  		try
            	  		{
            	      		var data = _client.Receive(ref _lastReceivedIP);
            	      
            	      		var mS = new MemoryStream(data);
            	      		var bF = new BinaryFormatter();
            	      		var message = (Message)bF.Deserialize(mS);

            	      		Console.WriteLine("Message received");
            	      
            	  				MessageReceived?.Invoke(message);
            	  		}
            	  		catch (Exception ex)
            	  		{
            	  				Console.WriteLine(ex);
            	  		}
            		}
            });
        }

        ~Multicaster()
        {
            if (_client != null)
            {
                _client.DropMulticastGroup(multicastAddress);
                _client.Close();
            }
        }

        public void BroadcastMessage(Message message)
        {
            var mS = new MemoryStream();
            var bF = new BinaryFormatter();
            bF.Serialize(mS, message);
            var data = mS.GetBuffer();

            Console.WriteLine("Message sended");

            _client.Send(data, data.Length, remoteEndPoint);
        }
    }
}
