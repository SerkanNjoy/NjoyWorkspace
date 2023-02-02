using SeroJob.ObjectPooling;
using UnityEngine;

public class ShovelMaskPool
{
    private ObjectPool<Transform> _maskPool;

    public ShovelMaskPool(GameObject maskPref)
    {
        GameObject poolHolder = new GameObject("shovelMaskPool");

        _maskPool = new ObjectPool<Transform>(maskPref, poolHolder.transform, 50);
    }

    public void SetMaskAtPoint(Vector3 position, float size = 1f)
    {
        var mask = _maskPool.Pull().transform;
        mask.localScale = Vector3.one * size;
        mask.position = position;
        mask.gameObject.SetActive(true);
    }

    /*public void CoverBounds(Vector3 worldPos, Bounds bounds)
    {
        Vector3 size = bounds.size;
        Vector3 maskSize = new Vector3(1.5f, 1.5f, 1);
        Vector3 startPos = worldPos - new Vector3(0, size.y * 0.5f, 0) + new Vector3(0, maskSize.y * 0.5f, 0);

        int xStep = Mathf.CeilToInt(size.x / maskSize.x);
        int yStep = Mathf.CeilToInt(size.y / maskSize.y);

        for(int i = 0; i < xStep; i++)
        {
            SetMaskAtPoint(startPos + new Vector3(maskSize.x * i, 0, 0));

            for (int j = 0; j < yStep; j++)
            {
                SetMaskAtPoint(startPos + new Vector3(0, maskSize.y * j, 0));
            }
        }
    }*/
}