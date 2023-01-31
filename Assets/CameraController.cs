using UnityEngine;
using UnityEngine.UI;

public class CameraController : MonoBehaviour
{
    [SerializeField] private RawImage rawImage;

    private void Awake()
    {
        Camera cam = GetComponent<Camera>();

        RenderTexture rt = new RenderTexture(Screen.width, Screen.height, 24);
        rt.enableRandomWrite = true;
        rt.Create();

        rawImage.rectTransform.sizeDelta = new Vector2(Screen.width, Screen.height);

        cam.targetTexture = rt;
        rawImage.texture = rt;
    }
}