using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;

public class PaintableObjectTest : MonoBehaviour, IDigable, IPickaxable
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private GameObject[] cracks;
    [SerializeField] private GameObject circleMask;
    [SerializeField] private GameObject rock;
    [SerializeField] private SpriteRenderer boneRenderer;

    public Renderer Renderer => spriteRenderer;

    private ExtractItemState _itemState;

    private int _pickaxeCount;

    public void OnHidden()
    {
        _itemState = ExtractItemState.UnderSurface;

        foreach(var item in cracks)
        {
            item.SetActive(false);
        }
    }

    public void OnFullyDigged()
    {
        Debug.Log("Fully Painted => " + gameObject.name);
        _itemState = ExtractItemState.Digged;
        _pickaxeCount = 0;
    }

    public void Pickaxe()
    {
        if (_itemState != ExtractItemState.Digged) return;

        foreach (var item in cracks)
        {
            item.SetActive(false);
        }

        _pickaxeCount++;
        
        if(_pickaxeCount >= cracks.Length)
        {
            Debug.Log("Fully Broken => " + gameObject.name);
            _itemState = ExtractItemState.Pickaxed;

            circleMask.SetActive(false);
            rock.SetActive(false);
            boneRenderer.material.SetInt("_StencilComp", 8);
            boneRenderer.material.SetInt("_StencilOp", 0);
            return;
        }

        cracks[_pickaxeCount - 1].SetActive(true);
    }
}