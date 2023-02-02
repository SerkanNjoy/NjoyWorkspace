using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;

public class ClearForegroundPaintArea : PaintArea
{
    [SerializeField] private SpriteRenderer foreground;
    IPaintable[] _paintables;

    public bool init = false;

    private void Update()
    {
        if (init)
        {
            init = false;
            Init();
        }
    }

    public override void Init()
    {
        SetPaintables();
        resultCam.Init(stencilReference);
        SetMaterials();

        StartCoroutine(WaitForEndOfFrame(() =>
        {
            TargetPixelsCount = resultCam.CalculateTargetPixelsAmount();
        }));
    }

    private void SetPaintables()
    {
        var components = transform.GetComponentsInChildren<IPaintable>();
        _paintables = new IPaintable[components.Length];
        for(int i = 0; i < _paintables.Length; i++)
        {
            _paintables[i] = components[i];
        }
    }

    private void SetMaterials()
    {
        foreground.material.SetInt("_StencilRef", stencilReference);
        foreground.material.SetInt("_DiscardAlpha", 0);
        foreground.material.renderQueue = (int)RenderQueue.Transparent + 1;

        foreach(var paintable in _paintables)
        {
            paintable.Renderer.material.SetInt("_StencilRef", stencilReference);
            paintable.Renderer.material.SetInt("_DiscardAlpha", 1);
            paintable.Renderer.material.renderQueue = (int)RenderQueue.Transparent + 2;
        }
    }

    private IEnumerator WaitForEndOfFrame(System.Action callback)
    {
        yield return new WaitForEndOfFrame();

        callback.Invoke();
    }

    [ContextMenu("Progress")]
    public void GetProgress()
    {
        float progress = Progress;

        Debug.Log(progress);

        if(progress >= 1.0f)
        {
            foreach(var item in _paintables)
            {
                item.OnFullyPainted();
            }
        }
    }
}