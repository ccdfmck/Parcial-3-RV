using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class keyScript : MonoBehaviour
{
    public GameObject inticon, key;
    public AudioClip sonidoLlave;
    public AudioSource audioSource;

    [Header("ID de esta llave")]
    public string keyID; // ej: "llave_roja", debe coincidir con el requiredKeyID de la puerta correspondiente

    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("MainCamera"))
        {
            inticon.SetActive(true);
            if (Input.GetKeyDown(KeyCode.E))
            {
                key.SetActive(false);
                Door.collectedKeys.Add(keyID);
                inticon.SetActive(false);
                if (audioSource != null && sonidoLlave != null)
                {
                    audioSource.PlayOneShot(sonidoLlave);
                }
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("MainCamera"))
        {
            inticon.SetActive(false);
        }
    }
}