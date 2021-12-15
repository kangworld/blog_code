using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum Dir
{
    Right,
    Left
}

public class Spaghetti : MonoBehaviour
{
    GameObject _player1;
    GameObject _player2;
    Dir _move = Dir.Right;

    void Start()
    {
        _player1 = GameObject.Find("Player1");
        _player2 = GameObject.Find("Player2");

        StartCoroutine(MixedAct());
    }

    IEnumerator MixedAct()
    {
        while(true)
        {
            // 회전
            _player1.transform.Rotate(90.0f * Vector3.up);
            _player2.transform.Rotate(90.0f * Vector3.up);

            if (_player1.transform.position.x < -4)
                _move = Dir.Right;
            else if (_player1.transform.position.x > 4)
                _move = Dir.Left;

            // 이동
            if(_move == Dir.Right)
            {
                _player1.transform.Translate(1.0f * Vector3.right, Space.World);
                _player2.transform.Translate(-1.0f * Vector3.right, Space.World);
            }
            else if(_move == Dir.Left)
            {
                _player1.transform.Translate(-1.0f * Vector3.right, Space.World);
                _player2.transform.Translate(1.0f * Vector3.right, Space.World);
            }

            yield return new WaitForSeconds(0.5f);
        }
    }
}
