using System.Collections;
using System.Threading;
using UnityEngine;
using UnityEngine.AI;

public class SPYGunner : UnitaskGunnerHuman
{
    private WaitForSecondsRealtime time = new WaitForSecondsRealtime(0.5f);
    private Coroutine currentMissionCoroutine; // 현재 실행 중인 코루틴 참조
    public bool Mission = false;

    // 스파이 미션 시작
    public override void MissionStart()
    {
        if (currentMissionCoroutine != null)
        {
            StopCoroutine(currentMissionCoroutine); // 이미 실행 중인 코루틴이 있다면 중단
        }
        Mission = true;
        currentMissionCoroutine = StartCoroutine(SPYMission());
    }

    // 스파이 미션 실행
    private IEnumerator SPYMission()
    {
        while (Mission)
        {
            if (GameManager.Instance.player.transform.position != null)
            {
                moveTarget = GameManager.Instance.player.gameObject;
                if (Vector3.Distance(this.transform.position, moveTarget.transform.position) < 3f)
                    agent.speed = 0;
                else
                {
                    agent.SetDestination(moveTarget.transform.position);
                    agent.speed = 5f;
                }
            }
            m_animator.SetFloat("Speed", agent.speed);
            yield return time;
        }
    }

    // 스파이 미션 중단
    public void StopMission()
    {
        if (currentMissionCoroutine != null)
        {
            StopCoroutine(currentMissionCoroutine);
            currentMissionCoroutine = null; // 참조 초기화
        }
        Mission = false;
    }
    public override void RespawnSetting()
    {
        m_isDie = false;
        target = null;
        moveTarget = null;
        state = State.Move;
        m_currentTime = Time2;
        m_animator.SetBool("Attack", false);
        _cancellationTokenSource = new CancellationTokenSource();
        AIStateAsync(_cancellationTokenSource.Token).Forget(); // 비동기 메서드 실행
    }
}