using System;
using System.Collections.Generic;
using UnityEngine;

namespace Rubik.Common
{
    public interface ILocalNotifyReceiver
    {
        void OnReceiveNotify();
        void OnReceiveNotify<T>(T data);
    }


    [Serializable]
    public class LocalNotifyChanel
    {
        public string chanelId;
        private List<ILocalNotifyReceiver> receivers = new List<ILocalNotifyReceiver>();
        private List<ILocalNotifyReceiver> deadReceivers = new List<ILocalNotifyReceiver>();
        
        public void AddReceiver(ILocalNotifyReceiver receiver)
        {
            if (receivers.Contains(receiver))
            {
                return;
            }
            receivers.Add(receiver);
        }

        public void RemoveReceiver(ILocalNotifyReceiver receiver)
        {
            receivers.Remove(receiver);
        }

        public void SendNotify()
        {
            deadReceivers.Clear();
            foreach (var receiver in receivers)
            {
                try
                {
                    receiver.OnReceiveNotify();
                }
                catch (Exception e)
                {
                    deadReceivers.Add(receiver);
                }
            }

            foreach (var receiver in deadReceivers)
            {
                RemoveReceiver(receiver);
            }
        }
        
        public void SendNotify<T>(T data)
        {
            deadReceivers.Clear();
            foreach (var receiver in receivers)
            {
                try
                {
                    receiver.OnReceiveNotify(data);
                }
                catch (Exception e)
                {
                    deadReceivers.Add(receiver);
                }
            }

            foreach (var receiver in deadReceivers)
            {
                RemoveReceiver(receiver);
            }
        }
    }
    
    public class LocalNotifyManager : Singleton<LocalNotifyManager>
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        static void InitOnLoad()
        {
            Init();
        }


        private LocalNotifyChanel _defaultChanel = null;

        private const string DEFAULT_CHANEL_ID = "main_chanel";

        public LocalNotifyChanel DefaultChanel
        {
            get
            {
                if (_defaultChanel == null)
                {
                    _defaultChanel = new LocalNotifyChanel()
                    {
                        chanelId = DEFAULT_CHANEL_ID
                    };
                }

                return _defaultChanel;
            }
        }
        
        public void AddReceiver(ILocalNotifyReceiver receiver)
        {
            DefaultChanel.AddReceiver(receiver);
        }

        public void RemoveReceiver(ILocalNotifyReceiver receiver)
        {
            DefaultChanel.RemoveReceiver(receiver);
        }

        public void SendNotify()
        {
            DefaultChanel.SendNotify();
        }
        
        public void SendNotify<T>(T data)
        {
            DefaultChanel.SendNotify(data);
        }

        #region customize chanel
        
        private List<LocalNotifyChanel> chanels = new List<LocalNotifyChanel>();

        public LocalNotifyChanel GetChanel(string chanelId)
        {
            if (chanelId.Equals(DEFAULT_CHANEL_ID))
            {
                return DefaultChanel;
            }

            foreach (var chanel in chanels)
            {
                if (chanel.chanelId.Equals(chanelId))
                {
                    return chanel;
                }
            }

            var newChanel = new LocalNotifyChanel()
            {
                chanelId = chanelId
            };
            
            chanels.Add(newChanel);

            return newChanel;
        }
        

        public void AddReceiver(string chanel, ILocalNotifyReceiver receiver)
        {
            GetChanel(chanel).AddReceiver(receiver);
        }

        public void RemoveReceiver(string chanel,ILocalNotifyReceiver receiver)
        {
            GetChanel(chanel).RemoveReceiver(receiver);
        }

        public void SendNotify(string chanel)
        {
            GetChanel(chanel).SendNotify();
        }
        
        public void SendNotify<T>(string chanel, T data)
        {
            GetChanel(chanel).SendNotify(data);
        }


        #endregion

    }
}
