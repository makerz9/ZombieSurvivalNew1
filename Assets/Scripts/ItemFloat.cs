using UnityEngine;


public class ItemFloat : MonoBehaviour
{
    [Header("회전")]
    [SerializeField] private float rotateSpeed = 90f;     // 초당 회전각

    [Header("부유")]
    [SerializeField] private float floatAmplitude = 0.25f; // 오르내리는 높이
    [SerializeField] private float floatSpeed = 2f;        // 오르내리는 속도

    private Vector3 startLocalPos;
    private float phase;

    private void Awake()
    {
        // 기준 위치를 기억해 두고, 매 프레임 "기준 + 오프셋"으로 계산
        startLocalPos = transform.localPosition;

        // 아이템마다 타이밍이 어긋나도록 랜덤 위상
        phase = Random.Range(0, Mathf.PI * 2f);
    }

    // Update is called once per frame
    void Update()
    {
        float offestY = Mathf.Sin(Time.time * floatSpeed + phase) * floatAmplitude;
        transform.localPosition = startLocalPos + new Vector3(0f, offestY, 0f);
        transform.Rotate(0f, rotateSpeed * Time.deltaTime, 0f, Space.World);//Space.Self
        
    }
}
