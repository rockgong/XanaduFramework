using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UIUtil
{
    public class MonoTypewriter : MonoBehaviour
    {
        public Text targetText;
        public float maxWidth;
        public float speed = 1.0f;
        // Use this for initialization
        private string _content;
        private float _letterCount = 0.0f;
        private bool _typing = false;
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            if (targetText != null &&_typing)
            {
                _letterCount += speed * Time.deltaTime;
                int length = (int)_letterCount;
                if (targetText.text.Length < length)
                    targetText.text = _content.Substring(0, length);
                if (length == _content.Length)
                    _typing = false;
            }
        }

        public void PlayTypewrite(string content)
        {
            if (targetText == null)
                return;
            targetText.text = content;
            _content = content;
            targetText.rectTransform.sizeDelta = new Vector2(Mathf.Min(maxWidth, targetText.preferredWidth), targetText.preferredHeight);
            targetText.rectTransform.sizeDelta = new Vector2(targetText.rectTransform.sizeDelta.x, targetText.preferredHeight);
            targetText.text = string.Empty;
            _typing = true;
            _letterCount = 0.0f;
        }

        public void EndType()
        {
            if (_typing)
            {
                _typing = false;
                targetText.text = _content;
            }
        }

        public bool typing
        {
            get
            {
                return _typing;
            }
        }
    }
}