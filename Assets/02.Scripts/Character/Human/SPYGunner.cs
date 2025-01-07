using System.Collections;
using System.Threading;
using UnityEngine;
using UnityEngine.AI;

public class SPYGunner : UnitaskGunnerHuman
{
    private WaitForSecondsRealtime time = new WaitForSecondsRealtime(0.5f);
    private Coroutine currentMissionCoroutine; // ���� ���� ���� �ڷ�ƾ ����
    public bool Mission = false;

    // ������ �̼� ����
    public override void MissionStart()
    {
        if (currentMissionCoroutine != null)
        {
            StopCoroutine(currentMissionCoroutine); // �̹� ���� ���� �ڷ�ƾ�� �ִٸ� �ߴ�
        }
        Mission = true;
        currentMissionCoroutine = StartCoroutine(SPYMission());
    }

    // ������ �̼� ����
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

    // ������ �̼� �ߴ�
    public void StopMission()
    {
        if (currentMissionCoroutine != null)
        {
            StopCoroutine(currentMissionCoroutine);
            currentMissionCoroutine = null; // ���� �ʱ�ȭ
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
        AIStateAsync(_cancellationTokenSource.Token).Forget(); // �񵿱� �޼��� ����
    }
}