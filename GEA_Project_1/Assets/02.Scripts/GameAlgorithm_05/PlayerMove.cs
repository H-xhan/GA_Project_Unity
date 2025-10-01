using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    [Header("Input & Record")]
    public float speed = 5f;                   // 입력 기반 누적 이동속도
    public float recordInterval = 0.05f;       // 위치 기록 간격(초)

    [Header("Playback (Queue)")]
    public float playbackSpeed = 10f;          // 큐 재생 보간 속도

    [Header("Rewind (Ekko)")]
    public float rewindSeconds = 3f;           // 최근 N초만 되감기(과제 권장: 2~3초)
    public float rewindSpeed = 12f;            // 되감기 보간 속도
    public int rewindMinSamples = 10;        // 기록 부족 시 최소 샘플 보장
    public Color rewindTint = Color.red;       // 되감기 중 머티리얼 컬러

    // ---- 내부 상태 ----
    private Queue<Vector3> moveQueue;          // FIFO: 순차 재생용
    private bool isMoving = false;             // 큐 재생 중
    private bool isRewinding = false;          // 되감기 중
    private Vector3 targetPos;                 // 입력 누적 목표

    private readonly List<(Vector3 pos, float t)> history = new(); // 되감기 히스토리
    private float recordTimer = 0f;

    private Renderer rend;                     // 색상 표시용
    private Rigidbody rb;                      // 물리 충돌 시 되감기 보호용

    void Start()
    {
        moveQueue = new Queue<Vector3>();
        targetPos = transform.position;

        rend = GetComponentInChildren<Renderer>();
        rb = GetComponent<Rigidbody>();

        // 시작 시점 1회 기록
        history.Add((transform.position, Time.time));
    }

    void Update()
    {
        // === 입력 수집(큐 기록) ===
        if (!isMoving && !isRewinding)
        {
            float x = Input.GetAxisRaw("Horizontal");
            float y = Input.GetAxisRaw("Vertical");

            if (x != 0f || y != 0f)
            {
                // 프레임 입력을 누적 좌표로 변환 → 큐에 좌표 스냅샷 저장
                Vector3 move = new Vector3(x, 0f, y).normalized * speed * Time.deltaTime;
                targetPos += move;
                moveQueue.Enqueue(targetPos);
            }

            // Space → 큐 재생 시작
            if (Input.GetKeyDown(KeyCode.Space) && moveQueue.Count > 0)
            {
                isMoving = true;
                StartCoroutine(PlayQueueSmooth());
            }
        }

        // R → 최근 N초 되감기 시작
        if (!isMoving && !isRewinding && Input.GetKeyDown(KeyCode.R) && history.Count > 1)
        {
            StartCoroutine(RewindRecent());
        }

        // === 히스토리 기록(최근 rewindSeconds만 유지) ===
        recordTimer += Time.deltaTime;
        if (!isRewinding && recordTimer >= recordInterval)
        {
            recordTimer = 0f;
            history.Add((transform.position, Time.time));

            float cutoff = Time.time - rewindSeconds;
            // 오래된 데이터 슬라이스(최소 1개는 유지)
            int i = 0;
            while (i < history.Count && history[i].t < cutoff) i++;
            if (i > 0) history.RemoveRange(0, Mathf.Min(i, history.Count - 1));
        }
    }

    // === 큐 재생: FIFO를 부드럽게 따라감 ===
    IEnumerator PlayQueueSmooth()
    {
        Vector3 current = transform.position;

        while (moveQueue.Count > 0)
        {
            Vector3 next = moveQueue.Dequeue();
            float dist = Vector3.Distance(current, next);
            float dur = Mathf.Max(0.01f, dist / playbackSpeed);

            float t = 0f;
            while (t < dur)
            {
                t += Time.deltaTime;
                float k = Mathf.Clamp01(t / dur);
                transform.position = Vector3.Lerp(current, next, k);
                yield return null;
            }

            transform.position = next;
            current = next;
        }

        isMoving = false;
        targetPos = transform.position; // 동기화
    }

    // === 되감기: 최신→과거로 최근 N초만 역보간 ===
    IEnumerator RewindRecent()
    {
        isRewinding = true;

        // 물리 간섭 차단
        bool hadRB = rb != null;
        bool prevKinematic = false;
        Vector3 prevVel = Vector3.zero, prevAng = Vector3.zero;
        if (hadRB)
        {
            prevKinematic = rb.isKinematic;
            prevVel = rb.velocity; prevAng = rb.angularVelocity;
            rb.isKinematic = true; rb.velocity = Vector3.zero; rb.angularVelocity = Vector3.zero;
        }

        // 시각 표시
        Color? prev = null;
        if (rend) { prev = rend.material.color; rend.material.color = rewindTint; }

        float cutoff = Time.time - rewindSeconds;

        // 되감기 범위 계산(최신 인덱스 start, 과거 경계 end)
        int start = history.Count - 1;
        int end = start;
        while (end >= 0 && history[end].t >= cutoff) end--;
        // 최근 샘플이 적으면 최소 rewindMinSamples 보장
        int ensure = Mathf.Min(rewindMinSamples - (start - end), start);
        end = Mathf.Max(-1, end - ensure);

        Vector3 current = transform.position;

        for (int i = start; i > end; i--)
        {
            Vector3 target = history[i].pos;

            float dist = Vector3.Distance(current, target);
            float dur = Mathf.Max(0.01f, dist / rewindSpeed);

            float t = 0f;
            while (t < dur)
            {
                t += Time.deltaTime;
                float k = Mathf.Clamp01(t / dur);
                transform.position = Vector3.Lerp(current, target, k);
                yield return null;
            }

            transform.position = target;
            current = target;
        }

        // 복구
        if (rend && prev.HasValue) rend.material.color = prev.Value;
        if (hadRB)
        {
            rb.isKinematic = prevKinematic;
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }

        isRewinding = false;

        // 상태 리셋
        moveQueue.Clear();
        targetPos = transform.position;
        history.Clear();
        history.Add((transform.position, Time.time)); // 기준점 재기록
    }
}
