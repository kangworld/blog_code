using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateBall : MonoBehaviour
{
    public GameObject Ball;

    private void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            float x, y, z;
            x = z = Random.Range(-4, 4);
            y = Random.Range(4, 8);

            Vector3 pos = new Vector3(x, y, z);
            //Quaternion.identity : �������� ���� ȸ������ �����
            Instantiate(Ball, pos, Quaternion.identity);
        }
    }
}
