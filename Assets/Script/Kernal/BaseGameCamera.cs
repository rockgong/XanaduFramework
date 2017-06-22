using UnityEngine;

namespace GameKernal
{
	abstract class BaseGameCamera : ICamera
	{
                public virtual Vector3 lookPosition
                {
                	get
                	{
                		return Vector3.zero;
                	}
                	set
                	{
                		return;
                	}
                }
                public virtual Vector3 offset
                {
                	get
                	{
                		return Vector3.zero;
                	}
                	set
                	{
                		return;
                	}
                }

                public abstract void Initialize();
                public abstract void Uninitialize();
                public abstract void EasingMoveTo(Vector3 target, System.Action onFinish = null);
        }
}