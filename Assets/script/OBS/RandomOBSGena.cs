using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class RandomOBSGena : MonoBehaviour
{
    [SerializeField] private GameObject OBSObj1;
    [SerializeField] private GameObject OBSObj2;
    [SerializeField] private GameObject OBSObj3;
    private List<List<bool>> mapList = new List<List<bool>>();
    private RandomMapGena randomMapGena;

    void Start()
    {
        randomMapGena = GetComponent<RandomMapGena>();
    }

    public void addMapRow(int mapRowNum, int buildingNum, Vector3 streatPos, GameObject parent)
    {
        EnsureMapListSize(mapRowNum);
        for(int i = buildingNum + 1; i < randomMapGena.LeftBuildingNum;i++)
        { 
            mapList[mapRowNum][i] = true;
        }
        InsOBS(mapRowNum, streatPos, parent);
    }

    private void InsOBS(int mapRowNum, Vector3 streatPos, GameObject parent)
    {
        Debug.Log("OBS生成");
        for (int i = 1; i < randomMapGena.LeftBuildingNum; i++)
        {
            if (mapList[mapRowNum][i] == false)
            {
                int OBSNum = Random.Range(1, 4);
                Vector3 pos = streatPos + new Vector3((i - 2) * (randomMapGena.streatObjLegth / 3), 0f, 0f);
                switch (OBSNum)
                {
                    case 1:
                        Instantiate(OBSObj1, pos, Quaternion.identity, parent.transform);
                        mapList[mapRowNum][i] = true;
                        break;
                    case 2:
                        Instantiate(OBSObj2, pos, Quaternion.identity, parent.transform);
                        mapList[mapRowNum][i] = true;
                        mapList[mapRowNum + 1][i] = true;
                        break;
                    case 3:
                        Instantiate(OBSObj3, pos, Quaternion.identity, parent.transform);
                        mapList[mapRowNum][i] = true;
                        mapList[mapRowNum + 1][i] = true;
                        mapList[mapRowNum + 2][i] = true;
                        break;
                    default:
                        break;
                }
            }
        }
    }

    public void EnsureMapListSize(int requiredRow)
    {
        while (mapList.Count <= requiredRow + 100)
        {
            var newRow = new List<bool>();
            for(int i = 0; i < randomMapGena.LeftBuildingNum; i++)
            {
                newRow.Add(false);
            }
            mapList.Add(newRow);
            Debug.Log($"New row added. Current size: {mapList.Count}");
        }
    }
}