using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Hassus.Map
{
    public class MapBlock : MonoBehaviour
    {
        [SerializeField] private GameObject[] foliageObjects;
        [SerializeField] private GameObject grassObject;

        [SerializeField] private GameObject leftPiece;
        [SerializeField] private GameObject rightPiece;
        [SerializeField] private GameObject bottomPiece;

        [SerializeField] private GameObject topLeftCorner;
        [SerializeField] private GameObject topRightCorner;
        [SerializeField] private GameObject bottomLeftCorner;
        [SerializeField] private GameObject bottomRightCorner;

        private MapBlockGroup _mapBlockGroup;

        private Transform activeFoliageObject = null;
        private float foliageSway = 1f;
        private float timer;

        private void Update()
        {
            if (activeFoliageObject != null)
            {
                timer += Time.deltaTime;
                var localEulerAngles = activeFoliageObject.localEulerAngles;
                activeFoliageObject.localRotation = Quaternion.Euler(Mathf.Sin(timer) * foliageSway, localEulerAngles.y, localEulerAngles.z);
            }
        }

        public void ToggleGrass(bool toggle)
        {
            grassObject.SetActive(toggle);
        }

        public void ToggleFoliage(Texture2D texture, Color color, float rotation, float sway, float foliageChance, int index = -1)
        {
            bool enableFoliage = foliageChance > Random.Range(0.0f, 1.0f);
            int randomIndex = index == -1 ? Random.Range(0, foliageObjects.Length) : index;
            for (int i = 0; i < foliageObjects.Length; i++)
            {
                bool toggle = i == randomIndex && enableFoliage;
                foliageObjects[i].SetActive(toggle);
                if (toggle)
                {
                    foliageSway = sway;
                    var material = foliageObjects[i].GetComponent<MeshRenderer>().material;
                    material.mainTexture = texture;
                    material.color = color;
                    foliageObjects[i].transform.localRotation = Quaternion.Euler(0f, rotation, 0f);
                    activeFoliageObject = foliageObjects[i].transform;
                }
            }
        }

        public void ToggleCorners(MapBlockGroup mapBlockGroup)
        {
            _mapBlockGroup = mapBlockGroup;
            ToggleCorners
            (
                mapBlockGroup.TopLeftBlock,
                mapBlockGroup.TopBlock,
                mapBlockGroup.TopRightBlock,
                mapBlockGroup.LeftBlock,
                mapBlockGroup.RightBlock,
                mapBlockGroup.BottomLeftBlock,
                mapBlockGroup.BottomBlock,
                mapBlockGroup.BottomRightBlock
            );
        }
        
        public void TogglePieces(MapBlockGroup mapBlockGroup, float pieceChance)
        {
            TogglePieces
            (
                mapBlockGroup.TopLeftBlock,
                mapBlockGroup.TopBlock,
                mapBlockGroup.TopRightBlock,
                mapBlockGroup.LeftBlock,
                mapBlockGroup.RightBlock,
                mapBlockGroup.BottomLeftBlock,
                mapBlockGroup.BottomBlock,
                mapBlockGroup.BottomRightBlock,
                pieceChance
            );
        }
        
        public void ResolveCorners()
        {
            if (_mapBlockGroup != null)
            {
                ToggleCorners(_mapBlockGroup);
            }
        }

        private void TogglePieces(MapBlock topLeftBlock, MapBlock topBlock, MapBlock topRightBlock, MapBlock leftBlock, MapBlock rightBlock, MapBlock bottomLeftBlock, MapBlock bottomBlock, MapBlock bottomRightBlock, float pieceChance)
        {
            if (leftBlock == null)
            {
                leftPiece.SetActive(pieceChance > Random.Range(0.0f, 1.0f));
            }
            else
            {
                leftPiece.SetActive(false);
            }

            if (rightBlock == null)
            {
                rightPiece.SetActive(pieceChance > Random.Range(0.0f, 1.0f));
            }
            else
            {
                rightPiece.SetActive(false);
            }

            if (bottomBlock == null)
            {
                bottomPiece.SetActive(pieceChance > Random.Range(0.0f, 1.0f));
            }
            else
            {
                bottomPiece.SetActive(false);
            }
        }

        private void ToggleCorners(MapBlock topLeftBlock, MapBlock topBlock, MapBlock topRightBlock, MapBlock leftBlock, MapBlock rightBlock, MapBlock bottomLeftBlock, MapBlock bottomBlock, MapBlock bottomRightBlock)
        {
            if (topBlock == null || !topBlock.isActiveAndEnabled)
            {
                if (topLeftBlock != null && topLeftBlock.isActiveAndEnabled)
                {
                    topLeftCorner.SetActive(true);
                }
                else
                {
                    topLeftCorner.SetActive(false);
                }

                if (topRightBlock != null && topRightBlock.isActiveAndEnabled)
                {
                    topRightCorner.SetActive(true);
                }
                else
                {
                    topRightCorner.SetActive(false);
                }
            }
            else
            {
                topLeftCorner.SetActive(false);
                topRightCorner.SetActive(false);
            }

            if (bottomBlock == null || !bottomBlock.isActiveAndEnabled)
            {
                if (bottomLeftBlock != null && bottomLeftBlock.isActiveAndEnabled)
                {
                    bottomLeftCorner.SetActive(true);
                }
                else
                {
                    bottomLeftCorner.SetActive(false);
                }

                if (bottomRightBlock != null && bottomRightBlock.isActiveAndEnabled)
                {
                    bottomRightCorner.SetActive(true);
                }
                else
                {
                    bottomRightCorner.SetActive(false);
                }
            }
            else
            {
                bottomLeftCorner.SetActive(false);
                bottomRightCorner.SetActive(false);
            }
        }

        private void OnDisable()
        {
            if (_mapBlockGroup != null)
            {
                if (_mapBlockGroup.TopBlock != null && _mapBlockGroup.TopBlock.isActiveAndEnabled)
                {
                    _mapBlockGroup.TopBlock.ResolveCorners();
                }

                if (_mapBlockGroup.BottomBlock != null && _mapBlockGroup.BottomBlock.isActiveAndEnabled)
                {
                    _mapBlockGroup.BottomBlock.ResolveCorners();
                }
            }
        }
    }
}