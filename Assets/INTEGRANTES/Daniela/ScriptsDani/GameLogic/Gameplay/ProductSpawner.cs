using System.Collections;
using UnityEngine;

namespace TamagotchiFactory
{
    /// <summary>Genera productos al inicio de la banda con modelo y defecto aleatorios.</summary>
    public class ProductSpawner : MonoBehaviour
    {
        [Header("Referencias")]
        [SerializeField] private Transform spawnPoint;
        [SerializeField] private ConveyorBelt belt;

        [Header("Datos (5 productos, 5 defectos)")]
        [SerializeField] private ProductData[] products;
        [SerializeField] private DefectData[] defects;

        [Header("Ritmo")]
        [SerializeField, Range(0f, 1f)] private float defectChance = 0.5f;
        [SerializeField] private float baseInterval = 7f;   // segundos entre productos a velocidad x1
        [SerializeField] private float minInterval = 2.5f;
        [SerializeField] private int maxProductsOnBelt = 5;

        private Coroutine routine;
        private int alive;

        private void OnEnable() => GameEvents.StateChanged += OnStateChanged;
        private void OnDisable() => GameEvents.StateChanged -= OnStateChanged;

        private void OnStateChanged(GameState s)
        {
            if (routine != null) { StopCoroutine(routine); routine = null; }
            if (s == GameState.Playing) routine = StartCoroutine(SpawnLoop());
        }

        private IEnumerator SpawnLoop()
        {
            yield return new WaitForSeconds(1f);
            while (true)
            {
                if (alive < maxProductsOnBelt) SpawnRandom();
                float speed = belt != null ? belt.SpeedMultiplier : 1f;
                yield return new WaitForSeconds(Mathf.Max(minInterval, baseInterval / speed));
            }
        }

        public Product SpawnRandom()
        {
            var data = products[Random.Range(0, products.Length)];
            DefectData defect = null;
            if (defects.Length > 0 && Random.value < defectChance)
                defect = defects[Random.Range(0, defects.Length)];
            return Spawn(data, defect);
        }

        /// <summary>Para el onboarding (Rol 3): generar un producto concreto con/sin defecto.</summary>
        public Product Spawn(ProductData data, DefectData defect)
        {
            var go = Instantiate(data.prefab, spawnPoint.position, spawnPoint.rotation);
            var product = go.GetComponent<Product>();
            product.Initialize(data, defect);
            alive++;
            var tracker = go.AddComponent<SpawnTracker>();
            tracker.onDestroyed = () => alive--;
            return product;
        }

        // Cuenta cuántos productos siguen vivos para limitar la banda.
        private class SpawnTracker : MonoBehaviour
        {
            public System.Action onDestroyed;
            private void OnDestroy() => onDestroyed?.Invoke();
        }
    }
}
