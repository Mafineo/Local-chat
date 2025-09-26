using System;

namespace LocalChat.Entities
{
    public abstract class Message
    {
        protected User _sender;
        protected DateTime _sendDate;
        protected MessageContainer _commentContainer = new MessageContainer();

        public User Sender => _sender;
        public DateTime SendDate => _sendDate;
        public MessageContainer CommentContainer => _commentContainer;

        public Message(User sender, DateTime sendDate)
        {
            _sender = sender;
            _sendDate = sendDate;
        }
    }
}