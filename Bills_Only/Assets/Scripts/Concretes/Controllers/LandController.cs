using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LandController : MonoBehaviour
{
    [SerializeField] private LandSO _landSO;
    [SerializeField] private Text _text;
    [SerializeField] private Transform[] _lands;
    private enum State
    {
        buyable,
        unbuyable,
        bought,
    }

    [SerializeField] private State _curState;
    
    private void Start()
    {
        if (!PlayerPrefs.HasKey("Index"))
        {
            PlayerPrefs.SetInt("Index", 1);
        }
        if (!PlayerPrefs.HasKey("Money"))
        {
            PlayerPrefs.SetInt("Money", 0);
        }


    }
    private void Update()
    {
        StateMachine();
        StateCheck();

        Debug.Log(PlayerPrefs.GetInt("Index"));
    }
    public void BuyLand()
    {
        if(_curState == State.buyable)
        {
            if (PlayerPrefs.GetInt("Money") >= _landSO.Price)
            {
                PlayerPrefs.SetInt("Money", PlayerPrefs.GetInt("Money") - _landSO.Price); // para eksilt
                PlayerPrefs.SetInt("Index", PlayerPrefs.GetInt("Index") + 1); // index++

                _curState = State.bought;
            }
        }
    }   

   
    private void StateCheck()
    {
        if(_landSO.Rank < PlayerPrefs.GetInt("Index"))
        {
            _curState = State.bought;
        }
        if (_curState == State.bought) return;

        if (_landSO.Rank == PlayerPrefs.GetInt("Index"))
        {
            _curState = State.buyable;
        }
        else if(_landSO.Rank != PlayerPrefs.GetInt("Index"))
        {
            _curState = State.unbuyable;
        }
    }

    private void StateMachine()
    {
        if(_curState == State.buyable)
        {
            SetLands(true);
        }
        else if(_curState == State.unbuyable)
        {
            SetLands(true);
        }
        else
        {
            SetLands(false);
        }
    }
    private void SetLands(bool open)
    {
        _lands[0].transform.gameObject.SetActive(open);
        _lands[1].transform.gameObject.SetActive(!open);
    }
}

