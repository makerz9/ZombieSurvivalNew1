using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//Collections = 배열, 리스트, 딕셔너리 등등의 자료구조를 제공하는 네임스페이스
public class ZombieSpawner : MonoBehaviour
{
    public UIManager uiManager;
    public Zombie zombiePrefab;
    public ZombieData[] zombieDatas;
    public Transform[] spawnPoints;

    private readonly List<Zombie> zombies = new List<Zombie>();
    private readonly List<Zombie> zombiePool = new List<Zombie>();

    private int wave = 0;

    private void Update()
    {
        if (zombies.Count == 0)
        {
            SpawnWave();
        }
    }

    private void SpawnWave()
    {
        wave++;

        int count = Mathf.RoundToInt(wave * 1.5f);
        for(int i = 0; i < count; i++)
        {
            CreateZombie();
        }

        uiManager.SetWaveText(wave, zombies.Count);
    }

    private void CreateZombie()
    {
        Transform point = spawnPoints[Random.Range(0, spawnPoints.Length)];
        ZombieData data = zombieDatas[Random.Range(0, zombieDatas.Length)];

        Zombie zombie;
        //좀비풀 비었나 확인
        //비었으면
        if(zombiePool.Count == 0)
        {   //생성
            zombie = Instantiate(zombiePrefab, point.position, point.rotation);
            //비활성화하고
            zombie.gameObject.SetActive(false);
            //풀에 넣어버림
            zombiePool.Add(zombie);
        }

        //이게 붙어있는 오브젝트를 생성하는거임
        //Zombie zombie = Instantiate(zombiePrefab, point.position, point.rotation);

        //활성화할때 정해줘야함
        zombie = zombiePool[0];
        zombie.Setup(data);//데이터 초기화
        zombie.transform.position = point.position;
        zombie.transform.rotation = point.rotation;
        zombie.gameObject.SetActive(true);
        zombies.Add(zombie);
        //뽑아썼으니 제거
        zombiePool.Remove(zombie);

        //zombies.Remove(zombie);

        //반환형도 없는거 참조
        zombie.OnDead.AddListener(() => zombies.Remove(zombie));
        zombie.OnDead.AddListener(() => zombiePool.Add(zombie));
        //zombie.OnDead.AddListener(() => Destroy(zombie.gameObject, 5f));
        zombie.OnDead.AddListener(() => uiManager.SetWaveText(wave, zombies.Count));
    }

    private IEnumerator CoReturnToPoolZombie(Zombie zombie)
    {

        yield return new WaitForSeconds(5f);
        zombie.gameObject.SetActive(false);
        zombiePool.Add(zombie);
    }

    
}
