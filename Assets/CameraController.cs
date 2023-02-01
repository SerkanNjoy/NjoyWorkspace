using UnityEngine;
using UnityEngine.UI;

public class CameraController : MonoBehaviour
{
    [SerializeField] private RawImage rawImage;

    private void Awake()
    {
        Camera cam = GetComponent<Camera>();

        RenderTextureDescriptor descriptor = new RenderTextureDescriptor(Screen.width, Screen.height, UnityEngine.Experimental.Rendering.GraphicsFormat.R8G8B8A8_SRGB, 24);
        RenderTexture rt = new RenderTexture(descriptor);
        rt.enableRandomWrite = true;
        rt.Create();

        rawImage.rectTransform.sizeDelta = new Vector2(Screen.width, Screen.height);

        cam.targetTexture = rt;
        rawImage.texture = rt;
    }
}