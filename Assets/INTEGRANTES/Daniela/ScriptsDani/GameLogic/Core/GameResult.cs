using System;

namespace TamagotchiFactory
{
    /// <summary>
    /// Resultado final de la partida. Es [Serializable] para que el Rol 3
    /// pueda guardarlo con JsonUtility.ToJson(result) y mostrarlo en la pantalla de cierre.
    /// </summary>
    [Serializable]
    public class GameResult
    {
        public int score;
        public int correct;          // decisiones correctas
        public int wrong;            // decisiones incorrectas
        public int defectsDetected;  // productos defectuosos desechados correctamente
        public float timeUsed;       // segundos jugados
        public bool timeExpired;     // terminó porque se acabó el tiempo
        public bool failedByErrors;  // terminó por superar el máximo de errores
        public string feedback;      // texto de retroalimentación
    }

    /// <summary>Qué hizo el jugador con un producto.</summary>
    public enum DecisionType { Discard, Deliver, Missed }

    /// <summary>Resultado de validar una decisión. Lo reciben UI, audio y efectos.</summary>
    public struct DecisionResult
    {
        public bool correct;
        public DecisionType type;
        public Product product;
        public string reason;   // texto corto para el panel de reporte (Rol 3)
    }
}
