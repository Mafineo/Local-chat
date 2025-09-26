using LocalChat.Entities;
using UnityEngine;
using VContainer;
using UnityEngine.Events;
using System.Collections;
using TMPro;
using System;

namespace LocalChat.UI
{
    public class VoiceInputManager : MonoBehaviour
    {
        private const int RecordingFrequency = 44100;
        private const int RecordingBufferSize = 300;
        private const float MinClipSize = 0.2f;

        [SerializeField] private UnityEvent _onVoiceRecordingStarted;
        [SerializeField] private UnityEvent _onVoiceRecordingFinished;
        [Space]
        [SerializeField] private TextMeshProUGUI _recordingTimerText;

        private AudioClip _clip;
        private string _microphone;
        private float _recordStartTime;
        private Coroutine _recordingCoroutine;

        [Inject] private MessageManager _messageManager;

        public void PostComment()
        {
            StopVoiceRecording();
            if (_clip.length < MinClipSize) return;
            _messageManager.PostComment(new VoiceMessage(_messageManager.LocalUser, DateTime.Now, _clip));
        }

        public void StartVoiceRecording()
        {
            if (Microphone.devices.Length == 0) return;
            _microphone = Microphone.devices[0];
            _clip = Microphone.Start(_microphone, false, RecordingBufferSize, RecordingFrequency);
            _recordStartTime = Time.time;
            _recordingCoroutine = StartCoroutine(ShowRecordingTimeCoroutine());
            _onVoiceRecordingStarted?.Invoke();
        }

        public void StopVoiceRecording()
        {
            if (!Microphone.IsRecording(_microphone)) return;
            float recordDuration = Time.time - _recordStartTime;
            int samples = Mathf.FloorToInt(recordDuration * _clip.frequency);
            Microphone.End(_microphone);
            float[] data = new float[samples * _clip.channels];
            _clip.GetData(data, 0);
            AudioClip trimmedClip = AudioClip.Create("TrimmedClip", samples, _clip.channels, _clip.frequency, false);
            trimmedClip.SetData(data, 0);
            _clip = trimmedClip;;
            StopCoroutine(_recordingCoroutine);
            _onVoiceRecordingFinished?.Invoke();
        }

        private IEnumerator ShowRecordingTimeCoroutine()
        {
            while(true)
            {
                _recordingTimerText.text = FormatTime(Time.time - _recordStartTime);
                yield return null;
            }
        }

        private string FormatTime(float time)
        {
            int minutes = Mathf.FloorToInt(time / 60f);
            int seconds = Mathf.FloorToInt(time % 60f);
            return $"{minutes:0}:{seconds:00}";
        }
    }
}