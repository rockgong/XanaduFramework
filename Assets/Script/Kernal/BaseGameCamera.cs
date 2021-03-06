﻿using UnityEngine;

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
                public virtual Vector3 easingTarget
                {
                        get
                        {
                                return Vector3.zero;
                        }
                }

                public virtual Transform attachTransform
                {
                        get
                        {
                                return null;
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