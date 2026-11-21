using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

namespace TamagotchiFactory
{
    /// <summary>
    /// Componente que va en cada prefab de tamagotchi. Guarda su modelo, su defecto (si tiene)
    /// y expone Inspect() para que las herramientas del Rol 1 revelen el defecto.
    /// </summary>
    [RequireComponent(typeof(Rigidbody))]
    public class Product : MonoBehaviour
    {
        /// <summary>Visual asociado a un defecto (ej. grieta, tornillo flojo).</summary>
        [Serializable]
        public struct DefectVisual
        {
            public DefectType type;
            public GameObject visual;
            [Tooltip("true = el objeto se OCULTA cuando hay este defecto (ej. botón faltante).")]
            public bool removeInsteadOfAdd;
        }

        [Header("Visuales")]
        [SerializeField] private Renderer[] tintRenderers;      // renderers que se pintan con el color base
        [SerializeField] private DefectVisual[] defectVisuals;

        [Header("Eventos (Inspector)")]
        public UnityEvent<DefectData> onDefectRevealed;

        public ProductData Data { get; private set; }
        public DefectData Defect { get; private set; }
        public bool HasDefect => Defect != null;
        public bool DefectRevealed { get; private set; }
        public bool IsHeld { get; private set; }
        public bool Resolved { get; set; }
        public Rigidbody Body { get; private set; }

        private XRGrabInteractable grab;
        private static readonly int BaseColorId = Shader.PropertyToID("_BaseColor"); // URP Lit
        private MaterialPropertyBlock block;

        private void Awake()
        {
            Body = GetComponent<Rigidbody>();
            grab = GetComponent<XRGrabInteractable>();
        }

        private void OnEnable()
        {
            if (grab == null) return;
            grab.selectEntered.AddListener(OnGrabbed);
            grab.selectExited.AddListener(OnReleased);
        }

        private void OnDisable()
        {
            if (grab == null) return;
            grab.selectEntered.RemoveListener(OnGrabbed);
            grab.selectExited.RemoveListener(OnReleased);
        }

        private void OnGrabbed(SelectEnterEventArgs _) => IsHeld = true;
        private void OnReleased(SelectExitEventArgs _) => IsHeld = false;

        /// <summary>Lo llama el ProductSpawner justo después de instanciar.</summary>
        public void Initialize(ProductData data, DefectData defect)
        {
            Data = data;
            Defect = defect;
            DefectRevealed = false;

            Color color = data.baseColor;
            if (defect != null && defect.overridesColor) color = defect.wrongColor;
            Tint(color);
            ApplyDefectVisuals();
        }

        /// <summary>
        /// Lo llaman la lupa, el destornillador y el martillo (Rol 1).
        /// Devuelve true si la herramienta reveló un defecto.
        /// </summary>
        public bool Inspect(ToolType tool)
        {
            if (!HasDefect || DefectRevealed || Defect.revealedBy != tool) return false;
            DefectRevealed = true;
            ShowVisual(Defect.type);
            onDefectRevealed?.Invoke(Defect);
            return true;
        }

        private void Tint(Color c)
        {
            if (tintRenderers == null) return;
            block ??= new MaterialPropertyBlock();
            foreach (var r in tintRenderers)
            {
                if (r == null) continue;
                r.GetPropertyBlock(block);
                block.SetColor(BaseColorId, c);
                r.SetPropertyBlock(block);
            }
        }

        private void ApplyDefectVisuals()
        {
            if (defectVisuals == null) return;
            foreach (var dv in defectVisuals)
            {
                if (dv.visual == null) continue;
                bool isThisDefect = Defect != null && dv.type == Defect.type;
                if (dv.removeInsteadOfAdd)
                    dv.visual.SetActive(!isThisDefect);
                else
                    dv.visual.SetActive(isThisDefect && !Defect.hiddenUntilRevealed);
            }
        }

        private void ShowVisual(DefectType type)
        {
            if (defectVisuals == null) return;
            foreach (var dv in defectVisuals)
                if (dv.visual != null && dv.type == type && !dv.removeInsteadOfAdd)
                    dv.visual.SetActive(true);
        }
    }
}
