using System.Collections.Generic;
using UnityEngine;

public class QueueMoving : MonoBehaviour
{
    public float speed = 5f;              // �̵� �ӵ�
    private Queue<Vector3> moveQueue;     // �̵� ����� ������ ť
    private bool isMoving = false;        // ���� �̵� ������ ����
    private Vector3 targetPos;            // ��ǥ ��ġ

    void Start()
    {
        moveQueue = new Queue<Vector3>();
        targetPos = transform.position;
    }

    void Update()
    {
        float x = Input.GetAxisRaw("Horizontal"); // �¿� �Է�
        float y = Input.GetAxisRaw("Vertical");   // ���� �Է�

        // �̵� ���� �ƴ� ��
        if (!isMoving)
        {
            // �Է��� ������ ���ο� ��ġ ���
            if (x != 0 || y != 0)
            {
                Vector3 move = new Vector3(x, y, 0).normalized * speed * Time.deltaTime;
                targetPos += move;

                // ť�� ��ǥ ��ġ �߰�
                moveQueue.Enqueue(targetPos);
            }

            // �����̽� Ű�� ������ ť���� �������� ����
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
            // ť�� �����Ͱ� ������ �ϳ� ������ �̵�
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
