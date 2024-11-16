using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainSlicing : MonoBehaviour
{
    [Header("プレイヤー")]
    public Transform player;  // プレイヤーのTransformコンポーネント
    [Header("地形の親オブジェクト")] 
    public Transform terrainParent;  // 生成された地形の親オブジェクト
    public Vector2 terrainSize = new Vector2(6f, 335f);  // 地形プレハブのサイズ(x:幅, y:長さ)
    [Header("固定地形の配置データ")]
    public StructData[] datas;  // シーン内の固定地形データの配列
    [Header("ランダム生成用地形プレハブ")]
    public List<GameObject> terrainObjects;  // ランダム生成用の地形プレハブリスト
    [Header("地形の削除距離")]
    public float deletionDistance = 500f;  // プレイヤーからこの距離以上離れた地形を削除

    private Vector2 terrainSizeValue;  // 地形サイズの実際の値を保持
    private Dictionary<Vector2, GameObject> terrainLoadedFixed;  // 固定地形を管理する辞書
    private Dictionary<(int x, int y), GameObject> terrainLoaded;  // 動的に生成された地形を管理する辞書
    private Dictionary<(int x, int y), GameObject> dictTemp;  // 一時的な地形データを保持する辞書
    private float lastTerrainYPosition;  // 最後に生成された地形のZ座標
    private List<GameObject> activeTerrains = new List<GameObject>();  // アクティブな地形を追跡

    /// <summary>
    /// 地形データを格納する構造体
    /// </summary>
    [System.Serializable]
    public struct StructData
    {
        public Vector2 key;      // 地形の位置座標
        public GameObject value; // 対応する地形のGameObject
    }

    private void Awake()
    {
        terrainSizeValue = new Vector2(terrainSize.x, terrainSize.y);
        terrainLoadedFixed = new Dictionary<Vector2, GameObject>();
        terrainLoaded = new Dictionary<(int x, int y), GameObject>();
        dictTemp = new Dictionary<(int x, int y), GameObject>();
        InitData();
    }

    private void Start()
    {
        FirstLoadTerrain();
    }

    private void FixedUpdate()
    {
        LoadTerrain();
    }

    // 固定の地形データを辞書に追加
    void InitData()
    {
        foreach (var data in datas)
        {
            if (!terrainLoadedFixed.ContainsKey(data.key))
                terrainLoadedFixed.Add(data.key, data.value);
        }
    }

    // 最初の地形を生成
    private void FirstLoadTerrain()
    {
        // プレイヤーの初期位置に基づいて最初の地形を生成
        lastTerrainYPosition = player.position.z;
        GenerateNewTerrainRow();
        int randomIndex = Random.Range(0, terrainObjects.Count);
        GameObject newTerrain = Instantiate(terrainObjects[randomIndex], terrainParent);

        // 新しい地形を適切な位置に配置
        newTerrain.transform.position = new Vector3(0, 0, player.position.z);
        activeTerrains.Add(newTerrain);
    }

    // プレイヤーの前方の地形のみ生成
    private void LoadTerrain()
    {
        if (player != null)
        {
            // プレイヤーが一定距離進んだら新しい地形を生成
            if (player.position.z > lastTerrainYPosition - terrainSizeValue.y)
            {
                GenerateNewTerrainRow();
            }

            // 古い地形を削除
            CleanupOldTerrain();
        }
    }

    // 新しい地形の行を生成
    private void GenerateNewTerrainRow()
    {
        // ランダムな地形プレハブを選択して生成
        int randomIndex = Random.Range(0, terrainObjects.Count);
        GameObject newTerrain = Instantiate(terrainObjects[randomIndex], terrainParent);

        // 新しい地形を適切な位置に配置
        newTerrain.transform.position = new Vector3(0, 0, lastTerrainYPosition + terrainSizeValue.y);
        lastTerrainYPosition += terrainSizeValue.y;
        
        // 新しい地形を追跡リストに追加
        activeTerrains.Add(newTerrain);
    }



    private void CleanupOldTerrain()
    {
        // プレイヤーから一定距離後ろにある地形を削除
        for (int i = activeTerrains.Count - 1; i >= 0; i--)
        {
            if (activeTerrains[i] != null &&
                activeTerrains[i].transform.position.z < player.position.z - deletionDistance)
            {
                Destroy(activeTerrains[i]);
                activeTerrains.RemoveAt(i);
            }
        }
    }
}