using System.Collections;
using System.Collections.Generic;
using MGame.Save;
using UnityEngine;

public class TopViewPlayerControl : MonoBehaviour, ISaveable
{
    Rigidbody2D rigidbody2d;
    Animator animator;

    private Collider2D inveractColl;
    // public Transform movePoint;
    public Sprite defaultSprite;
    // speed(基础速度),moveSpeed(走路速度),quickMoveSpeed(加速速度)
    public float speed, moveSpeed, quickMoveSpeed;
    public float inputX, inputY;
    public bool horizontalMove = false, verticalMove = false;   // 水平移动、垂直移动
    float stopX, stopY;
    public float moveCurrentTime;  // 移动间隔时间
    public float invokeTime;   // 移动计时
    public bool isMove = true;

    public string GUID => GetComponent<DataGUID>().guid;

    private void Awake()
    {
        invokeTime = moveCurrentTime;   // 移动间隔
        moveSpeed = speed;              // 移动速度
        // Transform movePoint的父对象为空
        // movePoint.parent = null;

        rigidbody2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        inveractColl = transform.GetChild(0).GetComponent<Collider2D>();
    }

    private void OnEnable()
    {
        EventHandler.BeforeSceneUnloadEvent += OnBeforeSceneUnloadEvent;
        EventHandler.AfterSceneLoadedEvent += OnAfterSceneLoadedEvent;
        EventHandler.MoveToPosition += OnMoveToPosition;
        EventHandler.StartNewGameEvent += OnStartNewGameEvent;
    }

    private void OnDisable()
    {
        EventHandler.BeforeSceneUnloadEvent -= OnBeforeSceneUnloadEvent;
        EventHandler.AfterSceneLoadedEvent -= OnAfterSceneLoadedEvent;
        EventHandler.MoveToPosition -= OnMoveToPosition;
        EventHandler.StartNewGameEvent -= OnStartNewGameEvent;
    }

    void Start()
    {
        ISaveable saveable = this;
        saveable.RegisterSaveable();
    }

    private void OnMoveToPosition(Vector3 pos)
    {
        transform.position = pos;
    }

    private void OnAfterSceneLoadedEvent()
    {
        isMove = true;
    }

    private void OnBeforeSceneUnloadEvent()
    {
        isMove = false;
    }

    private void OnStartNewGameEvent(int index)
    {
        transform.position = new Vector2(4, 0);
    }

    void Update()
    {
        // 角色移动
        if (isMove && !DialogueUI.Instance.dialoguePanel.activeSelf)
        {
            rigidbody2d.constraints = RigidbodyConstraints2D.FreezeRotation;
            Move();
        }
        else
        {
            rigidbody2d.constraints = RigidbodyConstraints2D.FreezeAll;
            animator.SetBool("isMove", false);
        }

    }

    void Move()
    {
        if (Input.GetButtonDown("Quick"))
        {
            moveSpeed = quickMoveSpeed;
            animator.speed = 2;
        }
        if (Input.GetButtonUp("Quick"))
        {
            moveSpeed = speed;
            animator.speed = 1;
        }

        // 获取玩家输入的水平方向值 -1 0 1
        inputX = Input.GetAxisRaw("Horizontal");
        // 获取玩家输入的垂直方向值 -1 0 1
        inputY = Input.GetAxisRaw("Vertical");

        Vector2 input = new Vector2(inputX, inputY).normalized;

        rigidbody2d.velocity = input * moveSpeed;

        if (input != Vector2.zero)
        {
            animator.SetBool("isMove", true);
            animator.SetFloat("inputX", inputX);
            animator.SetFloat("inputY", inputY);
        }
        else
        {
            animator.SetBool("isMove", false);
        }


        #region 角色跟随movePoint移动
        /*
        // 角色跟随movePoint移动
        transform.position = Vector3.MoveTowards(transform.position, movePoint.position, moveSpeed * Time.deltaTime);

        // movePoint和Player之间的距离 小于等于0.05f时停止移动
        if (Vector3.Distance(transform.position, movePoint.position) <= 0.05f)
        {
            animator.SetBool("isMove", false);

            // 获取玩家输入的水平方向值 -1 0 1
            inputX = Input.GetAxisRaw("Horizontal");
            // 获取玩家输入的垂直方向值 -1 0 1
            inputY = Input.GetAxisRaw("Vertical");
            
            // 按下方向键时
            if (inputX != 0 || inputY != 0)
            {
                // 按键按下时进行计时
                invokeTime += Time.deltaTime;
                // 间隔时间未大于自定义时间执行
                if (!(invokeTime - moveCurrentTime > 0.11f)) 
                {
                    animator.SetFloat("inputX", inputX);
                    animator.SetFloat("inputY", inputY);
                }
                else
                {
                    // 上下移动
                    if (Mathf.Abs(inputY) == 1f)
                    {
                        // 上下移动中按下左右移动键
                        if (Mathf.Abs(inputX) == 1f)
                        {
                            // 判断是否是为垂直移动状态
                            if (verticalMove){
                                // 向水平方向移动
                                // transform.position += new Vector3(inputX, 0f, 0f);
                                movePoint.position += new Vector3(inputX, 0f, 0f);
                                stopY = 0;
                                stopX = inputX;
                            }else{
                                // 不是垂直状态则向垂直方向移动
                                // transform.position += new Vector3(0f, inputY, 0f);
                                movePoint.position += new Vector3(0f, inputY, 0f);
                                stopX = 0;
                                stopY = inputY;
                                horizontalMove = true;
                            }
                        }
                        // 
                        else{
                            // transform.position += new Vector3(0f, inputY, 0f);
                            movePoint.position += new Vector3(0f, inputY, 0f);
                            stopX = 0;
                            stopY = inputY;
                            horizontalMove = false;
                            verticalMove = true;
                        }
                    }
                    // 左右移动
                    else if (Mathf.Abs(inputX) == 1f)
                    {
                        // 上下移动中按下左右移动键
                        if (Mathf.Abs(inputY) == 1f)
                        {
                            // 判断是否是为水平移动状态
                            if (horizontalMove){
                                // 向垂直方向移动
                                // transform.position += new Vector3(0f, inputY, 0f);
                                movePoint.position += new Vector3(0f, inputY, 0f);
                                stopX = 0;
                                stopY = inputY;
                            }else{
                                // 不是水平状态则向水平方向移动
                                // transform.position += new Vector3(inputX, 0f, 0f);
                                movePoint.position += new Vector3(inputX, 0f, 0f);
                                stopY = 0;
                                stopX = inputX;
                                verticalMove = true;
                            }
                        }
                        // 
                        else{
                            // transform.position += new Vector3(inputX, 0f, 0f);
                            movePoint.position += new Vector3(inputX, 0f, 0f);
                            stopY = 0;
                            stopX = inputX;
                            horizontalMove = true;
                            verticalMove = false;
                        }
                    }
                }
            }
            // 没有按下按键时
            else
            {
                invokeTime = moveCurrentTime;
            }
        }
        // 移动中
        else
        {
            animator.SetFloat("inputX", stopX);
            animator.SetFloat("inputY", stopY);
            animator.SetBool("isMove", true);
        }
        */
        #endregion
    }


    public GameSaveData GenerateSaveData()
    {
        GameSaveData saveData = new GameSaveData();
        // 保存角色位置
        saveData.characterPosDict = new Dictionary<string, SerializableVector3>();
        saveData.characterPosDict.Add(this.name, new SerializableVector3(transform.position));
        return saveData;
    }

    public void RestoreLoadData(GameSaveData saveData)
    {
        var targetPosition = saveData.characterPosDict[this.name].ToVector3();

        transform.position = targetPosition;
    }
}
