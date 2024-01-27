using System.Collections.Generic;
using UnityEngine;

namespace Hassus.Map
{
    public class MapLoader : MonoBehaviour
    {
        [Header("Map References")]
        [SerializeField] private TextAsset[] mapTextAssets;
        [SerializeField] private MapBlock mapBlockPrefab;
        [SerializeField] private Texture2D[] treeTextures;

        [Header("Map Settings")]
        [SerializeField] [Range(0, 1.0f)] private float foliageChance = 0.5f;
        [SerializeField] [Range(1, 9)] private int foliageHeightSpace = 2;
        [SerializeField] [Range(0, 1.0f)] private float pieceChance = 0.5f;
        
        private Dictionary<Vector2Int, MapBlock> mapBlocks = new();
        private Dictionary<Vector2Int, BoxCollider2D> mapColliders = new();

        private int randomMapIndex = 0;
        
        private readonly int mapWidth = 32;
        private readonly int mapHeight = 32;
        private readonly string mapEmptySymbol = ".";
        private readonly string mapBlockSymbol = "#";
        
        private void Start()
        {
            GenerateMap();
        }

        private void Update()
        {
            #if UNITY_EDITOR
            if (Input.GetKeyDown(KeyCode.R))
            {
                DestroyMap();
                GenerateMap();
            }
            #endif
        }

        private void DestroyMap()
        {
            for (int i = 0; i < mapWidth; i++)
            {
                for (int j = 0; j < mapHeight; j++)
                {
                    mapBlocks.TryGetValue(new Vector2Int(i, j), out var mapBlock);
                    if (mapBlock != null)
                    {
                        Destroy(mapBlock.gameObject);
                    }
                    mapColliders.TryGetValue(new Vector2Int(i, j), out var mapCollider);
                    if (mapCollider != null)
                    {
                        Destroy(mapCollider.gameObject);
                    }
                }
            }
            mapBlocks.Clear();
            mapColliders.Clear();
        }

        private void GenerateMap()
        {
            randomMapIndex = Random.Range(0, mapTextAssets.Length);
            var map = MapTextAssetToList(mapTextAssets[randomMapIndex]);

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
                        var mapBlock = Instantiate(mapBlockPrefab, new Vector2(i, j), Quaternion.Euler(0, -90f, 0), transform);
                        
                        var boxCollider = new GameObject().AddComponent<BoxCollider2D>();
                        boxCollider.transform.position = new Vector2(i, j);
                        boxCollider.transform.parent = transform;
                        
                        mapBlocks.Add(new Vector2Int(i, j), mapBlock);
                        mapColliders.Add(new Vector2Int(i, j), boxCollider);
                    }
                }
            }
            
            for (int i = 0; i < mapWidth; i++)
            {
                for (int j = 0; j < mapHeight; j++)
                {
                    int topEmptySpace = 0;
                    for (int k = 1; k <= foliageHeightSpace; k++)
                    {
                        mapBlocks.TryGetValue(new Vector2Int(i, j + k), out var upperMapBlock);
                        if (upperMapBlock == null)
                        {
                            topEmptySpace++;
                        }
                        else
                        {
                            break;
                        }
                    }
                    
                    mapBlocks.TryGetValue(new Vector2Int(i, j), out var mapBlock);
                    if (mapBlock != null)
                    {
                        MapBlockGroup adjacentMapBlockGroup = new MapBlockGroup();
                        for (int x = -1; x <= 1; x++)
                        {
                            for (int y = -1; y <= 1; y++)
                            {
                                mapBlocks.TryGetValue(new Vector2Int(i + x, j + y), out var adjacentMapBlock);
                                if (adjacentMapBlock != null)
                                {
                                    if (x == -1 && y ==  1) adjacentMapBlockGroup.TopLeftBlock = adjacentMapBlock;
                                    if (x ==  0 && y ==  1) adjacentMapBlockGroup.TopBlock = adjacentMapBlock;
                                    if (x ==  1 && y ==  1) adjacentMapBlockGroup.TopRightBlock = adjacentMapBlock;
                                    if (x == -1 && y ==  0) adjacentMapBlockGroup.LeftBlock = adjacentMapBlock;
                                    if (x ==  0 && y ==  0) adjacentMapBlockGroup.CenterBlock = adjacentMapBlock;
                                    if (x ==  1 && y ==  0) adjacentMapBlockGroup.RightBlock = adjacentMapBlock;
                                    if (x == -1 && y == -1) adjacentMapBlockGroup.BottomLeftBlock = adjacentMapBlock;
                                    if (x ==  0 && y == -1) adjacentMapBlockGroup.BottomBlock = adjacentMapBlock;
                                    if (x ==  1 && y == -1) adjacentMapBlockGroup.BottomRightBlock = adjacentMapBlock;
                                }
                            }
                        }
                        
                        InitializeMapBlock(mapBlock, adjacentMapBlockGroup, topEmptySpace);
                    }
                }
            }
        }

        private void InitializeMapBlock(MapBlock mapBlock, MapBlockGroup adjacentMapBlockGroup, int topEmptySpace)
        {
            mapBlock.ToggleGrass(topEmptySpace >= 1);
            mapBlock.ToggleFoliage(treeTextures[Random.Range(0, treeTextures.Length)], topEmptySpace >= foliageHeightSpace ? foliageChance : 0);
            mapBlock.ToggleCorners(adjacentMapBlockGroup);
            mapBlock.TogglePieces(adjacentMapBlockGroup, pieceChance);
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
            var map = MapTextAssetToList(mapTextAssets[randomMapIndex]);
            
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
}