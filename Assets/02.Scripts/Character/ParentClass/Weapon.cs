using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public  class Weapon : MonoBehaviour
{
    private WaitForSecondsRealtime m_Timer = new WaitForSecondsRealtime(0.1f);
    private LineRenderer lineRenderer;
    public float FireRate;
    public float Damage;
    public float detectionRange = 10f;  // 탐지 거리
    public Transform FirePosition;
    private void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }
    public virtual void Using(Character target)
    {
        target.TakeDamage(Damage);
        print("플레이어가 공격했습니다.");
        lineRenderer.SetPosition(0, FirePosition.position); // 총구 위치
        lineRenderer.SetPosition(1, target.transform.position); // 발사 방향
        lineRenderer.enabled = true; // Line Renderer 활성화
        StartCoroutine(DisableLineRenderer()); // 일정 시간 후 비활성화
    }

    // 일정 시간 후 Line Renderer 비활성화
    private IEnumerator DisableLineRenderer()
    {
        yield return m_Timer; //효과 지속 시간
        lineRenderer.enabled = false; // Line Renderer 비활성화
    }

}
