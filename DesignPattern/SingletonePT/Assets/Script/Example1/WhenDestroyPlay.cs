using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhenDestroyPlay : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        gameObject.GetComponent<AudioSource>().Play();

        Destroy(gameObject);
    }
}
