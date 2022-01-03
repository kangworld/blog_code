using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyAddPoint3 : MonoBehaviour
{
    public int MyNum = 0;

    void Start()
    {
        MyFunc();
    }

    void MyFunc()
    {
        PointManager2.Instance().AddPoint(MyNum);
        int myResult = PointManager2.Instance().GetPoint();
        Debug.Log("Point : " + myResult);
    }
}
