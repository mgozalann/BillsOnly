using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : SingletonMonoBehaviorObject<GameManager>
{
    public event System.Action OnSuccess;

    private void Awake()
    {
        SingletonThisObject(this);
    }

    public void Success()
    {
        OnSuccess?.Invoke();
    }



}
