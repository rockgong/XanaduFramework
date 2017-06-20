using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestBehavior : MonoBehaviour {
    public Animator animator;
    public float crossFadeTime = 0.3f;

    private string _stateName = string.Empty;
	// Use this for initialization
	void Start () {
        return;
	}
	
	// Update is called once per frame
	void Update () {
        
	}

    private void OnGUI()
    {
        if (animator == null)
            return;

        _stateName = GUILayout.TextField(_stateName);
        if (GUILayout.Button("Play"))
        {
            animator.CrossFade(_stateName, crossFadeTime);
        }
    }
}
