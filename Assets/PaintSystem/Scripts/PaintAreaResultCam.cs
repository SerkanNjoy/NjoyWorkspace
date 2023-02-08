using UnityEngine;
using UnityEngine.Rendering;

[System.Serializable]
public class PaintAreaResultCam
{
    [SerializeField] private GameObject root;
    [SerializeField] private Camera resultCam;
    [SerializeField] private SpriteRenderer bgSprite;
    [SerializeField] private SpriteRenderer resultExposerSprite;
    [SerializeField] private SpriteRenderer bufferSetterSprite;
    [SerializeField] private ComputeShader textureScalerShader;

    public bool IsActive
    {
        set
        {
            root.SetActive(value);
        }
    }

    private RenderTexture _rt;
    private RenderTexture _scaledRt;
    private TextureScaler _textureScaler;
    private Texture2D _resultTexture;
    private Rect? _textureReadRect;
    private Color[] _pixels;

    public void Init(int StencilRef)
    {
        SetTransforms();
        SetMaterials(StencilRef);

        RenderTextureDescriptor descriptor = new RenderTextureDescriptor(Screen.width, Screen.height, UnityEngine.Experimental.Rendering.GraphicsFormat.R8G8B8A8_UNorm, 24);
        _rt = new RenderTexture(descriptor);
        _rt.enableRandomWrite = true;
        _rt.Create();
        if (resultCam.targetTexture != null) resultCam.targetTexture.Release();
        resultCam.targetTexture = _rt;
        _textureScaler = new TextureScaler(textureScalerShader);
        _textureReadRect = null;

        bufferSetterSprite.gameObject.SetActive(false);
        root.SetActive(false);
    }

    public int CalculateTargetPixelsAmount()
    {
        bufferSetterSprite.gameObject.SetActive(true);
        root.SetActive(true);

        resultCam.Render();

        _scaledRt = _textureScaler.ScaleRT(_rt, 0.1f);

        root.SetActive(false);

        return CalculatePixels();
    }

    public int CalculateCurrentPixelsAmount()
    {
        bufferSetterSprite.gameObject.SetActive(false);
        root.SetActive(true);

        resultCam.Render();

        _scaledRt = _textureScaler.ScaleRT(_rt, 0.1f);

        root.SetActive(false);

        return CalculatePixels();
    }

    private void SetTransforms()
    {
        Transform mainCamTransform = Camera.main.transform;

        root.transform.SetPositionAndRotation(mainCamTransform.position, mainCamTransform.rotation);
        resultCam.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);

        bgSprite.transform.SetLocalPositionAndRotation(new Vector3(0, 0, 10), Quaternion.identity);
        resultExposerSprite.transform.SetLocalPositionAndRotation(new Vector3(0, 0, 9), Quaternion.identity);
        bufferSetterSprite.transform.SetLocalPositionAndRotation(new Vector3(0, 0, 10), Quaternion.identity);

        bufferSetterSprite.gameObject.SetActive(false);

        SetBounds(mainCamTransform.GetComponent<Camera>());
    }

    private void SetBounds(Camera mainCam)
    {
        resultCam.orthographic = true;
        resultCam.orthographicSize = mainCam.orthographicSize;
        resultCam.clearFlags = CameraClearFlags.SolidColor;
        resultCam.backgroundColor = Color.black;
    }

    private void SetMaterials(int StencilRef)
    {
        resultExposerSprite.material.SetInt("_StencilRef", StencilRef + 2);
        resultExposerSprite.material.SetInt("_DiscardAlpha", 0);
        resultExposerSprite.material.renderQueue = (int)RenderQueue.Transparent + 3;

        bgSprite.material.SetInt("_StencilRef", StencilRef - 1);
        bgSprite.material.SetInt("_DiscardAlpha", 0);
        bgSprite.material.renderQueue = (int)RenderQueue.Transparent + 2;

        bufferSetterSprite.material.SetInt("_StencilRef", StencilRef);
        bufferSetterSprite.material.SetInt("_DiscardAlpha", 0);
        bufferSetterSprite.material.renderQueue = (int)RenderQueue.Transparent;
    }

    private int CalculatePixels()
    {
        RenderTexture activeRt = RenderTexture.active;
        RenderTexture.active = _scaledRt;

        if(_resultTexture == null) _resultTexture = new Texture2D(_scaledRt.width, _scaledRt.height, TextureFormat.RGB24, false);
        if(_textureReadRect == null) _textureReadRect = new Rect(0, 0, _scaledRt.width, _scaledRt.height);

        _resultTexture.ReadPixels((Rect)_textureReadRect, 0, 0, false);
        _resultTexture.Apply();

        RenderTexture.active = activeRt;

        _pixels = _resultTexture.GetPixels();

        int step = _pixels.Length;
        int whitePixelAmount = 0;
        int blackPixelAmount = 0;

        for (int i = 0; i < step; i++)
        {
            if (_pixels[i] == Color.black) blackPixelAmount++;
            else if (_pixels[i] == Color.white) whitePixelAmount++;
        }

        Debug.Log("Total Pixel Amount => " + step);
        Debug.Log("White Pixel Amount => " + whitePixelAmount);
        Debug.Log("Black Pixel Amount => " + blackPixelAmount);

        return whitePixelAmount;
    }
}