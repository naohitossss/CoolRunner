using UnityEngine;

// カメラの動きを制御するクラス
[RequireComponent(typeof(Camera))]
public class CameraPos : MonoBehaviour
{
    // 追跡対象のプレイヤー
    [Header("追跡対象")]
    public Transform target = null;

    // カメラの各種設定
    [Header("マウス感度")]
    [Range(0, 1)]
    public float linearSpeed = 1;

    [Header("カメラとプレイヤーの距離")]
    [Range(10, 20)]
    public float distanceFromTarget = 10f;

    [Header("カメラの移動速度")]
    [Range(1, 50)]
    public float speed = 5;

    [Header("X軸オフセット")]
    public float xOffset = 0.5f;

    [Header("カメラの固定角度")]
    public Vector3 fixedRotation = new Vector3(20f, 0f, 0f);

     [Header("カメラの高さ")]
     public float yOffset = 5f;

    public float baseFOV = 60f;
    public float maxFOV = 90f;
    [SerializeField] private float speedMultiplier = 0.2f;
    private float currentSpeed = 0f;
    private LaneMovement targetLaneMovement;
    private float defaultSpeed;

    // 初期設定
    void Start()
    {
        if (target != null)
        {
            targetLaneMovement = target.GetComponent<LaneMovement>();
            defaultSpeed = targetLaneMovement.moveSpeed;
            gameObject.layer = target.gameObject.layer = 2;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;            
            currentSpeed = defaultSpeed;
        }
    }
    public void SetFollowSpeed(float speed)
    {
        this.speed = speed;
    }

    // カメラの位置更新
    private void LateUpdate()
    {
        if (target != null)
        {
            Quaternion targetRotation = Quaternion.Euler(fixedRotation);
            
            // カメラの目標位置を計算
            Vector3 targetPosition = target.position + targetRotation * new Vector3(xOffset, yOffset, -distanceFromTarget) + target.GetComponent<CharacterController>().center * 1.75f;

            // カメラの位置と回転を滑らかに更新
            transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * currentSpeed);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * 25f);
        }

        // カメラの視野角を計算
        float targetFOV = baseFOV + (currentSpeed * speedMultiplier);
        
        // カメラの視野角を滑らかに更新
        GetComponent<Camera>().fieldOfView = Mathf.Lerp(GetComponent<Camera>().fieldOfView, Mathf.Clamp(targetFOV, baseFOV, maxFOV), Time.deltaTime * 5f);
    }

    // カーソルの表示/非表示を切り替え
    public void CursorArise()
    {
        if (Input.GetKeyUp(KeyCode.Escape) && Cursor.visible == false)
        {
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true;
        }
    }

    // カメラを指定位置に瞬間移動
    public void Teleport(Vector3 position, Quaternion rotation)
    {
        transform.position = position;
        transform.rotation = rotation;
    }
}
