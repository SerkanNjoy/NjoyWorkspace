using UnityEngine;
using UnityEngine.UIElements;

public static class PhysicUtilities
{
    private static Ray _tempRay;
    private static RaycastHit _tempRaycastHit;

    private static Camera _mainCam;

    public static T GetInteractableOnRayPoint<T>(Vector3 position, LayerMask layerMask)
    {
        if (_mainCam == null) _mainCam = Camera.main;

        _tempRay = _mainCam.ScreenPointToRay(position);
        if (Physics.Raycast(_tempRay, out _tempRaycastHit, float.MaxValue, layerMask))
            return _tempRaycastHit.collider.gameObject.GetComponent<T>();
        else return default;
    }

    public static T GetInteractableOnRayPoint<T>(Vector3 position)
    {
        if (_mainCam == null) _mainCam = Camera.main;

        _tempRay = _mainCam.ScreenPointToRay(position);
        if (Physics.Raycast(_tempRay, out _tempRaycastHit, float.MaxValue))
            return _tempRaycastHit.collider.gameObject.GetComponent<T>();
        else return default;
    }

    public static bool TryGetInteractableOnRayPoint<T>(Vector3 position, out T result)
    {
        if (_mainCam == null) _mainCam = Camera.main;

        _tempRay = _mainCam.ScreenPointToRay(position);

        if (Physics.Raycast(_tempRay, out _tempRaycastHit, float.MaxValue))
        {
            result = _tempRaycastHit.collider.gameObject.GetComponent<T>();
            return true;
        }

        result = default;
        return false;
    }

    public static bool TryGetInteractableOnRayPoint<T>(Vector3 position, out T result, LayerMask layerMask)
    {
        if (_mainCam == null) _mainCam = Camera.main;

        _tempRay = _mainCam.ScreenPointToRay(position);
        if (Physics.Raycast(_tempRay, out _tempRaycastHit, float.MaxValue, layerMask))
        {
            result = _tempRaycastHit.collider.gameObject.GetComponent<T>();
            return true;
        }

        result = default;
        return false;
    }

    public static bool TryGetPointOnRayPoint(Vector3 screenPos, out Vector3 result, LayerMask layerMask)
    {
        if (_mainCam == null) _mainCam = Camera.main;

        _tempRay = _mainCam.ScreenPointToRay(screenPos);
        if (Physics.Raycast(_tempRay, out _tempRaycastHit, float.MaxValue, layerMask))
        {
            result = _tempRaycastHit.point;
            return true;
        }

        result = default;
        return false;
    }

    public static bool TryGetInteractableOnRayPoint2D<T>(Vector3 position, out T result)
    {
        if (_mainCam == null) _mainCam = Camera.main;

        _tempRay = _mainCam.ScreenPointToRay(position);

        RaycastHit2D hit = Physics2D.Raycast(_tempRay.origin, _tempRay.direction, 200f);

        if (hit)
        {
            if(hit.collider.gameObject.TryGetComponent(out T component))
            {
                result = component;
                return true;
            }
        }

        result = default;
        return false;
    }
}