using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveAct : MonoBehaviour
{
    enum Dir
    {
        Right,
        Left
    }

    Dir _move = Dir.Right;

    void Start()
    {
        StartCoroutine(Move());    
    }

    IEnumerator Move()
    {
        while (true)
        {
            if (gameObject.transform.position.x < -4)
                _move = Dir.Right;
            else if (gameObject.transform.position.x > 4)
                _move = Dir.Left;

            // ¿Ãµø
            if (_move == Dir.Right)
                gameObject.transform.Translate(1.0f * Vector3.right, Space.World);
            else if (_move == Dir.Left)
                gameObject.transform.Translate(-1.0f * Vector3.right, Space.World);

            yield return new WaitForSeconds(0.5f);
        }
    }
}
