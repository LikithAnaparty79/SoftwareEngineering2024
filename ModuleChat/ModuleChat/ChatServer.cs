
using Networking.Communication;
using Networking;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text.Json;

namespace ChatApplication
{
    public class ChatServer : INotificationHandler
    {

        private ICommunicator _communicator = CommunicationFactory.GetCommunicator(false);

        public readonly Dictionary<int, string> _clientUsernames = new(); // Maps clientId to username

        public event EventHandler ClientUsernamesUpdated; //new

        public string clientId { get; set; }

        public int messageCounter = 0;


        public void Start()
        {
            _communicator.Subscribe("ChatModule", this, isHighPriority: true);

        }

        public void Stop()
        {
            _communicator.Stop();
        }


        public void OnDataReceived(string serializedData)
        {
            var dataParts = serializedData.Split('|');
            if (dataParts.Length < 3) return;

            string messageType = dataParts[0];
            string messageContent = dataParts[1];
            string senderUsername = dataParts[2];
            string senderId = dataParts[3];
            string recipientId = dataParts.Length > 4 ? dataParts[4] : null;
       
            messageCounter++;


            if (messageType == "connect")
            {

                int clientIdInt = Int32.Parse(clientId);
                _clientUsernames[clientIdInt] = senderUsername;
                string tp = "";

                tp = JsonSerializer.Serialize(_clientUsernames);

                string formattedMessage = $"clientlist|{tp}";


                _communicator.Send(formattedMessage, "ChatModule", destination: null);

            }

            if (messageType == "private")
            {
                messageContent = $"[PRIVATE] : {messageContent}";

                //_communicator.Send($"private|{messageContent}|{senderUsername}", "ChatModule", recipientId);
                _communicator.Send($"{senderUsername} :.: {messageContent} |private|{senderUsername}|{messageContent} ", "ChatModule", recipientId);
            }
            else
            {

                _communicator.Send($"{senderUsername} :.: {messageContent} |abc", "ChatModule", destination: null);
            }

        }


    }
}