using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 实现了IPlayerState->IBuffState接口的GameState类的一个分支实体
/// </summary>
public partial class GameState
{

    /// <summary>
    /// buff状态的更新
    /// </summary>
    void IBuffStateUpdate()
    { }

    #region IBuffSate  Buff状态
    /// <summary>
    /// 活力
    /// </summary>
    BuffState? _Huoli;
    /// <summary>
    /// 活力
    /// </summary>
    public BuffState Huoli
    {
        get
        {
            if (_Huoli == null)
                _Huoli = new BuffState(EnumStatusEffect.hl2, 0, null);
            return _Huoli.Value;
        }
        set
        {
            if (_Huoli == null)
                _Huoli = Huoli;
            if (_Huoli.Value.statusEffect != value.statusEffect)
                return;
            BuffState tempHuoLi = _Huoli.Value;
            _Huoli = value;
            if (!tempHuoLi.Equals(value))
                Call<IBuffState, BuffState>(temp => temp.Huoli);

        }
    }

    /// <summary>
    /// 加速
    /// </summary>
    BuffState? _Jiasu;
    /// <summary>
    /// 加速
    /// </summary>
    public BuffState Jiasu
    {
        get
        {
            if (_Jiasu == null)
                _Jiasu = new BuffState(EnumStatusEffect.js1, 0, null);
            return _Jiasu.Value;
        }
        set
        {
            if (_Jiasu == null)
                _Jiasu = Jiasu;
            if (_Jiasu.Value.statusEffect != value.statusEffect)
                return;
            BuffState tempJiasu = _Jiasu.Value;
            _Jiasu = value;
            if (!tempJiasu.Equals(value))
                Call<IBuffState, BuffState>(temp => temp.Jiasu);

        }
    }

    /// <summary>
    /// 净化
    /// </summary>
    BuffState? _Jinghua;
    /// <summary>
    /// 净化
    /// </summary>
    public BuffState Jinghua
    {
        get
        {
            if (_Jinghua == null)
                _Jinghua = new BuffState(EnumStatusEffect.jh5, 0, null);
            return _Jinghua.Value;
        }
        set
        {
            if (_Jinghua == null)
                _Jinghua = Jinghua;
            if (_Jinghua.Value.statusEffect != value.statusEffect)
                return;
            BuffState tempJinghua = _Jinghua.Value;
            _Jinghua = value;
            if (!tempJinghua.Equals(value))
                Call<IBuffState, BuffState>(temp => temp.Jinghua);

        }
    }

    /// <summary>
    /// 敏捷
    /// </summary>
    BuffState? _Minjie;
    /// <summary>
    /// 敏捷
    /// </summary>
    public BuffState Minjie
    {
        get
        {
            if (_Minjie == null)
                _Minjie = new BuffState(EnumStatusEffect.mj1, 0, null);
            return _Minjie.Value;
        }
        set
        {
            if (_Minjie == null)
                _Minjie = Minjie;
            if (_Minjie.Value.statusEffect != value.statusEffect)
                return;
            BuffState tempMinjie = _Minjie.Value;
            _Minjie = value;
            if (!tempMinjie.Equals(value))
                Call<IBuffState, BuffState>(temp => temp.Minjie);

        }
    }

    /// <summary>
    /// 强力
    /// </summary>
    BuffState? _Qiangli;
    /// <summary>
    /// 强力
    /// </summary>
    public BuffState Qiangli
    {
        get
        {
            if (_Qiangli == null)
                _Qiangli = new BuffState(EnumStatusEffect.ql1, 0, null);
            return _Qiangli.Value;
        }
        set
        {
            if (_Qiangli == null)
                _Qiangli = Qiangli;
            if (_Qiangli.Value.statusEffect != value.statusEffect)
                return;
            BuffState tempQiangli = _Qiangli.Value;
            _Qiangli = value;
            if (!tempQiangli.Equals(value))
                Call<IBuffState, BuffState>(temp => temp.Qiangli);

        }
    }

    /// <summary>
    /// 驱散
    /// </summary>
    BuffState? _Qusan;
    /// <summary>
    /// 驱散
    /// </summary>
    public BuffState Qusan
    {
        get
        {
            if (_Qusan == null)
                _Qusan = new BuffState(EnumStatusEffect.qs2, 0, null);
            return _Qusan.Value;
        }
        set
        {
            if (_Qusan == null)
                _Qusan = Qusan;
            if (_Qusan.Value.statusEffect != value.statusEffect)
                return;
            BuffState tempQusan = _Qusan.Value;
            _Qusan = value;
            if (!tempQusan.Equals(value))
                Call<IBuffState, BuffState>(temp => temp.Qusan);

        }
    }

    /// <summary>
    /// 睿智
    /// </summary>
    BuffState? _Ruizhi;
    /// <summary>
    /// 睿智
    /// </summary>
    public BuffState Ruizhi
    {
        get
        {
            if (_Ruizhi == null)
                _Ruizhi = new BuffState(EnumStatusEffect.rz1, 0, null);
            return _Ruizhi.Value;
        }
        set
        {
            if (_Ruizhi == null)
                _Ruizhi = Ruizhi;
            if (_Ruizhi.Value.statusEffect != value.statusEffect)
                return;
            BuffState tempRuizhi = _Ruizhi.Value;
            _Ruizhi = value;
            if (!tempRuizhi.Equals(value))
                Call<IBuffState, BuffState>(temp => temp.Ruizhi);

        }
    }

    /// <summary>
    /// 吸血-物理
    /// </summary>
    BuffState? _XixueWuli;
    /// <summary>
    /// 吸血-物理
    /// </summary>
    public BuffState XixueWuli
    {
        get
        {
            if (_XixueWuli == null)
                _XixueWuli = new BuffState(EnumStatusEffect.xx3, 0, null);
            return _XixueWuli.Value;
        }
        set
        {
            if (_XixueWuli == null)
                _XixueWuli = XixueWuli;
            if (_XixueWuli.Value.statusEffect != value.statusEffect)
                return;
            BuffState tempXixueWuli = _XixueWuli.Value;
            _XixueWuli = value;
            if (!tempXixueWuli.Equals(value))
                Call<IBuffState, BuffState>(temp => temp.XixueWuli);

        }
    }

    /// <summary>
    /// 吸血-魔法
    /// </summary>
    BuffState? _XixueMofa;
    /// <summary>
    /// 吸血-魔法
    /// </summary>
    public BuffState XixueMofa
    {
        get
        {
            if (_XixueMofa == null)
                _XixueMofa = new BuffState(EnumStatusEffect.xx4, 0, null);
            return _XixueMofa.Value;
        }
        set
        {
            if (_XixueMofa == null)
                _XixueMofa = XixueMofa;
            if (_XixueMofa.Value.statusEffect != value.statusEffect)
                return;
            BuffState tempXixueMofa = _XixueMofa.Value;
            _XixueMofa = value;
            if (!tempXixueMofa.Equals(value))
                Call<IBuffState, BuffState>(temp => temp.XixueMofa);

        }
    }


    #endregion

}