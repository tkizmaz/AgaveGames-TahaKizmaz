using UnityEngine;

public class Chip : MovableTile
{
    public override void SpawnObject()
    {
        base.SpawnObject();
    }

    public override void OnTileDestroyed()
    {
        //Can be overridden
    }
}
