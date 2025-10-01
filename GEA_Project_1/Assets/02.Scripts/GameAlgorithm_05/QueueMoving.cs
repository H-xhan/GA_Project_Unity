using System.Collections.Generic;
using UnityEngine;

public class QueueMoving : MonoBehaviour
{
    public float speed = 5f;              // 이동 속도
    private Queue<Vector3> moveQueue;     // 이동 기록을 저장할 큐
    private bool isMoving = false;        // 현재 이동 중인지 여부
    private Vector3 targetPos;            // 목표 위치

    void Start()
    {
        moveQueue = new Queue<Vector3>();
        targetPos = transform.position;
    }

    void Update()
    {
        float x = Input.GetAxisRaw("Horizontal"); // 좌우 입력
        float y = Input.GetAxisRaw("Vertical");   // 상하 입력

        // 이동 중이 아닐 때
        if (!isMoving)
        {
            // 입력이 있으면 새로운 위치 계산
            if (x != 0 || y != 0)
            {
                Vector3 move = new Vector3(x, y, 0).normalized * speed * Time.deltaTime;
                targetPos += move;

                // 큐에 목표 위치 추가
                moveQueue.Enqueue(targetPos);
            }

            // 스페이스 키를 누르면 큐에서 꺼내오기 시작
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (!isMoving && moveQueue.Count > 0)
                {
                    isMoving = true;
                }
            }
        }
        else
        {
            // 큐에 데이터가 있으면 하나 꺼내서 이동
            if (moveQueue.Count > 0)
            {
                transform.position = moveQueue.Dequeue();
                isMoving = true;
            }
            else
            {
                isMoving = false;
            }
        }
    }
}
