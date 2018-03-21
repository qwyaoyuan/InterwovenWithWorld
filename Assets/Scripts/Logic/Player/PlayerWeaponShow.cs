using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 玩家装备显示
/// </summary>
public class PlayerWeaponShow : MonoBehaviour
{
    /// <summary>
    /// 单手剑
    /// </summary>
    public GameObject SingleWeapon;
    /// <summary>
    /// 双手剑
    /// </summary>
    public GameObject TwiceWeapon;
    /// <summary>
    /// 弓
    /// </summary>
    public GameObject Arch;


	void Start ()
    {
        GameState.Instance.Registor<IPlayerState>(IPlayerStateChagned);
        IPlayerState iPlayerState = GameState.Instance.GetEntity<IPlayerState>();
        IPlayerStateChagned(iPlayerState, GameState.GetFieldNameStatic<IPlayerState, EnumWeaponTypeByPlayerState>(temp => temp.WeaponTypeByPlayerState));

    }

    private void OnDestroy()
    {
        GameState.Instance.UnRegistor<IPlayerState>(IPlayerStateChagned);
    }

    private void IPlayerStateChagned(IPlayerState iPlayerState, string fieldName)
    {
        if (string.Equals(fieldName, GameState.GetFieldNameStatic<IPlayerState, EnumWeaponTypeByPlayerState>(temp => temp.WeaponTypeByPlayerState)))
        {
            EnumWeaponTypeByPlayerState weaponType_Right = iPlayerState.WeaponTypeByPlayerState | EnumWeaponTypeByPlayerState.Shield - EnumWeaponTypeByPlayerState.Shield;//去除盾牌
            switch (weaponType_Right)
            {
                case EnumWeaponTypeByPlayerState.None:
                    SingleWeapon.SetActive(false);
                    TwiceWeapon.SetActive(false);
                    Arch.SetActive(false);
                    break;
                case EnumWeaponTypeByPlayerState.SingleHandedSword:
                    SingleWeapon.SetActive(true);
                    TwiceWeapon.SetActive(false);
                    Arch.SetActive(false);
                    break;
                case EnumWeaponTypeByPlayerState.TwoHandedSword:
                    SingleWeapon.SetActive(false);
                    TwiceWeapon.SetActive(true);
                    Arch.SetActive(false);
                    break;
                case EnumWeaponTypeByPlayerState.Arch:
                    SingleWeapon.SetActive(false);
                    TwiceWeapon.SetActive(false);
                    Arch.SetActive(true);
                    break;
                case EnumWeaponTypeByPlayerState.CrossBow:
                    break;
                case EnumWeaponTypeByPlayerState.Shield:
                    break;
                case EnumWeaponTypeByPlayerState.Dagger:
                    break;
                case EnumWeaponTypeByPlayerState.LongRod:
                    break;
                case EnumWeaponTypeByPlayerState.ShortRod:
                    break;
                case EnumWeaponTypeByPlayerState.CrystalBall:
                    break;
            }
        }
    }


}
