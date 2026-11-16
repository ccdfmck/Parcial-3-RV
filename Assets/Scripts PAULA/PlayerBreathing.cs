using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBreathing : MonoBehaviour
{
    [Header("Referencias")]
    public AudioSource breathingSource; // AudioSource 2D (Spatial Blend = 0)
    public Transform monster;
    public Transform player;

    [Header("Distancias")]
    public float closeDistance = 2f;   // a esta distancia o menos, respiración al máximo
    public float farDistance = 15f;    // a esta distancia o más, respiración normal/tranquila

    [Header("Volumen")]
    public float minVolume = 0.15f; // volumen "en calma"
    public float maxVolume = 1f;    // volumen cuando está muy cerca

    [Header("Pitch (velocidad de respiración)")]
    public float minPitch = 0.9f;  // respiración calmada
    public float maxPitch = 1.6f;  // respiración agitada/rápida

    [Header("Suavizado")]
    public float smoothSpeed = 2f; // que tan rápido transiciona entre estados

    void Start()
    {
        if (breathingSource != null)
        {
            breathingSource.loop = true;
            breathingSource.spatialBlend = 0f; // aseguramos que sea 2D
            breathingSource.volume = minVolume;
            breathingSource.pitch = minPitch;

            if (!breathingSource.isPlaying)
            {
                breathingSource.Play();
            }
        }
    }

    void Update()
    {
        if (breathingSource == null || monster == null || player == null) return;

        float distance = Vector3.Distance(player.position, monster.position);

        // Convertimos distancia a un valor 0 (lejos/calmado) a 1 (cerca/panico)
        float t = Mathf.InverseLerp(farDistance, closeDistance, distance);
        t = Mathf.Clamp01(t);

        float targetVolume = Mathf.Lerp(minVolume, maxVolume, t);
        float targetPitch = Mathf.Lerp(minPitch, maxPitch, t);

        // Suavizamos para que no sea un cambio brusco de golpe
        breathingSource.volume = Mathf.Lerp(breathingSource.volume, targetVolume, Time.deltaTime * smoothSpeed);
        breathingSource.pitch = Mathf.Lerp(breathingSource.pitch, targetPitch, Time.deltaTime * smoothSpeed);
    }
}