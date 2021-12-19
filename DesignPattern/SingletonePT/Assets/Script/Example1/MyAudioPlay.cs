using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyAudioPlay : MonoBehaviour
{
    public AudioClip AudioClip;

    private void OnCollisionEnter(Collision collision)
    {
        AudioManager.Instance().Play(AudioClip);

        Destroy(gameObject);
    }
}
