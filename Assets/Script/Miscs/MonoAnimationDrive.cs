using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Miscs
{
	public class MonoAnimationDrive : MonoBehaviour
	{
		public Animator targetAnimator;

		public void PlayAnimation(string animName)
		{
			if (targetAnimator != null)
			{
				targetAnimator.Play(animName);
			}
		}
	}
}