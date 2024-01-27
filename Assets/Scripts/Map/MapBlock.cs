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

        private Transform _activeFoliageObject = null;
        private float _cornerSizeMin = 1f;
        private float _cornerSizeMax = 1f;
        private float _foliageSway = 1f;
        private float _timer;
        private float _explosionTimer = 1f;
        private float _explosionAccelerationSpeed = 10f;
        private float _explosionDecelerationSpeed = 0.2f;
        private float _explosiveForceMax = 1f;
        private float _explosiveForce = 1f;
        private float _currentExplosiveForce = 1f;

        private readonly float foliageMinRotation = -60f;
        private readonly float foliageMaxRotation = 60f;

        private void Update()
        {
            if (_activeFoliageObject != null)
            {
                _timer += Time.deltaTime;
                
                if (_explosionTimer < 1f)
                {
                    _explosionTimer += Time.deltaTime * _explosionAccelerationSpeed;
                    _currentExplosiveForce = Mathf.Lerp(1f, _explosiveForce, _explosionTimer);
                    
                    var localEulerAngles = _activeFoliageObject.localEulerAngles;
                    _activeFoliageObject.localRotation = Quaternion.Euler(Mathf.Clamp(Mathf.Sin(_timer) * _currentExplosiveForce, foliageMinRotation, foliageMaxRotation), localEulerAngles.y, localEulerAngles.z);
                }
                else if (_explosionTimer < 2f)
                {
                    _explosionTimer += Time.deltaTime * _explosionDecelerationSpeed;
                    _currentExplosiveForce = Mathf.Lerp(_explosiveForce, 1f, _explosionTimer - 1f);
                    
                    var localEulerAngles = _activeFoliageObject.localEulerAngles;
                    _activeFoliageObject.localRotation = Quaternion.Euler(Mathf.Clamp(Mathf.Sin(_timer) * _currentExplosiveForce, foliageMinRotation, foliageMaxRotation), localEulerAngles.y, localEulerAngles.z);
                }
                else
                {
                    var localEulerAngles = _activeFoliageObject.localEulerAngles;
                    _activeFoliageObject.localRotation = Quaternion.Euler(Mathf.Clamp(Mathf.Sin(_timer) * _foliageSway, foliageMinRotation, foliageMaxRotation), localEulerAngles.y, localEulerAngles.z);
                }
            }
        }

        public void ToggleGrass(bool toggle, float scaleMin, float scaleMax)
        {
            if (toggle)
            {
                grassObject.transform.localScale = new Vector3(1f, Random.Range(scaleMin, scaleMax), 1f);
            }
            grassObject.SetActive(toggle);
        }

        public void ToggleFoliage(Texture2D texture, Color color, float rotation, float sway, float foliageChance, float explosiveForceMax, int index = -1)
        {
            _explosiveForceMax = explosiveForceMax;
            bool enableFoliage = foliageChance > Random.Range(0.0f, 1.0f);
            int randomIndex = index == -1 ? Random.Range(0, foliageObjects.Length) : index;
            for (int i = 0; i < foliageObjects.Length; i++)
            {
                bool toggle = i == randomIndex && enableFoliage;
                foliageObjects[i].SetActive(toggle);
                if (toggle)
                {
                    _foliageSway = sway;
                    var material = foliageObjects[i].GetComponent<MeshRenderer>().material;
                    material.mainTexture = texture;
                    material.color = color;
                    foliageObjects[i].transform.localRotation = Quaternion.Euler(0f, rotation, 0f);
                    _activeFoliageObject = foliageObjects[i].transform;
                }
            }
        }

        public void ToggleCorners(MapBlockGroup mapBlockGroup, float cornerMin, float cornerMax)
        {
            _cornerSizeMin = cornerMin;
            _cornerSizeMax = cornerMax;
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
                mapBlockGroup.BottomRightBlock,
                cornerMin,
                cornerMax
            );
        }
        
        public void TogglePieces(MapBlockGroup mapBlockGroup, float sizeMin, float sizeMax, float pieceChance)
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
                sizeMin,
                sizeMax,
                pieceChance
            );
        }
        
        public void ResolveCorners()
        {
            if (_mapBlockGroup != null)
            {
                ToggleCorners(_mapBlockGroup, _cornerSizeMin, _cornerSizeMax);
            }
        }

        public void SetExplosiveForce(float force)
        {
            _timer = 180f;
            _explosionTimer = 0f;
            _explosiveForce = force;
        }

        private void TogglePieces(MapBlock topLeftBlock, MapBlock topBlock, MapBlock topRightBlock, MapBlock leftBlock, MapBlock rightBlock, MapBlock bottomLeftBlock, MapBlock bottomBlock, MapBlock bottomRightBlock, float sizeMin, float sizeMax, float pieceChance)
        {
            if (leftBlock == null)
            {
                leftPiece.transform.localScale = new Vector3(1f, 1f, Random.Range(sizeMin, sizeMax));
                leftPiece.SetActive(pieceChance > Random.Range(0.0f, 1.0f));
            }
            else
            {
                leftPiece.SetActive(false);
            }

            if (rightBlock == null)
            {
                rightPiece.transform.localScale = new Vector3(1f, 1f, Random.Range(sizeMin, sizeMax));
                rightPiece.SetActive(pieceChance > Random.Range(0.0f, 1.0f));
            }
            else
            {
                rightPiece.SetActive(false);
            }

            if (bottomBlock == null)
            {
                bottomPiece.transform.localScale = new Vector3(1f, Random.Range(sizeMin, sizeMax), 1f);
                bottomPiece.SetActive(pieceChance > Random.Range(0.0f, 1.0f));
            }
            else
            {
                bottomPiece.SetActive(false);
            }
        }

        private void ToggleCorners(MapBlock topLeftBlock, MapBlock topBlock, MapBlock topRightBlock, MapBlock leftBlock, MapBlock rightBlock, MapBlock bottomLeftBlock, MapBlock bottomBlock, MapBlock bottomRightBlock, float cornerMin, float cornerMax)
        {
            if (topBlock == null || !topBlock.isActiveAndEnabled)
            {
                if (topLeftBlock != null && topLeftBlock.isActiveAndEnabled)
                {
                    topLeftCorner.transform.localScale = new Vector3(1f, Random.Range(cornerMin, cornerMax), Random.Range(cornerMin, cornerMax));
                    topLeftCorner.SetActive(true);
                }
                else
                {
                    topLeftCorner.SetActive(false);
                }

                if (topRightBlock != null && topRightBlock.isActiveAndEnabled)
                {
                    topRightCorner.transform.localScale = new Vector3(1f, Random.Range(cornerMin, cornerMax), Random.Range(cornerMin, cornerMax));
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
                    bottomLeftCorner.transform.localScale = new Vector3(1f, Random.Range(cornerMin, cornerMax), Random.Range(cornerMin, cornerMax));
                    bottomLeftCorner.SetActive(true);
                }
                else
                {
                    bottomLeftCorner.SetActive(false);
                }

                if (bottomRightBlock != null && bottomRightBlock.isActiveAndEnabled)
                {
                    bottomRightCorner.transform.localScale = new Vector3(1f, Random.Range(cornerMin, cornerMax), Random.Range(cornerMin, cornerMax));
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

                if (_mapBlockGroup.LeftBlock != null && _mapBlockGroup.LeftBlock.isActiveAndEnabled)
                {
                    _mapBlockGroup.LeftBlock.SetExplosiveForce(-_explosiveForceMax);

                    var block = _mapBlockGroup.LeftBlock._mapBlockGroup.LeftBlock;
                    if (block != null && block.isActiveAndEnabled)
                    {
                        block.SetExplosiveForce(-_explosiveForceMax / 2f);
                    }
                }
                
                if (_mapBlockGroup.RightBlock != null && _mapBlockGroup.RightBlock.isActiveAndEnabled)
                {
                    _mapBlockGroup.RightBlock.SetExplosiveForce(_explosiveForceMax);
                    
                    var block = _mapBlockGroup.RightBlock._mapBlockGroup.RightBlock;
                    if (block != null && block.isActiveAndEnabled)
                    {
                        block.SetExplosiveForce(_explosiveForceMax / 2f);
                    }
                }
            }
        }
    }
}