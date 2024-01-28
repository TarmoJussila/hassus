using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Hassus.Map
{
    public class MapLoader : MonoBehaviour
    {
        public static MapLoader Instance;

        public List<Vector3> ItemPoints => itemPoints;

        [Header("Map References")]
        [SerializeField] private TextAsset[] mapTextAssets;
        [SerializeField] private MapBlock mapBlockPrefab;
        [SerializeField] private Texture2D[] foliageTextures;

        [Header("Map Settings")]
        [SerializeField] [Range(0, 1.0f)] private float foliageChance = 0.5f;
        [SerializeField] [Range(1, 9)] private int foliageHeightSpace = 2;
        [SerializeField] [Range(0, 1.0f)] private float pieceChance = 0.5f;
        [SerializeField] [Range(0, 1f)] private float pieceSizeMin = 0.5f;
        [SerializeField] [Range(1f, 2f)] private float pieceSizeMax = 1.2f;
        [SerializeField] [Range(0, 1f)] private float cornerSizeMin = 0.5f;
        [SerializeField] [Range(1f, 2f)] private float cornerSizeMax = 1.2f;
        [SerializeField] private Color foliageColorVarianceMin = Color.white;
        [SerializeField] private Color foliageColorVarianceMax = Color.white;
        [SerializeField] private Color grassColorVarianceMin = Color.white;
        [SerializeField] private Color grassColorVarianceMax = Color.white;
        [SerializeField] [Range(0f, 10f)] private float foliageRotationMax = 5f;
        [SerializeField] [Range(0, 30f)] private float foliageSwayMax = 5f;
        [SerializeField] private float foliageExplosiveForceMax = 45f;
        [SerializeField] [Range(0, 1f)] private float grassScaleMin = 0.7f;
        [SerializeField] [Range(1f, 2f)] private float grassScaleMax = 1.7f;
        
        private Dictionary<Vector2Int, MapBlock> mapBlocks = new();
        private Dictionary<Vector2Int, BoxCollider2D> mapColliders = new();
        private List<Vector3> spawnPoints = new List<Vector3>();
        private List<Vector3> itemPoints = new List<Vector3>();
        private int randomMapIndex = 0;
        private int nextSpawnPointIndex = 0;
        
        private readonly int mapWidth = 40;
        private readonly int mapHeight = 14;
        private readonly string mapEmptySymbol = ".";
        private readonly string mapBlockSymbol = "#";
        private readonly string mapSpawnSymbol = "S";
        private readonly string mapItemSymbol = "R";
        private readonly bool enableDebugInput = true;

        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            GenerateMap();
        }

        private void Update()
        {
            #if UNITY_EDITOR
            if (enableDebugInput)
            {
                if (Input.GetKeyDown(KeyCode.R))
                {
                    DestroyMap();
                    GenerateMap();
                }

                if (Input.GetKeyDown(KeyCode.F))
                {
                    int randomX = Random.Range(0, mapWidth);
                    int randomY = Random.Range(0, mapHeight);
                    Explode(new Vector2(randomX, randomY), Random.Range(1, 5), Random.Range(0f, 1f) > 0.5f);
                }
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
                        boxCollider.gameObject.layer = 8;
                        boxCollider.transform.parent = transform;
                        
                        mapBlocks.Add(new Vector2Int(i, j), mapBlock);
                        mapColliders.Add(new Vector2Int(i, j), boxCollider);
                    }
                    else if (symbol == mapSpawnSymbol)
                    {
                        spawnPoints.Add(new Vector2(i, j));
                    }
                    else if (symbol == mapItemSymbol)
                    {
                        itemPoints.Add(new Vector2(i, j));
                    }
                }
            }
            
            ShuffleSpawnPoints();
            
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
            mapBlock.ToggleGrass
            (
                topEmptySpace >= 1,
                grassScaleMin,
                grassScaleMax,
                Color.Lerp(grassColorVarianceMin, grassColorVarianceMax, Random.Range(0.0f, 1.0f))
            );
            mapBlock.ToggleFoliage
            (
                foliageTextures[Random.Range(0, foliageTextures.Length)],
                Color.Lerp(foliageColorVarianceMin, foliageColorVarianceMax, Random.Range(0.0f, 1.0f)),
                Random.Range(-foliageRotationMax, foliageRotationMax),
                Random.Range(0f, foliageSwayMax),
                topEmptySpace >= foliageHeightSpace ? foliageChance : 0,
                foliageExplosiveForceMax
            );
            mapBlock.ToggleCorners(adjacentMapBlockGroup, cornerSizeMin, cornerSizeMax);
            mapBlock.TogglePieces(adjacentMapBlockGroup, pieceSizeMin, pieceSizeMax, pieceChance);
        }
        
        private List<string> MapTextAssetToList(TextAsset mapTextAsset)
        {
            var list = new List<string>();
            var lineArray = mapTextAsset.text.Split('\n').Reverse();
            foreach (var line in lineArray)
            {
                list.Add(line);
            }
            return list;
        }
        
        private void ShuffleSpawnPoints()
        {
            spawnPoints = spawnPoints.OrderBy(x => Guid.NewGuid()).ToList();
        }
        
        public Vector3 GetRandomSpawnPoint()
        {
            nextSpawnPointIndex++;
            if (nextSpawnPointIndex >= spawnPoints.Count)
            {
                nextSpawnPointIndex = 0;
            }
            return spawnPoints[nextSpawnPointIndex];
        }

        public Vector3 GetRandomItemPoint()
        {
            return itemPoints[Random.Range(0, itemPoints.Count)];
        }
        
        public void Explode(Vector3 center, int circleRadius = 1, bool reverseOrder = false)
        {
            int start = reverseOrder ? circleRadius : -circleRadius;
            int end = reverseOrder ? -circleRadius : circleRadius;
            int step = reverseOrder ? -1 : 1;

            for (int x = start; x != end + step; x += step)
            {
                for (int y = start; y != end + step; y += step)
                {
                    if (x * x + y * y <= circleRadius * circleRadius)
                    {
                        var coordinate = new Vector2Int(Mathf.RoundToInt(center.x + x), Mathf.RoundToInt(center.y + y));

                        mapColliders.TryGetValue(coordinate, out var collider);
                        mapBlocks.TryGetValue(coordinate, out var block);
                        if (block != null)
                        {
                            block.gameObject.SetActive(false);
                        }
                        if (collider != null)
                        {
                            collider.enabled = false;
                        }
                    }
                }
            }
        }
        
        private void OnDrawGizmos()
        {
            if (Application.isPlaying)
            {
                return;
            }
            
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
                        Gizmos.color = Color.magenta;
                        Gizmos.DrawWireCube(new Vector2(i, j), Vector3.one);
                    }
                    else if (symbol == mapSpawnSymbol)
                    {
                        Gizmos.color = Color.red;
                        Gizmos.DrawSphere(new Vector2(i, j), 1f);
                    }
                    else if (symbol == mapItemSymbol)
                    {
                        Gizmos.color = Color.yellow;
                        Gizmos.DrawSphere(new Vector2(i, j), 0.8f);
                    }
                }
            }
        }

        private void OnDrawGizmosSelected()
        {
            if (!Application.isPlaying)
            {
                return;
            }
            
            var map = MapTextAssetToList(mapTextAssets[randomMapIndex]);
            
            for (int i = 0; i < mapWidth; i++)
            {
                for (int j = 0; j < mapHeight; j++)
                {
                    string symbol = map[j][i].ToString();

                    if (symbol == mapSpawnSymbol)
                    {
                        Gizmos.color = new Color(255f, 0f, 255f, 0.5f);
                        Gizmos.DrawSphere(new Vector2(i, j), 1f);
                    }
                    else if (symbol == mapItemSymbol)
                    {
                        Gizmos.color = new Color(255f, 255f, 0f, 0.5f);
                        Gizmos.DrawSphere(new Vector2(i, j), 0.8f);
                    }
                }
            }
        }
    }
}