using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Experimental.Rendering;
using Unity.VisualScripting;

public class Test : MonoBehaviour
{
    [SerializeField] private ComputeShader changerShader;

    public MeshRenderer quadRenderer;

    public Sprite sourceSprite;

    public Text fpsText;

    public float timeBtwPerPaint = 0.05f;
    public float pixelScale;

    private Rect _sourceTextureRect;

    RenderTextureDescriptor descriptor;
    RenderTexture source;
    Material quadMaterial;

    private TextureFormat _textureFormat;

    int fpsCount;
    float fpsTimer;
    float _lastPaintTime;

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

        Initialize();
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

    public void Initialize()
    {
        _initialized = true;
        quadMaterial = quadRenderer.material;

        _sourceTextureRect = sourceSprite.textureRect;
        var sourceTex = new Texture2D((int)_sourceTextureRect.width, (int)_sourceTextureRect.height, _textureFormat, false);
        var pixels = sourceSprite.texture.GetPixels((int)_sourceTextureRect.x, (int)_sourceTextureRect.y, (int)_sourceTextureRect.width, (int)_sourceTextureRect.height);
        sourceTex.SetPixels(pixels);
        sourceTex.Apply();

        descriptor = new RenderTextureDescriptor((int)_sourceTextureRect.width, (int)_sourceTextureRect.height, GraphicsFormat.R8G8B8A8_UNorm, 24);
        source = new RenderTexture(descriptor);
        source.enableRandomWrite = true;
        source.Create();

        Graphics.Blit(sourceTex, source);

        quadRenderer.transform.localScale = new Vector3(source.width * (pixelScale * 0.1f), source.height * (pixelScale * 0.1f), 1);
        quadMaterial.SetTexture("_BaseMap", source);

        quadRenderer.transform.AddComponent<BoxCollider2D>();
    }

    public void Change(Vector2 textureCoord)
    {
        if (Time.realtimeSinceStartup - _lastPaintTime < timeBtwPerPaint) return;

        _lastPaintTime = Time.realtimeSinceStartup;

        if (!_initialized) Initialize();

        int workerGroupsX = Mathf.CeilToInt((int)_sourceTextureRect.width / 8f);
        int workerGroupsY = Mathf.CeilToInt((int)_sourceTextureRect.height / 8f);

        changerShader.SetTexture(0, "Result", source);

        changerShader.SetInt("Width", (int)_sourceTextureRect.width);
        changerShader.SetInt("Height", (int)_sourceTextureRect.height);

        changerShader.SetFloat("InputCoordX", textureCoord.x);
        changerShader.SetFloat("InputCoordY", textureCoord.y);

        changerShader.Dispatch(0, workerGroupsX, workerGroupsY, 1);

        quadMaterial.SetTexture("_BaseMap", source);
    }
}