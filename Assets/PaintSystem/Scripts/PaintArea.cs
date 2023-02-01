using UnityEngine;

public abstract class PaintArea : MonoBehaviour
{
    [SerializeField] protected PaintAreaResultCam resultCam;

    [Header("Mask Settings")]
    [SerializeField] [Range(1, 255)] protected int stencilReference = 1;

    public int TargetPixelsCount { get; protected set; }

    public float Progress
    {
        get
        {
            return (float)resultCam.CalculateCurrentPixelsAmount() / TargetPixelsCount;
        }
    }

    public abstract void Init();
}