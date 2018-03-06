using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 实现了IPlayerState->ISpecialState接口的GameState类的一个分支实体
/// </summary>
public partial class GameState
{

    #region ISpecialState 特殊状态

    /// <summary>
    /// 嘲讽
    /// </summary>
    BuffState? _Chaofeng;
    /// <summary>
    /// 嘲讽
    /// </summary>
    public BuffState Chaofeng
    {
        get
        {
            if (_Chaofeng == null)
                _Chaofeng = new BuffState(EnumStatusEffect.cf2,0, null);
            return _Chaofeng.Value;
        }
        set
        {
            if (_Chaofeng == null)
                _Chaofeng = Chaofeng;
            if (_Chaofeng.Value.statusEffect != value.statusEffect)
                return;
            BuffState tempChaofeng = _Chaofeng.Value;
            _Chaofeng = value;
            if (!tempChaofeng.Equals(value))
                Call<ISpecialState, BuffState>(temp => temp.Chaofeng);

        }
    }

    /// <summary>
    /// 混乱
    /// </summary>
    BuffState? _Hunluan;
    /// <summary>
    /// 混乱
    /// </summary>
    public BuffState Hunluan
    {
        get
        {
            if (_Hunluan == null)
                _Hunluan = new BuffState(EnumStatusEffect.hl1, 0, null);
            return _Hunluan.Value;
        }
        set
        {
            if (_Hunluan == null)
                _Hunluan = Hunluan;
            if (_Hunluan.Value.statusEffect != value.statusEffect)
                return;
            BuffState tempHunluan = _Hunluan.Value;
            _Hunluan = value;
            if (!tempHunluan.Equals(value))
                Call<ISpecialState, BuffState>(temp => temp.Hunluan);

        }
    }

    /// <summary>
    /// 僵直
    /// </summary>
    BuffState? _Jiangzhi;
    /// <summary>
    /// 僵直
    /// </summary>
    public BuffState Jiangzhi
    {
        get
        {
            if (_Jiangzhi == null)
                _Jiangzhi = new BuffState(EnumStatusEffect.jz6, 0, null);
            return _Jiangzhi.Value;
        }
        set
        {
            if (_Jiangzhi == null)
                _Jiangzhi = Jiangzhi;
            if (_Jiangzhi.Value.statusEffect != value.statusEffect)
                return;
            BuffState tempJiangzhi = _Jiangzhi.Value;
            _Jiangzhi = value;
            if (!tempJiangzhi.Equals(value))
                Call<ISpecialState, BuffState>(temp => temp.Jiangzhi);

        }
    }

    /// <summary>
    /// 恐惧
    /// </summary>
    BuffState? _Kongju;
    /// <summary>
    /// 恐惧
    /// </summary>
    public BuffState Kongju
    {
        get
        {
            if (_Kongju == null)
                _Kongju = new BuffState(EnumStatusEffect.kj1, 0, null);
            return _Kongju.Value;
        }
        set
        {
            if (_Kongju == null)
                _Kongju = Kongju;
            if (_Kongju.Value.statusEffect != value.statusEffect)
                return;
            BuffState tempKongju = _Kongju.Value;
            _Kongju = value;
            if (!tempKongju.Equals(value))
                Call<ISpecialState, BuffState>(temp => temp.Kongju);

        }
    }

    /// <summary>
    /// 魅惑
    /// </summary>
    BuffState? _Meihuo;
    /// <summary>
    /// 魅惑
    /// </summary>
    public BuffState Meihuo
    {
        get
        {
            if (_Meihuo == null)
                _Meihuo = new BuffState(EnumStatusEffect.mh4, 0, null);
            return _Meihuo.Value;
        }
        set
        {
            if (_Meihuo == null)
                _Meihuo = Meihuo;
            if (_Meihuo.Value.statusEffect != value.statusEffect)
                return;
            BuffState tempMeihuo = _Meihuo.Value;
            _Meihuo = value;
            if (!tempMeihuo.Equals(value))
                Call<ISpecialState, BuffState>(temp => temp.Meihuo);

        }
    }

    /// <summary>
    /// 眩晕
    /// </summary>
    BuffState? _Xuanyun;
    /// <summary>
    /// 眩晕
    /// </summary>
    public BuffState Xuanyun
    {
        get
        {
            if (_Xuanyun == null)
                _Xuanyun = new BuffState(EnumStatusEffect.xy1, 0, null);
            return _Xuanyun.Value;
        }
        set
        {
            if (_Xuanyun == null)
                _Xuanyun = Xuanyun;
            if (_Xuanyun.Value.statusEffect != value.statusEffect)
                return;
            BuffState tempXuanyun = _Xuanyun.Value;
            _Xuanyun = value;
            if (!tempXuanyun.Equals(value))
                Call<ISpecialState, BuffState>(temp => temp.Xuanyun);

        }
    }

    /// <summary>
    /// 致盲
    /// </summary>
    BuffState? _Zhimang;
    /// <summary>
    /// 致盲
    /// </summary>
    public BuffState Zhimang
    {
        get
        {
            if (_Zhimang == null)
                _Zhimang = new BuffState(EnumStatusEffect.zm1, 0, null);
            return _Zhimang.Value;
        }
        set
        {
            if (_Zhimang == null)
                _Zhimang = Zhimang;
            if (_Zhimang.Value.statusEffect != value.statusEffect)
                return;
            BuffState tempZhimang = _Zhimang.Value;
            _Zhimang = value;
            if (!tempZhimang.Equals(value))
                Call<ISpecialState, BuffState>(temp => temp.Zhimang);

        }
    }

    /// <summary>
    /// 禁锢
    /// </summary>
    BuffState? _Jingu;
    /// <summary>
    /// 禁锢
    /// </summary>
    public BuffState Jingu
    {
        get
        {
            if (_Jingu == null)
                _Jingu = new BuffState(EnumStatusEffect.jg2, 0, null);
            return _Jingu.Value;
        }
        set
        {
            if (_Jingu == null)
                _Jingu = Jingu;
            if (_Jingu.Value.statusEffect != value.statusEffect)
                return;
            BuffState tempJingu = _Jingu.Value;
            _Jingu = value;
            if (!tempJingu.Equals(value))
                Call<ISpecialState, BuffState>(temp => temp.Jingu);

        }
    }

    /// <summary>
    /// 禁魔
    /// </summary>
    BuffState? _Jinmo;
    /// <summary>
    /// 禁魔
    /// </summary>
    public BuffState Jinmo
    {
        get
        {
            if (_Jinmo == null)
                _Jinmo = new BuffState(EnumStatusEffect.jm3, 0, null);
            return _Jinmo.Value;
        }
        set
        {
            if (_Jinmo == null)
                _Jinmo = Jinmo;
            if (_Jinmo.Value.statusEffect != value.statusEffect)
                return;
            BuffState tempJinmo = _Jinmo.Value;
            _Jinmo = value;
            if (!tempJinmo.Equals(value))
                Call<ISpecialState, BuffState>(temp => temp.Jinmo);

        }
    }

    /// <summary>
    /// 麻痹
    /// </summary>
    BuffState? _Mabi;
    /// <summary>
    /// 麻痹
    /// </summary>
    public BuffState Mabi
    {
        get
        {
            if (_Mabi == null)
                _Mabi = new BuffState(EnumStatusEffect.mb2, 0, null);
            return _Mabi.Value;
        }
        set
        {
            if (_Mabi == null)
                _Mabi = Mabi;
            if (_Mabi.Value.statusEffect != value.statusEffect)
                return;
            BuffState tempMabi = _Mabi.Value;
            _Mabi = value;
            if (!tempMabi.Equals(value))
                Call<ISpecialState, BuffState>(temp => temp.Mabi);

        }
    }


    #endregion

}