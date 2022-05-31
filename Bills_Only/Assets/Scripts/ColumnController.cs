using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColumnController : MonoBehaviour
{
    public List<Transform> objects = new List<Transform>();

    [SerializeField] Transform spawnManager;
    public void OrganizeList()
    {
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
        StartCoroutine(CheckList());
    }
    IEnumerator CheckList()
    {
        yield return new WaitForSeconds(2f);
        for (int i = objects.Count - 1; i >= 1; i--)
        {
            if (objects[i].GetComponent<ObjectController>().ObjectSO.Value == objects[i - 1].GetComponent<ObjectController>().ObjectSO.Value)
            {
                objects[i].gameObject.SetActive(false);
                objects[i-1].gameObject.SetActive(false);
                objects.Remove(objects[i]);
                var newGameObj = Instantiate(objects[i - 1].GetComponent<ObjectController>().ObjectSO.NextValueGameObject, objects[i - 1].position, objects[i - 1].rotation);
                newGameObj.transform.parent = spawnManager;
                objects[i - 1] = newGameObj.transform;
                OrganizeList();
            }
        }
        yield return null;
    }
}
