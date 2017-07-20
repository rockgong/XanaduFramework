using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameKernal;
using UnityEngine.UI;
using UIUtil;

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
        }

		private MonoView _monoView;
		private IInteractViewListener _listener;

        private MonoTypewriter _dialogTypewriter = null;
        private MonoTypewriter _messageTypewriter = null;
        private MonoSelectPanel _selectPanel = null;

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
                                CloseMessage();
                                CloseDialog();
                                if (_listener != null)
                                    _listener.OnViewClosed();
                                bgButton.gameObject.SetActive(false);
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
                            tw.PlayTypewrite();
                    });
                    _viewState = ViewState.Message;
            	}
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
                            tw.PlayTypewrite();
                    });

                    _viewState = ViewState.Dialog;
                }
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
                            tw.PlayTypewrite();
                    });
                    _viewState = ViewState.Select;
                }
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
        }
	}
}