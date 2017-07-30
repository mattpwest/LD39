using UnityEngine;
using UnityEngine.UI;

namespace Utils
{
    public class OnButtonClickPlayAudioClipOnce
    {
        private readonly AudioSource audioSource;
        private readonly AudioClip audioClip;
        private readonly Button button;
        private readonly GameObject produceButton;

        private OnButtonClickPlayAudioClipOnce(AudioSource audioSource, AudioClip audioClip, GameObject produceButton, Button button)
        {
            this.audioSource = audioSource;
            this.audioClip = audioClip;
            this.button = button;
            this.produceButton = produceButton;
        }

        public static void PlayAudioOnce(AudioSource audioSource, AudioClip audioClip, GameObject produceButton, Button button)
        {
            var play = new OnButtonClickPlayAudioClipOnce(audioSource, audioClip, produceButton, button);
            button.onClick.AddListener(play.PlayAudioOnceThenRemove);
        }

        private void PlayAudioOnceThenRemove()
        {
            this.produceButton.SetActive(true);

            if(this.audioSource.isPlaying)
            {
                this.audioSource.Stop();
            }

            this.audioSource.PlayOneShot(this.audioClip);

            this.button.onClick.RemoveListener(this.PlayAudioOnceThenRemove);
        }
    }
}
