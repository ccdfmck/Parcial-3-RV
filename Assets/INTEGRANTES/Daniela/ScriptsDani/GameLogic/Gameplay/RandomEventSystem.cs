using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace TamagotchiFactory
{
    /// <summary>
    /// Evento inesperado durante la partida. Elige al azar entre:
    ///  - BeltRush: la banda se acelera.
    ///  - Blackout: apagón (el Rol 4 conecta las luces en onBlackoutStart / onBlackoutEnd).
    ///  - UrgentOrder: pedido urgente de N unidades de un modelo; bonus si se cumple a tiempo.
    /// </summary>
    public class RandomEventSystem : MonoBehaviour
    {
        public enum WorldEventType { BeltRush, Blackout, UrgentOrder }

        [Header("Referencias")]
        [SerializeField] private ConveyorBelt belt;
        [SerializeField] private ProductData[] products;   // para elegir el modelo del pedido urgente

        [Header("Cuándo")]
        [SerializeField] private float minDelay = 40f;
        [SerializeField] private float maxDelay = 80f;
        [SerializeField] private float eventDuration = 20f;
        [SerializeField] private int maxEventsPerGame = 1;

        [Header("Parámetros")]
        [SerializeField] private float beltRushMultiplier = 2f;
        [SerializeField] private int urgentOrderAmount = 2;
        [SerializeField] private int urgentOrderBonus = 200;

        [Header("UnityEvents (luces, alarma, partículas...)")]
        public UnityEvent onEventStarted;
        public UnityEvent onEventEnded;
        public UnityEvent onBlackoutStart;
        public UnityEvent onBlackoutEnd;

        public WorldEventType? CurrentEvent { get; private set; }
        public ProductData UrgentModel { get; private set; }
        public int UrgentRemaining { get; private set; }

        private Coroutine routine;

        private void OnEnable() => GameEvents.StateChanged += OnStateChanged;
        private void OnDisable()
        {
            GameEvents.StateChanged -= OnStateChanged;
            GameEvents.DecisionMade -= OnDecision;
        }

        private void OnStateChanged(GameState s)
        {
            if (routine != null) { StopCoroutine(routine); routine = null; }
            if (CurrentEvent.HasValue) EndEvent();   // limpieza si se corta a media partida
            if (s == GameState.Playing) routine = StartCoroutine(EventLoop());
        }

        private IEnumerator EventLoop()
        {
            for (int i = 0; i < maxEventsPerGame; i++)
            {
                yield return new WaitForSeconds(Random.Range(minDelay, maxDelay));
                StartEvent((WorldEventType)Random.Range(0, 3));
                yield return new WaitForSeconds(eventDuration);
                EndEvent();
            }
        }

        private void StartEvent(WorldEventType type)
        {
            CurrentEvent = type;
            switch (type)
            {
                case WorldEventType.BeltRush:
                    belt.EventMultiplier = beltRushMultiplier;
                    break;
                case WorldEventType.Blackout:
                    onBlackoutStart?.Invoke();
                    break;
                case WorldEventType.UrgentOrder:
                    UrgentModel = products[Random.Range(0, products.Length)];
                    UrgentRemaining = urgentOrderAmount;
                    GameEvents.DecisionMade += OnDecision;
                    break;
            }
            onEventStarted?.Invoke();
            GameEvents.RaiseWorldEventStarted(Describe(type));
        }

        private void EndEvent()
        {
            if (!CurrentEvent.HasValue) return;
            var type = CurrentEvent.Value;
            switch (type)
            {
                case WorldEventType.BeltRush: belt.EventMultiplier = 1f; break;
                case WorldEventType.Blackout: onBlackoutEnd?.Invoke(); break;
                case WorldEventType.UrgentOrder: GameEvents.DecisionMade -= OnDecision; break;
            }
            CurrentEvent = null;
            onEventEnded?.Invoke();
            GameEvents.RaiseWorldEventEnded(Describe(type));
        }

        private void OnDecision(DecisionResult r)
        {
            if (CurrentEvent != WorldEventType.UrgentOrder) return;
            if (r.correct && r.type == DecisionType.Deliver && r.product != null && r.product.Data == UrgentModel)
            {
                UrgentRemaining--;
                if (UrgentRemaining <= 0)
                {
                    GameManager.Instance.AddBonus(urgentOrderBonus);
                    EndEvent();
                }
            }
        }

        private string Describe(WorldEventType type)
        {
            switch (type)
            {
                case WorldEventType.BeltRush: return "¡Banda acelerada!";
                case WorldEventType.Blackout: return "¡Apagón!";
                default: return $"¡Pedido urgente: {urgentOrderAmount} x {UrgentModel.displayName}!";
            }
        }
    }
}
