using System.Collections.Generic;
using UnityEngine;

namespace TamagotchiFactory
{
    /// <summary>
    /// Banda transportadora de velocidad variable. Necesita:
    ///  - un Collider normal (la superficie) en este objeto o un hijo, y
    ///  - un BoxCollider con "Is Trigger" justo encima de la superficie (este script lo usa para detectar productos).
    /// La velocidad final = baseSpeed * DifficultyMultiplier * EventMultiplier.
    /// </summary>
    public class ConveyorBelt : MonoBehaviour
    {
        [SerializeField] private float baseSpeed = 0.25f;                    // m/s
        [SerializeField] private Vector3 localDirection = Vector3.forward;   // dirección en espacio local
        [SerializeField] private bool runDuringOnboarding = true;

        [Header("Visual (opcional)")]
        [SerializeField] private Renderer beltRenderer;
        [SerializeField] private float textureScrollFactor = 1f;

        /// <summary>Lo sube el DifficultyManager con el tiempo.</summary>
        public float DifficultyMultiplier { get; set; } = 1f;
        /// <summary>Lo cambia el evento inesperado (banda acelerada).</summary>
        public float EventMultiplier { get; set; } = 1f;
        public float SpeedMultiplier => DifficultyMultiplier * EventMultiplier;
        public float CurrentSpeed => IsRunning ? baseSpeed * SpeedMultiplier : 0f;
        public bool IsRunning { get; private set; }

        private readonly HashSet<Product> onBelt = new HashSet<Product>();
        private float uvOffset;

        private void OnEnable() => GameEvents.StateChanged += OnStateChanged;
        private void OnDisable() => GameEvents.StateChanged -= OnStateChanged;

        private void OnStateChanged(GameState s)
        {
            IsRunning = s == GameState.Playing || (s == GameState.Onboarding && runDuringOnboarding);
            if (s == GameState.Menu)
            {
                DifficultyMultiplier = 1f;
                EventMultiplier = 1f;
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            var p = other.GetComponentInParent<Product>();
            if (p != null) onBelt.Add(p);
        }

        private void OnTriggerExit(Collider other)
        {
            var p = other.GetComponentInParent<Product>();
            if (p != null) onBelt.Remove(p);
        }

        private void FixedUpdate()
        {
            onBelt.RemoveWhere(p => p == null);
            if (!IsRunning) return;

            Vector3 step = transform.TransformDirection(localDirection.normalized) * (CurrentSpeed * Time.fixedDeltaTime);
            foreach (var p in onBelt)
            {
                if (p.IsHeld) continue;   // no arrastrar lo que el jugador tiene en la mano
                p.Body.MovePosition(p.Body.position + step);
            }
        }

        private void Update()
        {
            if (beltRenderer == null || !IsRunning) return;
            uvOffset += CurrentSpeed * textureScrollFactor * Time.deltaTime;
            beltRenderer.material.mainTextureOffset = new Vector2(0f, uvOffset);
        }
    }
}
