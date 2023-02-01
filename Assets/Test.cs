using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Test : MonoBehaviour
{
    [SerializeField] private Camera stencilCam;
    [SerializeField] private RawImage displayImage;
    [SerializeField] private GameObject buffetSetter;
    [SerializeField] private TextureScaler scaler;

    public bool calculateCurrent = false;
    public bool calculateTarget = false;

    private int _targetWhitePixelsAmount;

    private void Awake()
    {
        stencilCam.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (calculateTarget)
        {
            calculateTarget = false;
            CalculateTargetPixels();
        }

        if (calculateCurrent)
        {
            calculateCurrent = false;
            Read();
        }
    }
    
    private void CalculateTargetPixels()
    {
        buffetSetter.SetActive(true);
        stencilCam.gameObject.SetActive(true);

        RenderTextureDescriptor descriptor = new RenderTextureDescriptor(Screen.width, Screen.height, UnityEngine.Experimental.Rendering.GraphicsFormat.R8G8B8A8_SRGB, 24);
        RenderTexture rt = new RenderTexture(descriptor);
        rt.enableRandomWrite = true;
        rt.Create();
        if (stencilCam.targetTexture != null) stencilCam.targetTexture.Release();
        stencilCam.targetTexture = rt;
        stencilCam.Render();

        RenderTexture result = scaler.ScaleRT(rt, 0.1f);

        StartCoroutine(CalculateTargetPercentage(result));

        stencilCam.gameObject.SetActive(false);
    }

    public void Read()
    {
        buffetSetter.SetActive(false);
        stencilCam.gameObject.SetActive(true);

        RenderTextureDescriptor descriptor = new RenderTextureDescriptor(Screen.width, Screen.height, UnityEngine.Experimental.Rendering.GraphicsFormat.R8G8B8A8_SRGB, 24);
        RenderTexture rt = new RenderTexture(descriptor);
        rt.enableRandomWrite = true;
        rt.Create();
        if (stencilCam.targetTexture != null) stencilCam.targetTexture.Release();
        stencilCam.targetTexture = rt;
        stencilCam.Render();

        RenderTexture result = scaler.ScaleRT(rt, 0.1f);

        StartCoroutine(CalculateCurrentPercentage(result));

        stencilCam.gameObject.SetActive(false);
    }

    private IEnumerator CalculateTargetPercentage(RenderTexture rt)
    {
        yield return new WaitForEndOfFrame();

        Texture2D resultTexture = new Texture2D(rt.width, rt.height, TextureFormat.RGB24, false);

        Rect regionToReadFrom = new Rect(0, 0, rt.width, rt.height);

        displayImage.rectTransform.sizeDelta = new Vector2(rt.width, rt.height);

        int xPosToWriteTo = 0;
        int yPosToWriteTo = 0;
        bool updateMipMapsAutomatically = false;

        RenderTexture activeRt = RenderTexture.active;
        RenderTexture.active = rt;

        resultTexture.ReadPixels(regionToReadFrom, xPosToWriteTo, yPosToWriteTo, updateMipMapsAutomatically);
        resultTexture.Apply();

        RenderTexture.active = activeRt;

        displayImage.texture = resultTexture;

        Color[] pixels = resultTexture.GetPixels();

        int step = pixels.Length;
        int whitePixelAmount = 0;
        int blackPixelAmount = 0;

        for(int i = 0; i < step; i++)
        {
            if(pixels[i] == Color.black) blackPixelAmount++;
            else if(pixels[i] == Color.white) whitePixelAmount++;
        }

        Debug.Log("Total Pixel Amount => " + step);
        Debug.Log("White Pixel Amount => " + whitePixelAmount);
        Debug.Log("Black Pixel Amount => " + blackPixelAmount);

        _targetWhitePixelsAmount = whitePixelAmount;
    }

    private IEnumerator CalculateCurrentPercentage(RenderTexture rt)
    {
        yield return new WaitForEndOfFrame();

        Texture2D resultTexture = new Texture2D(rt.width, rt.height, TextureFormat.RGB24, false);

        Rect regionToReadFrom = new Rect(0, 0, rt.width, rt.height);

        displayImage.rectTransform.sizeDelta = new Vector2(rt.width, rt.height);

        int xPosToWriteTo = 0;
        int yPosToWriteTo = 0;
        bool updateMipMapsAutomatically = false;

        RenderTexture activeRt = RenderTexture.active;
        RenderTexture.active = rt;

        resultTexture.ReadPixels(regionToReadFrom, xPosToWriteTo, yPosToWriteTo, updateMipMapsAutomatically);
        resultTexture.Apply();

        RenderTexture.active = activeRt;

        displayImage.texture = resultTexture;

        Color[] pixels = resultTexture.GetPixels();

        int step = pixels.Length;
        int whitePixelAmount = 0;
        int blackPixelAmount = 0;

        for (int i = 0; i < step; i++)
        {
            if (pixels[i] == Color.black) blackPixelAmount++;
            else if (pixels[i] == Color.white) whitePixelAmount++;
        }

        Debug.Log("Total Pixel Amount => " + step);
        Debug.Log("White Pixel Amount => " + whitePixelAmount);
        Debug.Log("Black Pixel Amount => " + blackPixelAmount);
        Debug.Log("Percentage => " + (float)whitePixelAmount / _targetWhitePixelsAmount);
    }
}