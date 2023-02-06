using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Experimental.Rendering;
using System.Collections;
using Unity.VisualScripting;

public class Test : MonoBehaviour
{
    [SerializeField] private ComputeShader changerShader;

    public SpriteRenderer spriteRenderer;
    public MeshRenderer quadRenderer;

    public Sprite sourceSprite;

    public Text fpsText;

    public float timeBtwPerPaint = 0.05f;

    public bool UseSprite = true;
    public bool SourceTexture = true;

    private Rect _sourceTextureRect;
    private Texture2D _generatedTexture;
    private Texture2D _sourceTexture;

    RenderTextureDescriptor descriptor;
    RenderTexture source;
    RenderTexture result;
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

    [ContextMenu("Init")]
    public void Initialize()
    {
        _initialized = true;
        quadMaterial = quadRenderer.material;

        _sourceTextureRect = sourceSprite.textureRect;
        _generatedTexture = new Texture2D((int)_sourceTextureRect.width, (int)_sourceTextureRect.height, _textureFormat, false);
        _sourceTexture = new Texture2D((int)_sourceTextureRect.width, (int)_sourceTextureRect.height, _textureFormat, false);
        var pixels = sourceSprite.texture.GetPixels((int)_sourceTextureRect.x, (int)_sourceTextureRect.y, (int)_sourceTextureRect.width, (int)_sourceTextureRect.height);
        _generatedTexture.SetPixels(pixels);
        _sourceTexture.SetPixels(pixels);
        _generatedTexture.Apply();
        _sourceTexture.Apply();

        descriptor = new RenderTextureDescriptor((int)_sourceTextureRect.width, (int)_sourceTextureRect.height, GraphicsFormat.R8G8B8A8_UNorm, 24);
        source = new RenderTexture(descriptor);
        source.enableRandomWrite = true;
        source.Create();

        result = new RenderTexture(descriptor);
        result.enableRandomWrite = true;
        result.Create();

        quadRenderer.transform.localScale = spriteRenderer.bounds.size;
        quadMaterial.SetTexture("_BaseMap", _generatedTexture);

        quadRenderer.transform.AddComponent<BoxCollider2D>();
    }

    public void Change(Vector2 textureCoord)
    {
        if (Time.realtimeSinceStartup - _lastPaintTime < timeBtwPerPaint) return;

        _lastPaintTime = Time.realtimeSinceStartup;

        if (!_initialized) Initialize();

        StartCoroutine(WaitForFrame(textureCoord));

        return;

        Graphics.Blit(_sourceTexture, source);

        int workerGroupsX = Mathf.CeilToInt((int)_sourceTextureRect.width / 8f);
        int workerGroupsY = Mathf.CeilToInt((int)_sourceTextureRect.height / 8f);

        changerShader.SetTexture(0, "Source", source);
        changerShader.SetTexture(0, "Result", result);

        changerShader.SetInt("Width", (int)_sourceTextureRect.width);
        changerShader.SetInt("Height", (int)_sourceTextureRect.height);

        changerShader.SetFloat("InputCoordX", textureCoord.x);
        changerShader.SetFloat("InputCoordY", textureCoord.y);

        changerShader.Dispatch(0, workerGroupsX, workerGroupsY, 1);

        RenderTexture.active = result;

        //_generatedTexture = new Texture2D(result.width, result.height, _textureFormat, false);
        _generatedTexture.ReadPixels(new Rect(0, 0, result.width, result.height), 0, 0, false);
        _generatedTexture.Apply();

        RenderTexture.active = null;

        if (UseSprite)
        {
            spriteRenderer.enabled = true;
            quadRenderer.enabled = false;
            spriteRenderer.sprite = Sprite.Create(_generatedTexture, new Rect(0, 0, _generatedTexture.width, _generatedTexture.height), new Vector2(0.5f, 0.5f));
        }
        else
        {
            spriteRenderer.enabled = false;
            quadRenderer.enabled = true;
            quadMaterial.SetTexture("_BaseMap", _generatedTexture);
        }
    }

    private IEnumerator WaitForFrame(Vector2 textureCoord)
    {
        
        if(SourceTexture) Graphics.Blit(_sourceTexture, source);
        else Graphics.Blit(_generatedTexture, source);




        int workerGroupsX = Mathf.CeilToInt((int)_sourceTextureRect.width / 8f);
        int workerGroupsY = Mathf.CeilToInt((int)_sourceTextureRect.height / 8f);

        changerShader.SetTexture(0, "Source", source);
        changerShader.SetTexture(0, "Result", result);

        changerShader.SetInt("Width", (int)_sourceTextureRect.width);
        changerShader.SetInt("Height", (int)_sourceTextureRect.height);

        changerShader.SetFloat("InputCoordX", textureCoord.x);
        changerShader.SetFloat("InputCoordY", textureCoord.y);

        changerShader.Dispatch(0, workerGroupsX, workerGroupsY, 1);

        RenderTexture.active = result;

        //_generatedTexture = new Texture2D(result.width, result.height, _textureFormat, false);
        _generatedTexture.ReadPixels(new Rect(0, 0, result.width, result.height), 0, 0, false);
        _generatedTexture.Apply();

        RenderTexture.active = null;

        yield return new WaitForEndOfFrame();

        if (UseSprite)
        {
            spriteRenderer.enabled = true;
            quadRenderer.enabled = false;
            spriteRenderer.sprite = Sprite.Create(_generatedTexture, new Rect(0, 0, _generatedTexture.width, _generatedTexture.height), new Vector2(0.5f, 0.5f));
        }
        else
        {
            spriteRenderer.enabled = false;
            quadRenderer.enabled = true;
            quadMaterial.SetTexture("_BaseMap", _generatedTexture);
        }
    }
}