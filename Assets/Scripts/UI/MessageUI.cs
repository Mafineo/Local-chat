using LocalChat.Entities;
using VContainer;
using UnityEngine;
using TMPro;
using System.Globalization;
using System;

namespace LocalChat.UI
{
    public abstract class MessageUI<T> : MessageBaseUI where T : Message
    {
        [SerializeField] private TextMeshProUGUI _senderUsernameText;
        [SerializeField] private TextMeshProUGUI _postDateText;

        [Inject] private MessageManager _messageManager;

        private T _reference;

        public override void Comment()
        {
            if (_commentContainer.IsMaxDepthReached)
            {
                OnMoreCommentDepthRequested();
            }
            _messageManager.Reply(_reference);
        }

        public virtual void SetReference(T message)
        {
            _reference = message;
            _senderUsernameText.text = _reference.Sender.Username;
            _postDateText.text = FormatDate(_reference.SendDate);
            message.CommentContainer.MessageAdded += OnCommentAdded;
        }

        protected override void Awake()
        {
            base.Awake();
            _commentContainer.MoreDepthRequested += OnMoreCommentDepthRequested;
        }

        protected virtual void OnDestroy()
        {
            _reference.CommentContainer.MessageAdded -= OnCommentAdded;
        }

        private void OnMoreCommentDepthRequested()
        {
            _messageManager.CreateCommentPage();
            _messageManager.PostComment(_reference);
        }

        private void OnCommentAdded(Message message)
        {
            _commentContainer.AddMessage(message);
        }

        private string FormatDate(DateTime dateTime)
        {
            int day = dateTime.Day;
            string suffix = GetOrdinal(day);
            return $"{dateTime:HH:mm}    {day}{suffix} {dateTime.ToString("MMMM yyyy", new CultureInfo("en-US"))}";
        }

        private static string GetOrdinal(int number)
        {
            if (number % 100 is 11 or 12 or 13) return "th";
            return (number % 10) switch
            {
                1 => "st",
                2 => "nd",
                3 => "rd",
                _ => "th"
            };
        }
    }
}