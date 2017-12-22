using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 粒子控制入口
/// </summary>
public abstract class ParticalControlEntry : MonoBehaviour, IParticalConduct
{
    /// <summary>
    /// 生命周期时间(这里是最长时间)
    /// </summary>
    protected float lifeTime;

    /// <summary>
    /// 当前的生命持续时间
    /// </summary>
    protected float _lifeTimeNow;

    /// <summary>
    /// 检测碰撞的间隔时间
    /// </summary>
    protected float checkCollisionIntervalTime = 1;

    /// <summary>
    /// 初始化
    /// </summary>
    /// <param name="pos">位置</param>
    /// <param name="forward">方向</param>
    /// <param name="layerMask">碰撞遮罩</param>
    /// <param name="CollisionCallback">碰撞后回调</param>
    /// 下面的这些参数属于可选参数,不同的粒子不一定有效 
    /// <param name="range">范围</param>
    /// <param name="targetObjs">跟踪目标(如果需要跟踪)</param>
    public abstract void Init(
        Vector3 pos,
        Vector3 forward,
        Color color,
        LayerMask layerMask,
        Func<CollisionHitCallbackStruct, bool> CollisionCallback,
        float range,
        params GameObject[] targetObjs);

    /// <summary>
    /// 设置生命周期
    /// </summary>
    /// <param name="lifeTime">生命周期时间(这里是最长时间)</param>
    public virtual void SetLifeCycle(float lifeTime)
    {
        this.lifeTime = lifeTime;
        _lifeTimeNow = 0;
    }

    /// <summary>
    /// 检测碰撞的间隔时间
    /// 只有持续性的效果会使用该参数
    /// </summary>
    /// <param name="intervalTime">检测碰撞的间隔时间</param>
    public virtual void SetCheckCollisionIntervalTime(float intervalTime)
    {
        this.checkCollisionIntervalTime = intervalTime;
    }

    /// <summary>
    /// 更新
    /// </summary>
    protected virtual void Update()
    {
        //如果现在是默认的生命周期时间(0),则将其设置为3
        if (this.lifeTime <= 0)
            this.lifeTime = 3;
        _lifeTimeNow += Time.deltaTime;
        if (_lifeTimeNow > this.lifeTime)//超出指定时间则销毁粒子
            Destroy(gameObject);
    }

    /// <summary>
    /// 便利子节点
    /// </summary>
    /// <typeparam name="T">要查找的类型</typeparam>
    /// <param name="trans">节点对象</param>
    /// <param name="callback">回调</param>
    private void ForeachChildNode<T>(Transform trans, Action<T> callback, bool first = true)
    {
        if (first)
        {
            T[] ts  = trans.GetComponents<T>();
            foreach (T t in ts)
            {
                if (!object.Equals(t, this) && callback != null)
                {
                    callback(t);
                }
                
            }
        }
        int count = trans.childCount;
        for (int i = 0; i < count; i++)
        {
            Transform childTrans = trans.GetChild(i);
            T[] ts = childTrans.GetComponents<T>();
            if (ts != null && callback != null)
            {
                foreach (T t in ts)
                {
                    callback(t);
                }
            }
            ForeachChildNode<T>(childTrans, callback, false);
        }
    }

    /// <summary>
    /// 设置子节点的碰撞回调
    /// </summary>
    /// <param name="CallBack"></param>
    public virtual void SetCollisionCallback(Func<CollisionHitCallbackStruct, bool> CallBack)
    {
        ForeachChildNode<IParticalConduct>(transform, temp => temp.SetCollisionCallback(CallBack));
    }

    /// <summary>
    /// 设置子节点的颜色
    /// </summary>
    /// <param name="color"></param>
    public virtual void SetColor(Color color)
    {
        ForeachChildNode<IParticalConduct>(transform, temp => temp.SetColor(color));
    }

    /// <summary>
    /// 设置子节点的朝向
    /// </summary>
    /// <param name="forward"></param>
    public virtual void SetForward(Vector3 forward)
    {
        ForeachChildNode<IParticalConduct>(transform, temp => temp.SetForward(forward));
    }

    /// <summary>
    /// 设置子结点的碰撞层
    /// </summary>
    /// <param name="layerMask"></param>
    public virtual void SetLayerMask(LayerMask layerMask)
    {
        ForeachChildNode<IParticalConduct>(transform, temp => temp.SetLayerMask(layerMask));
    }

    public virtual void SetRange(float range)
    {
        ForeachChildNode<IParticalConduct>(transform, temp => temp.SetRange(range));
    }
}

/// <summary>
/// 粒子初始化时所用参数的结构
/// </summary>
public struct ParticalInitParamData
{
    /// <summary>
    /// 位置
    /// </summary>
    public Vector3 position;
    /// <summary>
    /// 粒子朝向
    /// </summary>
    public Vector3 forward;
    /// <summary>
    /// 粒子颜色
    /// </summary>
    public Color color;
    /// <summary>
    /// 粒子的碰撞遮罩
    /// </summary>
    public LayerMask layerMask;
    /// <summary>
    /// 碰撞后的回掉
    /// </summary>
    public Func<CollisionHitCallbackStruct, bool> CollisionCallBack;
    /// <summary>
    /// 范围属性(也许是长度也许是范围)
    /// </summary>
    public float range;
    /// <summary>
    /// 目标对象(有些粒子有 有些粒子没有)
    /// </summary>
    public GameObject[] targetObjs;
    /// <summary>
    /// 粒子的生命周期
    /// </summary>
    public float lifeTime;
    /// <summary>
    /// 粒子碰撞的间隔时间(有些粒子有 有些粒子没有)
    /// </summary>
    public float checkCollisionIntervalTime;
}
