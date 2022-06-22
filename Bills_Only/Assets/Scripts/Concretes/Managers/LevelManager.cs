using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : SingletonMonoBehaviorObject<LevelManager>
{
    [SerializeField] private LevelDataSO _levelDataSO;
    [SerializeField] private GameObject[] _objectsPrefab;

    public GameObject[] ObjectsPrefab => _objectsPrefab;
    public LevelDataSO LevelDataSO => _levelDataSO;


    private void Awake()
    {
        SingletonThisObject(this);
    }

    private void OnEnable()
    {
        GameManager.Instance.OnSuccess += Success;
    }

    private void OnDisable()
    {
        GameManager.Instance.OnSuccess -= Success;

    }

    private void Success()
    {
        PlayerPrefs.SetInt("Money", PlayerPrefs.GetInt("Money") + _levelDataSO.MoneyCount);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1); // Loadnext scene
    }
}
