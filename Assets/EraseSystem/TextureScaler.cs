using UnityEngine;

namespace Sero.Utilities
{
    public static class TextureScaler
    {
        public static Texture2D Scale(ComputeShader scaleCS, RenderTexture source, float scaleAmount)
        {
            int sourceWidth = source.width;
            int sourceHeight = source.height;
            int resultWidth = Mathf.FloorToInt(sourceWidth * scaleAmount);
            int resulHeight = Mathf.FloorToInt(sourceHeight * scaleAmount);

            RenderTexture resultTexture = new RenderTexture(resultWidth, resulHeight, 24);
            resultTexture.enableRandomWrite = true;
            resultTexture.Create();

            scaleCS.SetInt("sourceWidth", sourceWidth);
            scaleCS.SetInt("sourceHeight", sourceHeight);
            scaleCS.SetInt("resultWidth", resultWidth);
            scaleCS.SetInt("resultHeight", resulHeight);

            scaleCS.SetTexture(0, "Source", source);
            scaleCS.SetTexture(0, "Result", resultTexture);

            int workerGroupsX = Mathf.CeilToInt(resultWidth / 8f);
            int workerGroupsY = Mathf.CeilToInt(resulHeight / 8f);

            scaleCS.Dispatch(0, workerGroupsX, workerGroupsY, 1);

            var previous = RenderTexture.active;

            RenderTexture.active = resultTexture;

            Texture2D result = new Texture2D(resultWidth, resulHeight, TextureFormat.RGBA32, false);
            result.ReadPixels(new Rect(0, 0, result.width, result.height), 0, 0);
            result.Apply();

            RenderTexture.active = previous;

            return result;
        }
    }
}