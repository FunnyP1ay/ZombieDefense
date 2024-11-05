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
    public float detectionRange = 10f;  // Ž�� �Ÿ�
    public Transform FirePosition;
    private void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }
    public virtual void Using(Character target)
    {
        target.TakeDamage(Damage);
        print("�÷��̾ �����߽��ϴ�.");
        lineRenderer.SetPosition(0, FirePosition.position); // �ѱ� ��ġ
        lineRenderer.SetPosition(1, target.transform.position); // �߻� ����
        lineRenderer.enabled = true; // Line Renderer Ȱ��ȭ
        StartCoroutine(DisableLineRenderer()); // ���� �ð� �� ��Ȱ��ȭ
    }

    // ���� �ð� �� Line Renderer ��Ȱ��ȭ
    private IEnumerator DisableLineRenderer()
    {
        yield return m_Timer; //ȿ�� ���� �ð�
        lineRenderer.enabled = false; // Line Renderer ��Ȱ��ȭ
    }

}
