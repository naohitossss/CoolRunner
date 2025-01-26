using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomMapGena : MonoBehaviour
{
    public GameObject streatObj;
    public GameObject building1;    // 建物タイプ1
    public GameObject building2;    // 建物タイプ2
    public GameObject building3;    // 建物タイプ3

    [SerializeField] private GameObject GrassObj;
    [SerializeField] private GameObject buildingParents;
    public float buildingWidth = 10f;  // 建物の幅
    public int startMapRow {get;private set;} = 10;
    public int streatCount = 3;
    public int LeftBuildingNum{get;private set;} = 4;
    private Transform player;  // プレイヤーのTransformコンポーネント

    public float streatObjLegth {get;private set;}
    private Vector3 setStreatPos;
    private Vector3 setBuildingPos;
    private List<List<int>> mapList = new List<List<int>>();

    private Quaternion LeftBuildingQuaternion = Quaternion.Euler(0f, 90f, 0f);
    private Quaternion rightBuildingQuaternion = Quaternion.Euler(0f, -90f, 0f);
    private float deletionDistance = 30f;  // プレイヤーからこの距離以上離れた地形を削除
    private float SetNewRowdistance = 250f;
    private RandomOBSGena randomOBSGena;
    private List<GameObject> activeMapObj = new List<GameObject>();  // アクティブな地形を追跡

    void Start()
    {
        randomOBSGena = GetComponent<RandomOBSGena>();
        if (randomOBSGena == null)
        {
            Debug.LogError("RandomOBSGena component not found!");
            return;
        }

        // streatObjのサイズからstreatObjLegthを自動計算
        streatObjLegth = streatObj.GetComponent<Renderer>().bounds.size.z / 2;

        setStreatPos = Vector3.zero + new Vector3(0f, 0f, streatObjLegth / 2);
        setBuildingPos = Vector3.zero;
        InitMapData();
    }

    private void FixedUpdate()
    {
        checkTerrainRow();
    }

    public void SetPlayer(Transform playerTransform)
    {
        player = playerTransform;
    }

    private void CleanupOldMapRow()
    {
        // プレイヤーから一定距離後ろにある地形を削除
        for (int i = activeMapObj.Count - 1; i >= 0; i--)
        {
            if (activeMapObj[i] != null &&
                activeMapObj[i].transform.position.z < player.position.z - deletionDistance)
            {
                Destroy(activeMapObj[i]);
                activeMapObj.RemoveAt(i);
            }
        }
    }

    private void GenerateNewTerrainRow()
    {
        List<int> mapRow = new List<int>();
        GameObject newRow = new GameObject();
        newRow.transform.position = setStreatPos;
        newRow.transform.parent = buildingParents.transform;
        activeMapObj.Add(newRow);

        // 通りの生成
        Instantiate(streatObj, setStreatPos, Quaternion.identity, newRow.transform);

        int buildingTypeRight = 3;
        randomOBSGena.addMapRow(activeMapObj.Count - 1, buildingTypeRight, setStreatPos, newRow);
        int buildingTypeLeft = Random.Range(1, 4);

        // 右側の建物
        Vector3 rightPos = setStreatPos + new Vector3(streatObjLegth, 0f, 0f);
        SpawnRandomBuilding(rightPos, buildingTypeRight, rightBuildingQuaternion, newRow.transform);

        // 左側の建物
        Vector3 leftPos = setStreatPos + new Vector3(-streatObjLegth, 0f, 0f);
        SpawnRandomBuilding(leftPos, buildingTypeLeft, LeftBuildingQuaternion, newRow.transform);

        setStreatPos += new Vector3(0f, 0f, streatObjLegth);
    }

    private void checkTerrainRow()
    {
        if (player != null)
        {
            // プレイヤーが一定距離進んだら新しい地形を生成
            if (player.position.z > setStreatPos.z - SetNewRowdistance)
            {
                GenerateNewTerrainRow();
            }

            // 古い地形を削除
            CleanupOldMapRow();
        }
    }

    void InitMapData()
    {
        for(int row = 0; row < startMapRow; row++)
        {
            GameObject newRow = new GameObject();
            newRow.transform.position = setStreatPos;
            newRow.transform.parent = buildingParents.transform;
            activeMapObj.Add(newRow);
            // 通りの生成
            Instantiate(streatObj, setStreatPos, Quaternion.identity, activeMapObj[row].transform);
            int buildingTypeRight = Random.Range(1, 4);
            Debug.Log("Calling EnsureMapListSize for InitMapData");
            int buildingTypeLeft = Random.Range(1, 4);

            // 右側の建物
            Vector3 rightPos = setStreatPos + new Vector3(streatObjLegth, 0f, 0f);
            SpawnRandomBuilding(rightPos, buildingTypeRight, rightBuildingQuaternion, activeMapObj[row].transform);

            // 左側の建物
            Vector3 leftPos = setStreatPos + new Vector3(-streatObjLegth, 0f, 0f);
            SpawnRandomBuilding(leftPos, buildingTypeLeft, LeftBuildingQuaternion, activeMapObj[row].transform);
            setStreatPos += new Vector3(0f, 0f, streatObjLegth);
        } 
    }

    private void SpawnRandomBuilding(Vector3 position, int buildingType, Quaternion quaternion, Transform buildingParents)
    {
        GameObject buildingPrefab = null;
        switch(buildingType)
        {
            case 1: buildingPrefab = building1; break;
            case 2: buildingPrefab = building2; break;
            case 3: buildingPrefab = building3; break;
        }
        float buildingLength = buildingPrefab.GetComponent<Renderer>().bounds.size.x / 2;

        if(buildingPrefab != null)
        {
            // グラスオブジェクトの生成
            Instantiate(GrassObj, position, Quaternion.identity, buildingParents);

            if(position.x < 0)
            {
                Instantiate(buildingPrefab, new Vector3(-buildingLength,0f,0f) + position, quaternion, buildingParents);
            }
            else Instantiate(buildingPrefab, new Vector3(buildingLength,0f,0f) + position, quaternion, buildingParents);
        }
    }
}
