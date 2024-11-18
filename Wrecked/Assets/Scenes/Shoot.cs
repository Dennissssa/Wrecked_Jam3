using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement; // 用于场景管理

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public int score = 0;
    public TextMeshProUGUI scoreText; // 显示分数的 TextMesh Pro 文本

    public GameObject cannonballPrefab; // 炮弹 Prefab
    public Transform firePoint;          // 炮口位置
    public float launchForce = 500f;     // 发射力度
    public float rotationSpeed = 100f;   // 转向速度
    public TMP_Text ammoCountText;       // 显示弹药数量的 TextMesh Pro 文本

    private int ammoCount = 10;          // 初始弹药数量
    private bool isCountingDown = false; // 是否正在倒计时

    public string successSceneName = "SuccessScene"; // 成功场景名称
    public string failureSceneName = "FailureScene"; // 失败场景名称
    public int scoreThreshold = 100; // 成功分数阈值

    public AudioClip fireSound;        // 发射音效
    public AudioClip successSound;     // 成功音效
    public AudioClip failureSound;     // 失败音效

    private AudioSource audioSource;   // 音频源

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            audioSource = gameObject.AddComponent<AudioSource>(); // 添加音频源
        }
        else
        {
            Destroy(gameObject); // 销毁多余的实例
        }
    }

    void Start()
    {
        UpdateScoreText();
        UpdateAmmoCountText();
    }

    void Update()
    {
        HandleRotation(); // 处理炮管转向
        if (Input.GetKeyDown(KeyCode.Space) && ammoCount > 0)
        {
            FireCannonball(); // 发射炮弹
        }

        // 检查弹药数量是否为零
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
        if (score >= scoreThreshold && !isCountingDown) // 检查分数是否达到阈值
        {
            isCountingDown = true;
            StartCoroutine(CountdownAndSwitchScene());
        }
    }

    public void ResetScore() // 新方法来重置分数
    {
        score = 0;
        UpdateScoreText();
    }

    void UpdateScoreText()
    {
        if (scoreText != null)
        {
            scoreText.text = "Score: " + score; // 更新分数文本
        }
        else
        {
            Debug.LogError("ScoreText is not assigned in the Inspector!");
        }
    }

    void HandleRotation()
    {
        float horizontalInput = Input.GetAxis("Horizontal"); // 获取水平输入
        float rotationAmount = horizontalInput * rotationSpeed * Time.deltaTime;

        // 直接更新炮口的旋转角度
        firePoint.Rotate(0, rotationAmount, 0);
    }

    void FireCannonball()
    {
        GameObject cannonball = Instantiate(cannonballPrefab, firePoint.position, firePoint.rotation);
        Rigidbody rb = cannonball.GetComponent<Rigidbody>();
        rb.AddForce(firePoint.forward * launchForce);

        ammoCount--; // 发射后减少弹药数量
        UpdateAmmoCountText(); // 更新弹药计数显示

        PlaySound(fireSound); // 播放发射音效

        StartCoroutine(DestroyCannonballAfterDelay(cannonball, 3f)); // 调用协程销毁炮弹
    }

    private System.Collections.IEnumerator DestroyCannonballAfterDelay(GameObject cannonball, float delay)
    {
        yield return new WaitForSeconds(delay); // 等待指定时间
        Destroy(cannonball); // 销毁炮弹
    }

    void UpdateAmmoCountText()
    {
        if (ammoCountText != null)
        {
            ammoCountText.text = "Ammo: " + ammoCount; // 更新弹药数量文本
        }
        else
        {
            Debug.LogError("AmmoCountText is not assigned in the Inspector!");
        }
    }

    private System.Collections.IEnumerator CountdownAndSwitchScene()
    {
        yield return new WaitForSeconds(3f);

        // 根据分数决定加载的场景
        if (score >= scoreThreshold)
        {
            PlaySound(successSound); // 播放成功音效
            yield return new WaitForSeconds(1f); // 等待一秒
            SceneManager.LoadScene(successSceneName); // 加载成功场景
        }
        else
        {
            PlaySound(failureSound); // 播放失败音效
            yield return new WaitForSeconds(1f);
            SceneManager.LoadScene(failureSceneName); // 加载失败场景
        }
    }

    private void PlaySound(AudioClip clip)
    {
        if (clip != null && audioSource != null)
        {
            audioSource.PlayOneShot(clip); // 播放音效
        }
    }
}