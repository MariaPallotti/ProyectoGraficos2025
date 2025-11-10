using UnityEngine;

public class PlayerPositionManager : MonoBehaviour
{
    void Start()
    {
        if (BattlePositionManager.HasSavedPosition())
        {
            // Restaura la posición guardada del jugador
            transform.position = BattlePositionManager.GetSavedPosition();
            Debug.Log("Posición restaurada: " + transform.position);
        }
        else
        {
            Debug.Log("No hay posición guardada. Usando la posición inicial predeterminada.");
        }
    }
}