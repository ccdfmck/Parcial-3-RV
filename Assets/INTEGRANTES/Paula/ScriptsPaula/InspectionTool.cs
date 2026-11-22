using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

namespace TamagotchiFactory
{
    [RequireComponent(typeof(XRGrabInteractable))]
    public class InspectionTool : MonoBehaviour
    {
        [SerializeField] private ToolType toolType;

        [Header("Condiciones de uso")]
        [Tooltip("Velocidad mínima de la herramienta (m/s). Martillo: ~1.5. Otras: 0.")]
        [SerializeField] private float minSpeed = 0f;
        [Tooltip("Segundos que debe permanecer sobre el producto. Lupa: ~0.5. Otras: 0.")]
        [SerializeField] private float dwellTime = 0f;
        [SerializeField] private float cooldown = 0.5f;

        [Header("Eventos (haptics, sonido, partículas)")]
        public UnityEvent onHitProduct;      // tocó un producto
        public UnityEvent onDefectRevealed;  // además reveló un defecto

        private XRGrabInteractable grab;
        private Vector3 lastPosition;
        private float speed;
        private float lastUseTime = -999f;
        private float dwell;
        private Product currentProduct;

        private void Awake()
        {
            grab = GetComponent<XRGrabInteractable>();
            lastPosition = transform.position;
        }

        // Mide la velocidad de la herramienta (sirve para el golpe del martillo)
        private void Update()
        {
            if (Time.deltaTime > 0f)
                speed = (transform.position - lastPosition).magnitude / Time.deltaTime;
            lastPosition = transform.position;
        }

        private void OnTriggerEnter(Collider other)
        {
            // Martillo y destornillador: actúan al contacto
            if (dwellTime <= 0f) TryUse(other);
        }

        private void OnTriggerStay(Collider other)
        {
            // Lupa: actúa tras permanecer un rato sobre el producto
            if (dwellTime <= 0f) return;

            Product product = other.GetComponentInParent<Product>();
            if (product == null) return;

            if (product != currentProduct)
            {
                currentProduct = product;
                dwell = 0f;
            }

            dwell += Time.fixedDeltaTime;
            if (dwell >= dwellTime)
            {
                TryUse(other);
                dwell = 0f;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.GetComponentInParent<Product>() == currentProduct)
            {
                currentProduct = null;
                dwell = 0f;
            }
        }

        private void TryUse(Collider other)
        {
            if (!grab.isSelected) return;                          // debe estar en la mano
            if (speed < minSpeed) return;                          // golpe muy suave
            if (Time.time - lastUseTime < cooldown) return;

            Product product = other.GetComponentInParent<Product>();
            if (product == null) return;

            lastUseTime = Time.time;
            onHitProduct?.Invoke();

            if (product.Inspect(toolType))
                onDefectRevealed?.Invoke();
        }
    }
}