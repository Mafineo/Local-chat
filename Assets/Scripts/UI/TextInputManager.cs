using UnityEngine;
using TMPro;
using VContainer;
using LocalChat.Entities;
using System;

namespace LocalChat.UI
{
    public class TextInputManager : MonoBehaviour
    {
        [SerializeField] private TMP_InputField _messageInputField;

        [Inject] private MessageManager _messageManager;

        public void PostComment()
        {
            if (String.IsNullOrEmpty(_messageInputField.text)) return;
            _messageManager.PostComment(new TextMessage(_messageManager.LocalUser, DateTime.Now, _messageInputField.text));
            _messageInputField.text = string.Empty;
        }
    }
}