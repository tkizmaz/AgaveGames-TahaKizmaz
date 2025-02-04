using DG.Tweening;
using System;
using UnityEngine;

public abstract class MovableTile : Tile
{
    public virtual void MoveToCell(Cell targetCell, Action OnComplete = null)
    {
        if (targetCell == null) return;
    
        Vector3 newPosition = targetCell.transform.position;
        float distance = Vector3.Distance(transform.position, newPosition);
        float moveTime = Mathf.Sqrt(distance) * 0.5f;
        transform.DOMove(newPosition, moveTime).SetEase(Ease.OutBounce).OnComplete(() =>
        {
            OnComplete?.Invoke();
        });
    }
}
