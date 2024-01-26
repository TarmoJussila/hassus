using System.Collections.Generic;
using UnityEngine;

public class MapLoader : MonoBehaviour
{
    [Header("Map References")]
    [SerializeField] private TextAsset mapTextAsset;
    
    private Dictionary<Vector2Int, BoxCollider2D> mapColliders = new();
    
    private readonly int mapWidth = 32;
    private readonly int mapHeight = 32;
    private readonly string mapEmptySymbol = ".";
    private readonly string mapBlockSymbol = "#";
    
    private void Start()
    {
        GenerateMapColliders();
    }
    
    private void GenerateMapColliders()
    {
        var map = MapTextAssetToList(mapTextAsset);

        for (int i = 0; i < mapWidth; i++)
        {
            for (int j = 0; j < mapHeight; j++)
            {
                string symbol = map[j][i].ToString();

                if (symbol == mapEmptySymbol)
                {
                    continue;
                }
                else if (symbol == mapBlockSymbol)
                {
                    var boxCollider = Instantiate(new GameObject(), new Vector2(i, j), Quaternion.identity, transform).AddComponent<BoxCollider2D>();

                    mapColliders.Add(new Vector2Int(i, j), boxCollider);
                }
            }
        }
    }
    
    private List<string> MapTextAssetToList(TextAsset mapTextAsset)
    {
        var list = new List<string>();
        var lineArray = mapTextAsset.text.Split('\n');
        foreach (var line in lineArray)
        {
            list.Add(line);
        }
        return list;
    }
    
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        var map = MapTextAssetToList(mapTextAsset);
        
        for (int i = 0; i < mapWidth; i++)
        {
            for (int j = 0; j < mapHeight; j++)
            {
                string symbol = map[j][i].ToString();

                if (symbol == mapEmptySymbol)
                {
                    continue;
                }
                else if (symbol == mapBlockSymbol)
                {
                    Gizmos.DrawCube(new Vector2(i, j), Vector3.one);
                }
            }
        }
    }
}
