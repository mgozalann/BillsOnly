using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectObject : MonoBehaviour
{
    public void Tick(GameObject selected,List<Transform> selectedObj)
    {
        if (selected.GetComponent<ObjectController>() != null)
        {
            ObjectController objectController = selected.GetComponent<ObjectController>();
            List<Transform> columnObj = objectController.WhichColumn.GetComponent<ColumnController>().objects;

            for (int i = objectController.Row; i <= columnObj.Count - 1; i++)
            {
                selectedObj.Add(columnObj[i]);
            }
        }
    }
}
