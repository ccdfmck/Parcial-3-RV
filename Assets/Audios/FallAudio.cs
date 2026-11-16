using UnityEngine;

public class FallAudio : MonoBehaviour
{
    public AudioSource audioSource;

    public void PlayFall()
    {
        if (audioSource.isPlaying) return;
        audioSource.Play();
    }
}