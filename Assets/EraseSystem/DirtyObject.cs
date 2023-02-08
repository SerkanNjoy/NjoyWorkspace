using UnityEngine;

public class DirtyObject : MonoBehaviour
{
    [field:SerializeField] public ErasableObject TargetErasableObject { get; private set; }

    public void SetTargetErasable(ErasableObject erasable)
    {
        TargetErasableObject = erasable;
    }
}