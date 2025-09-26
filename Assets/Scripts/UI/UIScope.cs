using VContainer;
using VContainer.Unity;
using UnityEngine;

namespace LocalChat.UI
{
    public class UIScope : LifetimeScope
    {
        [SerializeField] private MessageManager _messageManager;
        [Space]
        [SerializeField] private CommentPage _commentPagePrefab;
        [SerializeField] private TextMessageUI _textMessageUIPrefab;
        [SerializeField] private VoiceMessageUI _voiceMessageUIPrefab;

        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterInstance(_messageManager);
            builder.RegisterInstance(_commentPagePrefab);
            builder.RegisterInstance(_textMessageUIPrefab);
            builder.RegisterInstance(_voiceMessageUIPrefab);
        }
    }
}