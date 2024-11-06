using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Text.Json;
using System.Text.Json.Serialization;
using Networking;
using Networking.Communication;

namespace Dashboard.SessionManager
{
    public enum Action
    {
        ClientUserConnected,
        ClientUserLeft,
        ServerSendUserID,
        ServerUserAdded,
        ServerUserLeft,
        ServerEnd,  
        StartOfMeeting
    }

   

    public class Server_Dashboard : ISessionManager , INotificationHandler
    {
        private ICommunicator _communicator;
        private string UserName { get; set; }
        private string UserEmail { get; set; }

        public Server_Dashboard(ICommunicator communicator,string username,string useremail)
        {
            _communicator = communicator;
            _communicator.Subscribe("Dashboard", this, true);
            UserName = username;
            UserEmail = useremail;
            Users.CollectionChanged += (s, e) => OnPropertyChanged(nameof(Users));
        }

        public string Initialize()
        {
            var server_user = new UserDetails();
            server_user.userName = UserName;
            server_user.userEmail = UserEmail;
            server_user.userId = "1";
            server_user.IsHost = true;
            Users.Add(server_user);

            string server_credentials = "failure";
            while (server_credentials == "failure")
            {
                server_credentials = _communicator.Start();
            }
            if (server_credentials != "failure")
            {
                string[] parts = server_credentials.Split(':');
                string server_ip = parts[0];
                string server_port = parts[1];
            }
            return server_credentials;
        }

   

        [JsonSerializable(typeof(DashboardDetails))]
        public class DashboardDetails
        {
            [JsonInclude]
            public UserDetails? User { get; set; }
            [JsonInclude]
            public bool IsConnected { get; set; }
            [JsonInclude]
            public Action Action { get; set; }
            [JsonInclude]
            public string? msg { get; set; }
        }



        private ObservableCollection<UserDetails> _serverUserDetailsList;

        public ObservableCollection<UserDetails> ServerUserDetailsList
        {
            get { return _serverUserDetailsList; }
            set
            {
                _serverUserDetailsList = value;
            }
        }

        public ObservableCollection<UserDetails> Users { get; set; } = new ObservableCollection<UserDetails>();

        public int user_count = 1;


        public void BroadcastMessage(string message)
        {
            foreach (UserDetails user in Users)
            {
                SendMessage(user.userId, message);
            }
        }
        public void SendMessage(string clientIP, string message)
        {
            try
            {
                _communicator.Send(message,"Dashboard", clientIP);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error sending message: {ex.Message}");
            }
        }

        public void OnDataReceived(string message)
        {
            try
            {
                DashboardDetails? details = JsonSerializer.Deserialize<DashboardDetails>(message);
                if (details == null)
                {
                    Console.WriteLine("Error: Deserialized message is null");
                    return;
                }

                switch (details.Action)
                {
                    case Action.ClientUserConnected:
                        HandleUserConnected(details);
                        break;
                    case Action.ClientUserLeft:
                        HandleUserLeft(details);
                        break;
                    default:
                        Console.WriteLine($"Unknown action: {details.Action}");
                        break;

                }
            }
            catch (JsonException ex)
            {
                Console.WriteLine($"Error deserializing message: {ex.Message}");
            }
        }

        private void HandleUserConnected(DashboardDetails details)
        {
            Trace.WriteLine("[dashServer] recieved client info");
            if (details.User != null)
            {
                string newuserid = "1"; 
                foreach(UserDetails user in Users)
                {
                    if(details.User.userId == user.userId)
                    {
                        user.userName=details.User.userName;
                        user.userEmail = details.User.userEmail;
                        user.IsHost=false;
                        newuserid = user.userId;
                    }
                }
                var listUsers = new List<UserDetails>(Users);
                var jsonUserList = JsonSerializer.Serialize(listUsers);
                SendMessage(newuserid,jsonUserList);
                DashboardDetails dashboardMessage = new();
                dashboardMessage.User = Users[int.Parse(newuserid)-1];
                dashboardMessage.Action = Action.ServerUserAdded;
                dashboardMessage.msg = "user with " + details.User.userName + " Joined";
                string json_message = JsonSerializer.Serialize(dashboardMessage);
                BroadcastMessage(json_message);
            }
        }

        private void HandleUserLeft(DashboardDetails details)
        {
            if (details.User != null)
            {
                DashboardDetails dashboardMessage = new();
                dashboardMessage.Action = Action.ServerUserLeft;
                dashboardMessage.msg = "user with " + details.User.userName + " Left ";
                string json_message = JsonSerializer.Serialize(dashboardMessage);
                BroadcastMessage(json_message);
                foreach (UserDetails user in Users)
                {
                    if(user.userId == details.User.userId)
                    {
                       Users.Remove(user);  
                    }
                }
            }
        }

        private void HandleEndOfMeeting()
        {
            DashboardDetails dashboardMessage = new();
            dashboardMessage.Action = Action.ServerEnd;
            dashboardMessage.msg = "Meeting Ended";
            string json_message = JsonSerializer.Serialize(dashboardMessage);
            BroadcastMessage(json_message);
            Users.Clear();
        }

        private void HandleStartOfMeeting()
        {
            Console.WriteLine("Meeting started");
        }
        


        public void OnClientJoined(TcpClient socket)
        {   
            user_count++;
            UserDetails details = new UserDetails();
            details.userId = user_count.ToString();
            Users.Add(details);
            _communicator.AddClient(user_count.ToString(), socket);
            DashboardDetails dashboardMessage = new DashboardDetails();
            dashboardMessage.User = details;
            dashboardMessage.Action = Action.ServerSendUserID;
            string json_message = JsonSerializer.Serialize(dashboardMessage);
            _communicator.Send(json_message,"Dashboard",user_count.ToString());
        }

        public void OnClientLeft(string clientId)
        {
            _communicator.RemoveClient(clientId);
            string leftuserName = null;
            foreach (UserDetails user in Users)
            {
                if (user.userId==clientId)
                {
                    leftuserName = user.userName;
                }
            }
            if (leftuserName == null) leftuserName = clientId;
            Users.Clear();
            BroadcastMessage("user with" + (leftuserName) + "left");
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string property)
        {
            foreach (UserDetails user in Users)
            {
            }
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }



    }
}

