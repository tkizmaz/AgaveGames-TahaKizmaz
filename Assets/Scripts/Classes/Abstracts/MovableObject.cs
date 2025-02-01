using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public abstract class MovableObject : MonoBehaviour, IPoolable
{
    public abstract void SpawnObject(); // Her nesne kendine özgü spawn olacak

    public virtual void MoveToCell(Cell targetCell)
    {
        Vector3 newPosition = GridManager.Instance.GetWorldPositionFromGridPosition(targetCell.GridPosition);
        transform.DOMove(newPosition, 0.3f).SetEase(Ease.OutBounce);
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

