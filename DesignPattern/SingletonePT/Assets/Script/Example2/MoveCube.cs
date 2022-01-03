using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCube : MonoBehaviour
{
    private float _speed = 20.0f;

    void Update()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        h = h * _speed * Time.deltaTime;
        v = v * _speed * Time.deltaTime;

        transform.Translate(Vector3.right * h);
        transform.Translate(Vector3.forward * v);
    }
}
