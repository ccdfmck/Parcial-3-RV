using UnityEngine;
using UnityEngine.Events;

namespace TamagotchiFactory
{
    public enum ZoneType { Disposal, Box, BeltEnd }

    /// <summary>
    /// Valida cada decisión. Se pone en un objeto con Collider "Is Trigger":
    ///  - Disposal: zona de desecho (acierto si el producto TIENE defecto).
    ///  - Box: caja de un modelo (acierto si NO tiene defecto y es el modelo correcto).
    ///  - BeltEnd: final de la banda (si llega aquí sin decisión, cuenta como error).
    /// El producto se evalúa cuando el jugador lo SUELTA dentro de la zona.
    /// </summary>
    [RequireComponent(typeof(Collider))]
    public class DeliveryZone : MonoBehaviour
    {
        [SerializeField] private ZoneType zoneType = ZoneType.Disposal;
        [SerializeField, Tooltip("Solo para Box: modelo que acepta esta caja.")]
        private ProductData acceptedModel;

        [Header("Feedback (Rol 1 y 4 enganchan haptics, sonido y partículas)")]
        public UnityEvent<bool> onDecision;   // true = acierto, false = error

        private void Reset() => GetComponent<Collider>().isTrigger = true;

        private void OnTriggerStay(Collider other)
        {
            var p = other.GetComponentInParent<Product>();
            if (p == null || p.Resolved || p.IsHeld) return;
            Resolve(p);
        }

        private void Resolve(Product p)
        {
            p.Resolved = true;
            var result = Evaluate(p);
            onDecision?.Invoke(result.correct);
            if (GameManager.Instance != null) GameManager.Instance.RegisterDecision(result);
            Destroy(p.gameObject, 0.15f);
        }

        private DecisionResult Evaluate(Product p)
        {
            var r = new DecisionResult { product = p };
            switch (zoneType)
            {
                case ZoneType.Disposal:
                    r.type = DecisionType.Discard;
                    r.correct = p.HasDefect;
                    r.reason = p.HasDefect
                        ? $"Defecto detectado: {p.Defect.displayName}"
                        : $"Desechaste un {p.Data.displayName} que estaba perfecto";
                    break;

                case ZoneType.Box:
                    r.type = DecisionType.Deliver;
                    if (p.HasDefect)
                    {
                        r.correct = false;
                        r.reason = $"Entregaste un {p.Data.displayName} con defecto: {p.Defect.displayName}";
                    }
                    else if (p.Data != acceptedModel)
                    {
                        r.correct = false;
                        r.reason = $"Caja equivocada: {p.Data.displayName} no va en la caja de {acceptedModel.displayName}";
                    }
                    else
                    {
                        r.correct = true;
                        r.reason = $"{p.Data.displayName} entregado correctamente";
                    }
                    break;

                default: // BeltEnd
                    r.type = DecisionType.Missed;
                    r.correct = false;
                    r.reason = $"Se te pasó un {p.Data.displayName} sin revisar";
                    break;
            }
            return r;
        }
    }
}
