using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


/// <summary>
/// 实现了IEnvironment接口的GameState类的一个分支实体
/// </summary>
public partial class GameState : IEnvironment
{
    private EnumEnvironmentType _TerrainEnvironmentType;
    /// <summary>
    /// 环境类型
    /// </summary>
    public EnumEnvironmentType TerrainEnvironmentType
    {
        get
        {
            return _TerrainEnvironmentType;
        }

        set
        {
            EnumEnvironmentType tempTerrainEnvironmentType = _TerrainEnvironmentType;
            _TerrainEnvironmentType = value;
            if (TerrainEnvironmentType != tempTerrainEnvironmentType)
                Call<IEnvironment, EnumEnvironmentType>(temp => temp.TerrainEnvironmentType);
        }
    }
}

