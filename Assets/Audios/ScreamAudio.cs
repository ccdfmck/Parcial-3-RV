using UnityEngine;

public class ScreamAudio : MonoBehaviour
{
    public AudioSource audioSource;

    public void PlayScream()
    {
        audioSource.Play();
    }
}