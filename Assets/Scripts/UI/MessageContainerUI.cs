using UnityEngine;
using LocalChat.Entities;
using System.Collections.Generic;
using VContainer;
using TMPro;
using VContainer.Unity;
using System;
using Cysharp.Threading.Tasks;

namespace LocalChat.UI
{
    public class MessageContainerUI : MonoBehaviour
    {
        private const int InitialVisibleComments = 2;
        private const int CommentsToShowPerClick = 3;
        private const int MaxMessageDepth = 4;

        public event Action MoreDepthRequested;
        public event Action HeightUpdated;

        [SerializeField] private bool _isInitContainer;
        [Space]
        [SerializeField] private RectTransform _rectTransform;
        [SerializeField] private GameObject _showCommentsPanel;
        [SerializeField] private TextMeshProUGUI _commentsCountText;

        [Inject] private MessageManager _messageManager;
        [Inject] private IObjectResolver _resolver;
        [Inject] private TextMessageUI _textMessageUIPrefab;
        [Inject] private VoiceMessageUI _voiceMessageUIPrefab;

        private List<MessageBaseUI> _messages = new List<MessageBaseUI>();
        private int _shownComments = 0;
        private int _messageDepth = 0;
        private float _startHeight;

        public bool IsMaxDepthReached => _messageDepth >= MaxMessageDepth;

        public void SetDepth(int depth)
        {
            _messageDepth = depth;
        }

        public void AddMessage(Message message)
        {
            if (message == null) return;
            Message[] messages = message.CommentContainer.Messages;
            MessageBaseUI createdMessageUI;
            switch (message)
            {
                case TextMessage textMessage:
                    TextMessageUI textMessageUI = _resolver.Instantiate(_textMessageUIPrefab, transform);
                    textMessageUI.SetReference(textMessage);
                    createdMessageUI = textMessageUI;
                    break;
                case VoiceMessage voiceMessage:
                    VoiceMessageUI voiceMessageUI = _resolver.Instantiate(_voiceMessageUIPrefab, transform);
                    voiceMessageUI.SetReference(voiceMessage);
                    createdMessageUI = voiceMessageUI;
                    break;
                default: return;
            }
            _messages.Insert(0, createdMessageUI);
            if (!IsMaxDepthReached)
            {
                if (_isInitContainer || message.Sender == _messageManager.LocalUser || _shownComments < InitialVisibleComments)
                {
                    _shownComments++;
                }
            }
            createdMessageUI.CommentContainer.SetDepth(_messageDepth + 1);
            foreach (Message mes in messages)
            {
                createdMessageUI.CommentContainer.AddMessage(mes);
            }
            createdMessageUI.HeightUpdated += UpdateCommentsCountText;
            UpdateCommentsCountText();
        }

        public void ShowMoreComments()
        {
            if (IsMaxDepthReached)
            {
                MoreDepthRequested?.Invoke();
            }
            else
            {
                _shownComments = Mathf.Min(_shownComments + CommentsToShowPerClick, _messages.Count);
            }
            UpdateCommentsCountText();
        }

        private void UpdateCommentsCountText()
        {
            float messagesSize = 0;
            for (int i = 0; i < _messages.Count; i++)
            {
                _messages[i].gameObject.SetActive(false);
            }
            for (int i = 0; i < _shownComments; i++)
            {
                _messages[i].gameObject.SetActive(true);
                _messages[i].transform.SetAsLastSibling();
                messagesSize += ((RectTransform)_messages[i].transform).sizeDelta.y;
            }
            if (!_isInitContainer)
            {
                int messageCount = _messages.Count;
                _commentsCountText.text = $"More comments ({messageCount - _shownComments})";
                _showCommentsPanel.SetActive(messageCount - _shownComments > 0);
                _showCommentsPanel.transform.SetAsLastSibling();
            }
            _rectTransform.sizeDelta = new Vector2(_rectTransform.sizeDelta.x, _startHeight + (_showCommentsPanel != null && _showCommentsPanel.activeSelf ? ((RectTransform)_showCommentsPanel.transform).sizeDelta.y : 0) + messagesSize);
            HeightUpdated?.Invoke();
        }

        private async void Awake()
        {
            await UniTask.Yield();
            _startHeight = _rectTransform.sizeDelta.y;
        }
    }
}