using UnityEngine;
using System.Collections;

public class ItemSpawner : MonoBehaviour
{
    [SerializeField] private GameObject icePrefab;
    [SerializeField] private GameObject energyDrinkPrefab;
    [SerializeField] private Transform playerTransform;               // プレイヤーのTransform
    [SerializeField] private float spawnInterval = 15f;       // 生成間隔（秒）
    //[SerializeField] private float spawnRadius = 4.5f;         // 前方の生成エリアの半径
    [SerializeField] private float spawnDistance = 100f;       // プレイヤーからの生成距離
    [SerializeField] private LayerMask overlapLayer;         // 重複判定用のレイヤー
    public GameObject itemParent; // アイテムの親オブジェクト
    private Vector3[] lanes = new Vector3[3];

    private void Start()
    {
        StartCoroutine(SpawnItems());
    }

    public void SetPlayerTransform(Transform playerTransform){
        this.playerTransform = playerTransform;
    }

    private IEnumerator SpawnItems()
    {
        while (true)
        {
            bool spawned = false;
            int retryCount = 0;

            while (!spawned && retryCount < 5) // 最大5回までリトライ
            {
                Vector3 spawnPosition = GetRandomSpawnPosition();

                // 重複チェック
                if (!Physics.CheckSphere(spawnPosition, 4f, overlapLayer))
                {
                    GameObject prefabToSpawn = Random.value < 0.5f ? icePrefab : energyDrinkPrefab;
                    Instantiate(prefabToSpawn, spawnPosition, Quaternion.identity, itemParent.transform);
                    spawned = true;
                }
                else
                {
                    retryCount++;
                }
            }

            // 次のスポーンまで待機
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    private Vector3 GetRandomSpawnPosition()
    {
        // プレイヤーの前方にランダムな位置を取得
        Vector3 forwardOffset = playerTransform.forward * spawnDistance;
        lanes[0] = new Vector3(0 - 5f, 0, playerTransform.position.z);
        lanes[1] = new Vector3(0, 0, playerTransform.position.z);
        lanes[2] = new Vector3(0 + 5f, 0, playerTransform.position.z);
        return GetRandomLanePosition() + forwardOffset ;
    }

    private Vector3 GetRandomLanePosition()
    {
        int randomIndex = Random.Range(0, lanes.Length);
        return lanes[randomIndex];
    }
}

