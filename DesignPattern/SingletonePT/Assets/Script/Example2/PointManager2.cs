using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointManager2 : MonoBehaviour
{
    private static PointManager2 _instance = null;
    private int _myPoint = 0;

    public static PointManager2 Instance()
    {
        return _instance;
    }

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else if (this != _instance)
            Destroy(this.gameObject);
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
