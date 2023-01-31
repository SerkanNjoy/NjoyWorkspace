using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class TextureScaler : MonoBehaviour
{
    [SerializeField] private Camera _camera;
    [SerializeField] private ComputeShader shader;
    [SerializeField] private RawImage scaledTextureShower;

    [SerializeField]
    [Range(0f, 1f)] private float scaleAmount;

    [ContextMenu("ScaleRT")]
    public void ScaleRT()
    {
        RenderTexture sourceTexture = _camera.targetTexture;

        int sourceWidth = sourceTexture.width;
        int sourceHeight = sourceTexture.height;
        int resultWidth = Mathf.FloorToInt(sourceWidth * scaleAmount);
        int resulHeight = Mathf.FloorToInt(sourceHeight * scaleAmount);

        RenderTexture resultTexture = new RenderTexture(resultWidth, resulHeight, 24);
        resultTexture.enableRandomWrite = true;
        resultTexture.Create();

        shader.SetInt("sourceWidth", sourceWidth);
        shader.SetInt("sourceHeight", sourceHeight);
        shader.SetInt("resultWidth", resultWidth);
        shader.SetInt("resultHeight", resulHeight);

        shader.SetTexture(0, "Source", sourceTexture);
        shader.SetTexture(0, "Result", resultTexture);

        int workerGroupsX = Mathf.CeilToInt(resultWidth / 8f);
        int workerGroupsY = Mathf.CeilToInt(resulHeight / 8f);

        shader.Dispatch(0, workerGroupsX, workerGroupsY, 1);

        scaledTextureShower.rectTransform.sizeDelta = new Vector2(resultWidth, resulHeight);
        scaledTextureShower.texture = resultTexture;

        //resultBuffer.GetData(resultData);
    }
}