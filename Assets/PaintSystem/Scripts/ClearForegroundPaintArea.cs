using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;

public class ClearForegroundPaintArea : PaintArea
{
    [SerializeField] private SpriteRenderer foreground;
    [SerializeField] private SpriteRenderer[] hiddenSprites;

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
        resultCam.Init(stencilReference);
        SetMaterials();

        StartCoroutine(WaitForEndOfFrame(() =>
        {
            TargetPixelsCount = resultCam.CalculateTargetPixelsAmount();
        }));
    }

    private void SetMaterials()
    {
        foreground.material.SetInt("_StencilRef", stencilReference);
        foreground.material.SetInt("_DiscardAlpha", 0);
        foreground.material.renderQueue = (int)RenderQueue.Transparent + 1;

        foreach(var sprite in hiddenSprites)
        {
            sprite.material.SetInt("_StencilRef", stencilReference);
            sprite.material.SetInt("_DiscardAlpha", 1);
            sprite.material.renderQueue = (int)RenderQueue.Transparent + 2;
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
        Debug.Log(Progress);
    }
}