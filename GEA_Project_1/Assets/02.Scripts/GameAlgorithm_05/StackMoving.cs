using System.Collections.Generic;
using UnityEngine;

public class StackMoving : MonoBehaviour
{
    public float speed = 5f;   // 이동 속도
    private Stack<Vector3> moveHistory;   // 이동 기록 저장용 스택

    void Start()
    {
        moveHistory = new Stack<Vector3>();
    }

    void Update()
    {
        float x = Input.GetAxisRaw("Horizontal"); // 좌우 입력
        float y = Input.GetAxisRaw("Vertical");   // 상하 입력

        // 이동 입력이 있을 때
        if (x != 0 || y != 0)
        {
            // 현재 위치를 스택에 저장
            moveHistory.Push(transform.position);

            // 이동 벡터 계산
            Vector3 move = new Vector3(x, y, 0).normalized * speed * Time.deltaTime;
            transform.position += move;
        }

        // 스페이스 키를 누르면 되돌아가기
        if (Input.GetKey(KeyCode.Space))
        {
            if (moveHistory.Count > 0)
            {
                // 이전 위치로 되돌림
                transform.position = moveHistory.Pop();
            }
        }
    }
}
