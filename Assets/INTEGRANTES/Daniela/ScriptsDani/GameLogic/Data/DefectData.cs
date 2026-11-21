using UnityEngine;

namespace TamagotchiFactory
{
    /// <summary>ScriptableObject de un defecto. Crear 5 desde Assets > Create > Tamagotchi Factory > Defect.</summary>
    [CreateAssetMenu(menuName = "Tamagotchi Factory/Defect", fileName = "Defect_")]
    public class DefectData : ScriptableObject
    {
        public DefectType type;
        public string displayName;
        [TextArea] public string description;      // se muestra en el panel de reporte

        [Header("Detección")]
        [Tooltip("Herramienta que revela este defecto (lupa: grietas/color, destornillador: tornillo, martillo: resistencia).")]
        public ToolType revealedBy = ToolType.Magnifier;
        [Tooltip("Si es true, el defecto no se ve a simple vista: hay que usar la herramienta.")]
        public bool hiddenUntilRevealed = true;

        [Header("Solo para WrongColor")]
        public bool overridesColor;
        public Color wrongColor = Color.magenta;
    }
}
