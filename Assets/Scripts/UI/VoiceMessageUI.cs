using LocalChat.Entities;
using UnityEngine;

namespace LocalChat.UI
{
    public class VoiceMessageUI : MessageUI<VoiceMessage>
    {
        [SerializeField] private AudioPlayer _audioPlayer;

        public override void SetReference(VoiceMessage message) 
        {
            base.SetReference(message);
            _audioPlayer.SetAudioClip(message.Clip);
        }
    }
}