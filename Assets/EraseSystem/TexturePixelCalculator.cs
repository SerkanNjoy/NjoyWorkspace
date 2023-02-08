using UnityEngine;

namespace Sero.Utilities
{
    public static class TexturePixelCalculator
    {
        static Color[] _pixels;

        public static int CalculateTransparentPixels(Texture2D source)
        {
            _pixels = source.GetPixels();
            int pixelCount = 0;
            int spte = _pixels.Length;

            for(int i = 0; i < spte; i++)
            {
                if(_pixels[i].a == 0)
                {
                    pixelCount++;
                }
            }

            return pixelCount;
        }

        public static int CalculateColoredPixels(Texture2D source)
        {
            _pixels = source.GetPixels();
            int pixelCount = 0;
            int spte = _pixels.Length;

            for (int i = 0; i < spte; i++)
            {
                if (_pixels[i].a != 0)
                {
                    pixelCount++;
                }
            }

            return pixelCount;
        }
    }
}