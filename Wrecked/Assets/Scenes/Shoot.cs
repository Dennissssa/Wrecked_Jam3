using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement; // ���ڳ�������

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public int score = 0;
    public TextMeshProUGUI scoreText; // ��ʾ������ TextMesh Pro �ı�

    public GameObject cannonballPrefab; // �ڵ� Prefab
    public Transform firePoint;          // �ڿ�λ��
    public float launchForce = 500f;     // ��������
    public float rotationSpeed = 100f;   // ת���ٶ�
    public TMP_Text ammoCountText;       // ��ʾ��ҩ������ TextMesh Pro �ı�

    private int ammoCount = 10;          // ��ʼ��ҩ����
    private bool isCountingDown = false; // �Ƿ����ڵ���ʱ

    public string successSceneName = "SuccessScene"; // �ɹ���������
    public string failureSceneName = "FailureScene"; // ʧ�ܳ�������
    public int scoreThreshold = 100; // �ɹ�������ֵ

    public AudioClip fireSound;        // ������Ч
    public AudioClip successSound;     // �ɹ���Ч
    public AudioClip failureSound;     // ʧ����Ч

    private AudioSource audioSource;   // ��ƵԴ

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            audioSource = gameObject.AddComponent<AudioSource>(); // �����ƵԴ
        }
        else
        {
            Destroy(gameObject); // ���ٶ����ʵ��
        }
    }

    void Start()
    {
        UpdateScoreText();
        UpdateAmmoCountText();
    }

    void Update()
    {
        HandleRotation(); // �����ڹ�ת��
        if (Input.GetKeyDown(KeyCode.Space) && ammoCount > 0)
        {
            FireCannonball(); // �����ڵ�
        }

        // ��鵯ҩ�����Ƿ�Ϊ��
        if (ammoCount <= 0 && !isCountingDown)
        {
            isCountingDown = true;
            StartCoroutine(CountdownAndSwitchScene());
        }
    }

    public void AddScore(int points)
    {
        score += points;
        UpdateScoreText();
        if (score >= scoreThreshold && !isCountingDown) // �������Ƿ�ﵽ��ֵ
        {
            isCountingDown = true;
            StartCoroutine(CountdownAndSwitchScene());
        }
    }

    public void ResetScore() // �·��������÷���
    {
        score = 0;
        UpdateScoreText();
    }

    void UpdateScoreText()
    {
        if (scoreText != null)
        {
            scoreText.text = "Score: " + score; // ���·����ı�
        }
        else
        {
            Debug.LogError("ScoreText is not assigned in the Inspector!");
        }
    }

    void HandleRotation()
    {
        float horizontalInput = Input.GetAxis("Horizontal"); // ��ȡˮƽ����
        float rotationAmount = horizontalInput * rotationSpeed * Time.deltaTime;

        // ֱ�Ӹ����ڿڵ���ת�Ƕ�
        firePoint.Rotate(0, rotationAmount, 0);
    }

    void FireCannonball()
    {
        GameObject cannonball = Instantiate(cannonballPrefab, firePoint.position, firePoint.rotation);
        Rigidbody rb = cannonball.GetComponent<Rigidbody>();
        rb.AddForce(firePoint.forward * launchForce);

        ammoCount--; // �������ٵ�ҩ����
        UpdateAmmoCountText(); // ���µ�ҩ������ʾ

        PlaySound(fireSound); // ���ŷ�����Ч

        StartCoroutine(DestroyCannonballAfterDelay(cannonball, 3f)); // ����Э�������ڵ�
    }

    private System.Collections.IEnumerator DestroyCannonballAfterDelay(GameObject cannonball, float delay)
    {
        yield return new WaitForSeconds(delay); // �ȴ�ָ��ʱ��
        Destroy(cannonball); // �����ڵ�
    }

    void UpdateAmmoCountText()
    {
        if (ammoCountText != null)
        {
            ammoCountText.text = "Ammo: " + ammoCount; // ���µ�ҩ�����ı�
        }
        else
        {
            Debug.LogError("AmmoCountText is not assigned in the Inspector!");
        }
    }

    private System.Collections.IEnumerator CountdownAndSwitchScene()
    {
        yield return new WaitForSeconds(3f);

        // ���ݷ����������صĳ���
        if (score >= scoreThreshold)
        {
            PlaySound(successSound); // ���ųɹ���Ч
            yield return new WaitForSeconds(1f); // �ȴ�һ��
            SceneManager.LoadScene(successSceneName); // ���سɹ�����
        }
        else
        {
            PlaySound(failureSound); // ����ʧ����Ч
            yield return new WaitForSeconds(1f);
            SceneManager.LoadScene(failureSceneName); // ����ʧ�ܳ���
        }
    }

    private void PlaySound(AudioClip clip)
    {
        if (clip != null && audioSource != null)
        {
            audioSource.PlayOneShot(clip); // ������Ч
        }
    }
}