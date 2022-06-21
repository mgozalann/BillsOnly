using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LandController : MonoBehaviour
{
    [SerializeField] private LandSO _landSO;
    [SerializeField] private Text _text;

    private enum State
    {
        buyable,
        unbuyable,
    }

    private State _curState;
    

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
        StateCheck();
    }
    public void BuyLand()
    {
        if (_curState == State.unbuyable) return;
        if (PlayerPrefs.GetInt("Money") >= _landSO.Price)
        {
            Debug.Log(this.gameObject.name);
            PlayerPrefs.SetInt("Money", PlayerPrefs.GetInt("Money") - _landSO.Price); // para eksilt
            PlayerPrefs.SetInt("Index", PlayerPrefs.GetInt("Index") + 1); // index++
        }
    }
    private void StateCheck()
    {
        if (_landSO.Rank == PlayerPrefs.GetInt("Index"))
        {
            _curState = State.buyable;
            //yanýp sönsün
            // _text.text = PlayerPrefs.GetInt("Money").ToString() + "/" + _landSO.Price.ToString();

        }
        else
        {
            _curState = State.unbuyable;
            //hiçbir þey yapmasýn
        }
    }

}

