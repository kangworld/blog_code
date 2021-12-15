using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateAct : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(Rotate());
    }

    IEnumerator Rotate()
    {
        while(true)
        {
            transform.Rotate(90.0f * Vector3.up);

            yield return new WaitForSeconds(0.3f);
        }
    }
}
