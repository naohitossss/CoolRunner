using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// ランダムな障害物を生成・管理するコンポーネント
public class RandomOBSGena : MonoBehaviour
{
    // 障害物のプレハブ
    [SerializeField] private GameObject OBSObj1;    // 1マス分の障害物
    [SerializeField] private GameObject OBSObj2;    // 2マス分の障害物
    [SerializeField] private GameObject OBSObj3;    // 3マス分の障害物

    [SerializeField] private LayerMask obslapLayer; // 障害物の重なり判定用レイヤー
    [SerializeField] private GameObject playerObj;  // プレイヤーの参照
    [SerializeField] private float checkRadius = 1.0f;  // 判定用の球の半径
    [SerializeField] private int GenaOBSprobability = 80;  // 判定用の球の半径

    private List<List<int>> mapList = new List<List<int>>(); // マップ状態管理 (0:空き, 1:使用済み)
    private RandomMapGena randomMapGena;           // マップ生成の参照
    private LaneMovement laneMovement;            // プレイヤー移動の参照

    private float obsLength1;  // 1マス分の障害物の縦長
    private float obsLength2;  // 2マス分の障害物の縦長
    private float obsLength3;  // 3マス分の障害物の縦長

    void Start()
    {
        randomMapGena = GetComponent<RandomMapGena>();
        if (playerObj != null)
        {
            laneMovement = playerObj.GetComponent<LaneMovement>();
        }

        // 障害物の縦長を初期化
        obsLength1 = GetObjectLength(OBSObj1);
        obsLength2 = GetObjectLength(OBSObj2);
        obsLength3 = GetObjectLength(OBSObj3);
    }

    public void SetGnenaOBSprobability(int probability)
    {
        GenaOBSprobability = probability;
    }

    // オブジェクトの縦長を取得するメソッド
    private float GetObjectLength(GameObject obj)
    {
        if (obj == null) return 0f;

        // プレハブの場合、一時的にインスタンスを生成して計算
        GameObject tempInstance = Instantiate(obj);
        tempInstance.SetActive(false); // 見えないように非アクティブ化

        Bounds bounds = new Bounds();
        bool hasBounds = false;

        // 全ての子オブジェクトのRendererを取得
        Renderer[] renderers = tempInstance.GetComponentsInChildren<Renderer>(true);
        if (renderers.Length > 0)
        {
            // 最初のRendererのBoundsで初期化
            bounds = renderers[0].bounds;
            // 他のすべてのRendererのBoundsを統合
            for (int i = 1; i < renderers.Length; i++)
            {
                bounds.Encapsulate(renderers[i].bounds);
            }
            hasBounds = true;
        }

        // Colliderがある場合も同様に計算
        if (!hasBounds)
        {
            Collider[] colliders = tempInstance.GetComponentsInChildren<Collider>(true);
            if (colliders.Length > 0)
            {
                bounds = colliders[0].bounds;
                for (int i = 1; i < colliders.Length; i++)
                {
                    bounds.Encapsulate(colliders[i].bounds);
                }
                hasBounds = true;
            }
        }

        // 一時オブジェクトを破棄
        DestroyImmediate(tempInstance);

        if (!hasBounds)
        {
            Debug.LogWarning($"Could not determine size for obstacle {obj.name}. Using default size.");
            return 1f; // デフォルト値
        }

        return bounds.size.z;
    }

    public void SetPlayer(GameObject player)
    {
        playerObj = player;
        laneMovement = player.GetComponent<LaneMovement>();
    }

    /// 新しい行のマップデータを追加し、障害物を生成
    public void addMapRow(int mapRowNum, int buildingNum, Vector3 streatPos, GameObject parent)
    {
        EnsureMapListSize(mapRowNum);
        for(int i = buildingNum + 1; i < randomMapGena.leftBuildingNum;i++)
        { 
            mapList[mapRowNum][i] = 1;
        }
        InsOBS(mapRowNum, streatPos, parent);
    }

    /// 障害物生成位置の妥当性チェック
    private bool CheckGenerateOBS(Vector3 spawnPosition,float objLegth) 
    { 
        if (Physics.CheckSphere(spawnPosition - new Vector3(0f, 0f, objLegth / 2),
        laneMovement.GetPlayerMoveVectorLength(), obslapLayer))
        {
            // 後方90度の範囲をチェック
            Vector3 back = Vector3.back;
            for (float angle = -45f; angle <= 45f; angle += 15f)
            {
                Vector3 direction = Quaternion.Euler(0, angle, 0) * back;
                if (Physics.SphereCast(spawnPosition - new Vector3(0f, 0f, objLegth / 2), checkRadius, direction, 
                out RaycastHit hit, laneMovement.GetPlayerMoveVectorLength(), obslapLayer))
                {
                    return false;
                }
            }
        }

        return true;
    }

    /// 障害物の実際の生成処理
    private void InsOBS(int mapRowNum, Vector3 streatPos, GameObject parent)
    {
        for (int i = 1; i < randomMapGena.leftBuildingNum; i++)
        {
            if (mapList[mapRowNum][i] == 0)
            {
                int OBSNum = Random.Range(1, 100) <= GenaOBSprobability ? Random.Range(1, 4) : 0;
                Vector3 pos = streatPos + new Vector3((2 - i) * (randomMapGena.streatObjLegth / 2), 0f, 0f);
                switch (OBSNum)
                {
                    case 1:
                        Instantiate(OBSObj1, pos, Quaternion.identity, parent.transform);
                        mapList[mapRowNum][i] = 1;
                        break;
                    case 2:
                        Instantiate(OBSObj2, pos, Quaternion.identity, parent.transform);
                        mapList[mapRowNum][i] = 1;
                        mapList[mapRowNum + 1][i] = 1;
                        break;
                    case 3:
                        if(!CheckGenerateOBS(pos,obsLength3)) return;
                        Instantiate(OBSObj3, pos, Quaternion.identity, parent.transform);
                        mapList[mapRowNum][i] = 1;
                        mapList[mapRowNum + 1][i] = 1;
                        mapList[mapRowNum + 2][i] = 1;
                        break;
                    default:
                        break;
                }
            }
        }
    }

    /// マップリストのサイズを確保
    public void EnsureMapListSize(int requiredRow)
    {
        while (mapList.Count <= requiredRow + 100)
        {
            var newRow = new List<int>();
            for(int i = 0; i < randomMapGena.leftBuildingNum; i++)
            {
                newRow.Add(0);
            }
            mapList.Add(newRow);
        }
    }
}