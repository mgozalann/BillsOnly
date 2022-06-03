using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AtmController : MonoBehaviour
{
    [SerializeField] private Transform[] target;

    int _index = 0;

    public Transform[] Target => target;
    public int Index
    {
        get 
        {
            return _index; 
        }
        set 
        {
            _index = value; 
        }
    }

}
