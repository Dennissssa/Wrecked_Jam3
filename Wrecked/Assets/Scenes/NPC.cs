using UnityEngine;

public class NPCMovement : MonoBehaviour
{
    public Transform[] waypoints; // Ŀ�������
    public float speed = 2f; // �ƶ��ٶ�
    private int currentWaypointIndex = 0; // ��ǰĿ�������
    private bool isMoving = true; // ����NPC�Ƿ��ƶ�

    public Material newMaterial; // ���ڸı���ʵĹ�������
    private bool hasChangedMaterial = false; // ��־����ʾ�Ƿ��Ѿ��ı������

    private GameManager gameManager; // �Ʒֹ���������
    public AudioClip collisionSound; // ��ײ��Ч
    private AudioSource audioSource; // ��Դ���

    void Start()
    {
        // ��ȡ�����е� GameManager
        gameManager = GameManager.Instance; // ʹ�� GameManager �ĵ���

        // ��ȡ AudioSource ���
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = collisionSound; // ������Ч
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
        Debug.Log($"Collided with: {collision.gameObject.name}"); // ��ӡ��ײ���������

        if (collision.gameObject.CompareTag("Player") && !hasChangedMaterial) // ����Ƿ��� Player ��ײ��δ�ı������
        {
            ChangeMaterial(newMaterial); // �ı�Ϊ�µĲ���
            hasChangedMaterial = true; // ���ñ�־Ϊ�Ѹı�
            gameManager.AddScore(15); // ���ӷ���
            isMoving = false; // ֹͣ�ƶ�
            PlayCollisionSound(); // ������ײ��Ч
        }
    }

    void ChangeMaterial(Material newMaterial)
    {
        Renderer renderer = GetComponent<Renderer>();
        if (renderer != null)
        {
            renderer.material = newMaterial; // �ı����
        }
    }

    void PlayCollisionSound()
    {
        if (audioSource != null && collisionSound != null)
        {
            audioSource.Play(); // ������Ч
        }
    }
}