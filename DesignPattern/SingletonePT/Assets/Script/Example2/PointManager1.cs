using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointManager1 : MonoBehaviour
{
    private static PointManager1 _instance = null;
    private int _myPoint = 0;

    public static PointManager1 Instance()
    {
        return _instance;
    }

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
    }

    public void AddPoint(int num)
    {
        _myPoint += num;
    }

    public int GetPoint()
    {
        return _myPoint;
    }
}
