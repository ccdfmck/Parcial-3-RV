using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace TamagotchiFactory
{
    /// <summary>
    /// Máquina de estados del juego + puntaje + temporizador + condición de fallo.
    /// Flujo: Menu -> Onboarding -> Playing -> Closing.
    /// Rol 3 llama a: StartOnboarding(), CompleteOnboarding(), Restart().
    /// </summary>
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }

        [Header("Reglas")]
        [SerializeField] private float gameDuration = 180f;   // segundos de partida
        [SerializeField] private int pointsCorrect = 100;
        [SerializeField] private int pointsWrong = -50;
        [SerializeField] private int maxErrors = 5;           // condición de fallo
        [SerializeField] private GameState initialState = GameState.Menu;

        [Header("UnityEvents (opcional, para conectar en el Inspector)")]
        public UnityEvent onGameStarted;
        public UnityEvent onGameWon;      // terminó por tiempo
        public UnityEvent onGameFailed;   // terminó por errores

        public GameState State { get; private set; }
        public int Score { get; private set; }
        public int Correct { get; private set; }
        public int Wrong { get; private set; }
        public int DefectsDetected { get; private set; }
        public float TimeRemaining { get; private set; }
        public float TimeElapsed => gameDuration - TimeRemaining;
        public int MaxErrors => maxErrors;
        public GameResult LastResult { get; private set; }

        private int lastSecondsRaised = -1;

        private void Awake()
        {
            if (Instance != null && Instance != this) { Destroy(gameObject); return; }
            Instance = this;
        }

        private void Start() => ChangeState(initialState);

        private void OnDestroy() { if (Instance == this) Instance = null; }

        // ------------------ Máquina de estados ------------------

        public void ChangeState(GameState newState)
        {
            State = newState;
            GameEvents.RaiseStateChanged(newState);
        }

        /// <summary>Botón "Jugar" del menú.</summary>
        public void StartOnboarding() => ChangeState(GameState.Onboarding);

        /// <summary>El Rol 3 la llama cuando termina el último paso del tutorial.</summary>
        public void CompleteOnboarding() => StartGame();

        public void StartGame()
        {
            Score = Correct = Wrong = DefectsDetected = 0;
            TimeRemaining = gameDuration;
            lastSecondsRaised = -1;
            GameEvents.RaiseScoreChanged(Score);
            ChangeState(GameState.Playing);
            onGameStarted?.Invoke();
        }

        /// <summary>Botón "Reiniciar" de la pantalla final.</summary>
        public void Restart() => SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

        // ------------------ Temporizador ------------------

        private void Update()
        {
            if (State != GameState.Playing) return;

            TimeRemaining -= Time.deltaTime;
            int secs = Mathf.CeilToInt(Mathf.Max(0f, TimeRemaining));
            if (secs != lastSecondsRaised)
            {
                lastSecondsRaised = secs;
                GameEvents.RaiseTimeChanged(secs);
            }
            if (TimeRemaining <= 0f) EndGame(timeExpired: true, failedByErrors: false);
        }

        // ------------------ Puntaje y validación ------------------

        /// <summary>La llama DeliveryZone con el resultado de cada decisión.</summary>
        public void RegisterDecision(DecisionResult result)
        {
            // En el onboarding se da feedback pero no se puntúa.
            if (State == GameState.Playing)
            {
                if (result.correct)
                {
                    Correct++;
                    Score += pointsCorrect;
                    if (result.type == DecisionType.Discard) DefectsDetected++;
                }
                else
                {
                    Wrong++;
                    Score += pointsWrong;
                }
                Score = Mathf.Max(0, Score);
                GameEvents.RaiseScoreChanged(Score);
            }

            GameEvents.RaiseDecisionMade(result);

            if (State == GameState.Playing && Wrong >= maxErrors)
                EndGame(timeExpired: false, failedByErrors: true);
        }

        /// <summary>Puntos extra (ej. pedido urgente cumplido).</summary>
        public void AddBonus(int points)
        {
            if (State != GameState.Playing) return;
            Score = Mathf.Max(0, Score + points);
            GameEvents.RaiseScoreChanged(Score);
        }

        // ------------------ Cierre ------------------

        private void EndGame(bool timeExpired, bool failedByErrors)
        {
            if (State != GameState.Playing) return;

            LastResult = new GameResult
            {
                score = Score,
                correct = Correct,
                wrong = Wrong,
                defectsDetected = DefectsDetected,
                timeUsed = Mathf.Clamp(TimeElapsed, 0f, gameDuration),
                timeExpired = timeExpired,
                failedByErrors = failedByErrors,
                feedback = BuildFeedback(failedByErrors)
            };

            ChangeState(GameState.Closing);
            GameEvents.RaiseGameFinished(LastResult);
            if (failedByErrors) onGameFailed?.Invoke(); else onGameWon?.Invoke();
        }

        private string BuildFeedback(bool failedByErrors)
        {
            int total = Correct + Wrong;
            float accuracy = total > 0 ? (float)Correct / total : 0f;

            if (failedByErrors)
                return "Turno terminado: demasiados errores. Revisa cada producto con las herramientas antes de decidir.";
            if (total == 0)
                return "No procesaste ningún producto. Recuerda revisar y decidir cada tamagotchi.";
            if (accuracy >= 0.9f)
                return "¡Excelente control de calidad! Casi no se te escapó ningún defecto.";
            if (accuracy >= 0.6f)
                return "Buen trabajo. Usa más la lupa y el destornillador para no dejar pasar defectos.";
            return "Necesitas más práctica: varios productos se clasificaron mal. Repite el tutorial si lo necesitas.";
        }
    }
}
