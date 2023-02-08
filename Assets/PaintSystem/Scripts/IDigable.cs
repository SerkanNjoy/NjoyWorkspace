using UnityEngine;

public interface IDigable
{
    public Renderer Renderer { get; }

    public void OnHidden();
    public void OnFullyDigged();
}