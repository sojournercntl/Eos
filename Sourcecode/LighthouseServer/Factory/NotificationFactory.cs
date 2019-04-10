using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security.Authentication.ExtendedProtection;
using System.Text;
using System.Threading.Tasks;

namespace LighthouseServer.Factory
{
    public class NotificationFactory
    {
        private static NotificationFactory _instance = null;
        private static object padlock = new object();

        public static NotificationFactory Instance
        {
            get
            {
                lock (padlock)
                {
                    if(_instance == null)
                        _instance = new NotificationFactory();
                    return _instance;
                }
            }
        }


        private NotificationFactory()
        {

        }

        public INotification CreateNotification(NotificationType type)
        {
            switch (type)
            {
                case NotificationType.Text:
                    throw new NotImplementedException();
                case NotificationType.Time:
                    TimeNotification tn = new TimeNotification();
                    tn.Type = type;
                    tn.Message = DateTime.Now.ToString();
                    return tn;
                default:
                    throw new NotImplementedException();
            }
        }

    }
}
