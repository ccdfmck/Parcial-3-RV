using UnityEngine;

namespace TamagotchiFactory
{
    /// <summary>ScriptableObject de un modelo de tamagotchi. Crear 5 desde Assets > Create > Tamagotchi Factory > Product.</summary>
    [CreateAssetMenu(menuName = "Tamagotchi Factory/Product", fileName = "Product_")]
    public class ProductData : ScriptableObject
    {
        public string id;
        public string displayName;
        [Tooltip("Prefab con el componente Product (lo deja listo el Rol 1).")]
        public GameObject prefab;
        public Color baseColor = Color.white;
        public Sprite icon;   // opcional, para la UI
    }
}
