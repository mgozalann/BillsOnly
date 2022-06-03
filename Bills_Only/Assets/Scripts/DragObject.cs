using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragObject : MonoBehaviour
{ 
    public void Tick(List<Transform> selectedObj)
    {
        float offSetz = -0.5f;
        float offSety = 0.1f;

        for (int i = 0; i <= selectedObj.Count - 2; i++)
        {
            selectedObj[i + 1].position = new Vector3(
                selectedObj[i].position.x,
                selectedObj[i].position.y + offSety,
                selectedObj[i].position.z + offSetz);
        }
    }
}
