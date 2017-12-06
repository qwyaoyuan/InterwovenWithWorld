using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 实现了IPlayerState->IDebuffState接口的GameState类的一个分支实体
/// </summary>
public partial class GameState
{
    #region IDebuffState Debuff状态
    /// <summary>
    /// 冰冻
    /// </summary>
    BuffState? _Bingdong;
    /// <summary>
    /// 冰冻
    /// </summary>
    public BuffState Bingdong
    {
        get
        {
            if (_Bingdong == null)
                _Bingdong = new BuffState(EnumStatusEffect.bd1, 0, null);
            return _Bingdong.Value;
        }
        set
        {
            if (_Bingdong == null)
                _Bingdong = Bingdong;
            if (_Bingdong.Value.statusEffect != value.statusEffect)
                return;
            BuffState tempBingdong = _Bingdong.Value;
            _Bingdong = value;
            if (!tempBingdong.Equals(value))
                Call<IDebuffState, BuffState>(temp => temp.Bingdong);

        }
    }

    /// <summary>
    /// 迟钝
    /// </summary>
    BuffState? _Chidun;
    /// <summary>
    /// 迟钝
    /// </summary>
    public BuffState Chidun
    {
        get
        {
            if (_Chidun == null)
                _Chidun = new BuffState(EnumStatusEffect.cd1, 0, null);
            return _Chidun.Value;
        }
        set
        {
            if (_Chidun == null)
                _Chidun = Chidun;
            if (_Chidun.Value.statusEffect != value.statusEffect)
                return;
            BuffState tempChidun = _Chidun.Value;
            _Chidun = value;
            if (!tempChidun.Equals(value))
                Call<IDebuffState, BuffState>(temp => temp.Chidun);

        }
    }

    /// <summary>
    /// 点燃
    /// </summary>
    BuffState? _Dianran;
    /// <summary>
    /// 点燃
    /// </summary>
    public BuffState Dianran
    {
        get
        {
            if (_Dianran == null)
                _Dianran = new BuffState(EnumStatusEffect.dr1, 0, null);
            return _Dianran.Value;
        }
        set
        {
            if (_Dianran == null)
                _Dianran = Dianran;
            if (_Dianran.Value.statusEffect != value.statusEffect)
                return;
            BuffState tempDianran = _Dianran.Value;
            _Dianran = value;
            if (!tempDianran.Equals(value))
                Call<IDebuffState, BuffState>(temp => temp.Dianran);

        }
    }

    /// <summary>
    /// 凋零
    /// </summary>
    BuffState? _Diaoling;
    /// <summary>
    /// 凋零
    /// </summary>
    public BuffState Diaoling
    {
        get
        {
            if (_Diaoling == null)
                _Diaoling = new BuffState(EnumStatusEffect.dl3, 0, null);
            return _Diaoling.Value;
        }
        set
        {
            if (_Diaoling == null)
                _Diaoling = Diaoling;
            if (_Diaoling.Value.statusEffect != value.statusEffect)
                return;
            BuffState tempDiaoling = _Diaoling.Value;
            _Diaoling = value;
            if (!tempDiaoling.Equals(value))
                Call<IDebuffState, BuffState>(temp => temp.Diaoling);

        }
    }

    /// <summary>
    /// 减速
    /// </summary>
    BuffState? _Jiansu;
    /// <summary>
    /// 减速
    /// </summary>
    public BuffState Jiansu
    {
        get
        {
            if (_Jiansu == null)
                _Jiansu = new BuffState(EnumStatusEffect.js4, 0, null);
            return _Jiansu.Value;
        }
        set
        {
            if (_Jiansu == null)
                _Jiansu = Jiansu;
            if (_Jiansu.Value.statusEffect != value.statusEffect)
                return;
            BuffState tempJiansu = _Jiansu.Value;
            _Jiansu = value;
            if (!tempJiansu.Equals(value))
                Call<IDebuffState, BuffState>(temp => temp.Jiansu);

        }
    }

    /// <summary>
    /// 迷惑
    /// </summary>
    BuffState? _Mihuo;
    /// <summary>
    /// 迷惑
    /// </summary>
    public BuffState Mihuo
    {
        get
        {
            if (_Mihuo == null)
                _Mihuo = new BuffState(EnumStatusEffect.mh3, 0, null);
            return _Mihuo.Value;
        }
        set
        {
            if (_Mihuo == null)
                _Mihuo = Mihuo;
            if (_Mihuo.Value.statusEffect != value.statusEffect)
                return;
            BuffState tempMihuo = _Mihuo.Value;
            _Mihuo = value;
            if (!tempMihuo.Equals(value))
                Call<IDebuffState, BuffState>(temp => temp.Mihuo);

        }
    }

    /// <summary>
    /// 无力
    /// </summary>
    BuffState? _Wuli;
    /// <summary>
    /// 无力
    /// </summary>
    public BuffState Wuli
    {
        get
        {
            if (_Wuli == null)
                _Wuli = new BuffState(EnumStatusEffect.wl1, 0, null);
            return _Wuli.Value;
        }
        set
        {
            if (_Wuli == null)
                _Wuli = Wuli;
            if (_Wuli.Value.statusEffect != value.statusEffect)
                return;
            BuffState tempWuli = _Wuli.Value;
            _Wuli = value;
            if (!tempWuli.Equals(value))
                Call<IDebuffState, BuffState>(temp => temp.Wuli);

        }
    }

    /// <summary>
    /// 虚弱
    /// </summary>
    BuffState? _Xuruo;
    /// <summary>
    /// 虚弱
    /// </summary>
    public BuffState Xuruo
    {
        get
        {
            if (_Xuruo == null)
                _Xuruo = new BuffState(EnumStatusEffect.xr2, 0, null);
            return _Xuruo.Value;
        }
        set
        {
            if (_Xuruo == null)
                _Xuruo = Xuruo;
            if (_Xuruo.Value.statusEffect != value.statusEffect)
                return;
            BuffState tempXuruo = _Xuruo.Value;
            _Xuruo = value;
            if (!tempXuruo.Equals(value))
                Call<IDebuffState, BuffState>(temp => temp.Xuruo);

        }
    }

    /// <summary>
    /// 中毒
    /// </summary>
    BuffState? _Zhongdu;
    /// <summary>
    /// 中毒
    /// </summary>
    public BuffState Zhongdu
    {
        get
        {
            if (_Zhongdu == null)
                _Zhongdu = new BuffState(EnumStatusEffect.zd2, 0, null);
            return _Zhongdu.Value;
        }
        set
        {
            if (_Zhongdu == null)
                _Zhongdu = Zhongdu;
            if (_Zhongdu.Value.statusEffect != value.statusEffect)
                return;
            BuffState tempZhongdu = _Zhongdu.Value;
            _Zhongdu = value;
            if (!tempZhongdu.Equals(value))
                Call<IDebuffState, BuffState>(temp => temp.Zhongdu);

        }
    }

    /// <summary>
    /// 诅咒
    /// </summary>
    BuffState? _Zuzhou;
    /// <summary>
    /// 诅咒
    /// </summary>
    public BuffState Zuzhou
    {
        get
        {
            if (_Zuzhou == null)
                _Zuzhou = new BuffState(EnumStatusEffect.zz3, 0, null);
            return _Zuzhou.Value;
        }
        set
        {
            if (_Zuzhou == null)
                _Zuzhou = Zuzhou;
            if (_Zuzhou.Value.statusEffect != value.statusEffect)
                return;
            BuffState tempZuzhou = _Zuzhou.Value;
            _Zuzhou = value;
            if (!tempZuzhou.Equals(value))
                Call<IDebuffState, BuffState>(temp => temp.Zuzhou);

        }
    }

    /// <summary>
    /// 流血
    /// </summary>
    BuffState? _LiuXue;
    /// <summary>
    /// 流血
    /// </summary>
    public BuffState LiuXue
    {
        get
        {
            if (_LiuXue == null)
                _LiuXue = new BuffState(EnumStatusEffect.lx1, 0, null);
            return _LiuXue.Value;
        }
        set
        {
            if (_LiuXue == null)
                _LiuXue = LiuXue;
            if (_LiuXue.Value.statusEffect != value.statusEffect)
                return;
            BuffState tempLiuXue = _LiuXue.Value;
            _LiuXue = value;
            if (!tempLiuXue.Equals(value))
                Call<IDebuffState, BuffState>(temp => temp.LiuXue);
        }
    }
    #endregion

}