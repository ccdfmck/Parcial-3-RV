using UnityEngine;

public class JumpscareCameraTrigger : MonoBehaviour
{
    public BloodOverlay bloodOverlay;

    public void ActivarSangre()
    {
        bloodOverlay.MostrarSangre();
    }
}