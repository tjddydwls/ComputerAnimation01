using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    GameObject ItemPrefab;
    
    [SerializeField]
    float ItemSpawnInterval = 2f;

    [SerializeField]
    Vector2 ItemSpawnYRange = new Vector2(-2f, 2f);

    [SerializeField]
    Vector2 ItemSpawnXRange = new Vector2(-2f, 2f);

    [SerializeField]
    Transform ItemParent;

    float ItemSpawnTimer;

    // Update is called once per frame
    void Update()
    {
        if (ItemPrefab == null)
            return;   

        ItemSpawnTimer += Time.deltaTime;

        if (ItemSpawnTimer < ItemSpawnInterval)
            return;
            
        ItemSpawnTimer = 0f;
        SpawnItem();
    }

    void SpawnItem()
    {
        float spawnX = Random.Range(ItemSpawnXRange.x, ItemSpawnXRange.y);
        float spawnY = Random.Range(ItemSpawnYRange.x, ItemSpawnYRange.y);
        Vector3 spawnPosition = new Vector3(spawnX, spawnY, 0f);

        GameObject item = Instantiate(ItemPrefab, spawnPosition, Quaternion.identity, ItemParent);
    }   
}
