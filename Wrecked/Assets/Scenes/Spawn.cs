using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject objectToSpawn; // 要生成的物体
    public float spawnInterval = 2f;  // 生成间隔时间（秒）

    private void Start()
    {
        // 启动协程
        StartCoroutine(SpawnObject());
    }

    private System.Collections.IEnumerator SpawnObject()
    {
        while (true) // 永久循环
        {
            Spawn();
            yield return new WaitForSeconds(spawnInterval); // 等待设定的时间
        }
    }

    private void Spawn()
    {
        if (objectToSpawn != null)
        {
            Instantiate(objectToSpawn, transform.position, transform.rotation); // 在当前物体位置生成新物体
        }
        else
        {
            Debug.LogWarning("Object to spawn is not assigned in the Inspector!");
        }
    }
}