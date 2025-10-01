using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestMove : MonoBehaviour
{
    [Header("Start Gate")]
    public KeyCode startKey = KeyCode.Space;  // 시작 키 (Space)
    private bool started = false;             // 시작 전엔 입력/기록 금지

    [Header("Live Move")]
    public float moveSpeed = 5f;              // WASD 이동 속도
    public bool useXZ = true;                 // 3D(XZ) / 2D(XY)

    [Header("Record / Rewind")]
    public float recordInterval = 0.05f;      // 기록 간격(초)
    public float rewindSeconds = 3f;          // 최근 몇 초 되감기
    public float rewindSpeed = 12f;           // 되감기 보간 속도
    public Color rewindTint = Color.red;      // 되감기 중 색상

    private float recordTimer = 0f;
    private bool isRewinding = false;

    // (위치, 시각) 히스토리 — 최근 rewindSeconds만 유지
    private readonly List<(Vector3 pos, float t)> history = new List<(Vector3, float)>();

    private Renderer meshRenderer;
    private SpriteRenderer spriteRenderer;
    private Rigidbody rb;

    void Start()
    {
        meshRenderer = GetComponentInChildren<Renderer>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        rb = GetComponent<Rigidbody>();
        // 시작 전에는 기록 안함. 시작 후 최초 지점 기록 예정.
    }

    void Update()
    {
        // 1) Space 눌러 시작
        if (!started && Input.GetKeyDown(startKey))
        {
            started = true;
            RecordNow(transform.position);        // 시작 지점 1회 기록
            return;
        }

        if (!started || isRewinding) return;      // 시작 전/되감기 중엔 아무 것도 안 함

        // 2) 연속 이동
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        Vector3 dir = useXZ ? new Vector3(h, 0f, v) : new Vector3(h, v, 0f);
        if (dir.sqrMagnitude > 0f)
        {
            dir = dir.normalized;
            transform.position += dir * moveSpeed * Time.deltaTime;
        }

        // 3) 위치 기록 (이동 여부와 무관하게 일정 간격으로 기록)
        recordTimer += Time.deltaTime;
        if (recordTimer >= recordInterval)
        {
            recordTimer = 0f;
            RecordNow(transform.position);
            PruneOld(); // 오래된 기록 제거
        }

        // 4) R: 최근 N초 되감기
        if (Input.GetKeyDown(KeyCode.R) && history.Count > 1)
        {
            StopAllCoroutines();
            StartCoroutine(RewindSmooth());
        }
    }

    // ----------------- Rewind -----------------
    IEnumerator RewindSmooth()
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

        var prevColor = SetTint(rewindTint);

        float cutoff = Time.time - rewindSeconds;
        Vector3 current = transform.position;

        // 최신 → 과거 순으로 cutoff 이상만 추적
        for (int i = history.Count - 1; i >= 0; i--)
        {
            if (history[i].t < cutoff) break;
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

        RestoreTint(prevColor);
        if (hadRB)
        {
            rb.isKinematic = prevKinematic;
            rb.velocity = Vector3.zero; rb.angularVelocity = Vector3.zero;
        }

        isRewinding = false;

        // 되감기 후 기준 재설정(큐/기록 꼬임 방지)
        history.Clear();
        RecordNow(transform.position);
        recordTimer = 0f;
    }

    // ----------------- Utils -----------------
    void RecordNow(Vector3 p) => history.Add((p, Time.time));

    void PruneOld()
    {
        float cutoff = Time.time - rewindSeconds;
        int i = 0;
        while (i < history.Count && history[i].t < cutoff) i++;
        if (i > 0) history.RemoveRange(0, Mathf.Min(i, history.Count - 1)); // 최소 1개 유지
    }

    Color? SetTint(Color c)
    {
        if (meshRenderer && meshRenderer.material) { var prev = meshRenderer.material.color; meshRenderer.material.color = c; return prev; }
        if (spriteRenderer) { var prev = spriteRenderer.color; spriteRenderer.color = c; return prev; }
        return null;
    }
    void RestoreTint(Color? prev)
    {
        if (!prev.HasValue) return;
        if (meshRenderer && meshRenderer.material) meshRenderer.material.color = prev.Value;
        if (spriteRenderer) spriteRenderer.color = prev.Value;
    }
}
