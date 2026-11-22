using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Inputs.Haptics;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

namespace TamagotchiFactory
{
    [RequireComponent(typeof(XRGrabInteractable))]
    public class HapticFeedback : MonoBehaviour
    {
        [System.Serializable]
        public struct Impulse
        {
            [Range(0f, 1f)] public float amplitude;
            public float duration;
        }

        [Header("Intensidades")]
        [SerializeField] private Impulse grab = new Impulse { amplitude = 0.2f, duration = 0.05f };
        [SerializeField] private Impulse assemble = new Impulse { amplitude = 0.5f, duration = 0.15f };
        [SerializeField] private Impulse hit = new Impulse { amplitude = 0.7f, duration = 0.1f };
        [SerializeField] private Impulse reveal = new Impulse { amplitude = 0.4f, duration = 0.25f };

        private XRGrabInteractable grabInteractable;
        private HapticImpulsePlayer lastPlayer; // control que lo sostuvo por última vez

        private void Awake()
        {
            grabInteractable = GetComponent<XRGrabInteractable>();
        }

        private void OnEnable()
        {
            grabInteractable.selectEntered.AddListener(OnSelectEntered);
        }

        private void OnDisable()
        {
            grabInteractable.selectEntered.RemoveListener(OnSelectEntered);
        }

        private void OnSelectEntered(SelectEnterEventArgs args)
        {
            // Si lo tomó un socket, es un ensamble correcto
            // (el TagSocket ya rechazó las piezas incorrectas)
            if (args.interactorObject is XRSocketInteractor)
            {
                PlayAssemble();
                return;
            }

            // Si lo tomó una mano, guardamos cuál para vibrarla después
            lastPlayer = FindPlayer(args.interactorObject.transform);
            PlayGrab();
        }

        // Estos métodos se pueden conectar desde UnityEvents en el Inspector
        public void PlayGrab() => Play(grab);
        public void PlayAssemble() => Play(assemble);
        public void PlayHit() => Play(hit);
        public void PlayReveal() => Play(reveal);

        private void Play(Impulse impulse)
        {
            if (lastPlayer == null) return;
            lastPlayer.SendHapticImpulse(impulse.amplitude, impulse.duration);
        }

        // Busca el Haptic Impulse Player del control al que pertenece el interactor
        private static HapticImpulsePlayer FindPlayer(Transform interactor)
        {
            HapticImpulsePlayer player = interactor.GetComponentInParent<HapticImpulsePlayer>();
            if (player == null && interactor.parent != null)
                player = interactor.parent.GetComponentInChildren<HapticImpulsePlayer>();
            return player;
        }
    }
}