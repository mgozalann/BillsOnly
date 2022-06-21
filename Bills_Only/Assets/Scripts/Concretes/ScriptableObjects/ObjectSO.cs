using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Create Object/Create New", fileName = "Object Information", order = 51)]
public class ObjectSO : ScriptableObject
{
    [SerializeField] GameObject _nextValueGameObject;
    [SerializeField] float value;


    public GameObject NextValueGameObject => _nextValueGameObject;
    public float Value => value;
}
