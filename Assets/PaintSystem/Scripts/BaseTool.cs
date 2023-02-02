using UnityEngine;

public abstract class BaseTool : MonoBehaviour
{
    public virtual bool IsUsable { get { return true; } }

    public abstract void Init();
    public abstract void Select();
    public abstract void Deselect();
    public abstract void Use();
}