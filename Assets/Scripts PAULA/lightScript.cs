using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lightScript : MonoBehaviour
{
    public GameObject flashlight_groud, inticon, flashlight_player;
    public AudioClip sonidoLinterna;
    public AudioSource audioSource;

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("MainCamera"))
        {
            inticon.SetActive(true);
            if (Input.GetKeyDown(KeyCode.E))
            {
                flashlight_groud.SetActive(false);
                inticon.SetActive(false);
                flashlight_player.SetActive(true);

                if (audioSource != null && sonidoLinterna != null)
                {
                    audioSource.PlayOneShot(sonidoLinterna);
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("MainCamera"))
        {
            inticon.SetActive(false);
        }
    }
}