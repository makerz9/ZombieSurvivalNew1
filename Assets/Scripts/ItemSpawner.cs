using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ItemSpawner : MonoBehaviour
{
    public GameObject[] itemPrefabs;
    private List<GameObject>[] itemPools;
    public float minX = -10f;
    public float maxX = 10f;
    public float minZ = -10f;
    public float maxZ = 10f;

    public float interval = 3f;

    private float timer = 0f;

    private void Awake()
    {
        itemPools = new List<GameObject>[itemPrefabs.Length];
        for(int i=0; i < itemPools.Length; ++i)
        {
            itemPools[i] = new List<GameObject>();

        }
    }


    private void Update()
    {
        timer += Time.deltaTime;
        if(timer > interval)
        {
            timer = 0f;
            int index = Random.Range(0, itemPrefabs.Length);
            if(itemPools[index].Count == 0)
            {
                GameObject newItemGo = Instantiate(itemPrefabs[index]);
                itemPools[index].Add(newItemGo);

                newItemGo.SetActive(false);
                IItem newItem = newItemGo.GetComponent<IItem>();
                if(newItem != null)
                {
                    newItem.OnUse.AddListener(() => itemPools[index].Add(newItemGo) );
                }
            }
    
            GameObject prefab = itemPrefabs[Random.Range(0, itemPrefabs.Length)];
            Vector3 position = Vector3.zero;
            position.y = 0.5f;
            position.x = Random.Range(minX, maxX);
            position.z = Random.Range(minZ, maxZ);

            if(NavMesh.SamplePosition(position, out NavMeshHit hit, 10f, NavMesh.AllAreas))
            {
                position = hit.position;
                position.y = 0.5f;;
                //Instantiate(prefab, hit.position + Vector3.up * 1.0f, Quaternion.identity); 
                GameObject newGo = itemPools[index][0];
                newGo.transform.position = position;
                newGo.SetActive(true);

                itemPools[index].RemoveAt(0);   
            }
            else
            {
                Debug.Log("실패");
            }

        }
    }



    #region 


    // public enum SpawnShape
    // {
    //     Circle,
    //     Box
    // }

    // public GameObject[] ItemList;

    // private int createCount = 3;
    // public float spawnRadius = 20f;

    // public SpawnShape spawnShape = SpawnShape.Circle;
    // public Vector2 boxSize = new Vector2(0,0);
    // public Vector3 spawningOffset = new Vector3(0,0,0);

    // // public Vector3 FindValidNavMeshSpawnPoint(Vector3 center, float radius)
    // // {
    // //     Vector3 randomPoint;

    // //     switch(spawnShape)
    // //     {
    // //         case SpawnShape.Circle;
    // //         randomPoint = center * Random.ins
    // //     }

    // // }

    // public UIManager uiManager;

    // private void OnDrawGizmosSelected()
    // {
    //     Gizmos.color = Color.green;

    //     switch(spawnShape)
    //     {
    //         case SpawnShape.Circle:
    //             Gizmos.DrawWireSphere(transform.position, spawnRadius);
    //         break;
    //         case SpawnShape.Box:
    //             Vector3 size = new Vector3(boxSize.x, 0, boxSize.y);
    //         Gizmos.DrawWireCube(transform.position, size);
    //         break;
    //         default:
    //             break;
    //     }
    // }


    // // Start is called once before the first execution of Update after the MonoBehaviour is created




    // private IEnumerator ItemSpawn()
    // {
    //     yield  return new WaitForSeconds(5f);

    // }
    #endregion


}