using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectController : MonoBehaviour
{
    private PlayerController playerController;

    [SerializeField] private Transform _whichColumn;
    [SerializeField] private int _row;

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
    private void Awake()
    {
        playerController = FindObjectOfType<PlayerController>();
    }

    private void Update()
    {

    }



}
