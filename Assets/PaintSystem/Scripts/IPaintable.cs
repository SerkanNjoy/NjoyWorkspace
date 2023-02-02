using UnityEngine;

public interface IPaintable
{
    public Renderer Renderer { get; }

    public void OnFullyPainted();
}