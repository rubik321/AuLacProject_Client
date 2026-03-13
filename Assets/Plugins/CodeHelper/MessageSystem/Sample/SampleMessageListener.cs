using System;
using System.Threading.Tasks;
using UnityEngine;

namespace CodeHelper.Sample
{
    public class SampleMessageListener : MonoBehaviour, IMessageHandle
    {
        private void OnEnable()
        {
            MessageManager.AddSubcriber<SampleMessageType>(this);
        }

        private void OnDisable()
        {
            MessageManager.RemoveSubcriber<SampleMessageType>(this);
        }

        private void Start()
        {
            Debug.LogFormat("Start at: {0}", DateTime.Now.Second);
            MessageManager.SendMessageWithDelay(new Message(nameof(SampleMessageType)), 3, () =>
            {
                Debug.LogFormat("Complete send at {0}", DateTime.Now.Second);
            });
        }

        public void Handle(Message message)
        {
            switch (message.type)
            {
                case nameof(SampleMessageType):
                    Debug.LogFormat("I have heard {0} message", message.type);
                    break;
            }
        }
    }
}
