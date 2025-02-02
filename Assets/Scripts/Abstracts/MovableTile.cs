using UnityEngine;
using DG.Tweening;
using System;

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

    public override void SpawnObject()
    {
        base.SpawnObject();
    }
}
