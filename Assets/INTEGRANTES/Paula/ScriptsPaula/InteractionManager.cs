using UnityEngine;

public class InteractionManager : MonoBehaviour
{
    public static InteractionManager instance { get; private set; }

    public bool panel1, panel2, panel3, panelBotonApagado;
    public string clave = "1234";
    public string regClave;

    [Header("Configuración de las Puertas")]
    public Animation puerta1;
    public Animation puerta2;
    public Animation puerta3;

    private void Awake()
    {
        if (instance != this && instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }

        panel1 = false;
        panel2 = false;
        panel3 = false;
        panelBotonApagado = true;
    }

    private void Update()
    {
        isPanelActive();
    }

    public void isPanelActive()
    {
        if ((panel1 && panel2 && panel3) && panelBotonApagado)
        {
            Debug.Log("Paneles encendidos, puedo activar los botones");
            panelBotonApagado = false;
        }

        if ((!panel1 || !panel2 || !panel3) && !panelBotonApagado)
        {
            Debug.Log("Panel Apagado, NO puedo activar los botones");
            panelBotonApagado = true;
        }
    }

    // Funciones específicas para cada botón (sin restricción de panelBotonApagado)
    public void OpenDoor1()
    {
        if (puerta1 != null) puerta1.Play();
    }

    public void OpenDoor2()
    {
        if (puerta2 != null) puerta2.Play();
    }

    public void OpenDoor3()
    {
        if (puerta3 != null) puerta3.Play();
    }
}