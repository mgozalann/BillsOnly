using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private ColumnController[] columnControllers; //Leveldata singleton tutabilir.
    [SerializeField] private List<GameObject> _objectList = new List<GameObject>();

    private int _count;
    private GameObject[] _prefabs;

    private void Start()
    {
        _prefabs = LevelManager.Instance.ObjectsPrefab;
        SpawnObjects();

    }
    private void SpawnObjects()
    {

        for (int i = 0; i < LevelManager.Instance.LevelDataSO.MoneyCount; i++)
        {
            GameObject newObj = Instantiate(_prefabs[0], transform.position, _prefabs[0].transform.rotation);

            newObj.transform.parent = this.transform;

            _objectList.Add(newObj);

            for (int j = 0; j < _prefabs.Length; j++)
            {
                newObj = Instantiate(_prefabs[j], transform.position, _prefabs[j].transform.rotation);

                newObj.transform.parent = this.transform;

                _objectList.Add(newObj);
            }
        }

        CheckListElements();

        _count = _objectList.Count;

        OrganizeObjectsRandomly();
    }

    private void CheckListElements() //Tekrar bakýlýp düzenlenebilir
    {
        _objectList = _objectList.OrderBy(p => Guid.NewGuid()).ToList();

        for (int i = 0; i < _objectList.Count - 3; i++)
        {
            if (_objectList[i].name == _objectList[i + 1].name)
            {
                var firstSwap = _objectList[i + 1];
                _objectList[i + 1] = _objectList[i + 2];
                _objectList[i + 2] = firstSwap;

                if(_objectList[i+1] == _objectList[i+2])
                {
                    _objectList = _objectList.OrderBy(p => Guid.NewGuid()).ToList();
                    CheckListElements();
                }
            }
        }
    }

    private void OrganizeObjectsRandomly()
    {
        var columnNum = columnControllers.Length;
        
        for (int j = 0; j < columnControllers.Length; j++)
        {
            var r = _count / columnNum;
            for (int i = 0; i < r; i++)
            {
                columnControllers[j].objects.Add(_objectList[_count-1].transform);
                _count--;
            }
            columnControllers[j].OrganizeList();
            columnNum--;
        }
    }
}
