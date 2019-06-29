using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class ButtonClickSoundService : MonoBehaviour
{
    private AudioSource audioSource;

    private void Start()
    {
        this.audioSource = this.GetComponent<AudioSource>();
    }

    public void PlayButtonClickSound()
    {
        this.audioSource.Play();
    }
}
