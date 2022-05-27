using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dragger : MonoBehaviour
{
    private float _dist;
    private bool _dragging = false;
    private Vector3 _offSet;
    private Transform _toDrag;
    RaycastHit hit;

    private void Update()
    {
        Vector3 v3;

        if(Input.touchCount != 1)
        {
            _dragging = false;

            return;
        }

        Touch touch = Input.touches[0];
        Vector3 pos = touch.position;

        if(touch.phase == TouchPhase.Began)
        {
            Ray ray = Camera.main.ScreenPointToRay(pos);

            if(Physics.Raycast(ray, out hit ))
            {
                if (hit.collider.CompareTag("Drag"))
                {
                    _toDrag = hit.transform;
                    _dist = hit.point.z - Camera.main.transform.position.z;
                    v3 = new Vector3(pos.x, pos.y, _dist);
                    v3 = Camera.main.ScreenToWorldPoint(v3);
                    _offSet = _toDrag.position - v3;
                    _dragging = true;
                }
            }
        }

        if(_dragging && touch.phase == TouchPhase.Moved)
        {

            v3 = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.WorldToScreenPoint(_toDrag.position).z);
            v3 = Camera.main.ScreenToWorldPoint(v3);
            _toDrag.position = new Vector3(v3.x, .3f, v3.z);
        }

        if(_dragging && (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled))
        {
            _toDrag.position = new Vector3(_toDrag.position.x, 0 , _toDrag.position.z);
            _dragging = false;


        }
    }
}
