using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace GameKernal
{
	interface IInteractSubject
	{
		void OnGetReadyToInteract(IInteractObject obj);
		void OnGetOutOfReadyToInteract(IInteractObject obj);
		void OnInteractWith(IInteractObject obj);
		Vector3 position{get; set;}
	}

	interface IInteractObject
	{
		void OnGetReadyToBeInteracted(IInteractSubject sub);
		void OnGetOutOfReadyToBeInteracted(IInteractSubject sub);
		void OnInteractedBy(IInteractSubject sub);
		Vector3 position{get; set;}
	}

	interface IInteractListener
	{
		void OnInteractHappen(IInteractSubject sub, IInteractObject obj);
	}

	class InteractSystem
	{
		private IInteractSubject _subject;
		private List<IInteractObject> _objectList = new List<IInteractObject>();
		private List<IInteractListener> _listenerList = new List<IInteractListener>();
		private IInteractObject _readyToInteractObject;

		public void Initialize(IInteractSubject subject)
		{
			_subject = subject;
		}

		public void AddInteractObject(IInteractObject interObject)
		{
			if (_objectList.Contains(interObject))
				return;
			_objectList.Add(interObject);
		}

		public void RemoveInteractObject(IInteractObject interObject)
		{
			if (!_objectList.Contains(interObject))
				_objectList.Remove(interObject);
		}

		public IInteractObject FindInteractObject(Predicate<IInteractObject> pred)
		{
			return _objectList.Find(pred);
		}

		public List<IInteractObject> FindInteractObjects(Predicate<IInteractObject> pred)
		{
			return _objectList.FindAll(pred);
		}

        public void AddInteractionListener(IInteractListener listener)
        {
            if (_listenerList.Contains(listener))
                return;

            _listenerList.Add(listener);
        }

        public void RemoveInteractionListener(IInteractListener listener)
        {
            if (_listenerList.Contains(listener))
                _listenerList.Remove(listener);
        }

		public void UpdateReadyToInteract()
		{
			IInteractObject result = null;
			for (int i = 0; i < _objectList.Count; i++)
			{
				if ((_objectList[i].position - _subject.position).magnitude < 3.0f)
				{
					result = _objectList[i];
					break;
				}
			}
			if (_readyToInteractObject != null && _readyToInteractObject != result)
			{
				_subject.OnGetOutOfReadyToInteract(_readyToInteractObject);
				_readyToInteractObject.OnGetOutOfReadyToBeInteracted(_subject);
			}

			if (result != null && _readyToInteractObject != result)
			{
				_subject.OnGetReadyToInteract(result);
				result.OnGetReadyToBeInteracted(_subject);
			}

			_readyToInteractObject = result;

			return;
		}

		public void TryInteract()
		{
			if (_readyToInteractObject == null)
				return;

			_subject.OnInteractWith(_readyToInteractObject);
			_readyToInteractObject.OnInteractedBy(_subject);
			for (int i = 0; i < _listenerList.Count; i++)
			{
				_listenerList[i].OnInteractHappen(_subject, _readyToInteractObject);
			}
		}
	}
}