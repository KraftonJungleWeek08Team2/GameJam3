using System.Collections.Generic;
using UnityEngine;

public class ParallaxBackground : MonoBehaviour
{
    [System.Serializable]
    public class Layer
    {
        [Tooltip("이 레이어의 컨테이너(빈 GameObject)")]
        public Transform container;

        [Tooltip("타일 프리팹(SpriteRenderer 필요)")]
        public GameObject tilePrefab;

        [Tooltip("스크롤 속도 비율 (0~1)")]
        [Range(0f, 1f)]
        public float parallaxFactor = 0.5f;

        [Tooltip("여유 타일 개수(양쪽)")]
        public int bufferTiles = 1;

        [HideInInspector] public List<Transform> tiles;
        [HideInInspector] public float tileWidth;
    }

    [Tooltip("관리할 레이어들")]
    public List<Layer> layers = new List<Layer>();

    [Tooltip("전역 스크롤 속도")]
    public float scrollSpeed = 4f;

    private Camera _cam;
    private float _halfCamWidth;

    void Start()
    {
        _cam = Camera.main;
        _halfCamWidth = _cam.orthographicSize * _cam.aspect;

        foreach (var layer in layers)
            InitializeLayer(layer);
    }

    void InitializeLayer(Layer layer)
    {
        // 타일 가로폭 계산
        var sr = layer.tilePrefab.GetComponent<SpriteRenderer>();
        layer.tileWidth = sr.bounds.size.x;

        // 화면폭 + 양쪽 buffer 폭으로 필요한 타일 수 계산
        float totalWidth = (_halfCamWidth * 2f) + (layer.tileWidth * layer.bufferTiles * 2);
        int needed = Mathf.CeilToInt(totalWidth / layer.tileWidth) + 1;

        // 시작 X 좌표: 화면 왼쪽 - buffer*타일폭
        float startX = _cam.transform.position.x - _halfCamWidth - (layer.tileWidth * layer.bufferTiles);

        layer.tiles = new List<Transform>(needed);
        for (int i = 0; i < needed; i++)
        {
            var go = Instantiate(layer.tilePrefab, layer.container);
            go.transform.position = new Vector3(
                startX + i * layer.tileWidth,
                layer.container.position.y,
                layer.container.position.z
            );
            layer.tiles.Add(go.transform);
        }
    }

    void Update()
    {
        float dt = Time.deltaTime;
        float camX = _cam.transform.position.x;

        foreach (var layer in layers)
        {
            // 1) 이동
            float delta = scrollSpeed * layer.parallaxFactor * dt;
            foreach (var t in layer.tiles)
                t.position += Vector3.left * delta;

            // 2) 재배치 기준선
            float leftThreshold = camX - _halfCamWidth - (layer.bufferTiles * layer.tileWidth);

            // 3) 현재 가장 오른쪽 X 찾기
            float maxX = float.MinValue;
            foreach (var t in layer.tiles)
                if (t.position.x > maxX) maxX = t.position.x;

            // 4) 벗어난 타일만 오른쪽 끝으로 옮기기
            for (int i = 0; i < layer.tiles.Count; i++)
            {
                var t = layer.tiles[i];
                if (t.position.x < leftThreshold)
                {
                    t.position = new Vector3(maxX + layer.tileWidth, t.position.y, t.position.z);
                    maxX = t.position.x;
                }
            }
        }
    }
}
