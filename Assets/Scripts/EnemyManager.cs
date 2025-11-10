using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    private static List<int> testigos = new List<int>();
    public GameObject enemigo;
    public int id;
    private static int guardado = -1;

    // Start is called before the first frame update
    void Start()
    {
        if (testigos.Contains(id))
        {
            Vector3 pos = enemigo.transform.position;
            enemigo.transform.position = new Vector3(pos.x, -10.0f, pos.z);
            Destroy(enemigo);
        }
    }

    public static void AddTestigo()
    {
        testigos.Add(guardado);
    }

    public void SaveId()
    {
        guardado = id;
    }

    public static void Reset()
    {
        testigos = new List<int>();
    }
}
