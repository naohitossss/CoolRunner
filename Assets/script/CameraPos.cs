using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraPos : MonoBehaviour
{
    [Header("要跟随的人物")]
    public Transform target = null;

    [Header("鼠标滑动速度")]
    [Range(0, 1)]
    public float linearSpeed = 1;
    [Header("摄像机与玩家距离")]
    [Range(15, 20)]
    public float distanceFromTarget = 5;
    [Header("摄像机速度")]
    [Range(1, 50)]
    public float speed = 5;
    [Header("x轴偏向量")]
    public float xOffset = 0.5f;

    [Header("カメラの固定角度")]
    public Vector3 fixedRotation = new Vector3(30f, 0f, 0f);

    private float yMouse;
    private float xMouse;

    // Start is called before the first frame update
    void Start()
    {
        if (target != null)
        {
            gameObject.layer = target.gameObject.layer = 2;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    private void LateUpdate()
    {
        if (target != null)
        {
            Quaternion targetRotation = Quaternion.Euler(fixedRotation);
            
            // CamCheckを使用せず、直接距離を計算
            Vector3 targetPosition = target.position + targetRotation * new Vector3(xOffset, 0, -distanceFromTarget) + target.GetComponent<CharacterController>().center * 1.75f;

            transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * speed);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * 25f);
        }
    }

    public void CursorArise()//cursor可视不可视
    {
        if (Input.GetKeyUp(KeyCode.Escape) && Cursor.visible == false)
        {
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true;
        }
    }

    public void Teleport(Vector3 position, Quaternion rotation)
    {
        transform.position = position;
        transform.rotation = rotation;

        
    }
}
