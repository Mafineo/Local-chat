using System;

namespace LocalChat.Entities
{
    public class TextMessage : Message
    {
        private string _text;

        public string Text => _text;

        public TextMessage(User sender, DateTime sendDate, string text) : base(sender, sendDate)
        {
            _text = text;
        }
    }
}