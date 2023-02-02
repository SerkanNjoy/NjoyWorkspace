using UnityEngine;

public class ShovelMask : MonoBehaviour
{
    [SerializeField] private Sprite[] sprites;
    [SerializeField] private SpriteRenderer spriteRenderer;

    private void OnEnable()
    {
        spriteRenderer.sprite = sprites[Random.Range(0, 4)];
    }
}