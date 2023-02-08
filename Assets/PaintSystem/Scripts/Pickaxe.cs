using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickaxe : BaseTool
{
    private bool _ready = false;

    [ContextMenu("Init")]
    public override void Init()
    {
        _ready = true;
    }

    public override void Select()
    {

    }

    public override void Deselect()
    {
        
    }

    public override void Use()
    {
        
    }

    private void Update()
    {
        if (!_ready) return;

        if (Input.GetMouseButtonDown(0))
        {
            if(PhysicUtilities.TryGetInteractableOnRayPoint2D(Input.mousePosition, out IPickaxable result))
            {
                result.Pickaxe();
            }
        }
    }
}