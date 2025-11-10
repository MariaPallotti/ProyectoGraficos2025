using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class Stats : MonoBehaviour
{
    private static int botellas = 0;

    [Header("TextosUI")]
    public TextMeshProUGUI botellastxt;

    private void Update(){
        if (botellastxt != null)
        {
            botellastxt.text = "MANA: " + botellas.ToString();
        }
    }

    public void UpdateBotellas(int numBotellas)
    {
        botellas += numBotellas;
    }

    public int GetBotellas()
    {
        return botellas;
    }

    public void SetBotellas(int nBotellas)
    {
        botellas = nBotellas;
    }

    public static void Reset()
    {
        botellas = 0;
    }

}
