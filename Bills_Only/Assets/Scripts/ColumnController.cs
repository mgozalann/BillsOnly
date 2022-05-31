using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ColumnController : MonoBehaviour
{
    public List<Transform> objects = new List<Transform>();

    [SerializeField] Transform _spawnManager;
    [SerializeField] PlayerController _playerController;
    [SerializeField] AtmController _atmController;

    WaitForSeconds oneSecWait = new WaitForSeconds(1f);
    WaitForSeconds halfSecWait = new WaitForSeconds(.5f);

    [SerializeField] float duration;

    public void OrganizeList()
    {
        if (objects.Count == 0) return;

        _playerController.Dragging = false;

        float offSetz = 0;
        float offSety = 0;
        int i = 0;

        foreach (var obj in objects)
        {
            var objectController = obj.GetComponent<ObjectController>();

            obj.position = new Vector3(transform.position.x, transform.position.y + offSety, transform.position.z + offSetz);
            objectController.WhichColumn = this.transform;
            objectController.Row = i;

            offSetz -= 0.5f;
            offSety += +0.25f;
            i++;
        }
        StartCoroutine(MakeWave());
    }
    IEnumerator CheckList()
    {
        yield return oneSecWait;
        if (objects.Count > 1)
        {
            for (int i = objects.Count - 1; i >= 1; i--)
            {
                if (objects[i].GetComponent<ObjectController>().ObjectSO.Value == objects[i - 1].GetComponent<ObjectController>().ObjectSO.Value)
                {
                    objects[i - 1].gameObject.SetActive(false);
                    objects[i].gameObject.SetActive(false);
                    objects.Remove(objects[i]);


                    var nextGO = objects[i - 1].GetComponent<ObjectController>().ObjectSO.NextValueGameObject;

                    nextGO = Instantiate(nextGO, objects[i - 1].position, nextGO.transform.rotation);

                    nextGO.transform.parent = _spawnManager;

                    if (nextGO.GetComponent<ObjectController>() != null)
                    {
                        objects[i - 1] = nextGO.transform;
                        nextGO.transform.DOPunchScale(Vector3.one * 2, duration * 2, 1).OnComplete(OrganizeList);
                    }
                    else
                    {
                        nextGO.transform.DOMove(_atmController.Target[_atmController.Index].position, 2f, false).OnComplete(OrganizeList);
                        _atmController.Index++;
                        objects.Remove(objects[i - 1]);
                    }
                }
            }
        }
        _playerController.Dragging = true;
    }
    IEnumerator MakeWave()
    {
        for (int i = 0; i <= objects.Count - 1; i++)
        {
            objects[i].DOPunchPosition(Vector3.up / 2, duration, 1, 1f, false);
            yield return new WaitForSeconds(.05f);
        }
        StartCoroutine(CheckList());
    }
}
