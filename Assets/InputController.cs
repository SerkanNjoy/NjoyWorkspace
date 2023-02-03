using Spine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputController : MonoBehaviour, IDragHandler
{
    [SerializeField] private Test _test;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray tempRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(tempRay.origin, tempRay.direction, 100f);
            if (hit)
            {
                Vector2 hitPoint = hit.point;
                Bounds colliderBounds = hit.collider.bounds;
                float width = colliderBounds.max.x - colliderBounds.min.x;
                float height = colliderBounds.max.y - colliderBounds.min.y;
                float xPos = (hitPoint.x - colliderBounds.min.x) / width;
                float yPos = (hitPoint.y - colliderBounds.min.y) / height;
                _test.Change(new Vector2(xPos, yPos));
            }
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        Ray tempRay = Camera.main.ScreenPointToRay(eventData.position);
        RaycastHit2D hit = Physics2D.Raycast(tempRay.origin, tempRay.direction, 100f);
        if (hit)
        {
            Vector2 hitPoint = hit.point;
            Bounds colliderBounds = hit.collider.bounds;
            float width = colliderBounds.max.x - colliderBounds.min.x;
            float height = colliderBounds.max.y - colliderBounds.min.y;
            float xPos = (hitPoint.x - colliderBounds.min.x) / width;
            float yPos = (hitPoint.y - colliderBounds.min.y) / height;
            _test.Change(new Vector2(xPos, yPos));
        }
    }
}