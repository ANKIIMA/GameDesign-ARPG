using UnityEngine;

namespace SharpUI.Source.Common.UI.Util.TimeUtils
{
    public interface ITimeProvider
    {
        float GetDeltaTime();
        float GetFixedDeltaTime();
    }
    
    public class TimeProvider : ITimeProvider
    {
        public float GetDeltaTime() => Time.deltaTime;
        
        public float GetFixedDeltaTime() => Time.fixedDeltaTime;
    }
}