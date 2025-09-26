using UnityEngine;
using System;
using Cysharp.Threading.Tasks;

namespace LocalChat.UI
{
    public abstract class MessageBaseUI : MonoBehaviour
    {
        public event Action HeightUpdated;

        [SerializeField] protected RectTransform _rectTransform;
        [SerializeField] protected MessageContainerUI _commentContainer;

        protected float _startHeight;

        public MessageContainerUI CommentContainer => _commentContainer;

        public virtual void ShowMoreComments()
        {
            CommentContainer.ShowMoreComments();
        }

        public abstract void Comment();

        protected async virtual void OnCommentContainerHeightUpdated()
        {
            await UniTask.Yield();
            _rectTransform.sizeDelta = new Vector2(_rectTransform.sizeDelta.x, _startHeight + ((RectTransform)_commentContainer.transform).sizeDelta.y);
            NotifyHeightUpdated();
        }

        protected void NotifyHeightUpdated()
        {
            HeightUpdated?.Invoke();
        }

        protected virtual void Awake()
        {
            _startHeight = _rectTransform.sizeDelta.y;
            _commentContainer.HeightUpdated += OnCommentContainerHeightUpdated;
        }
    }
}