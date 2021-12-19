using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyAddPoint1 : MonoBehaviour
{
    public int MyNum = 0;

    private void Start()
    {
        MyFunc();
    }

    void MyFunc()
    {
        PointManager1.Instance().AddPoint(MyNum);
        int myResult = PointManager1.Instance().GetPoint();
        Debug.Log("Point : " + myResult);
    }
}
