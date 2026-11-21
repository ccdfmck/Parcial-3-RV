using UnityEngine;

namespace TamagotchiFactory
{
    /// <summary>Sube la dificultad con el tiempo: cada "stepSeconds" la banda va más rápido.</summary>
    public class DifficultyManager : MonoBehaviour
    {
        [SerializeField] private ConveyorBelt belt;
        [SerializeField] private float stepSeconds = 30f;
        [SerializeField] private float speedIncrease = 0.15f;
        [SerializeField] private float maxMultiplier = 2f;

        private float timer;
        private bool active;

        private void OnEnable() => GameEvents.StateChanged += OnStateChanged;
        private void OnDisable() => GameEvents.StateChanged -= OnStateChanged;

        private void OnStateChanged(GameState s)
        {
            active = s == GameState.Playing;
            if (s == GameState.Playing) { timer = 0f; belt.DifficultyMultiplier = 1f; }
        }

        private void Update()
        {
            if (!active) return;
            timer += Time.deltaTime;
            if (timer >= stepSeconds)
            {
                timer = 0f;
                belt.DifficultyMultiplier = Mathf.Min(maxMultiplier, belt.DifficultyMultiplier + speedIncrease);
            }
        }
    }
}
