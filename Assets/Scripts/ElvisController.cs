using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElvisController : MonoBehaviour
{
    private Vector3 _target;
    public float speed = 1.0f;
    private Camera _camera;

    // Start is called before the first frame update
    void Start()
    {
        _camera = Camera.main;
        var position = transform.position;
        SetNewTarget (new Vector3(
            position.x + 10,
            position.y,
            position.z + 10
        ));
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0) && _camera != null)
        {
            var ray = _camera.ScreenPointToRay(Input.mousePosition);
            if(Physics.Raycast(ray.origin, ray.direction, out var hitInfo))
            {
                SetNewTarget(new Vector3(hitInfo.point.x, transform.position.y, hitInfo.point.z));
            }
        }
        var direction = _target - transform.position;
        transform.Translate(direction.normalized * (speed * Time.deltaTime), Space.World);
    }

    void SetNewTarget(Vector3 newTarget)
    {
        _target = newTarget;
        transform.LookAt(_target);
    }
}

