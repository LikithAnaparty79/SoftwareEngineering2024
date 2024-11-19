﻿// MainViewModel.cs
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.DirectoryServices;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.Pkcs;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using Content;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using static System.Net.Mime.MediaTypeNames;

//using Dashboard;

namespace Content.ChatViewModel
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private ChatClient _client;
        private ChatServer _server;
        private string _chatHistory;
        private string _message;
        private ObservableCollection<string> _clientList = new();
        public string Username { get; set; }
        public ObservableCollection<ChatMessage> Messages { get; set; }
        public ObservableCollection<ChatMessage> SearchResults { get; set; }
        ObservableCollection<string> ClientUsernames { get; set; }
        public string UserProfileUrl { get; set; }

        public MainViewModel()
        {
            _server = new ChatServer();
            _server.Start();
            _client = new ChatClient();
            _client.LatestAction += SyncClientList;
            _client.MessageReceived += OnMessageReceived;

            //_server = new ChatServer();

            ClientUsernames = new ObservableCollection<string>();
            _client.ClientListUpdated += (s, list) =>
            {
                Console.WriteLine("ClientListUpdated event triggered with clients: " + string.Join(", ", list)); // Debug log
                ClientList.Clear();
                foreach (string clientId in list)
                {
                    ClientList.Add(clientId);
                }
            };

            Messages = new ObservableCollection<ChatMessage>();
            SendMessageCommand = new RelayCommand(SendButton_Click);
            EnterKeyCommand = new RelayCommand(OnEnterKeyPressed);
        }


        /// <summary>
        /// Sets user details for the client including username, user ID, and profile URL.
        /// </summary>
        /// <param name="username">The username of the client.</param>
        /// <param name="userid">The user ID of the client.</param>
        /// <param name="userProfileUrl">The profile URL of the client.</param>

        public void SetUserDetails_client(string username, string userid, string userProfileUrl)
        {

            _client = new ChatClient
            {
                ClientId = userid,
                Username = username,
                UserProfileUrl = userProfileUrl
            };

        }

        /// <summary>
        /// Sets user details for the server, including username and profile URL, and starts the server.
        /// </summary>
        /// <param name="username">The username of the server user.</param>
        /// <param name="userProfileUrl">The profile URL of the server user.</param>

        public void SetUserDetails_server(string username, string userProfileUrl)
        {

            _server = new ChatServer();
            _client = new ChatClient
            {
                ClientId = "1",
                Username = username,
                UserProfileUrl = userProfileUrl
            };

        }

        public string ChatHistory
        {
            get => _chatHistory;
            set { _chatHistory = value; OnPropertyChanged(); }
        }

        public string Message
        {
            get => _message;
            set { _message = value; OnPropertyChanged(); }
        }
        public ObservableCollection<string> ClientList
        {
            get => _clientList;
            set { _clientList = value; OnPropertyChanged(); }
        }
        private bool _isNotFoundPopupOpen = false;
        public bool IsNotFoundPopupOpen
        {
            get => _isNotFoundPopupOpen;
            set
            {
                if (_isNotFoundPopupOpen != value)
                {
                    _isNotFoundPopupOpen = value;
                    OnPropertyChanged(nameof(IsNotFoundPopupOpen));
                }
            }
        }

        //connect button command
        public string _username_text;
        public string Username_Text
        {
            get => _username_text;
            set
            {
                _username_text = value; OnPropertyChanged(nameof(Username_Text));
            }
        }

        //----send button command
        public string _messagetextbox_text;
        public string MessageTextBox_Text
        {
            get => _messagetextbox_text;
            set
            {
                _messagetextbox_text = value; OnPropertyChanged(nameof(MessageTextBox_Text));
            }
        }

        //selected username
        public string _selectedusername;
        public string Selectedusername
        {
            get => _selectedusername;
            set
            {
                _selectedusername = value; OnPropertyChanged(nameof(Selectedusername));
            }
        }

        public event EventHandler RequestVariable;

        private string _recipientt;
        public string Recipientt
        {
            get => _recipientt;
            set
            {
                _recipientt = value;
                OnPropertyChanged(nameof(Recipientt));
            }
        }

        //selected emoji
        public string _emoji;
        public string Emoji
        {
            get => _emoji;
            set
            {
                _emoji = value; OnPropertyChanged(nameof(Emoji));
            }
        }


        public ObservableCollection<string> Clientte => _client._clientListobs;

        public string _dash_ServerIp;
        public string _dash_ServerPort;

        public ICommand ConnectCommand { get; }
        public ICommand SendMessageCommand { get; }
        public ICommand SearchMessagesCommmand { get; }
        public ICommand EnterKeyCommand { get; }




        private static MainViewModel? s_contentVMInstance;
        public static MainViewModel GetInstance
        {
            get
            {
                if (s_contentVMInstance == null)
                {
                    s_contentVMInstance = new MainViewModel();
                }
                return s_contentVMInstance;
            }
        }

        /// <summary>
        /// Deletes a message by marking it as deleted, updating its content and notifying the UI.
        /// </summary>
        /// <param name="message">The message to delete.</param>

        /// </summary>
        public void DeleteMessage(ChatMessage message)
        {
            if (message != null)
            {
                message.IsDeleted = true;       // Notify UI about the deleted state
                message.Content = "[Message deleted]";
                message.Text = "[Message deleted]";
            }
        }

        /// <summary>
        /// Sends a message to the recipient (if specified) or broadcasts it publicly.
        /// Clears the input field after sending.
        /// </summary>

        private void SendButton_Click()
        {
            string message = MessageTextBox_Text;

            string recipient = Recipientt;

            string? recipient_id = null;

            if (recipient != null && recipient != "Everyone")
            {
                recipient_id = _client._client_dict.FirstOrDefault(x => x.Value == recipient).Key.ToString();
            }

            if (!string.IsNullOrWhiteSpace(message) && message != "  Type something...")
            {
                _client.SendMessage(
                    message,
                    recipientId: recipient_id); // Send private if recipient selected

                MessageTextBox_Text = "  Type something...";
            }
        }
        public event Action<ChatMessage> MessageAdded;

        /// <summary>
        /// Handles updating the chat history and processes incoming messages.
        /// Adds the message to the Messages collection if it does not already exist.
        /// </summary>
        /// <param name="sender">The sender of the message.</param>
        /// <param name="message">The received message content.</param>

        private void OnMessageReceived(object sender, string message)
        {
            Debug.WriteLine($"Message received: {message}");
            ChatHistory += message + Environment.NewLine;
            string[] parts = message.Split(new[] { " :.: " }, StringSplitOptions.None);
            if (parts.Length == 2)
            {
                string user = parts[0].Trim();          // senderUsername
                string[] msg = parts[1].Split('|');
                string messageContent = msg[0].Trim();
                bool isSent = false;
                isSent = (user == Username_Text);
                // Prevent adding the same message again by checking if it already exists in the collection
                bool messageExists = Messages.Any(m => m.User == user && m.Content == messageContent);

                if (!messageExists)
                {
                    //Application.Current.Dispatcher.Invoke(() =>
                    System.Windows.Application.Current.Dispatcher.Invoke(() =>
                    {
                        ChatMessage chatMessage = new ChatMessage
                        (
                            user,
                            messageContent,
                            DateTime.Now.ToString("HH:mm"),
                            isSent
                        );
                        Messages.Add(chatMessage);
                        MessageAdded?.Invoke(chatMessage);

                    });
                }
            }
        }
        private void MessageTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                OnEnterKeyPressed();
            }
        }

        /// <summary>
        /// Handles pressing the Enter key to send a message if the input field is not empty.
        /// </summary>

        private void OnEnterKeyPressed()
        {

            if (!string.IsNullOrWhiteSpace(MessageTextBox_Text))
            {
                SendButton_Click();
                MessageTextBox_Text = string.Empty;

            }
        }

        /// <summary>
        /// Sends a message to the server and clears the input field.
        /// </summary>

        private void SendMessage()
        {
            _client.SendMessage(Message);
            Message = string.Empty;
        }

        /// <summary>
        /// Handles updates to the client username list when the server triggers an update event.
        /// </summary>
        /// <param name="sender">The sender of the event.</param>
        /// <param name="e">Event data.</param>

        public void OnClientUsernamesUpdated(object? sender, EventArgs e)
        {
            Console.WriteLine("OnClientUsernamesUpdated event triggered");
        }

        /// <summary>
        /// Searches messages based on a query string. Highlights the matched text in messages and adds the message to the SearchResults collection.
        /// </summary>
        /// <param name="query">The query string to search for in messages.</param>

        public void SearchMessages(string query)
        {
            if (SearchResults == null)
            {
                SearchResults = new ObservableCollection<ChatMessage>();
            }
            SearchResults.Clear();
            if (string.IsNullOrWhiteSpace(query))
            {
                return;
            }

            string lowerCaseQuery = query.ToLower();
            var foundMessages = Messages.Where(m => m != null && (m.Content).ToLower().Contains(lowerCaseQuery)).ToList();

            foreach (ChatMessage message in Messages)
            {
                message.Content = message.Text;

                if (message.Content.ToLower().Contains(lowerCaseQuery))
                {
                    int startIndex = message.Content.ToLower().IndexOf(lowerCaseQuery);
                    string highlightedText = message.Content.Substring(startIndex, lowerCaseQuery.Length);
                    string beforeText = message.Content.Substring(0, startIndex);
                    string afterText = message.Content.Substring(startIndex + lowerCaseQuery.Length);

                    // Assign highlighted properties
                    message.Content = beforeText;
                    message.HighlightedText = highlightedText;
                    message.HighlightedAfterText = afterText;
                    Debug.WriteLine($"Search Query: {query}");
                    Debug.WriteLine($"Found Messages: {foundMessages.Count}");


                }
                else
                {
                    message.Content = message.Text;
                    message.HighlightedText = string.Empty;
                    message.HighlightedAfterText = string.Empty;
                }
                SearchResults.Add(message);
            }
            OnPropertyChanged(nameof(SearchResults)); // Notify the view
        }

        /// <summary>
        /// Restores all messages to their original state, clearing any highlights applied during a search.
        /// </summary>

        public void BackToOriginalMessages()
        {
            foreach (ChatMessage message in Messages)
            {
                message.Content = message.Text;
                message.HighlightedText = string.Empty;
                message.HighlightedAfterText = string.Empty;
            }
            OnPropertyChanged(nameof(Messages));
        }

        /// <summary>
        /// Synchronizes the client list with the server and updates the ObservableCollection for UI binding.
        /// </summary>

        private void SyncClientList()
        {
            //Dispatcher.CurrentDispatcher.Invoke(() =>
            System.Windows.Application.Current.Dispatcher.Invoke(() =>
            {
                _client._clientListobs.Clear();
                // Clear and update the copy
                Clientte.Clear();
                Clientte.Add("Everyone");
                foreach (KeyValuePair<int, string> kvp in _client._client_dict)
                {
                    _client._clientListobs.Add(kvp.Value);
                    Clientte.Add(kvp.Value);
                }
            });
        }

        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Handles property change notifications for data binding.
        /// </summary>
        /// <param name="name">The name of the property that has changed.</param>

        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}