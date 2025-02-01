using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

public abstract class MovableObject : MonoBehaviour, IPoolable
{
    public abstract void SpawnObject(bool shouldMoveOnSpawn = false, Cell targetCell = null, Action OnComplete = null);

    public virtual void MoveToCell(Cell targetCell, Action OnComplete = null)
    {
        if (targetCell == null) return;

        Vector3 newPosition = GridManager.Instance.GetWorldPositionFromGridPosition(targetCell.GridPosition);
        transform.DOMove(newPosition, 1f).SetEase(Ease.OutBounce).OnComplete(() =>
        {
            OnComplete?.Invoke();
        });
    }

    public virtual void OnSpawnFromPool()
    {
        SpawnObject();
    }

    public virtual void OnReturnToPool()
    {
        transform.DOKill();
        gameObject.SetActive(false);
    }
}
