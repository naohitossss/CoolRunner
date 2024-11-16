using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunNPCGenerate : MonoBehaviour
{
    [SerializeField] private GameObject runnerPrefab;
    [SerializeField] private float spawnInterval = 5f;
    [SerializeField] private int maxRunners = 10;
    [SerializeField] private Transform spawnPos;
    
    private Vector3[] lanes = new Vector3[3];
    private List<GameObject> activeRunners = new List<GameObject>();

    private bool isSpawning = true;

    void Start()
    {
        if (spawnPos == null)
        {
            Debug.LogError("Spawn position is not set!");
            return;
        }

        if (runnerPrefab == null)
        {
            Debug.LogError("Runner prefab is not set!");
            return;
        }

        InitializeLanes();
        StartCoroutine(SpawnRunners());
    }

    private void InitializeLanes()
    {
        lanes[0] = new Vector3(spawnPos.position.x - 5f, spawnPos.position.y, spawnPos.position.z);
        lanes[1] = new Vector3(spawnPos.position.x, spawnPos.position.y, spawnPos.position.z);
        lanes[2] = new Vector3(spawnPos.position.x + 5f, spawnPos.position.y, spawnPos.position.z);
    }

    private IEnumerator SpawnRunners()
    {
        while (isSpawning)
        {
            if (activeRunners.Count < maxRunners)
            {
                SpawnRunner();
            }

            // 破棄されたランナーをリストから削除
            activeRunners.RemoveAll(runner => runner == null);
            
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    private void SpawnRunner()
    {
        Vector3 spawnPosition = GetRandomLanePosition();
        GameObject runner = Instantiate(runnerPrefab, spawnPosition, Quaternion.identity);
        runner.GetComponent<RunnerAI>().SetLanePosition(spawnPosition);
        
        
        activeRunners.Add(runner);
        StartCoroutine(DestroyRunnerAfterDistance(runner));
    }

    private IEnumerator DestroyRunnerAfterDistance(GameObject runner)
    {
        while (runner != null)
        {
            if (runner.transform.position.z -spawnPos.position.z < -330f)
            {
                activeRunners.Remove(runner);  // リストからも削除
                Destroy(runner);
                break;
            }
            yield return new WaitForSeconds(1f);
        }
    }

    private Vector3 GetRandomLanePosition()
    {
        int randomIndex = Random.Range(0, lanes.Length);
        return lanes[randomIndex];
    }

    // スポーン停止用のメソッドを追加
    public void StopSpawning()
    {
        isSpawning = false;
    }
}
