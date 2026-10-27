using System;
using UnityEngine;

public class ItemsGet : MonoBehaviour
{
    //public string ItemName;
    public enum ItemType {AmmoPack, HealthPack}
    [SerializeField] private ItemType itemType;
    [SerializeField] private LayerMask playerLayer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            switch(itemType)
            {
                case ItemType.AmmoPack:
                AmmoPackGet(other.gameObject);
                break;

                case ItemType.HealthPack:
                AmmoPackGet(other.gameObject);
                break;

                default:
                Debug.Log("존재하지 않는 아이템입니다.");
                break;
            }
            Debug.Log(other.gameObject);
        }

    }
  
    

    private void AmmoPackGet(GameObject target)
    {
        
    }

    private void HealthPackGet(GameObject target)
    {
        PlayerHealth player = target.GetComponentInParent<PlayerHealth>();
        if (player == null) return;
        //player.Health += 10;
    }

}
