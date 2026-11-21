using System;

namespace TamagotchiFactory
{
    /// <summary>
    /// Sistema reactivo central (eventos C#). UI, audio y efectos se SUSCRIBEN aquí
    /// en OnEnable y se DESUSCRIBEN en OnDisable. Así el GameManager no conoce a nadie.
    /// </summary>
    public static class GameEvents
    {
        public static event Action<GameState> StateChanged;
        public static event Action<int> ScoreChanged;
        public static event Action<float> TimeChanged;            // segundos restantes (1 vez por segundo)
        public static event Action<DecisionResult> DecisionMade;
        public static event Action<string> WorldEventStarted;     // nombre del evento inesperado
        public static event Action<string> WorldEventEnded;
        public static event Action<GameResult> GameFinished;

        public static void RaiseStateChanged(GameState s) => StateChanged?.Invoke(s);
        public static void RaiseScoreChanged(int s) => ScoreChanged?.Invoke(s);
        public static void RaiseTimeChanged(float t) => TimeChanged?.Invoke(t);
        public static void RaiseDecisionMade(DecisionResult d) => DecisionMade?.Invoke(d);
        public static void RaiseWorldEventStarted(string n) => WorldEventStarted?.Invoke(n);
        public static void RaiseWorldEventEnded(string n) => WorldEventEnded?.Invoke(n);
        public static void RaiseGameFinished(GameResult r) => GameFinished?.Invoke(r);
    }
}
