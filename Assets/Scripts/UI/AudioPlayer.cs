using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace LocalChat.UI
{
    public class AudioPlayer : MonoBehaviour
    {
        private const float EmptySampleBarHeightCoefficient = 0.1f;

        [SerializeField] private GameObject _playButton;
        [SerializeField] private GameObject _pauseButton;
        [Space]
        [SerializeField] private AudioSource _audioSource;
        [Space]
        [SerializeField] private float _waveBarHeight;
        [SerializeField] private RectTransform[] _waveBars;
        [SerializeField] private RectTransform[] _playedBars;
        [SerializeField] private RectTransform _playedBarsContainer;
        [SerializeField] private RectMask2D _playedBarsMask;

        private AudioClip _clip;
        private Coroutine _playCoroutine;

        public void SetAudioClip(AudioClip clip)
        {
            if (clip == null) return;
            _clip = clip;
            FillSamplesBars(_waveBars, _waveBarHeight, _clip);
            FillSamplesBars(_playedBars, _waveBarHeight, _clip);
            SetClipPosition(0);
        }

        public void Play()
        {
            if (_clip == null) return;
            _playCoroutine = StartCoroutine(PlayCoroutine());
            _playButton.SetActive(false);
            _pauseButton.SetActive(true);
        }

        public void Pause()
        {
            if (_playCoroutine == null) return;
            StopCoroutine(_playCoroutine);
            _pauseButton.SetActive(false);
            _playButton.SetActive(true);
            _audioSource.Pause();
        }

        private void SetClipPosition(float position)
        {
            _playedBarsMask.padding = new Vector4(0, 0, (1f - position) * _playedBarsContainer.sizeDelta.x, 0);
        }

        private IEnumerator PlayCoroutine()
        {
            _audioSource.clip = _clip;
            _audioSource.Play();
            while (true)
            {
                float progress = _audioSource.time / _audioSource.clip.length;
                SetClipPosition(progress);
                yield return null;
                if (progress == 1)
                {
                    Pause();
                    break;
                }
            }
        }

        private void FillSamplesBars(RectTransform[] bars, float waveBarHeight, AudioClip clip)
        {
            float[] samples = new float[clip.samples * clip.channels];
            clip.GetData(samples, 0);
            int samplesPerBar = samples.Length / bars.Length;
            float[] values = new float[bars.Length];
            float max = 0f;
            for (int i = 0; i < bars.Length; i++)
            {
                float sum = 0f;
                for (int j = 0; j < samplesPerBar; j++)
                {
                    int index = i * samplesPerBar + j;
                    sum += Mathf.Abs(samples[index]);
                }
                float avg = sum / samplesPerBar;
                values[i] = avg;
                if (avg > max) max = avg;
            }
            for (int i = 0; i < bars.Length; i++)
            {
                float normalized = values[i] / max;
                float h = normalized * waveBarHeight;
                bars[i].sizeDelta = new Vector2(bars[i].sizeDelta.x, Mathf.Max(_waveBarHeight * EmptySampleBarHeightCoefficient, h));
            }
        }
    }
}