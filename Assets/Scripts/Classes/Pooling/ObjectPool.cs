using System.Collections.Generic;
using UnityEngine;

public class ObjectPool<T> where T : MonoBehaviour, IPoolable
{
    private Queue<T> pool = new Queue<T>(); 
    private T prefab;
    private Transform parentTransform;

    public ObjectPool(T prefab, int initialSize, Transform parent = null)
    {
        this.prefab = prefab;
        this.parentTransform = parent;

        for (int i = 0; i < initialSize; i++)
        {
            T obj = GameObject.Instantiate(prefab, parent);
            obj.gameObject.SetActive(false);
            pool.Enqueue(obj);
        }
    }

    public T GetFromPool()
    {
        T obj;
        if (pool.Count > 0)
        {
            obj = pool.Dequeue();
        }
        else
        {
            obj = GameObject.Instantiate(prefab, parentTransform);
        }

        obj.gameObject.SetActive(true);
        obj.OnSpawnFromPool();
        return obj;
    }

    public void ReturnToPool(T obj)
    {
        obj.OnReturnToPool();
        obj.gameObject.SetActive(false);
        pool.Enqueue(obj);
    }
}
