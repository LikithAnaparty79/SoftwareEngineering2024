using System;
using System.Windows;
using System.Windows.Media.Animation;
using System.Windows.Media;
using Networking.Communication;
using ModuleChat;
using System.Windows.Input;
using System.Windows.Threading;
using System.Windows.Controls;

namespace ChatApplication
{
    public partial class MainWindow : Window
    {
        private ChatClient _client;
        private MainViewModel _viewModel;
        public string username;

        public string recipient_xaml;



        public MainWindow()
        {
            InitializeComponent();
            
            //_client = new ChatClient();
            //_client.MessageReceived += OnMessageReceived;
            //_client.ClientListUpdated += UpdateClientList;
            _viewModel = MainViewModel.GetInstance;
            DataContext = _viewModel;

            //recipient_xaml = (string)ClientDropdown.SelectedItem;
            //_viewModel.RequestVariable += (sender, e) =>
            //{
            //    _viewModel.Recipientt = recipient_xaml; // Pass the variable
            //};
            //MessagesListView.ItemsSource = _viewModel.Messages;
        }

        //private void ConnectButton_Click(object sender, RoutedEventArgs e)
        //{
        //    username = UsernameTextBox.Text;
        //    if (!string.IsNullOrEmpty(username))
        //    {
        //        _client.Username = username;
        //        _client.Start("10.32.11.43", "60881");
        //        Console.WriteLine(username);
        //        _viewModel.Messages.Add(new ChatMessage
        //        {
        //            User = username,
        //            Content = "Connected as " + username,
        //            Time = DateTime.Now.ToString("HH:mm"),
        //            IsSentByUser = true,
        //            text = "Connected as " + username
        //        });
        //        //_viewModel.Connect();
        //    }
        //    else
        //    {
        //        MessageBox.Show("Please enter a username.");
        //    }
        //}

        //private void SendButton_Click(object sender, RoutedEventArgs e)
        //{
        //    string message = MessageTextBox.Text;
        //    string recipient = (string)ClientDropdown.SelectedItem;

        //    if (!string.IsNullOrWhiteSpace(message) && message != "  Type something...")
        //    {
        //        _client.SendMessage(message, recipient); // Send private if recipient selected
        //        //_viewModel.SendMessage();
        //        MessageTextBox.Text = "  Type something...";
        //        MessageTextBox.FontStyle = FontStyles.Italic;
        //        Dispatcher.Invoke(() => {
        //            if (_viewModel.Messages.Any())
        //            {
        //                MessagesListView.ScrollIntoView(_viewModel.Messages.Last());
        //            }
        //        });
        //    }
        //}

        private void UpdateClientList(object sender, List<string> clientList)
        {
            Dispatcher.Invoke(() =>
            {
                ClientDropdown.ItemsSource = clientList;
                ClientDropdown.SelectedIndex = -1; // Ensure no default selection
            });
        }

        //private void OnMessageReceived(object sender, string message)
        //{
        //    string[] parts = message.Split(new[] { " :.: " }, StringSplitOptions.None);
        //    if (parts.Length == 2)
        //    {
        //        string user = parts[0].Trim();          // senderUsername
        //        string messageContent = parts[1].Trim();
        //        bool isSent = false;
        //        isSent = (user == username);

        //        //if (user == username)
        //        //{
        //        //    isSent = true;
        //        //}

        //        // Prevent adding the same message again by checking if it already exists in the collection
        //        bool messageExists = _viewModel.Messages.Any(m => m.User == user && m.Content == messageContent);

        //        if (!messageExists)
        //        {
        //            Dispatcher.Invoke(() =>
        //            {
        //                _viewModel.Messages.Add(new ChatMessage
        //                {
        //                    User = user,
        //                    Content = messageContent + username,
        //                    Time = DateTime.Now.ToString("HH:mm"),
        //                    IsSentByUser = isSent,
        //                    text = messageContent
        //                });

        //                if (_viewModel.Messages.Any())
        //                {
        //                    MessagesListView.ScrollIntoView(_viewModel.Messages.Last());
        //                }
        //            });
        //        }
        //    }
        //}


        private void MessageTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (MessageTextBox.Text == "  Type something..." )
            {
                MessageTextBox.Text = string.Empty;
                MessageTextBox.FontStyle = FontStyles.Normal; // Reset font style
            }
        }

        private void SearchTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (SearchTextBox.Text == " ")
            {
                SearchTextBox.Text = string.Empty;
                SearchTextBox.FontStyle = FontStyles.Normal; // Reset font style
                SearchTextBox.Background = Brushes.Teal;
            }
        }

        private void SearchTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            {
                if (e.Key == Key.Escape) // Check if the ESC key was pressed
                {
                    SearchTextBox.Clear();
                    _viewModel.BackToOriginalMessages();
                    MessagesListView.ItemsSource = _viewModel.Messages;
                    ReverseSearchTransition(); // Call the method to reverse the transition
                }
            }
        }
        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {

            SearchPanel.Background = new SolidColorBrush(Colors.Teal);
            var fadeOutStoryboard = new Storyboard();
            var fadeOutAnimation = new DoubleAnimation
            {
                To = 0,
                Duration = TimeSpan.FromSeconds(0.5)
            };

            fadeOutAnimation.Completed += (s, args) =>
            {
                TopDockPanel.Visibility = Visibility.Collapsed; // Hide the top dock panel
                SearchPanel.Visibility = Visibility.Visible; // Show the search panel
                SearchPanel.Opacity = 0; // Reset opacity for fade-in
                SearchTextBox.Focus(); // Focus on the search box
            };

            Storyboard.SetTarget(fadeOutAnimation, TopDockPanel);
            Storyboard.SetTargetProperty(fadeOutAnimation, new PropertyPath("Opacity"));
            fadeOutStoryboard.Children.Add(fadeOutAnimation);

            // Fade in the search panel
            var fadeInAnimation = new DoubleAnimation
            {
                To = 1,
                Duration = TimeSpan.FromSeconds(0.5)
            };

            Storyboard.SetTarget(fadeInAnimation, SearchPanel);
            Storyboard.SetTargetProperty(fadeInAnimation, new PropertyPath("Opacity"));
            fadeOutStoryboard.Children.Add(fadeInAnimation);

            fadeOutStoryboard.Begin();
        }
        private void ReverseSearchTransition()
        {
            var fadeOutStoryboard = new Storyboard();
            var fadeOutAnimation = new DoubleAnimation
            {
                To = 0,
                Duration = TimeSpan.FromSeconds(0.5)
            };

            fadeOutAnimation.Completed += (s, args) =>
            {
                SearchPanel.Visibility = Visibility.Collapsed; // Hide the search panel
                TopDockPanel.Visibility = Visibility.Visible; // Show the top dock panel
                TopDockPanel.Opacity = 0; // This line makes it transparent; you should set it to 1

                // 
            };

            Storyboard.SetTarget(fadeOutAnimation, SearchPanel);
            Storyboard.SetTargetProperty(fadeOutAnimation, new PropertyPath("Opacity"));
            fadeOutStoryboard.Children.Add(fadeOutAnimation);

            // Fade in the top dock panel (should come after setting visibility)
            var fadeInAnimation = new DoubleAnimation
            {
                To = 1, // This is correct; you want to fade in
                Duration = TimeSpan.FromSeconds(0.5)
            };

            Storyboard.SetTarget(fadeInAnimation, TopDockPanel);
            Storyboard.SetTargetProperty(fadeInAnimation, new PropertyPath("Opacity"));
            fadeOutStoryboard.Children.Add(fadeInAnimation);

            // Begin the storyboard
            fadeOutStoryboard.Begin();
        }


        private void ClearSearchButton_Click(object sender, RoutedEventArgs e)
        {
            SearchTextBox.Clear();
            if (_viewModel.SearchResults != null)
            {
                _viewModel.SearchResults.Clear();
            }
            _viewModel.BackToOriginalMessages();
            SearchPanel.Visibility = Visibility.Visible;
            MessagesListView.ItemsSource = _viewModel.Messages;
            MessagesListView.ScrollIntoView(_viewModel.Messages.Last());
        }
        private void BackFromSearchButton_Click(object sender, RoutedEventArgs e)
        {
            SearchTextBox.Clear();
            _viewModel.BackToOriginalMessages();
            MessagesListView.ItemsSource = _viewModel.Messages;
            ReverseSearchTransition();
        }

        private void ClientDropdown_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {

        }
        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            PerformDynamicSearch();
        }
        private void PerformDynamicSearch()
        {
            string query = SearchTextBox.Text;

            if (!string.IsNullOrWhiteSpace(query))
            {
                _viewModel.SearchMessages(query);
                MessagesListView.ItemsSource = _viewModel.SearchResults;
            }
            if (SearchTextBox.Text == "")
            {
                _viewModel.BackToOriginalMessages();
                MessagesListView.ItemsSource = _viewModel.Messages;
            }
        }

        //private void MessageTextBox_enter(object sender, KeyEventArgs e)
        //{
        //    if (e.Key == Key.Enter)
        //    {
        //        SendButton_Click(sender, e); // Call the Send button click handler
        //        MessageTextBox.Clear();
        //        MessageTextBox.FontStyle = FontStyles.Normal;
        //    }
        //}

        private void EmojiPopupButton_Click(object sender, RoutedEventArgs e)
        {
            if (EmojiPopup.IsOpen)
            {
                EmojiPopup.IsOpen = false;
            }
            else
                EmojiPopup.IsOpen = true;
        }
        private void Emoji_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button emojiButton)
            {
                // Get the emoji from the button content
                string emoji = emojiButton.Content.ToString();

                if (MessageTextBox.Text == "  Type something...")
                {
                    MessageTextBox.Text = string.Empty;
                    MessageTextBox.FontStyle = FontStyles.Normal; // Reset font style
                }
                // Insert the emoji into the MessageTextBox
                MessageTextBox.Text += emoji;

                // Set focus back to the text box
                MessageTextBox.Focus();
                MessageTextBox.CaretIndex = MessageTextBox.Text.Length;
            }
        }
        private void OptionsButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.ContextMenu != null)
            {
                // Ensure PlacementTarget is set
                button.ContextMenu.PlacementTarget = button;
                button.ContextMenu.DataContext = button.DataContext; // Ensure DataContext is set
                button.ContextMenu.IsOpen = true;
            }
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            if (sender is MenuItem menuItem && menuItem.Parent is ContextMenu contextMenu)
            {
                var placementTarget = contextMenu.PlacementTarget as FrameworkElement;
                if (placementTarget?.DataContext is ChatMessage message)
                {
                    _viewModel.DeleteMessage(message);
                }
            }
        }

        private void ClientDropdown_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
