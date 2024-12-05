using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.XR;

public class LaneMovement : MonoBehaviour
{
    private CharacterController controller;  // キャラクターコントローラーコンポーネント

    // 3つの移動可能な位置（レーン）
    private Vector3[] lanes = new Vector3[3]; // 0: 左, 1: 中央, 2: 右
    private int currentLane = 1;              // 現在のレーン位置（初期は中央）

    public float moveSpeed = 15f;             // 前進速度
    public float laneChangeSpeed = 12f;        // 横移動（レーン変更）の速度
    public float rotationSpeed = 12f;         // 回転速度
    public float slideSpeed = 20f;            // スライディング時の速度
    public float slideDuration = 0.9f;        // スライディングの持続時間
    private float slideTimer = 0f;            // スライディングのタイマー
    public float knockbackForce = 8f;       // ノックバックの強さ
    public float knockbackDuration = 2.7f;    // ノックバックの持続時間
    private float knockbackTimer = 0f;        // ノックバックの経過時間
    public float energyTimer;                 // エナジーアイテムの持続時間
    private Vector3 knockbackDirection;       // ノックバック方向のベクトル
    private bool isGrounded;                  // 地面に接地しているかどうか
    private HeatStroke heatStroke;           // 熱中症管理コンポーネント
    public ParticleSystem sweatParticles;

    public float jumpHeight = 13f;             // ジャンプの高さ
    public float gravity = -9.81f;              // 重力加速度
    private Vector3 velocity;                 // 現在の速度ベクトル

    public LayerMask groundLayer;             // 地面判定用のレイヤーマスク
    private Transform startPoint;              // スタート地点のTransform
    private Animator anim;                    // アニメーターコンポーネント
    private Vector3 moveDirection;            // 移動方向ベクトル
    private Vector3 targetLanePosition;       // 目標レーン位置

    public AudioSource jumpSound;             // ジャンプ効果音
    public AudioSource landSound;             // 着地効果音
    public AudioSource smashSound;            // 衝突効果音
    public AudioSource hitSound;              // ヒット効果音
    public AudioSource slideSound;            // スライディング効果音

    private enum CharacterState { Normal,  Knockback, Jumping, Sliding }  // キャラクターの状態を管理する列挙型
    private CharacterState state = CharacterState.Normal;  // 現在のキャラクター状態
    private ShadowCollider shadowCollider;     // 影判定用コンポーネント
    private bool isEnergy = false;

    public void SetStartPoint(Transform point)
    {
        startPoint = point;
        InitializeLanes();
    }

    private void InitializeLanes()
    {
        Vector3 center = startPoint.position;
        
        // レーンの位置を初期化
        lanes[0] = new Vector3(center.x - 4.45f, transform.position.y, transform.position.z);  // 左レーン
        lanes[1] = new Vector3(center.x, transform.position.y, transform.position.z);          // 中央レーン
        lanes[2] = new Vector3(center.x + 4.45f, transform.position.y, transform.position.z);  // 右レーン

        // プレイヤーを中央レーンに配置
        transform.position = lanes[currentLane];
    }

    void Start()
    {
        // 必要なコンポーネントの取得
        heatStroke = GetComponent<HeatStroke>();
        controller = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();
        shadowCollider = GetComponent<ShadowCollider>();
    }

    void Update()
    {
        energyTimer -= Time.deltaTime;
        if (energyTimer < 0){
            isEnergy = false;
        }
        if (shadowCollider.ifShadow == false && !isEnergy)
        {
            heatStroke.currentStroke += heatStroke.sunExposureRate * Time.deltaTime;
            if (!sweatParticles.isPlaying)
            {
                sweatParticles.Play();
            }
        }
        else if (shadowCollider.ifShadow == true && heatStroke.currentStroke > heatStroke.minStroke)
        {
            heatStroke.currentStroke -= heatStroke.shadeRecoveryRate * Time.deltaTime;
            if (sweatParticles.isPlaying)
            {
                sweatParticles.Stop();
            }
        }
        // 地面との接地判定
        isGrounded = Physics.CheckSphere(transform.position, 0.5f, groundLayer);

        // キャラクターの状態に応じた処理
        switch (state)
        {
            case CharacterState.Normal:  // 通常状態
                if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
                {
                    JumpStar();  // ジャンプ開始
                }
                else if (Input.GetKeyDown(KeyCode.LeftShift) && isGrounded)
                {
                    StartSlide();  // スライディング開始
                }

                // 前進移動
                moveDirection = Vector3.forward * moveSpeed;

                HandleLaneChange();  // レーン変更処理

                // 重力の適用
                velocity.y += gravity * Time.deltaTime;

                // 移動の実行
                controller.Move((moveDirection + velocity) * Time.deltaTime);

                RotateTowardsMovementDirection();  // キャラクターの向きを更新
                break;

            case CharacterState.Knockback:  // ノックバック状態
                knockbackTimer -= Time.deltaTime;
                bool afterJump = false;

                if (knockbackTimer > 2.2f)
                {
                    // ノックバック移動の実行
                    controller.Move((knockbackDirection) * knockbackForce * Time.deltaTime);
                }
                else if (knockbackTimer > 0f)
                {
                    // 一時停止中のレーン変更処理
                    controller.Move(Vector3.zero);
                    if (Input.GetKeyDown(KeyCode.A) && currentLane > 0)
                    {
                        currentLane--;
                        targetLanePosition = new Vector3(lanes[currentLane].x, transform.position.y, transform.position.z);
                    }
                    else if (Input.GetKeyDown(KeyCode.D) && currentLane < lanes.Length - 1)
                    {
                        currentLane++;
                        targetLanePosition = new Vector3(lanes[currentLane].x, transform.position.y, transform.position.z);
                    }
                    else if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
                    {
                        afterJump = true;
                    }
                }
                else
                {
                    // ノックバック状態の終了
                    state = CharacterState.Normal;
                    if (afterJump) state = CharacterState.Jumping;
                    anim.SetTrigger("EndKnockback"); 
                }
                break;

            case CharacterState.Jumping:  // ジャンプ状態
                if (isGrounded && velocity.y < 0)
                {
                    landSound.Play();  // 着地音の再生
                    velocity.y = -2f;
                    state = CharacterState.Normal;
                    anim.SetTrigger("Grand"); 
                }

                velocity.y += gravity * Time.deltaTime;
                HandleLaneChange();
                controller.Move((moveDirection + velocity) * Time.deltaTime);
                break;

            case CharacterState.Sliding:  // スライディング状態
                slideTimer -= Time.deltaTime;
                if (slideTimer <= 0)
                {
                    state = CharacterState.Normal;  // スライディング終了
                }

                moveDirection = Vector3.forward * slideSpeed;
                velocity.y += gravity * Time.deltaTime;
                controller.Move((moveDirection + velocity) * Time.deltaTime);
                break;
        }
    }

    // レーン変更の処理
    void HandleLaneChange()
    {
        // レーン変更入力の検出
        if (Input.GetKeyDown(KeyCode.A) && currentLane > 0)
        {
            currentLane--;
            targetLanePosition = new Vector3(lanes[currentLane].x, transform.position.y, transform.position.z);
        }
        else if (Input.GetKeyDown(KeyCode.D) && currentLane < lanes.Length - 1)
        {
            currentLane++;
            targetLanePosition = new Vector3(lanes[currentLane].x, transform.position.y, transform.position.z);
        }

        // レーン変更移動の実行
        controller.Move(new Vector3(lanes[currentLane].x - transform.position.x, 0, 0) * Time.deltaTime * laneChangeSpeed);
    }

    // エナジーアイテム効果の適用
    public void DrinkEnergy(float time)
    {
        energyTimer = time;
        isEnergy = true;
    }

    // キャラクターの回転処理
    void RotateTowardsMovementDirection()
    {
        Vector3 combinedMoveDirection = new Vector3(targetLanePosition.x - transform.position.x, 0, moveDirection.z);
        if (combinedMoveDirection != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(combinedMoveDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
        }
    }

    // ジャンプ開始処理
    void JumpStar() {
        jumpSound.Play();  // ジャンプ音の再生
        velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);  // ジャンプ速度の計算
        anim.SetTrigger("Jump");  // ジャンプアニメーション開始
        state = CharacterState.Jumping;
    }

    // 衝突判定処理
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject.CompareTag("Enemy") && state != CharacterState.Knockback)
        {
            // スライディング中の敵との衝突処理
            if(hit.gameObject.GetComponent<GetCollider>() != null && state == CharacterState.Sliding){
                hit.gameObject.GetComponent<GetCollider>().GetColliderOFF();
            }
            else{
                // 通常の敵との衝突処理
                smashSound.Play();
                anim.SetTrigger("Hit");
                hitSound.Play();
                knockbackDirection = -transform.forward;
                knockbackDirection.y = 0;
                heatStroke.currentStroke += 10f;  // 熱中症値の増加
                state = CharacterState.Knockback;
                knockbackTimer = knockbackDuration;
            }
        }
    }

    // スライディング開始処理
    void StartSlide()
    {   
        slideSound.Play();  // スライディング音の再生
        state = CharacterState.Sliding;
        slideTimer = slideDuration;
        anim.SetTrigger("Slide");  // スライディングアニメーション開始
    }

    // 速度を増加させるメソッド
    public void IncreaseSpeed(float amount)
    {
        moveSpeed *= amount;
        // スライディング速度も比例して増加
        slideSpeed = moveSpeed + 5f;
    }
}



