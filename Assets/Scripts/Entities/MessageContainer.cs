using System;
using System.Collections.Generic;

namespace LocalChat.Entities
{
    public class MessageContainer
    {
        public event Action<Message> MessageAdded;

        private List<Message> _messages = new List<Message>();

        public Message[] Messages => _messages.ToArray();

        public void AddMessage(Message message)
        {
            _messages.Add(message);
            MessageAdded?.Invoke(message);
        }
    }
}