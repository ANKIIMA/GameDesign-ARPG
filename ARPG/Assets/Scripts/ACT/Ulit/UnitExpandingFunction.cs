using UnityEngine;

public static class UnitExpandingFunction
{

    /// <summary>
    /// 检测动画标签
    /// </summary>
    /// <param name="animator"></param>
    /// <param name="tagName"></param>
    /// <param name="animationIndex"></param>
    /// <returns></returns>
    public static bool CheckAnimationTag(this Animator animator, string tagName, int animationIndex = 0)
    {
        return animator.GetCurrentAnimatorStateInfo(animationIndex).IsTag(tagName);
    }
    /// <summary>
    /// 检测动画片段名称
    /// </summary>
    /// <param name="animator"></param>
    /// <param name="animationName"></param>
    /// <param name="animationIndex"></param>
    /// <returns></returns>
    public static bool CheckAnimationName(this Animator animator, string animationName, int animationIndex = 0)
    {
        return animator.GetCurrentAnimatorStateInfo(animationIndex).IsName(animationName);
    }
    
    /// <summary>
    /// 锁定目标方向
    /// </summary>
    /// <param name="transform"></param>
    /// <param name="target"></param>
    /// <param name="self"></param>
    /// <param name="lerpTime"></param>
    /// <returns></returns>
    public static Quaternion LockOnTarget(this Transform transform, Transform target,Transform self,float lerpTime)
    {
        if (target == null) return self.rotation;

        Vector3 targetDirection = (target.position - self.position).normalized;
        targetDirection.y = 0f;

        Quaternion newRotation = Quaternion.LookRotation(targetDirection);
        
        return  Quaternion.Lerp(self.rotation,newRotation,lerpTime * Time.deltaTime);
    }

    /// <summary>
    /// 检查当前播放时间是否大于给定时间
    /// </summary>
    /// <param name="animator"></param>
    /// <param name="tagName">状态标签</param>
    /// <param name="time">给定时间</param>
    /// <returns></returns>
    public static bool CheckCurrentAnimationTagTimeIsGreater(this Animator animator, string tagName, float time)
    {
        if(animator.CheckAnimationTag(tagName))
        {
            return animator.GetCurrentAnimatorStateInfo(0).normalizedTime > time ? true : false;
        }

        return false;
    }

    /// <summary>
    /// 检查当前播放时间是否大于给定时间
    /// </summary>
    /// <param name="animator"></param>
    /// <param name="tagName">状态标签</param>
    /// <param name="time">给定时间</param>
    /// <returns></returns>
    public static bool CheckCurrentAnimationNameTimeIsGreater(this Animator animator, string name, float time)
    {
        if (animator.CheckAnimationTag(name))
        {
            return animator.GetCurrentAnimatorStateInfo(0).normalizedTime > time ? true : false;
        }

        return false;
    }

    /// <summary>
    /// 检查当前播放时间是否小于给定时间
    /// </summary>
    /// <param name="animator"></param>
    /// <param name="tagName">状态标签</param>
    /// <param name="time">给定时间</param>
    /// <returns></returns>
    public static bool CheckCurrentAnimationTagTimeIsLower(this Animator animator, string tagName, float time)
    {
        if (animator.CheckAnimationTag(tagName))
        {
            return animator.GetCurrentAnimatorStateInfo(0).normalizedTime < time ? true : false;
        }

        return false;
    }

    /// <summary>
    /// 检查当前播放时间是否小于给定时间
    /// </summary>
    /// <param name="animator"></param>
    /// <param name="tagName">状态标签</param>
    /// <param name="time">给定时间</param>
    /// <returns></returns>
    public static bool CheckCurrentAnimationNameTimeIsLower(this Animator animator, string name, float time)
    {
        if (animator.CheckAnimationName(name))
        {
            return animator.GetCurrentAnimatorStateInfo(0).normalizedTime < time ? true : false;
        }

        return false;
    }
}
