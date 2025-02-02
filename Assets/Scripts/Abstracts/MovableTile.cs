using DG.Tweening;
using System;
using UnityEngine;

public abstract class MovableTile : Tile
{
    public virtual void MoveToCell(Cell targetCell, Action OnComplete = null)
    {
        if (targetCell == null) return;

        Vector3 newPosition = targetCell.transform.position;
        transform.DOMove(newPosition, 1f).SetEase(Ease.OutBounce).OnComplete(() =>
        {
            OnComplete?.Invoke();
        });
    }
}
