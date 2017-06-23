using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UIUtil
{
    public class MonoViewOpenAnim : MonoBehaviour
    {
        public float duration = 1.0f;

        private System.Action _onFinish;
        private bool _isPlaying;
        
        private float _timer = 0.0f;
        private RectTransform _rectTrans;
        
        // Use this for initialization
        void Start()
        {
            _rectTrans = GetComponent<RectTransform>();
        }

        // Update is called once per frame
        void Update()
        {
            if (_isPlaying)
            {
                _timer += Time.deltaTime;
                if (_timer >= duration)
                {
                    if (_onFinish != null)
                        _onFinish();
                    _rectTrans.localScale = Vector3.one;
                    _isPlaying = false;
                }
                else
                {
                    _rectTrans.localScale = Vector3.one * (_timer / duration);
                }
            }
        }

        public void Play(System.Action onFinish)
        {
            if (!_isPlaying)
            {
                _onFinish = onFinish;
                _isPlaying = true;
                _timer = 0.0f;
                _rectTrans = GetComponent<RectTransform>();
                if (_rectTrans != null)
                    _rectTrans.localScale = Vector3.zero;
            }
        }
    }
}