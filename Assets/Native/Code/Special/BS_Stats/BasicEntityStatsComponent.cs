using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicEntityStatsComponent : MonoBehaviour
{
    /* �, ������ �����. ����� ���� ���������� BasicEntityStats � 
     * �������-���������? ������ � ���, ��� ��� ����� BasicEntityStats
     * ��� ������� ����� � ��� ��������� MonoBehaviour. � ������ ������ � ����
     * ������� ��������� ���������� �������� - ��, ��� � �������, ���� � ������. �����
     * ����, � ���� ��������� ���������� ����� ������, �� �������� �� ������� Unity, ��� �
     * � ������ � EffectStatsConverter, ����� ��������� ���� �� �����, ��� ��������� ���������
     * �������. �� ������ ������ � ���� ���������, ������� ����� �������� �� ������, �������, ��
     * � ������ �� ��� ������ � ������������ ��������� ������ MonoBehaviour
     * 
     * ��������� �������� - ����� �������� ������ �������� � ������ ������� ������, �������� ��
     * ������� �������� ����� ��������������� ����� ��������� ������, �� � ���� ����� ����� ���-��
     * �������
     */

    public BasicEntityStats stats;

    void Start()
    {
        InvokeRepeating("HandleTickingValues", 0f, 0.1f);
    }

    /* �����, �������� ��� ������� ������� ��� � ������� � ��������� �������� ����
     * � ������� � ���� ���������� ���� ���������� �������� (� ������� ��� 
     * ��� ����� ����� � ��������), �� ��� ������������ ��� �������� � ������ Update
     * ������� ����������� ������ ����, � ��������� �������� ����������? �� � �����������
     * ������ � ���������� ������ ���-�� �������. ������� � �������� ��� � �������� �������
     * ������ "���" ���������� �� ���������� ���� ���� ������ ������� ���� �������, ��������
     * ��� �������� ����������, �� � �� ���������� ������������ ������ ���� + ����� ��������
     * � �������� �������� ������� �������� ��������
     * 
     * �� �������� - ������� ������� ������� �� ��������, ��-�� ���� ���������� �� ������� 
     * ������� ���� �����. � ���� ���� ����� ���-�� ����� �������.
     */
    void HandleTickingValues()
    {
        if (stats.healPerSecond > 0.0f)
            stats.RecieveHealing(stats.healPerSecond / 10.0f);

        if (stats.incomingDamagePerSecond > 0.0f)
            stats.RecieveDamage(stats.incomingDamagePerSecond / 10.0f, DamageSource.effect);
    }
}
