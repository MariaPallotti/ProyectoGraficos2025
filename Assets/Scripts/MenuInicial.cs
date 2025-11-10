using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuInicial : MonoBehaviour
{
    public void Jugar()
    {
        BattlePositionManager.DeleteSavedPosition();
        Stats.Reset();
        HeartsUI.Reset();
        EnemyManager.Reset();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex +1);
    }
}
