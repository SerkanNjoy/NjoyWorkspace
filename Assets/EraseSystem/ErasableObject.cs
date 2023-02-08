using UnityEngine;

public abstract class ErasableObject : MonoBehaviour
{
    public RenderTexture SourceRT { get; protected set; }

    [field:SerializeField]public int PixelsToErase { get; protected set; }

    public abstract void Init();

    public abstract void CalculateProgress();
}