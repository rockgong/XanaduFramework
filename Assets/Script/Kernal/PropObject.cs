using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameKernal
{
    class PropObject : BasePropObject, IMonoEntityHost, IInteractObject
    {
        private MonoEntity _entity;
        private Animator _animator;

        public override void Initialize(PropObjectDesc desc)
        {
            GameObject gameObject = GameObject.Instantiate(desc.prototype);
            _entity = gameObject.AddComponent<MonoEntity>();
            _entity.SetHost(this);
            _animator = gameObject.GetComponentInChildren<Animator>();

            return;
        }

        public override Vector3 position
        {
            get
            {
                if (_entity != null)
                    return _entity.transform.position;
                return Vector3.zero;
            }
            set
            {
                if (_entity != null)
                    _entity.transform.position = value;
            }
        }

        public override Vector3 viewPosition
        {
            get
            {
                if (_entity != null)
                {
                    MonoPlayerConfig config = _entity.GetComponent<MonoPlayerConfig>();
                    if (config != null)
                        return config.viewTransform.position;
                }
                return Vector3.zero;
            }
        }

        public void OnCollisionEnter(MonoEntity entity, Collision collision)
        {

        }

        public void OnCollisionExit(MonoEntity entity, Collision collision)
        {

        }

        public void OnCollisionStay(MonoEntity entity, Collision collision)
        {

        }

        public void OnFixedUpdate(MonoEntity entity)
        {

        }

        public void OnGetOutOfReadyToBeInteracted(IInteractSubject sub)
        {

        }

        public void OnGetReadyToBeInteracted(IInteractSubject sub)
        {

        }

        public void OnInteractedBy(IInteractSubject sub)
        {

        }

        public void OnLateUpdate(MonoEntity entity, float deltaTime)
        {

        }

        public void OnStart(MonoEntity entity)
        {

        }

        public void OnTriggerEnter(MonoEntity entity, Collider other)
        {

        }

        public void OnTriggerExit(MonoEntity entity, Collider other)
        {

        }

        public void OnTriggerStay(MonoEntity entity, Collider other)
        {

        }

        public void OnUpdate(MonoEntity entity, float deltaTime)
        {

        }

        public override void Uninitialize(PropObjectDesc desc)
        {
            _entity.SetHost(null);
            GameObject.Destroy(_entity.gameObject);

            return;
        }
    }
}