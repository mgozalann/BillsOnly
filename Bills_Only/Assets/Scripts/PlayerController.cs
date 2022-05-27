using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{


    [SerializeField] List<Transform> selectedObjects = new List<Transform>();

    private float _dist;
    private bool _dragging = false;
    private Vector3 _offSet;
    private Transform _selected;
    private Transform _pickedColumn;
    [SerializeField] private LayerMask _dragMask;
    [SerializeField] private LayerMask _columnMask;

    private Vector3 v3;

    ColumnController columnController;

    RaycastHit hit;

    private void Update()
    {
       

        if (Input.touchCount != 1)
        {
            _dragging = false;
            return;
        }

        Touch touch = Input.touches[0];
        Vector3 pos = touch.position;

        if (touch.phase == TouchPhase.Began)
        {
            Ray ray = Camera.main.ScreenPointToRay(pos);
            if (Physics.Raycast(ray, out hit, _dragMask))
            {

                _selected = hit.transform;
                //_dist = hit.point.z - Camera.main.transform.position.z;

                v3 = new Vector3(pos.x, pos.y, _dist);
                v3 = Camera.main.ScreenToWorldPoint(v3);

                Jeton(hit.collider.gameObject);

                _dragging = true;

            }
        }

        if (_dragging && touch.phase == TouchPhase.Moved)
        {
            if(selectedObjects.Count != 0)
            {
                v3 = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.WorldToScreenPoint(selectedObjects[0].position).z);
                v3 = Camera.main.ScreenToWorldPoint(v3);

                selectedObjects[0].position = new Vector3(v3.x, .3f, v3.z);

                DragJeton();            
            }
           
        }

        if (_dragging && (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled))
        {
            _selected.position = new Vector3(_selected.position.x, .15f, _selected.position.z);

            LeaveJeton();
            selectedObjects.Clear();

            _dragging = false;

        }
    }

    //genelden özele
    //ya da özelden genele
    //ray attýðýnda getcomponent kullanabilir.
    private void Jeton(GameObject selected)
    {
        if (selected.GetComponent<ObjectController>() != null)
        {
            ObjectController objectController = selected.GetComponent<ObjectController>();

            for (int i = objectController.Row; i <= objectController.WhichColumn.GetComponent<ColumnController>().objects.Count - 1; i++)
            {
                selectedObjects.Add(objectController.WhichColumn.GetComponent<ColumnController>().objects[i]);
            }
        }

    }

    private void DragJeton()
    {
       
        float offSetz = -0.5f;
        float offSety = 0.1f;

        for (int i = 0; i <= selectedObjects.Count - 2; i++)
        {
            selectedObjects[i + 1].position = new Vector3(
                selectedObjects[i].position.x,
                selectedObjects[i].position.y + offSety,
                selectedObjects[i].position.z + offSetz);
        }

        
    }

    private void LeaveJeton()
    {
        Touch touch = Input.touches[0];
        Vector3 pos = touch.position;
        Ray ray = Camera.main.ScreenPointToRay(pos);
        if (Physics.Raycast(ray, out hit, _columnMask))
        {
            Debug.Log("maske çarptý");
            if (hit.collider.GetComponent<ColumnController>() != null)
            {
                Debug.Log("içine girdi");

                ColumnController columnController = hit.collider.GetComponent<ColumnController>();
                Debug.Log(columnController.gameObject.name);
                //foreach (var obj in selectedObjects)
                //{
                //    columnController.objects.Add(obj);
                //    columnController.OrganizeList();
                //}
            }


        }
    }
}
