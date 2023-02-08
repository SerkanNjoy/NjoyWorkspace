using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class ObjectEraser : MonoBehaviour, IDragHandler, IEndDragHandler
{
    [SerializeField] private ComputeShader textureEraserCS;

    [field:SerializeField] public bool IsEnabled { get; set; }

    private Ray _tempRay;
    private RaycastHit2D _tempHit;
    private DirtyObject _tempDirtyObject;

    public void OnDrag(PointerEventData eventData)
    {
        if (!IsEnabled) return;

        // ToDo change Camera.main code
        _tempRay = Camera.main.ScreenPointToRay(eventData.position);
        _tempHit = Physics2D.Raycast(_tempRay.origin, _tempRay.direction, 100f);
        if (_tempHit)
        {
            if(_tempHit.transform.TryGetComponent(out DirtyObject dirtyObject))
            {
                Vector2 hitPoint = _tempHit.point;
                Bounds colliderBounds = _tempHit.collider.bounds;
                float width = colliderBounds.max.x - colliderBounds.min.x;
                float height = colliderBounds.max.y - colliderBounds.min.y;
                float xPos = (hitPoint.x - colliderBounds.min.x) / width;
                float yPos = (hitPoint.y - colliderBounds.min.y) / height;

                EraseObject(dirtyObject, new Vector2(xPos, yPos));
            }
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        _tempDirtyObject.TargetErasableObject.CalculateProgress();
        _tempDirtyObject = null;
    }

    private void EraseObject(DirtyObject dirtyObject, Vector2 coords)
    {
        _tempDirtyObject = dirtyObject;
        RenderTexture erasableRT = dirtyObject.TargetErasableObject.SourceRT;

        int workerGroupsX = Mathf.CeilToInt(erasableRT.width / 8f);
        int workerGroupsY = Mathf.CeilToInt(erasableRT.height / 8f);

        textureEraserCS.SetTexture(0, "Result", erasableRT);

        textureEraserCS.SetInt("Width", erasableRT.width);
        textureEraserCS.SetInt("Height", erasableRT.height);

        textureEraserCS.SetFloat("InputCoordX", coords.x);
        textureEraserCS.SetFloat("InputCoordY", coords.y);

        textureEraserCS.Dispatch(0, workerGroupsX, workerGroupsY, 1);
    }
}