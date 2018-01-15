using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/// <summary>
/// 将当前时间的毫秒数转换成ID
/// </summary>
public class NowTimeToID
{
    /// <summary>
    /// 开始编号
    /// </summary>
    static int startNum = 0;

    /// <summary>
    /// 获取一个当前的随机id 
    /// </summary>
    /// <param name="gameRunnedState">游戏运行过的状态</param>
    /// <returns></returns>
    public static int GetNowID(GameRunnedState gameRunnedState)
    {
        int id = gameRunnedState.StartTimes * 100000 + (++startNum);
        return id;
    }


}


