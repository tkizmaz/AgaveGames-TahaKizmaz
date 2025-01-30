using System.Collections.Generic;
using UnityEngine;

public class ColorManager : MonoBehaviour
{
    public static ColorManager Instance { get; private set; }

    [SerializeField]
    private Sprite red;
    [SerializeField]
    private Sprite blue;
    [SerializeField]
    private Sprite yellow;
    [SerializeField]
    private Sprite green;

    private Dictionary<TileColor, Sprite> colorSpriteDictionary;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        colorSpriteDictionary = new Dictionary<TileColor, Sprite>
        {
            { TileColor.Red, red },
            { TileColor.Blue, blue },
            { TileColor.Yellow, yellow },
            { TileColor.Green, green }
        };
    }

    public Sprite GetSprite(TileColor color)
    {
        if (!colorSpriteDictionary.ContainsKey(color))
        {
            return null;
        }

        return colorSpriteDictionary[color];
    }
}
