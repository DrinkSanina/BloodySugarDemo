using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class EntityEffectComponent
{
    public Affect affect;
    
    [Tooltip("�������� ��������� ��������. ����������� � ����� ��� �������. ������������� �������� �������� ���������� �����")]
    public float change_value;
}
