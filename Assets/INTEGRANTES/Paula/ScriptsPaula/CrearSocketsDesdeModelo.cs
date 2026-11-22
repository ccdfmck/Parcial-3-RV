using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

public static class CrearSocketsDesdeModelo
{
    [MenuItem("Tools/Tamagotchi/Crear sockets desde modelo seleccionado")]
    private static void CrearSockets()
    {
        GameObject modelo = Selection.activeGameObject;
        if (modelo == null)
        {
            Debug.LogWarning("Selecciona el modelo armado en la Hierarchy.");
            return;
        }

        // Objeto contenedor de los sockets
        GameObject contenedor = new GameObject("Sockets");
        Undo.RegisterCreatedObjectUndo(contenedor, "Crear sockets");
        contenedor.transform.SetPositionAndRotation(modelo.transform.position, modelo.transform.rotation);

        foreach (Transform pieza in modelo.transform)
        {
            string nombre = pieza.name;

            // Crea el tag si no existe
            if (System.Array.IndexOf(InternalEditorUtility.tags, nombre) < 0)
                InternalEditorUtility.AddTag(nombre);

            GameObject socketObj = new GameObject("Socket_" + nombre);
            Undo.RegisterCreatedObjectUndo(socketObj, "Crear sockets");
            socketObj.transform.SetParent(contenedor.transform, false);
            socketObj.transform.SetPositionAndRotation(pieza.position, pieza.rotation);

            // Trigger que detecta la pieza
            SphereCollider col = socketObj.AddComponent<SphereCollider>();
            col.isTrigger = true;
            col.radius = 0.03f;

            // Socket con el tag de la pieza
            TagSocket socket = socketObj.AddComponent<TagSocket>();
            SerializedObject so = new SerializedObject(socket);
            so.FindProperty("targetTag").stringValue = nombre;
            so.ApplyModifiedProperties();
        }

        Debug.Log("Sockets creados para " + modelo.transform.childCount + " piezas.");
    }
}