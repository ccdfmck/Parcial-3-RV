using UnityEngine;
using UnityEngine.XR.Content.Interaction;

public class LeverController : MonoBehaviour
{
    public XRLever palanca;
    public float Speed = 2.0f;
    public float limiteDistancia = 5.0f; // Distancia máxima en metros que se moverá hacia atrás

    private Vector3 posicionInicial;

    void Start()
    {
        // Guardamos la posición donde inicia el objeto en la escena
        posicionInicial = transform.position;
    }

    void Update()
    {
        // Si la palanca está activada, calculamos el movimiento hacia atrás
        if (palanca.value)
        {
            // Calculamos cuánto se ha movido desde el punto inicial
            float distanciaRecorrida = Vector3.Distance(posicionInicial, transform.position);

            // Solo se mueve si aún no ha alcanzado la distancia límite
            if (distanciaRecorrida < limiteDistancia)
            {
                Vector3 velocity = new Vector3(0, 0, -Speed);
                transform.position += velocity * Time.deltaTime;

                // Aseguramos que no sobrepase exactamente la distancia límite
                if (Vector3.Distance(posicionInicial, transform.position) > limiteDistancia)
                {
                    transform.position = posicionInicial + new Vector3(0, 0, -limiteDistancia);
                }
            }
        }
    }
}