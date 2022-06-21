using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "New Land", fileName = "Land Data", order = 51)]
public class LandSO : ScriptableObject
{
    [SerializeField] int _rank;
    [SerializeField] int _price;

    public int Rank => _rank;
    public int Price => _price;

}
