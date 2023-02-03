using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Experimental.Rendering;

public class Test : MonoBehaviour
{
    [SerializeField] private ComputeShader changerShader;

    public SpriteRenderer spriteRenderer;

    public Sprite sourceSprite;

    public Text fpsText;

    public float timeBtwPerPaint = 0.05f;

    private Rect _resultRect;
    private Texture2D _resultTexture;

    RenderTextureDescriptor descriptor;
    RenderTexture source;
    RenderTexture result;

    private TextureFormat _textureFormat;

    int fpsCount;
    float fpsTimer;
    float _lastPaintTime;
    Color[] _pixels;

    bool _initialized;

    private void Awake()
    {
        _initialized = false;
        _textureFormat = TextureFormat.RGBA32;

        Debug.Log("Is Graphic Format Supported => " + SystemInfo.IsFormatSupported(GraphicsFormat.R8G8B8A8_UNorm, FormatUsage.Render));
        Debug.Log("Is" + _textureFormat + "Texture Format Supported => " + SystemInfo.SupportsTextureFormat(_textureFormat));

        _lastPaintTime = Time.realtimeSinceStartup;
    }

    private void Start()
    {
        Debug.Log("Processor Count => " + SystemInfo.processorCount);

        GenerateData();
    }

    private void Update()
    {
        fpsTimer += Time.deltaTime;
        fpsCount++;
        if(fpsTimer >= 1f)
        {
            fpsText.text = "FPS: " + fpsCount;
            fpsTimer = 0f;
            fpsCount = 0;
        }
    }

    public void GenerateData()
    {
        _resultRect = sourceSprite.textureRect;
        _resultTexture = new Texture2D((int)_resultRect.width, (int)_resultRect.height, _textureFormat, false);
        _pixels = sourceSprite.texture.GetPixels((int)_resultRect.x, (int)_resultRect.y, (int)_resultRect.width, (int)_resultRect.height);
        _resultTexture.SetPixels(_pixels);
        _resultTexture.Apply();

        descriptor = new RenderTextureDescriptor((int)_resultRect.width, (int)_resultRect.height, GraphicsFormat.R8G8B8A8_UNorm, 24);
        source = new RenderTexture(descriptor);
        source.enableRandomWrite = true;
        source.Create();

        result = new RenderTexture(descriptor);
        result.enableRandomWrite = true;
        result.Create();
    }

    public void Change(Vector2 textureCoord)
    {
        if (Time.realtimeSinceStartup - _lastPaintTime < timeBtwPerPaint) return;

        _lastPaintTime = Time.realtimeSinceStartup;

        if (!_initialized)
        {
            _initialized = true;
            _resultRect = sourceSprite.textureRect;
            _resultTexture = new Texture2D((int)_resultRect.width, (int)_resultRect.height, _textureFormat, false);
            _pixels = sourceSprite.texture.GetPixels((int)_resultRect.x, (int)_resultRect.y, (int)_resultRect.width, (int)_resultRect.height);
            _resultTexture.SetPixels(_pixels);
            _resultTexture.Apply();
        }

        var sourceRect = sourceSprite.textureRect;
        var resultTexture = new Texture2D((int)sourceRect.width, (int)sourceRect.height, _textureFormat, false);
        resultTexture.SetPixels(_resultTexture.GetPixels());
        resultTexture.Apply();

        Graphics.Blit(resultTexture, source);

        int workerGroupsX = Mathf.CeilToInt((int)_resultRect.width / 8f);
        int workerGroupsY = Mathf.CeilToInt((int)_resultRect.height / 8f);

        changerShader.SetTexture(0, "Source", source);
        changerShader.SetTexture(0, "Result", result);

        changerShader.SetInt("Width", (int)_resultRect.width);
        changerShader.SetInt("Height", (int)_resultRect.height);

        changerShader.SetFloat("InputCoordX", textureCoord.x);
        changerShader.SetFloat("InputCoordY", textureCoord.y);

        changerShader.Dispatch(0, workerGroupsX, workerGroupsY, 1);

        RenderTexture.active = result;

        _resultTexture = new Texture2D(result.width, result.height, _textureFormat, false);
        _resultTexture.ReadPixels(new Rect(0, 0, result.width, result.height), 0, 0, false);
        _resultTexture.Apply();

        RenderTexture.active = null;

        spriteRenderer.sprite = Sprite.Create(_resultTexture, new Rect(0, 0, _resultTexture.width, _resultTexture.height), new Vector2(0.5f, 0.5f));
    }
}