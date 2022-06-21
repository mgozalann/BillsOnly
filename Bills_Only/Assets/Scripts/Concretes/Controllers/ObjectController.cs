using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectController : MonoBehaviour
{
    [SerializeField] ObjectSO _objectSO;

    [SerializeField] private Transform _whichColumn;
    [SerializeField] private int _row;

    public ObjectSO ObjectSO => _objectSO;
    //injection araþtýr
    //buna gerek yok , indexof
    public int Row
    {
        get
        {
            return _row;
        }
        set
        {
            _row = value;
        }
    }
    public Transform WhichColumn
    {
        get
        {
            return _whichColumn;
        }
        set
        {
            _whichColumn = value;
        }
    }
}
