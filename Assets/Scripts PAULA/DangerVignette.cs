using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class DangerVignette : MonoBehaviour
{
    [Header("Referencias")]
    public PostProcessVolume globalVolume; // el Post-process Volume de la escena
    public Transform monster;
    public Transform player;

    [Header("Distancias")]
    public float closeDistance = 2f;
    public float farDistance = 15f;

    [Header("Intensidad del Vignette")]
    public float minIntensity = 0f;
    public float maxIntensity = 0.5f;

    [Header("Suavizado")]
    public float smoothSpeed = 2f;

    private Vignette vignette;

    void Start()
    {
        if (globalVolume != null && globalVolume.profile.TryGetSettings(out vignette))
        {
            vignette.intensity.value = minIntensity;
        }
        else
        {
            Debug.LogWarning("No se encontró el efecto Vignette en el Post-process Profile asignado.");
        }
    }

    void Update()
    {
        if (vignette == null || monster == null || player == null) return;

        float distance = Vector3.Distance(player.position, monster.position);

        float t = Mathf.InverseLerp(farDistance, closeDistance, distance);
        t = Mathf.Clamp01(t);

        float targetIntensity = Mathf.Lerp(minIntensity, maxIntensity, t);

        vignette.intensity.value = Mathf.Lerp(vignette.intensity.value, targetIntensity, Time.deltaTime * smoothSpeed);
    }
}