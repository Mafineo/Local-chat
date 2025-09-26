using LocalChat.Entities;
using LocalChat.UI;
using UnityEngine;
using TMPro;
using System.Collections.Generic;
using VContainer;
using VContainer.Unity;

namespace LocalChat
{
    public class MessageManager : MonoBehaviour
    {
        private enum PostMode { Comment, Reply }

        [SerializeField] private GameObject _inputPanel;
        [SerializeField] private GameObject _commentPanel;
        [SerializeField] private TextMeshProUGUI _commentText;
        [SerializeField] private CommentPage _initCommentPage;

        [Inject] private IObjectResolver _objectResolver;
        [Inject] private CommentPage _commentPagePrefab;

        private PostMode _postMode = PostMode.Comment;
        private Message _replyMessage;
        private List<CommentPage> _commentPages = new List<CommentPage>();

        private User _authorizedLocalUser = new User("Mafineo");
        public User LocalUser => _authorizedLocalUser;

//Debug
/*
        private User _anotherUser = new User("AnotherUser");

        private async void Start()
        {
            await UniTask.Yield();
            Message temp = new TextMessage(_anotherUser, DateTime.Now, "pupa");
            temp.CommentContainer.AddMessage(new TextMessage(_anotherUser, DateTime.Now, "pupa1"));
            temp.CommentContainer.AddMessage(new TextMessage(_anotherUser, DateTime.Now, "pupa2"));
            Message temp2 = new TextMessage(_anotherUser, DateTime.Now, "pupa3");
            Message temp3 = new TextMessage(_anotherUser, DateTime.Now, "pupa4");
            temp3.CommentContainer.AddMessage(new TextMessage(_anotherUser, DateTime.Now, "pupa5"));
            temp2.CommentContainer.AddMessage(temp3);
            temp.CommentContainer.AddMessage(temp2);
            _commentPages[_commentPages.Count - 1].MessageContainer.AddMessage(temp);
        }
*/
//Debug

        public void CreateCommentPage()
        {
            CancelReply();
            CommentPage commentPage = _objectResolver.Instantiate(_commentPagePrefab, transform);
            _commentPages[_commentPages.Count - 1].gameObject.SetActive(false);
            _commentPages.Add(commentPage);
            commentPage.PageClosingRequested += OnCommentPageClosingRequested;
            _inputPanel.transform.SetAsLastSibling();
        }    

        public void PostComment(Message message)
        {
            switch (_postMode)
            {
                case PostMode.Comment:
                    _commentPages[_commentPages.Count - 1].MessageContainer.AddMessage(message);
                    break;
                case PostMode.Reply:
                    _replyMessage.CommentContainer.AddMessage(message);
                    CancelReply();
                    break;
                default: return;
            }
        }

        public void CancelReply()
        {
            if (_postMode != PostMode.Reply) return;
            _replyMessage = null;
            _postMode = PostMode.Comment;
            _commentPanel.SetActive(false);
        }

        public void Reply(Message message)
        {
            if (message == null) return;
            switch (message)
            {
                case TextMessage textMessage:
                    _commentText.text = textMessage.Text;
                    break;
                case VoiceMessage voiceMessage:
                    _commentText.text = "Voice message";
                    break;
                default: return;
            }
            _replyMessage = message;
            _postMode = PostMode.Reply;
            _commentPanel.SetActive(true);
        }

        private void OnCommentPageClosingRequested()
        {
            CommentPage commentPage = _commentPages[_commentPages.Count - 1];
            _commentPages.Remove(commentPage);
            CancelReply();
            Destroy(commentPage.gameObject);
            _commentPages[_commentPages.Count - 1].gameObject.SetActive(true);
        }

        private void Awake()
        {
            _commentPages.Add(_initCommentPage);
        }
    }
}