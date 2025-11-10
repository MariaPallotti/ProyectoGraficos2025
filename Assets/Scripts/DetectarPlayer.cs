using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class DetectarPlayer : MonoBehaviour
{
    public string EncounterScene;
    public Transform player;
    public EnemyManager enemyManager;
    private bool active = false;

    void Start()
    {
        StartCoroutine(DisableAndReactivateCollider());
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && active)
        {
            BattlePositionManager.SavePosition(player.position);
            enemyManager.SaveId();
            SceneManager.LoadScene(EncounterScene);
        }
    }

    private IEnumerator DisableAndReactivateCollider()
    {
        yield return new WaitForSeconds(1);
        active = true;
    }
}
