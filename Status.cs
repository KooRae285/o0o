using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Status : MonoBehaviour
{
    protected int AtkPoint;
    protected int DefPoint;
    protected float _CurHP;
    protected float _MaxHP;
    
    public int _FinishATT
    {
        get
        {
            return AtkPoint;
        }
    }

    public float _nowHP
    {
        get
        {
            return _CurHP;
        }
    }
    public float _FullHP
    {
        get
        {
            return _MaxHP;
        }
    }

    protected void InitData(int att, int def, int life)
    {
        AtkPoint = att;
        DefPoint = def;
        _MaxHP = _CurHP = life;
    }
    protected void AddData(int att, int def, int hp)
    {
        AtkPoint += att;
        DefPoint += def;
        _MaxHP =_CurHP += hp;
    }
    /// <summary>
    /// 맞으면 호출되는 함수. 죽으면 true를 반환함
    /// </summary>
    /// <param name="damagbe">받는 데미지.</param>
    /// <returns>true면 죽음.</returns>
    protected bool DamageMe(int damage)
    {
        int finishDam = damage - DefPoint;
        if (finishDam < 1)
            finishDam = 1;

        _CurHP -= finishDam;
        if (_CurHP <= 0)
        {
            _CurHP = 0;
            return true;
        }
        return false;
    }
}

