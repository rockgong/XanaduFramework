using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameKernal;
using UnityEngine.UI;
using UIUtil;
using Miscs;
using Audio;

namespace MainGame
{
	interface IInteractViewListener
	{
		void OnViewClosed();
	}

	class InteractView
	{
        public enum ViewState
        {
            None,
            Message,
            Dialog,
            Select,
            GetInv
        }

		private MonoView _monoView;
		private IInteractViewListener _listener;

        private MonoTypewriter _dialogTypewriter = null;
        private MonoTypewriter _messageTypewriter = null;
        private MonoSelectPanel _selectPanel = null;
        private MonoDelegate _updateDelegate = null;
        private System.Action _tempFadeAction = null;

        private ViewState _viewState;
        public ViewState viewState
        {
            get
            {
                return _viewState;
            }
        }

		public void Initialize()
		{
			GameObject viewProto = Resources.Load<GameObject>("UI/InteractView");
			if (viewProto != null)
			{
				GameObject viewInst = GameObject.Instantiate(viewProto);
				_monoView = viewInst.GetComponent<MonoView>();
				if (_monoView != null)
				{
					Button bgButton = _monoView.GetWidget<Button>("bg_button");
					if (bgButton != null)
						bgButton.onClick.AddListener(() =>
						{
                            if (_dialogTypewriter.typing)
                            {
                                _dialogTypewriter.EndType();
                            }
                            else if (_messageTypewriter.typing)
                            {
                                _messageTypewriter.EndType();
                            }
                            else
							{
                                if (_viewState != ViewState.GetInv)
                                {
                                    CloseMessage();
                                    CloseDialog();
                                    if (_listener != null)
                                        _listener.OnViewClosed();
                                    bgButton.gameObject.SetActive(false);
                                }
                            }
						});
                    _dialogTypewriter = _monoView.GetWidget<MonoTypewriter>("dialog_root");
                    _messageTypewriter = _monoView.GetWidget<MonoTypewriter>("message_root");
                    _selectPanel = _monoView.GetWidget<MonoSelectPanel>("select_root");
                }
			}
		}

		public void SetListener(IInteractViewListener listener)
		{
			_listener = listener;

			return;
		}

		public void ShowMessage(string msg)
		{
			Transform trans = _monoView.GetWidget<Transform>("message_root");
            if (trans != null)
            {
                trans.gameObject.SetActive(true);
                MonoViewOpenAnim anim = trans.GetComponent<MonoViewOpenAnim>();
                MonoTypewriter tw = trans.GetComponent<MonoTypewriter>();
                if (tw != null)
                    tw.Setup(msg);
                if (anim != null)
                {
	                anim.Play(() =>
	                {
		                Button bgButton = _monoView.GetWidget<Button>("bg_button");
		                bgButton.gameObject.SetActive(true);

                        if (tw != null)
                        {
                            tw.PlayTypewrite();
                        }
                    });
                    SoundEffect.Play("Btn_1");
                    _viewState = ViewState.Message;
            	}
                HideReady();
            }

			return;
		}

		public void CloseMessage()
		{
			Transform trans = _monoView.GetWidget<Transform>("message_root");
			if (trans != null)
				trans.gameObject.SetActive(false);
            /*
			if (_listener != null)
				_listener.OnViewClosed();
            */

            _viewState = ViewState.None;
            TryResumeReady();

            return;
		}

		public void ShowDialog(string msg)
		{
			Transform trans = _monoView.GetWidget<Transform>("dialog_root");
			if (trans != null)
			{
				trans.gameObject.SetActive(true);

                MonoViewOpenAnim anim = trans.GetComponent<MonoViewOpenAnim>();
                MonoTypewriter tw = trans.GetComponent<MonoTypewriter>();
                if (tw != null)
                    tw.Setup(msg);
                if (anim != null)
                {
                    anim.Play(() =>
                    {
                        Button bgButton = _monoView.GetWidget<Button>("bg_button");
                        bgButton.gameObject.SetActive(true);

                        if (tw != null)
                        {
                            tw.PlayTypewrite();
                        }
                    });
                    SoundEffect.Play("Btn_1");
                    _viewState = ViewState.Dialog;
                }
                HideReady();
            }

			return;
		}

		public void CloseDialog()
		{
			Transform trans = _monoView.GetWidget<Transform>("dialog_root");
			if (trans != null)
				trans.gameObject.SetActive(false);
            /*
			if (_listener != null)
				_listener.OnViewClosed();
            */
            _viewState = ViewState.None;
            TryResumeReady();

            return;
		}

        public void ShowSelect(string title, string[] options, System.Action<int> callback)
        {
            Transform trans = _monoView.GetWidget<Transform>("message_root");
            if (trans != null)
            {
                trans.gameObject.SetActive(true);
                MonoViewOpenAnim anim = trans.GetComponent<MonoViewOpenAnim>();
                MonoTypewriter tw = trans.GetComponent<MonoTypewriter>();
                _selectPanel.Setup(options, (i) =>
                {
                    CloseSelect();
                    callback(i);
                });
                if (tw != null)
                    tw.Setup(title, () =>
                    {
                        _selectPanel.gameObject.SetActive(true);
                        MonoViewOpenAnim _selectAnim = _selectPanel.GetComponent<MonoViewOpenAnim>();
                        if (_selectAnim != null)
                            _selectAnim.Play(null);
                    });
                if (anim != null)
                {
                    anim.Play(() =>
                    {
                        Button bgButton = _monoView.GetWidget<Button>("bg_button");
                        bgButton.gameObject.SetActive(false);

                        if (tw != null)
                        {
                            tw.PlayTypewrite();
                        }
                    });
                    SoundEffect.Play("Btn_1");
                    _viewState = ViewState.Select;
                }
                HideReady();
            }


            return;
        }

        public void CloseSelect()
        {
            _selectPanel.gameObject.SetActive(false);

            Transform trans = _monoView.GetWidget<Transform>("message_root");
            if (trans != null)
                trans.gameObject.SetActive(false);

            if (_listener != null)
                _listener.OnViewClosed();


            _viewState = ViewState.None;
            TryResumeReady();
        }

        public void ShowReady(INonPlayerCharacter nonPlayer)
        {
            Transform trans = _monoView.GetWidget<Transform>("ready_root");
            if (trans != null)
            {
                trans.gameObject.SetActive(true);
                MonoScreenCover sc = trans.GetComponent<MonoScreenCover>();
                if (sc != null)
                {
                    sc.targetAlpha = 1.0f;
                    sc.action = null;
                }

                if (trans is RectTransform)
                {
                    RectTransform rt = (RectTransform)trans;
                    rt.anchoredPosition = UIUtils.WorldPointToCanvasAnchoredPosition(nonPlayer.viewPosition, new Vector2(1280.0f, 720.0f));
                    _updateDelegate = MonoDelegate.Create(() =>
                    {
                        rt.anchoredPosition = UIUtils.WorldPointToCanvasAnchoredPosition(nonPlayer.viewPosition, new Vector2(1280.0f, 720.0f));
                    }, "_ReadyToInteractDelegate");
                }
            }

            return;
        }

        public void ShowReady(IPropObject propObject)
        {
            Transform trans = _monoView.GetWidget<Transform>("ready_root");
            if (trans != null)
            {
                trans.gameObject.SetActive(true);
                MonoScreenCover sc = trans.GetComponent<MonoScreenCover>();
                if (sc != null)
                {
                    sc.targetAlpha = 1.0f;
                    sc.action = null;
                }

                if (trans is RectTransform)
                {
                    RectTransform rt = (RectTransform)trans;
                    rt.anchoredPosition = UIUtils.WorldPointToCanvasAnchoredPosition(propObject.viewPosition, new Vector2(1280.0f, 720.0f));
                    if (_updateDelegate != null)
                        GameObject.Destroy(_updateDelegate.gameObject);
                    _updateDelegate = MonoDelegate.Create(() =>
                    {
                        rt.anchoredPosition = UIUtils.WorldPointToCanvasAnchoredPosition(propObject.viewPosition, new Vector2(1280.0f, 720.0f));
                    }, "_ReadyToInteractDelegate");
                }
            }

            return;
        }

        public void CloseReady()
        {
            Transform trans = _monoView.GetWidget<Transform>("ready_root");
            if (trans != null)
            {
                MonoScreenCover sc = trans.GetComponent<MonoScreenCover>();
                if (sc != null)
                {
                    sc.targetAlpha = 0.0f;
                    sc.action = () => 
                    {
                        trans.gameObject.SetActive(false);

                        if (_updateDelegate != null)
                        {
                            GameObject.Destroy(_updateDelegate.gameObject);
                            _updateDelegate = null;
                        }
                    };
                }
            }

            return;
        }

        private void HideReady()
        {
            Transform trans = _monoView.GetWidget<Transform>("ready_root");
            if (trans != null)
            {
                MonoScreenCover sc = trans.GetComponent<MonoScreenCover>();
                if (sc != null)
                {
                    sc.targetAlpha = 0.0f;
                    _tempFadeAction = sc.action;
                    sc.action = null;
                }
            }
        }

        private void TryResumeReady()
        {
            if (_updateDelegate != null)
            {
                Transform trans = _monoView.GetWidget<Transform>("ready_root");
                if (trans != null)
                {
                    MonoScreenCover sc = trans.GetComponent<MonoScreenCover>();
                    if (sc != null)
                    {
                        sc.targetAlpha = 1.0f;
                        sc.action = _tempFadeAction;
                    }
                }
            }
        }

        public void ShowGetInv(string name)
        {
            Transform trans = _monoView.GetWidget<Transform>("getinv_root");
            if (trans != null)
            {
                trans.gameObject.SetActive(true);
                MonoViewOpenAnim anim = trans.GetComponent<MonoViewOpenAnim>();

                if (anim != null)
                {
                    Text nameText = _monoView.GetWidget<Text>("getinv_msg");
                    if (nameText != null)
                        nameText.text = string.Empty;
                    anim.Play(() =>
                    {
                        if (nameText != null)
                            nameText.text = name;
                    });
                    SoundEffect.Play("Get_Item");
                    _viewState = ViewState.GetInv;
                }
                HideReady();
            }

            return;
        }

        public void CloseGetInv()
        {
            Transform trans = _monoView.GetWidget<Transform>("getinv_root");
            if (trans != null)
                trans.gameObject.SetActive(false);

            _viewState = ViewState.None;
            TryResumeReady();
        }
	}
}