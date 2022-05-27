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
    [SerializeField] LayerMask _dragMask;
    [SerializeField] LayerMask _columnMask;

    ColumnController _columnController;

    RaycastHit hit;

    private void Update()
    {
        Vector3 v3;

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
                if (!hit.collider.CompareTag("Drag")) return;

                Debug.Log(hit.collider.name);
                _selected = hit.transform;

                _dist = hit.point.z - Camera.main.transform.position.z;
                v3 = new Vector3(pos.x, pos.y, _dist);
                v3 = Camera.main.ScreenToWorldPoint(v3);

                Jeton(hit.collider.gameObject);

                _dragging = true;


            }
            else return;
        }

        if (_dragging && touch.phase == TouchPhase.Moved)
        {
            if (selectedObjects.Count != 0)
            {

                v3 = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.WorldToScreenPoint(selectedObjects[0].position).z);
                v3 = Camera.main.ScreenToWorldPoint(v3);

                selectedObjects[0].position = new Vector3(v3.x, .3f, v3.z);

                DragJeton();
            }

        }

        if (_dragging && (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled))
        {
            if (selectedObjects.Count == 0) return;

            if (Physics.Raycast(selectedObjects[0].position,Vector3.down,100f,_columnMask))
            {
                Debug.DrawRay(selectedObjects[0].position, Vector3.down,Color.green);
                if (hit.collider.GetComponent<ColumnController>() != null)
                {

                    ColumnController columnController = hit.collider.GetComponent<ColumnController>();

                    foreach (var obj in selectedObjects)
                    {

                        columnController.objects.Add(obj);
                        _selected.position = new Vector3(_selected.position.x, .15f, _selected.position.z);

                    }
                    columnController.OrganizeList();
                }
            }
            else
            {
                if (_selected != null)
                {

                    ColumnController columnController = _selected.GetComponent<ObjectController>().WhichColumn.GetComponent<ColumnController>();
                    for (int i = 0; i <= selectedObjects.Count - 1; i++)
                    {
                        columnController.objects.Add(selectedObjects[i]);
                    }
                    columnController.OrganizeList();

                }
            }

            selectedObjects.Clear();
            _selected = null;
            _dragging = false;

        }
    }
    private void Jeton(GameObject selected)
    {
        if (selected.GetComponent<ObjectController>() != null)
        {
            ObjectController objectController = selected.GetComponent<ObjectController>();
            List<Transform> columnObj = objectController.WhichColumn.GetComponent<ColumnController>().objects;

            for (int i = objectController.Row; i <= columnObj.Count - 1; i++)
            {
                selectedObjects.Add(columnObj[i]);
                columnObj.Remove(columnObj[i]);
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
}
