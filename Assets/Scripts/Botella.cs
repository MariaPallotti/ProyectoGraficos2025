using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Botella : MonoBehaviour
{
    private AudioSource audioSource;
    public AudioClip recoger;
    public Stats _stats;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _stats.UpdateBotellas(20);
            transform.position = new Vector3(0, -10, 0);
            audioSource.PlayOneShot(recoger);
            Destroy(gameObject, recoger.length);
        }
    }
}
