using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class InputController : SingletonMonoBehaviorObject<InputController>
{
    private List<Transform> _selectedObjects = new List<Transform>();

    [SerializeField] private LayerMask _columnMask, _dragMask, _landMask;

    private float _dist;
    private bool _dragging = true;
    private bool _canPlay = false;

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

    DragObject _dragObject;
    SelectObject _selectObject;

    private void Awake()
    {
        SingletonThisObject(this);

        _dragObject = gameObject.AddComponent<DragObject>();
        _selectObject = gameObject.AddComponent<SelectObject>();

    }
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
            LandClick();
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

                    _selectObject.Tick(hit.collider.gameObject, _selectedObjects);

                    _dragging = true;
                }
            }
            else return;
        }

        if (_dragging && touch.phase == TouchPhase.Moved)
        {
            if (_selectedObjects.Count != 0)
            {
                v3 = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.WorldToScreenPoint(_selectedObjects[0].position).z);
                v3 = Camera.main.ScreenToWorldPoint(v3);

                _selectedObjects[0].position = new Vector3(v3.x, 1.75f, v3.z);

                _dragObject.Tick(_selectedObjects,-1.2f);
            }

        }

        if (_dragging && (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled))
        {
            if (_selectedObjects.Count == 0) return;
            ColumnController columnController = _selected.GetComponent<ObjectController>().WhichColumn.GetComponent<ColumnController>();

            if (Physics.Raycast(_selectedObjects[0].localPosition, _selectedObjects[0].TransformDirection(Vector3.down), out hit, Mathf.Infinity, _columnMask))
            {
                if (hit.collider.GetComponent<ColumnController>() != null)
                {
                    ColumnController _columnController = hit.collider.GetComponent<ColumnController>();
                    foreach (var obj in _selectedObjects)
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

            _selectedObjects.Clear();
            _selected = null;
            _dragging = false;

        }

    }


    void LandClick()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, _landMask))
        {
            var landController = hit.collider.GetComponent<LandController>();
            if (landController != null)
            {
                landController.BuyLand();
            }
        }
        else return;
    }
}


