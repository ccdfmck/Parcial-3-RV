using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Door : MonoBehaviour
{
    public GameObject door_closed, door_opened, intText, lockedtext;
    public AudioSource open, close;
    public bool opened, locked;

    [Header("Llave requerida")]
    public string requiredKeyID; // ej: "llave_roja", "llave_sotano"

    // Lista compartida de todas las llaves que el jugador ha recogido
    public static HashSet<string> collectedKeys = new HashSet<string>();

    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("MainCamera"))
        {
            if (locked)
            {
                lockedtext.SetActive(true);
                return;
            }
            intText.SetActive(true);
            if (Input.GetKeyDown(KeyCode.E))
            {
                if (!opened)
                {
                    OpenDoor();
                }
                else
                {
                    CloseDoor();
                }
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("MainCamera"))
        {
            intText.SetActive(false);
            lockedtext.SetActive(false);
        }
    }

    void OpenDoor()
    {
        door_closed.SetActive(false);
        door_opened.SetActive(true);
        opened = true;
        if (open != null) open.Play();
    }

    void CloseDoor()
    {
        door_closed.SetActive(true);
        door_opened.SetActive(false);
        opened = false;
        if (close != null) close.Play();
    }

    void Update()
    {
        // Se desbloquea solo si la llave especifica de ESTA puerta fue recogida
        if (locked && !string.IsNullOrEmpty(requiredKeyID) && collectedKeys.Contains(requiredKeyID))
        {
            locked = false;
        }
    }
}