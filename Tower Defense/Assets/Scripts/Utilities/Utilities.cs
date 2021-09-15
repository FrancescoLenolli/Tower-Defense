using UnityEngine;

public static class Utilities
{
    public static SpriteRenderer CreateSpriteRenderer(Transform parent, Vector3 localPosition, Sprite sprite, Color color)
    {
        return GetSpriteRenderer(parent, localPosition, sprite, color);
    }

    private static SpriteRenderer GetSpriteRenderer(Transform parent, Vector3 localPosition, Sprite sprite, Color color)
    {
        GameObject gameObject = new GameObject("Node_Marker", typeof(SpriteRenderer));
        Transform transform = gameObject.transform;
        transform.SetParent(parent, false);
        transform.localPosition = localPosition;
        SpriteRenderer spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = sprite;
        spriteRenderer.color = color;

        return spriteRenderer;
    }
}
