using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyDelayed : MonoBehaviour
{
    private AudioSource _audioSource;

    private void Start()
    {
        _audioSource = gameObject.GetComponent<AudioSource>();    
    }
    private void OnCollisionEnter(Collision collision)
    {
        _audioSource.Play();

        Destroy(gameObject, _audioSource.clip.length);
    }
}
