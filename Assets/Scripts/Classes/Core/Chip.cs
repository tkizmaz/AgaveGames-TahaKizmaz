using UnityEngine;
public class Chip : MovableTile, ISelectable
{
    public void SetSelected(bool isSelected)
    {
        if (spriteRenderer != null)
        {
            spriteRenderer.color = isSelected ? Color.gray : Color.white;
        }
    }

    public override void OnReturnToPool()
    {
        SetSelected(false);
        base.OnReturnToPool();
    }
}
