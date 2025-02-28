using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMannger : MonoBehaviour
{
    [SerializeField] private GameObject playerPrefab;  // プレイヤーのプレハブ
    [SerializeField] private Transform spawnPoint;     // プレイヤーの初期スポーン位置
    [SerializeField] private UnityEngine.UI.Slider heatStrokeBar; // 熱中症ゲージのUIスライダー
    [SerializeField] private GameObject inventoryPanel; // インベントリパネルのUI
    [SerializeField] private CameraPos cameraPos; // プレイヤーを追従するカメラの制御コンポーネント
    public string targetLightName = "Directional Light"; // 影の計算に使用する太陽光源の名前

    [Header("Speed Increase Settings")]
    [SerializeField] private float speedIncreaseInterval = 60f; // プレイヤーの速度が増加する間隔(秒)
    [SerializeField] private float speedIncreaseAmount = 1.5f; // 1回の速度増加量
    private float speedIncreaseTimer = 0f; // 速度増加までの経過時間

    private float distanceFromSpawn; // プレイヤーがスポーン地点から移動した距離
    private Transform playerTransform; // プレイヤーの位置情報
    private LaneMovement playerMovement; // プレイヤーの移動制御コンポーネント
    private TerrainSlicing terrainSlicing;
    private ItemSpawner itemSpawner;
    private RandomMapGena randomMapGena; // RandomMapGenaのインスタンス
    private RandomOBSGena randomOBSGena; // RandomOBSGenaのインスタンス
    private float currentDistance; // プレイヤーの現在位置

    private float increaseDistance = 1000f; // 速度を増加させる距離間隔

    private float DistanceIncreasedForward = 0f; // 速度を増加させるための距離

    void Awake()
    {
        SpawnPlayer(); // ゲーム開始時にプレイヤーを生成
        terrainSlicing = GetComponent<TerrainSlicing>();
        if(terrainSlicing != null){
            SetTerrain();
        }
        itemSpawner = GetComponent<ItemSpawner>();
        if(itemSpawner != null){
           itemSpawner.SetPlayerTransform(playerTransform);
        }
        randomMapGena = GetComponent<RandomMapGena>();
        if(randomMapGena != null){
            randomMapGena.SetPlayer(playerTransform);
        }
        randomOBSGena = GetComponent<RandomOBSGena>();
    }

    void Update()
    {
        // プレイヤーと初期位置の参照が有効な場合のみ処理を実行
        if (playerTransform != null && spawnPoint != null)
        {
            currentDistance = playerTransform.position.z - spawnPoint.position.z; // プレイヤーの現在位置からスポーン地点までの距離を計算
            // スポーン地点からプレイヤーまでの直線距離を計算
            distanceFromSpawn = Vector3.Distance(spawnPoint.position, playerTransform.position);
            // イベントを発火（デバッグログを追加）
            EventHandler.CallUpdateDistanceEvent(distanceFromSpawn);
            if ( currentDistance - DistanceIncreasedForward >= increaseDistance)
            {
                IncreasePlayerSpeed();
                DistanceIncreasedForward = currentDistance;
            }
        }
    }

    // プレイヤーの移動速度を増加させる処理
    private void IncreasePlayerSpeed()
    {
        
        if (playerMovement != null)
        {
            playerMovement.IncreaseSpeed(speedIncreaseAmount);
            Debug.Log("Speed Increased!");
        }
        
    }

    private void SetTerrain(){
        terrainSlicing.player = playerTransform;
    }

    // プレイヤーを生成し、必要なコンポーネントを初期化する処理
    private void SpawnPlayer()
    {
        if (playerPrefab != null && spawnPoint != null)
        {
            // プレイヤーをスポーン位置に生成
            GameObject player = Instantiate(playerPrefab, spawnPoint.position, spawnPoint.rotation);
            playerTransform = player.transform; // プレイヤーの位置情報を保存

            // レーン移動の制御コンポーネントを取得し初期化
            LaneMovement laneMovement = player.GetComponent<LaneMovement>();

            laneMovement.SetStartPoint(spawnPoint);


            // 熱中症システムの制御コンポーネントを取得し初期化
            HeatStroke heatStroke = player.GetComponent<HeatStroke>();

            heatStroke.SetStrokeBar(heatStrokeBar);


            // インベントリシステムの制御コンポーネントを取得し初期化
            Inventory inventory = player.GetComponent<Inventory>();

            inventory.SetInventoryPanel(inventoryPanel);


            // 影判定の制御コンポーネントを取得し、光源を設定
            ShadowCollider shadowCollider = player.GetComponent<ShadowCollider>();

            Light[] lights = FindObjectsOfType<Light>();
            foreach (Light light in lights)
            {
                if (light.name == targetLightName)
                {
                    shadowCollider.lightTarget = light.transform;
                    break;
                }
            }


            // カメラの追従対象をプレイヤーに設定

            cameraPos.target = player.transform;


            // プレイヤーの移動制御コンポーネントを保存し初期化
            playerMovement = player.GetComponent<LaneMovement>();

            playerMovement.SetStartPoint(spawnPoint);

            // RandomOBSGenaにプレイヤーを設定
            randomOBSGena = GetComponent<RandomOBSGena>();
            if (randomOBSGena != null)
            {
                randomOBSGena.SetPlayer(player);
            }
        }
    }
}
