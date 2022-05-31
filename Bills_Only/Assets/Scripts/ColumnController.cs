using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ColumnController : MonoBehaviour
{
    public List<Transform> objects = new List<Transform>();

    [SerializeField] Transform _spawnManager;
    [SerializeField] PlayerController _playerController;
    [SerializeField] Transform[] target;

    [SerializeField] float duration;

    int j = 0;
    public void OrganizeList()
    {
        if(objects.Count == 0) return;

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
        yield return new WaitForSeconds(1f);
        for (int i = objects.Count - 1; i >= 1; i--)
        {
            if (objects[i].GetComponent<ObjectController>().ObjectSO.Value == objects[i - 1].GetComponent<ObjectController>().ObjectSO.Value)
            {
                objects[i - 1].gameObject.SetActive(false);
                objects[i].gameObject.SetActive(false);
                objects.Remove(objects[i]);

                var nextGO = objects[i - 1].GetComponent<ObjectController>().ObjectSO.NextValueGameObject;
                
                //çýkardýðý i ile ayný olduðu için tekrar yapýyor.

                nextGO = Instantiate(nextGO, objects[i - 1].position, nextGO.transform.rotation);
                nextGO.transform.parent = _spawnManager;

                if (nextGO.GetComponent<ObjectController>() != null)
                {
                    objects[i - 1] = nextGO.transform;
                }
                else
                {
                    objects.Remove(objects[i-1]);
                    nextGO.transform.DOMove(target[j].position, 2f, false);
                    j++;
                }

                OrganizeList();
            }
        }
        _playerController.Dragging = true;
    }

    IEnumerator MakeWave()
    {
        for (int i = 0; i <= objects.Count - 1; i++)
        {
            objects[i].DOPunchPosition(Vector3.up/2, duration, 1, 1f, false);
            yield return new WaitForSeconds(.05f);
        }
        StartCoroutine(CheckList());
    }
}
