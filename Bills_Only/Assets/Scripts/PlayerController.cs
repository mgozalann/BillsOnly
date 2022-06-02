using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerController : MonoBehaviour
{


    [SerializeField] List<Transform> selectedObjects = new List<Transform>();

    [SerializeField] private LayerMask _columnMask, _dragMask;

    private float _dist;

    private bool _dragging = true;
    private bool _canPlay = true;

    private Transform _selected;

    public bool Dragging
    {
        get
        {
            return _dragging;
        }
        set
        {
            _dragging = value;
        }
    }
    public bool CanPlay
    {
        get
        {
            return _canPlay;
        }
        set
        {
            _canPlay = value;
        }
    }

    RaycastHit hit;
    private void Update()
    {
        if (!_canPlay) return;

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
                _selected = hit.transform;

                ObjectController objectController = _selected.GetComponent<ObjectController>();
                if (objectController != null)
                {
                    _dist = hit.point.z - Camera.main.transform.position.z;

                    v3 = new Vector3(pos.x, pos.y, _dist);
                    v3 = Camera.main.ScreenToWorldPoint(v3);

                    Jeton(hit.collider.gameObject);

                    _dragging = true;
                }
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
            ColumnController columnController = _selected.GetComponent<ObjectController>().WhichColumn.GetComponent<ColumnController>();

            if (Physics.Raycast(selectedObjects[0].localPosition, selectedObjects[0].TransformDirection(Vector3.down), out hit, Mathf.Infinity, _columnMask))
            {
                if (hit.collider.GetComponent<ColumnController>() != null)
                {
                    ColumnController _columnController = hit.collider.GetComponent<ColumnController>();
                    foreach (var obj in selectedObjects)
                    {
                        _columnController.objects.Add(obj);
                        columnController.objects.Remove(obj);
                    }
                    _columnController.OrganizeList();
                }
            }
            else
            {
                if (_selected != null)
                {
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


