using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ColumnController : MonoBehaviour
{
    public List<Transform> objects = new List<Transform>();

    [SerializeField] private Transform _spawnManager;
    [SerializeField] private InputController _playerController;
    [SerializeField] private GainController _gainController;

    WaitForSeconds _longWait = new WaitForSeconds(.75f);
    WaitForSeconds _shortWait = new WaitForSeconds(.05f);

    [SerializeField] private float _offSetz;

    [SerializeField] private float duration;

    public void OrganizeList()
    {
        _playerController.CanPlay = false;

        if (objects.Count == 0)
        {
            _playerController.CanPlay = true;
            return;
        }

        float offSetz = 0;

        int i = 0;

        foreach (var obj in objects)
        {
            var objectController = obj.GetComponent<ObjectController>();

            obj.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + offSetz);

            objectController.WhichColumn = this.transform;
            objectController.Row = i;

            offSetz -= _offSetz;

            i++;
        }
        MakeWaveAsync();
    }
    private void CheckListAysnc()
    {
        StartCoroutine(CheckList());
    }
    private void MakeWaveAsync()
    {
        StartCoroutine(MakeWave());
    }
    IEnumerator MakeWave()
    {
        for (int i = 0; i <= objects.Count - 1; i++)
        {
            objects[i].DOPunchPosition(Vector3.up / 2, duration, 1, 1f, false);
            yield return _shortWait;
        }
        CheckListAysnc();
    }
    IEnumerator CheckList()
    {
        if (objects.Count > 1)
        {
            for (int i = objects.Count - 1; i >= 1; i--)
            {
                if (objects[i].GetComponent<ObjectController>().ObjectSO.Value == objects[i - 1].GetComponent<ObjectController>().ObjectSO.Value)
                {
                    yield return _longWait;
                    objects[i - 1].gameObject.SetActive(false);
                    objects[i].gameObject.SetActive(false);
                    objects.Remove(objects[i]);

                    var nextGO = objects[i - 1].GetComponent<ObjectController>().ObjectSO.NextValueGameObject;
                    nextGO = Instantiate(nextGO, objects[i - 1].position, nextGO.transform.rotation);
                    nextGO.transform.parent = _spawnManager;

                    if (nextGO.GetComponent<ObjectController>() != null)
                    {
                        objects[i - 1] = nextGO.transform;
                        nextGO.transform.DOPunchScale(Vector3.one, duration * 2, 1);
                    }
                    else
                    {
                        objects.Remove(objects[i - 1]);
                        nextGO.transform.DOMove(_gainController.Target[_gainController.Index].position, 1.5f, false).OnComplete(_gainController.CheckIndex);
                    }
                    OrganizeList();
                    yield break;
                }
            }
        }
        _playerController.CanPlay = true;
    }
   
}
