using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using System.Diagnostics;
using Networking;
using Networking.Communication;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace Dashboard.SessionManager
{
    [JsonSerializable(typeof(UserDetails))]
    public class UserDetails
    {
        [JsonInclude]
        public string? userName { get; set; }
        [JsonInclude]
        public bool IsHost { get; set; }
        [JsonInclude]
        public string? userId { get; set; }
        [JsonInclude]
        public string? userEmail { get; set; }
    }

    public class Client_Dashboard : ISessionManager,INotificationHandler,INotifyPropertyChanged
    {
        private ICommunicator _communicator;
        private string UserName { get; set; }
        private string UserEmail { get; set; }
        private string UserID { get; set; }


        private ObservableCollection<UserDetails> _clientUserDetailsList;

        public ObservableCollection<UserDetails> ClientUserDetailsList
        {
            get { return _clientUserDetailsList; }
            set
            {
                _clientUserDetailsList = value;
                OnPropertyChanged(nameof(ClientUserDetailsList));
            }
        }

        public ObservableCollection<UserDetails> Users { get; set; } = new ObservableCollection<UserDetails>();
        /// <summary>
        /// Creates an instance of the chat messenger.
        /// </summary>
        /// <param name="communicator">The communicator instance to use</param>
        public Client_Dashboard(ICommunicator communicator,string username,string useremail)
        {
            _communicator = communicator;
            _communicator.Subscribe("Dashboard", this,isHighPriority:true);
            UserName = username;
            UserEmail = useremail;
            Users.CollectionChanged += (s, e) => OnPropertyChanged(nameof(Users));
        }

        public string Initialize(string serverIP, string serverPort)
        {

            string server_response = _communicator.Start(serverIP, serverPort);
            if (server_response == "success")
            {
                return "success";
            }
            return "failure";
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

        public int user_count = 0;

        public void SendMessage(string clientIP,string message)
        {
            string json_message = JsonSerializer.Serialize(message);
            try
            {
                _communicator.Send(json_message, "Dashboard",null);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error sending message: {ex.Message}");
            }
        }

        public void SendInfo(string username,string useremail)
        {
            Trace.WriteLine("[dash] sent info from client to server");
            DashboardDetails details = new DashboardDetails();
            details.User = new UserDetails();
            details.User.userName =  username;
            details.User.userEmail = useremail;
            details.User.userId = UserID;
            details.Action = Action.ClientUserConnected;
            string json_message = JsonSerializer.Serialize(details);
            try
            {
                _communicator.Send(json_message, "Dashboard", null);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error sending message: {ex.Message}");
            }
        }

        public void UserLeft()
        {
            DashboardDetails details = new DashboardDetails();
            UserDetails userData = new UserDetails();
            userData.userName = UserName;
            details.Action = Action.ClientUserLeft;
            string json_message = JsonSerializer.Serialize(details);
            try
            {
                _communicator.Send(json_message, "Dashboard", null);
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
                try
                {
                    var details = JsonSerializer.Deserialize<DashboardDetails>(message);
                    if (details == null)
                    {
                        Console.WriteLine("Error: Deserialized message is null");
                        return;
                    }

                    switch (details.Action)
                    {
                        case Action.ServerSendUserID:
                            HandleRecievedUserInfo(details);
                            break;
                        case Action.ServerUserAdded:
                            HandleUserConnected(details);
                            break;
                        case Action.ServerUserLeft:
                            HandleUserLeft(details);
                            break;
                        case Action.ServerEnd:
                            HandleEndOfMeeting();
                            break;
                        default:
                            Console.WriteLine($"Unknown action: {details.Action}");
                            break;

                    }
                }
                catch (JsonException)
                {
                    var userList = JsonSerializer.Deserialize<List<UserDetails>>(message);
                    Users = new ObservableCollection<UserDetails>(userList);
                }
            }
            catch (JsonException ex)
            {
                Console.WriteLine($"Error deserializing message: {ex.Message}");
            }
        }

       private void HandleRecievedUserInfo(DashboardDetails message)
        {
            UserID = message.User.userId;
            SendInfo(UserName, UserEmail);
        }
        private void HandleUserConnected(DashboardDetails message)
        {
            UserDetails userData = message.User;
            string newuserid = userData.userId;
            if (Users.Count >= int.Parse(newuserid))
            {
                Users[int.Parse(newuserid)-1] = userData;
            }
            else
            {
                Users.Add(userData);
            }
        }

        private void HandleUserLeft(DashboardDetails message)
        {
            UserDetails userData = message.User;
            Users.Clear();
        }
        private void HandleEndOfMeeting()
        {
            
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string property)
        {
            Trace.WriteLine("[dashClient] user list chnaged");
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }

    }
}
