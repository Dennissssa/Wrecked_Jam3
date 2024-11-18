using UnityEngine;

public class NPCMovement : MonoBehaviour
{
    public Transform[] waypoints; // 目标点数组
    public float speed = 2f; // 移动速度
    private int currentWaypointIndex = 0; // 当前目标点索引
    private bool isMoving = true; // 控制NPC是否移动

    public Material newMaterial; // 用于改变材质的公共变量
    private bool hasChangedMaterial = false; // 标志，表示是否已经改变过材质

    private GameManager gameManager; // 计分管理器引用
    public AudioClip collisionSound; // 碰撞音效
    private AudioSource audioSource; // 音源组件

    void Start()
    {
        // 获取场景中的 GameManager
        gameManager = GameManager.Instance; // 使用 GameManager 的单例

        // 获取 AudioSource 组件
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = collisionSound; // 设置音效
    }

    void Update()
    {
        if (isMoving)
        {
            MoveNPC();
        }
    }

    void MoveNPC()
    {
        if (waypoints.Length == 0) return;

        Transform targetWaypoint = waypoints[currentWaypointIndex];
        transform.position = Vector3.MoveTowards(transform.position, targetWaypoint.position, speed * Time.deltaTime);

        if (Vector3.Distance(transform.position, targetWaypoint.position) < 0.1f)
        {
            currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        Debug.Log($"Collided with: {collision.gameObject.name}"); // 打印碰撞对象的名称

        if (collision.gameObject.CompareTag("Player") && !hasChangedMaterial) // 检查是否与 Player 碰撞且未改变过材质
        {
            ChangeMaterial(newMaterial); // 改变为新的材质
            hasChangedMaterial = true; // 设置标志为已改变
            gameManager.AddScore(15); // 增加分数
            isMoving = false; // 停止移动
            PlayCollisionSound(); // 播放碰撞音效
        }
    }

    void ChangeMaterial(Material newMaterial)
    {
        Renderer renderer = GetComponent<Renderer>();
        if (renderer != null)
        {
            renderer.material = newMaterial; // 改变材质
        }
    }

    void PlayCollisionSound()
    {
        if (audioSource != null && collisionSound != null)
        {
            audioSource.Play(); // 播放音效
        }
    }
}