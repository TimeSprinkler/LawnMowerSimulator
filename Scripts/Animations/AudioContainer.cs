using UnityEngine;
using System.Collections;

public class AudioContainer : MonoBehaviour {

    [SerializeField] private AudioClip mOneTimeSound;
    [SerializeField] private AudioClip mAmbientSound;

    [SerializeField] private AudioSource mAudioSource;

    void Start() {
        if (mAudioSource == null) {
            Debug.LogError("No AudioSource on GameObject " + this.gameObject.name + " with " + this.name);
        }

    }

    void PlaySound(AudioClip clip) {
        
         mAudioSource.clip = clip;
         mAudioSource.Play();
        
    }

    public void PlayOneTimeSound() {
        if (this.gameObject.activeSelf) {
            StartCoroutine(PlaySoundEffect(mOneTimeSound));
        }

    }

    IEnumerator PlaySoundEffect(AudioClip audioClip) {
        if (audioClip != null) {
            float clipTime = audioClip.length;
            PlaySound(audioClip);
            yield return new WaitForSeconds(clipTime);

            if (mAmbientSound != null) {
                PlaySound(mAmbientSound);
            }
        }
    }

    public void TurnOffAmbientSound() { mAudioSource.Stop(); }
}
