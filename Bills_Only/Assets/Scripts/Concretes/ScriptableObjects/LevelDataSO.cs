using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "New Level", fileName = "Level Name", order = 51)]

public class LevelDataSO : ScriptableObject
{

    [SerializeField] int moneyCount;

    public int MoneyCount => moneyCount;

}
