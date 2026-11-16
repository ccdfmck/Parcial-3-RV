using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class monsAppear : MonoBehaviour
{
    //    public GameObject monster;
    //    public Collider collision1;
    // Start is called before the first frame update
    //    void OnTriggerEnter(Collider other)
    //   {
    //        if (other.CompareTag("Player"))
    //        {
    //            monster.SetActive(true);
    //            collision1.enabled = false;
    //        }
    //    }

    public GameObject monster;
    public Collider collision1;

    [Header("Linterna")]
    public GameObject flashlight_player;

    [Header("Configuración")]
    public float delayBeforeMonster = 2.5f;
    public float blinkSpeed = 0.15f;
    public int blinkCount = 5;

    private bool activated = false;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !activated)
        {
            activated = true;

            // Desactivar el trigger para que no vuelva a activarse
            collision1.enabled = false;

            // Comenzar secuencia de terror
            StartCoroutine(MonsterSequence());
        }
    }

    IEnumerator MonsterSequence()
    {
        // Asegurarnos de que el monstruo esté oculto
        monster.SetActive(false);

        // Parpadear linterna
        for (int i = 0; i < blinkCount; i++)
        {
            flashlight_player.SetActive(false);
            yield return new WaitForSeconds(blinkSpeed);

            flashlight_player.SetActive(true);
            yield return new WaitForSeconds(blinkSpeed);
        }

        // Pequeña pausa antes de que aparezca
        yield return new WaitForSeconds(delayBeforeMonster);

        // APARECE EL MONSTRUO
        monster.SetActive(true);
    }
}


