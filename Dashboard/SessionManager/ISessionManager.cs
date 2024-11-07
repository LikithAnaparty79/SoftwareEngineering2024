using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dashboard.SessionManager
{
    public interface ISessionManager
    {
        public void SendMessage(string message, string clientIP);

        public void OnDataReceived(string message);

    }
}
