using UnityEngine;

public class BattlePositionManager : MonoBehaviour
{
    public static Vector3 savedPosition;
    public static bool hasSavedPosition = false;

    public static void SavePosition(Vector3 position)
    {
        savedPosition = position;
        hasSavedPosition = true;
        Debug.Log("Posición guardada: " + savedPosition);
    }

    public static Vector3 GetSavedPosition()
    {
        return savedPosition;
    }

    public static bool HasSavedPosition()
    {
        return hasSavedPosition;
    }

    public static void DeleteSavedPosition()
    {
        hasSavedPosition = false;
    }
}



