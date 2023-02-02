using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaintableObjectTest : MonoBehaviour, IPaintable
{
    [SerializeField] private SpriteRenderer spriteRenderer;

    public Renderer Renderer => spriteRenderer;

    public void OnFullyPainted()
    {
        Debug.Log("Fully Painted => " + gameObject.name);
    }
}