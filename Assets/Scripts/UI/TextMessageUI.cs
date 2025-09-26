using UnityEngine;
using TMPro;
using LocalChat.Entities;
using Cysharp.Threading.Tasks;

namespace LocalChat.UI
{
    public class TextMessageUI : MessageUI<TextMessage>
    {
        [SerializeField] private TextMeshProUGUI _messageText;
        [SerializeField] private RectTransform _messageTextBackground;
        [SerializeField] private RectTransform _body;

        private float _startMessageBackgroundHeight;

        public override async void SetReference(TextMessage message)
        {
            base.SetReference(message);
            await UniTask.Yield();
            _startMessageBackgroundHeight = _messageTextBackground.sizeDelta.y;
            _messageText.text = message.Text;
            OnCommentContainerHeightUpdated();
        }

        protected async override void OnCommentContainerHeightUpdated()
        {
            await UniTask.Yield();
            _rectTransform.sizeDelta = new Vector2(_rectTransform.sizeDelta.x, _startHeight + ((RectTransform)_commentContainer.transform).sizeDelta.y + (_messageTextBackground.sizeDelta.y - _startMessageBackgroundHeight));
            _body.sizeDelta = new Vector2(_body.sizeDelta.x, _startHeight + _messageTextBackground.sizeDelta.y - _startMessageBackgroundHeight);
            NotifyHeightUpdated();
        }
    }
}