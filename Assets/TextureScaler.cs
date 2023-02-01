using UnityEngine;

public class TextureScaler
{
    private ComputeShader _shader;

    public TextureScaler(ComputeShader shader)
    {
        _shader = shader;
    }

    public RenderTexture ScaleRT(RenderTexture source, float scaleAmount)
    {
        int sourceWidth = source.width;
        int sourceHeight = source.height;
        int resultWidth = Mathf.FloorToInt(sourceWidth * scaleAmount);
        int resulHeight = Mathf.FloorToInt(sourceHeight * scaleAmount);

        RenderTexture resultTexture = new RenderTexture(resultWidth, resulHeight, 24);
        resultTexture.enableRandomWrite = true;
        resultTexture.Create();

        _shader.SetInt("sourceWidth", sourceWidth);
        _shader.SetInt("sourceHeight", sourceHeight);
        _shader.SetInt("resultWidth", resultWidth);
        _shader.SetInt("resultHeight", resulHeight);

        _shader.SetTexture(0, "Source", source);
        _shader.SetTexture(0, "Result", resultTexture);

        int workerGroupsX = Mathf.CeilToInt(resultWidth / 8f);
        int workerGroupsY = Mathf.CeilToInt(resulHeight / 8f);

        _shader.Dispatch(0, workerGroupsX, workerGroupsY, 1);

        return resultTexture;
    }
}