using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject objectToSpawn; // Ҫ���ɵ�����
    public float spawnInterval = 2f;  // ���ɼ��ʱ�䣨�룩

    private void Start()
    {
        // ����Э��
        StartCoroutine(SpawnObject());
    }

    private System.Collections.IEnumerator SpawnObject()
    {
        while (true) // ����ѭ��
        {
            Spawn();
            yield return new WaitForSeconds(spawnInterval); // �ȴ��趨��ʱ��
        }
    }

    private void Spawn()
    {
        if (objectToSpawn != null)
        {
            Instantiate(objectToSpawn, transform.position, transform.rotation); // �ڵ�ǰ����λ������������
        }
        else
        {
            Debug.LogWarning("Object to spawn is not assigned in the Inspector!");
        }
    }
}