using Cysharp.Threading.Tasks; // UniTask 네임스페이스
using NUnit.Framework;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;
using UnityEngine.VFX;

public class UnitaskZombie : Character
{
    private static readonly float Time1 = 1f;
    private static readonly float Time2 = 1.1f;
    private float m_currentTime;
    private NavMeshAgent agent;
    public GameObject baseTarget;
    public int m_moveSupportCount = 0;
    private float m_detectionRadius = 10f;
    private bool m_isDie = false;
    private bool m_isPlayerTarget = false;
    private CancellationTokenSource _cancellationTokenSource;
    public VisualEffect BloodVFX;
    private int triggerID = 0; // 트리거 ID로 고유 이벤트 식별
    public List<SkinnedMeshRenderer> skinList;
    public SkinnedMeshRenderer currentSkin;
    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        m_animator = GetComponent<Animator>();
    }

    public override void RespawnSetting()
    {
        health += 5;
        triggerID = 0;
        baseTarget = GameManager.Instance.playerCityData.playerBase;
        m_isDie = false;
        target = null;
        agent.SetDestination(baseTarget.transform.position);
        m_currentTime = Time2;
        currentSkin = skinList[Random.Range(0, skinList.Count)];
        currentSkin.gameObject.SetActive(true);
        _cancellationTokenSource = new CancellationTokenSource();
        AIStateAsync(_cancellationTokenSource.Token).Forget();
    }

    private async UniTaskVoid AIStateAsync(CancellationToken cancellationToken)
    {
        try
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                Move();

                // m_currentTime 만큼 대기 (밀리초로 변환)
                await UniTask.Delay((int)(m_currentTime * 1000), cancellationToken: cancellationToken);
            }
        }
        catch
        {
        }
    }

    public override void Move()
    {
        SettingTarget();
        Attack();
        NextCoroutineTime();
    }

    public void SettingTarget()
    {
        if (target == null || !target.activeSelf || Vector3.Distance(transform.position, target.transform.position) > m_detectionRadius)
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, m_detectionRadius, targetLayer);
            if (colliders.Length > 0)
            {
                int randnum = Random.Range(0, colliders.Length);
                target = colliders[randnum].gameObject;
                agent.SetDestination(target.transform.position);
                if (target.name == "Player")
                    m_isPlayerTarget = true;
                else
                    m_isPlayerTarget = false;

            }
            else
            {
                print("좀비가 그냥 기지로 돌격합니다.");
                agent.SetDestination(baseTarget.transform.position);
            }
        }
        else
        {
            agent.SetDestination(target.transform.position);
        }
    }
    public override void Attack()
    {
        if (target != null && Vector3.Distance(transform.position, target.transform.position) <= attackRange && target.GetComponent<Character>().health > 0)
        {
            target.GetComponent<Character>().TakeDamage(attackPower);
            m_animator.SetTrigger("Attack");
        }
        else
            target = null;
    }

    public void NextCoroutineTime()
    {
        m_currentTime = Random.Range(0, 2) == 0 ? Time1 : Time2;
    }
    public override void TakeDamage(float damage)
    {
        // 데미지를 입었습니다.
        health -= damage;
        if (health < 0)
        {
            Die();
            return;
        }
        else
        {
            if (BloodVFX != null)
            {
                triggerID = Random.Range(1000, 10000);
                // 고유 트리거 이벤트 생성
                BloodVFX.SetInt("TriggerID", triggerID);
                BloodVFX.SendEvent("OnTrigger");
            }
        }
    }
    protected override void Die()
    {
        if (!m_isDie)
        {
            m_isDie = true;
            // _cancellationTokenSource가 이미 Dispose 되었는지 확인 후 취소
            if (_cancellationTokenSource != null && !_cancellationTokenSource.IsCancellationRequested)
            {
                _cancellationTokenSource.Cancel();
                _cancellationTokenSource.Dispose();
                _cancellationTokenSource = null; // 중복 호출 방지를 위해 null로 설정
            }
            agent.ResetPath();
            target = null;
            m_animator.SetTrigger("Die");
        }
    }
    public void OnDieAnimationEnd()
    {
        currentSkin.gameObject.SetActive(false);
        base.Die();
    }
}