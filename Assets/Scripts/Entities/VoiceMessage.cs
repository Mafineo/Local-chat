using UnityEngine;
using System;

namespace LocalChat.Entities
{
    public class VoiceMessage : Message
    {
        private AudioClip _clip;

        public AudioClip Clip => _clip;

        public VoiceMessage(User sender, DateTime sendDate, AudioClip clip) : base(sender, sendDate)
        {
            _clip = clip;
        }
    }
}