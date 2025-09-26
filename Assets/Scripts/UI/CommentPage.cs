using System;
using UnityEngine;

namespace LocalChat.UI
{
    public class CommentPage : MonoBehaviour
    {
        public event Action PageClosingRequested;

        [SerializeField] private MessageContainerUI _messageContainer;

        public MessageContainerUI MessageContainer => _messageContainer;

        public void Close()
        {
            PageClosingRequested?.Invoke();
        }
    }
}