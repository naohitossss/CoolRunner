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

    private float playerMoveDistance;               // プレイヤーの移動距離
    private List<List<int>> mapList = new List<List<int>>(); // マップ状態管理 (0:空き, 1:使用済み)
    private RandomMapGena randomMapGena;           // マップ生成の参照
    private LaneMovement laneMovement;            // プレイヤー移動の参照


    void Start()
    {
        randomMapGena = GetComponent<RandomMapGena>();
        if (playerObj != null)
        {
            laneMovement = playerObj.GetComponent<LaneMovement>();
        }
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
    private bool CheckGenerateOBS(Vector3 spawnPosition) 
    { 
        if (!Physics.CheckSphere(spawnPosition, laneMovement.getPlayerMoveVectorLength(), obslapLayer))
        {
            
            return true;
        }
        else
        {
            return false;
        }
    }

    /// 障害物の実際の生成処理
    private void InsOBS(int mapRowNum, Vector3 streatPos, GameObject parent)
    {
        for (int i = 1; i < randomMapGena.leftBuildingNum; i++)
        {
            if (mapList[mapRowNum][i] == 0)
            {
                int OBSNum = Random.Range(1, 100) <= 80 ? Random.Range(1, 4) : 0;
                Vector3 pos = streatPos + new Vector3((i - 2) * (randomMapGena.streatObjLegth / 2), 0f, 0f);
                if(CheckGenerateOBS(pos)){
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