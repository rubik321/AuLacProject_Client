using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace CodeHelper
{
    public class Message
    {
        public string type;
        public object[] data;

        public Message(string type)
        {
            this.type = type;
        }

        public Message(Type type)
        {
            this.type = nameof(type);
        }

        public Message(string type, object[] data)
        {
            this.type = type;
            this.data = data;
        }

        public Message(Type type, object[] data)
        {
            this.type = nameof(type);
            this.data = data;
        }
    }

    public abstract class MessageType { }

    public interface IMessageHandle
    {
        void Handle(Message message);

    }
    public static class MessageManager
    {
        private static Dictionary<string, List<IMessageHandle>> subcribers = new Dictionary<string, List<IMessageHandle>>();

        public static void AddSubcriber<T>(IMessageHandle handle) where T : MessageType
        {
            string _type = typeof(T).Name;
            if (!subcribers.ContainsKey(_type))
                subcribers.Add(_type, new List<IMessageHandle>());
            if (!subcribers[_type].Contains(handle))
                subcribers[_type].Add(handle);
        }
        public static void RemoveSubcriber<T>(IMessageHandle handle)
        {
            string _type = typeof(T).Name;
            if (subcribers.ContainsKey(_type))
                if (subcribers[_type].Contains(handle))
                    subcribers[_type].Remove(handle);
        }
        public static void AddSubcriber(string type, IMessageHandle handle)
        {
            if (!subcribers.ContainsKey(type))
                subcribers.Add(type, new List<IMessageHandle>());
            if (!subcribers[type].Contains(handle))
                subcribers[type].Add(handle);
        }
        public static void RemoveSubcriber(string type, IMessageHandle handle)
        {
            if (subcribers.ContainsKey(type))
                if (subcribers[type].Contains(handle))
                    subcribers[type].Remove(handle);
        }
        public static void SendMessage(Message message)
        {
            if (subcribers.TryGetValue(message.type, out List<IMessageHandle> _subcribers))
            {
                for (int i = _subcribers.Count - 1; i > -1; i--)
                {
                    _subcribers[i].Handle(message);
                }
            }
        }
        /// <summary>
        /// Send message after <paramref name="delay"/> in second
        /// </summary>
        /// <param name="message"></param>
        /// <param name="delay"></param>
        /// <returns></returns>
        public static async void SendMessageWithDelay(Message message, float delay, Action onSendingMessage = null)
        {
            await Task.Delay((int)(delay * 1000));
            SendMessage(message);
            onSendingMessage?.Invoke();
        }
    }
}

