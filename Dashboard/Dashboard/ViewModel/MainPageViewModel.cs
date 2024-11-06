using static System.Net.Mime.MediaTypeNames;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading;
using System.Runtime.InteropServices;
using System.Reflection.Emit;
using Networking;
using Networking.Communication;
using Dashboard.SessionManager;
using System.Windows.Threading;

namespace Dashboard.ViewModel
{
    public class MainPageViewModel : INotifyPropertyChanged
    {
        private ICommunicator _communicator;
        private Server_Dashboard _serverSessionManager;
        private Client_Dashboard _clientSessionManager;

        private ObservableCollection<UserDetails> _userDetailsList;
        public ObservableCollection<UserDetails> UserDetailsList { 
            get { return _userDetailsList; } 
            set 
            { 
                OnPropertyChanged(nameof(UserDetailsList));
            } 
        } 
        

        public MainPageViewModel()  
        {
           
        }

        private string _serverPort;
        private string _serverIP;
        private string _userName;

        public string? UserName {
            get { return _userName; }
            set
            {
                _userName = value;
                OnPropertyChanged(nameof(UserName));
            }
        }
        public string? UserEmail {  get; private set; }

        public string? ServerIP { 
            get { return _serverIP; }
            set 
            {
                _serverIP = value; 
                OnPropertyChanged(nameof(ServerIP));
            } 
        }
        public string ServerPort
        {
            get { return _serverPort; }
            set
            {
                if (_serverPort != value)
                {
                    _serverPort = value;
                    OnPropertyChanged(nameof(ServerPort));
                }
            }
        }
        public bool IsHost { get; private set; } = false;

    

        public string CreateSession(string username,string useremail)
        {
            IsHost = true;
            _communicator = CommunicationFactory.GetCommunicator(isClientSide:false);
            _serverSessionManager = new(_communicator,username,useremail);
            _serverSessionManager.PropertyChanged += UpdateUserListOnPropertyChanged;
            string server_credentials =_serverSessionManager.Initialize();
            if (server_credentials != "failure")
            {
                string[] parts = server_credentials.Split(':');
                SynchronizationContext.Current?.Post(_ =>
                {
                    ServerIP = parts[0];
                }, null);
                SynchronizationContext.Current?.Post(_ =>
                {
                    ServerPort = parts[1];
                }, null);
                return "success";
            }
            return "failure";     
        }

        public string JoinSession(string username,string useremail,string serverip,string serverport)
        {
            IsHost = false;
            _communicator = CommunicationFactory.GetCommunicator();
            _clientSessionManager = new(_communicator,username,useremail);
            _clientSessionManager.PropertyChanged += UpdateUserListOnPropertyChanged;
            string server_response = _clientSessionManager.Initialize(serverip,serverport);
            if(server_response == "success")
            {
                UserName = username;
                UserEmail = useremail;
            }
            return server_response;
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged(string property)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }


        //public void UpdateParticipantsList(List<UserDetails> users)
        //{
        //    UserDetailsList.Clear();
        //    var userslist = IsHost ? _serverSessionManager.Users : _clientSessionManager.Users;

        //    foreach (var currUser in userslist)
        //    {
        //        string currUserId = currUser.userId;
        //        if (currUserId == "1")
        //        {
        //            string currUserName = currUser.userName + "  (Instructor)";

        //                UserDetailsList.Add(currUser);

        //        }
        //    }

        //    foreach (var currUser in users)
        //    {
        //        string currUserId = currUser.userId;
        //        if (currUserId == "1")
        //        {
        //            continue;
        //        }
        //        string currUserName = currUser.userName;
        //        UserDetailsList.Add(currUser);

        //    }

        //    return;
        //}
        private void UpdateUserListOnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(_serverSessionManager.Users) || e.PropertyName == nameof(_clientSessionManager.Users))
            {
                
                    var tempuserlist = new ObservableCollection<UserDetails>();

                    var users = _serverSessionManager?.Users ?? _clientSessionManager?.Users;
                    foreach (var user in users)
                    {
                        tempuserlist.Add(user);
                    }
                

                SynchronizationContext.Current?.Post(_ =>
                {
                    UserDetailsList=tempuserlist;
                }, null);

            }
        }


    }
}
