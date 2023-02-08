using UnityEngine;
using UnityEngine.Experimental.Rendering;
using Sero.Utilities;

public class ErasableTextureFromSprite : ErasableObject
{
    [SerializeField] private ComputeShader textureScalerCS;
    [SerializeField] private Sprite dirtySprite;
    [SerializeField] private Renderer cleanObjectRenderer;
    [SerializeField] private Mesh quadMesh;
    [SerializeField] private Material dirtyObjectMat;
    [SerializeField] private float pixelScale = 0.2f;

    private MeshRenderer _renderer;

    [ContextMenu("Init")]
    public override void Init()
    {
        Transform dirtyObject = new GameObject(transform.name + "_dirty", typeof(MeshFilter), typeof(MeshRenderer), typeof(BoxCollider2D), typeof(DirtyObject)).transform;
        dirtyObject.SetParent(transform);

        var rect = dirtySprite.textureRect;
        var sourceTex = new Texture2D((int)rect.width, (int)rect.height, TextureFormat.RGBA32, false);
        var pixels = dirtySprite.texture.GetPixels((int)rect.x, (int)rect.y, (int)rect.width, (int)rect.height);
        sourceTex.SetPixels(pixels);
        sourceTex.Apply();

        var descriptor = new RenderTextureDescriptor((int)rect.width, (int)rect.height, GraphicsFormat.R8G8B8A8_UNorm, 24);
        SourceRT = new RenderTexture(descriptor);
        SourceRT.enableRandomWrite = true;
        SourceRT.Create();

        Graphics.Blit(sourceTex, SourceRT);

        dirtyObject.GetComponent<MeshFilter>().mesh = quadMesh;
        _renderer = dirtyObject.GetComponent<MeshRenderer>();
        _renderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;

        dirtyObject.localScale = new Vector3(SourceRT.width * (pixelScale * 0.1f), SourceRT.height * (pixelScale * 0.1f), 1);
        _renderer.material = dirtyObjectMat;
        _renderer.material.SetTexture("_BaseMap", SourceRT);

        dirtyObject.GetComponent<BoxCollider2D>().isTrigger = true;
        dirtyObject.GetComponent<DirtyObject>().SetTargetErasable(this);

        
        dirtyObject.SetLocalPositionAndRotation(new Vector3(0, 0, -0.1f), Quaternion.identity);

        PixelsToErase = TexturePixelCalculator.CalculateColoredPixels(Sero.Utilities.TextureScaler.Scale(textureScalerCS, SourceRT, 0.05f));

        _renderer.material.renderQueue = cleanObjectRenderer.material.renderQueue + 1;
    }

    public override void CalculateProgress()
    {
        PixelsToErase = TexturePixelCalculator.CalculateColoredPixels(Sero.Utilities.TextureScaler.Scale(textureScalerCS, SourceRT, 0.05f));
    }
}