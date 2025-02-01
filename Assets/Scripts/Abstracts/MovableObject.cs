using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

public abstract class MovableObject : MonoBehaviour, IPoolable
{
    public abstract void SpawnObject();

    public virtual void MoveToCell(Cell targetCell, Action OnComplete = null)
    {
        Vector3 newPosition = GridManager.Instance.GetWorldPositionFromGridPosition(targetCell.GridPosition);
        transform.DOMove(newPosition, 1f).SetEase(Ease.OutBounce).OnComplete(() =>
        {
            OnComplete?.Invoke();
        });
    }

    public virtual void OnSpawnFromPool()
    {
        gameObject.SetActive(true);
    }

    public virtual void OnReturnToPool()
    {
        transform.DOKill();
        gameObject.SetActive(false);
    }
}

