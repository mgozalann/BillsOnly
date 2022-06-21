using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GainController : MonoBehaviour
{
    [SerializeField] private Transform[] target;

    private int _index = 0;

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
    public void CheckIndex()
    {
        _index++;
        if(_index == LevelManager.Instance.LevelDataSO.MoneyCount)
        {
            GameManager.Instance.Success();
        }
    }
}
