using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomOBSGena : MonoBehaviour
{
    [SerializeField] private GameObject OBSObj1;
    [SerializeField] private GameObject OBSObj2;
    [SerializeField] private GameObject OBSObj3;

    [SerializeField] private LayerMask obslapLayer;         // 重複判定用のレイヤー
    [SerializeField] private GameObject playerObj;

    private float playerMoveDistance;
    private List<List<int>> mapList = new List<List<int>>();
    private RandomMapGena randomMapGena;
    private LaneMovement laneMovement;

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

    public void addMapRow(int mapRowNum, int buildingNum, Vector3 streatPos, GameObject parent)
    {
        EnsureMapListSize(mapRowNum);
        for(int i = buildingNum + 1; i < randomMapGena.leftBuildingNum;i++)
        { 
            mapList[mapRowNum][i] = 1;
        }
        InsOBS(mapRowNum, streatPos, parent);
    }

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