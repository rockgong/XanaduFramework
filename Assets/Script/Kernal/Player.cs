using UnityEngine;
using System;

namespace GameKernal
{
    class Player : BasePlayer, IMonoEntityHost, IInteractSubject, IInteractObject
    {
        private MonoEntity _entity;
        private Rigidbody _rigidbody;
        private Animator _animator;

        public override void Initialize(PlayerCharacterDesc desc)
        {
            if (_entity == null)
            {
                GameObject gameObject = GameObject.Instantiate(desc.prototype);
                _entity = gameObject.AddComponent<MonoEntity>();
                _entity.SetHost(this);

                _rigidbody = gameObject.GetComponent<Rigidbody>();
                _rigidbody.isKinematic = false;
                _animator = gameObject.GetComponentInChildren<Animator>();
            }

            return;
        }

        public override void Initialize(NonPlayerCharacterDesc desc)
        {
            if (_entity == null)
            {
                GameObject gameObject = GameObject.Instantiate(desc.prototype);
                _entity = gameObject.AddComponent<MonoEntity>();
                _entity.SetHost(this);

                _rigidbody = gameObject.GetComponent<Rigidbody>();
                _rigidbody.isKinematic = true;
                _animator = gameObject.GetComponentInChildren<Animator>();
            }

            return;
        }

        public override void PlayAnimation(string name)
        {
            if (_animator != null)
                _animator.CrossFade(name, 0.3f);
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
                return position;
            }
        }

        public override Vector3 controlPosition
        {
            get
            {
                if (_entity != null)
                {
                    MonoPlayerConfig config = _entity.GetComponent<MonoPlayerConfig>();
                    if (config != null)
                        return config.controlTransform.position;
                }
                return Vector3.zero;
            }
        }

        public void OnCollisionEnter(MonoEntity entity, Collision collision)
        {
            //Do nothing...
            return;
        }

        public void OnCollisionExit(MonoEntity entity, Collision collision)
        {
            //Do nothing...
            return;
        }

        public void OnCollisionStay(MonoEntity entity, Collision collision)
        {
            //Do nothing...
            return;
        }

        public void OnFixedUpdate(MonoEntity entity)
        {
            //Do nothing...
            return;
        }

        public void OnLateUpdate(MonoEntity entity, float deltaTime)
        {
            //Do nothing...
            return;
        }

        public void OnStart(MonoEntity entity)
        {
            //Do nothing...
            return;
        }

        public void OnTriggerEnter(MonoEntity entity, Collider other)
        {
            //Do nothing...
            return;
        }

        public void OnTriggerExit(MonoEntity entity, Collider other)
        {
            //Do nothing...
            return;
        }

        public void OnTriggerStay(MonoEntity entity, Collider other)
        {
            //Do nothing...
            return;
        }

        public void OnUpdate(MonoEntity entity, float deltaTime)
        {
            if (entity != null && entity == _entity)
            {
                entity.transform.eulerAngles = new Vector3(0.0f, yaw, 0.0f);
                if (_rigidbody != null)
                    _rigidbody.velocity = entity.transform.forward * velocity;

                if (_animator != null)
                    _animator.SetFloat("velocity", velocity);
            }
            return;
        }

        public override void Uninitialize()
        {
            if (_entity != null)
            {
                _entity.SetHost(null);
                GameObject.Destroy(_entity.gameObject);
                _entity = null;
            }

            return;
        }

        public Transform GetPlayerTransform()
        {
            if (_entity != null)
                return _entity.transform;

            return null;
        }

        public Transform GetViewTransform()
        {
            if (_entity != null)
            {
                MonoPlayerConfig config = _entity.GetComponent<MonoPlayerConfig>();
                if (config != null)
                    return config.viewTransform;
            }
            return null;
        }

        public void OnGetReadyToInteract(IInteractObject obj)
        {

        }

        public void OnGetOutOfReadyToInteract(IInteractObject obj)
        {

        }

        public void OnInteractWith(IInteractObject obj)
        {

        }

        public void OnGetReadyToBeInteracted(IInteractSubject sub)
        {

        }
        public void OnGetOutOfReadyToBeInteracted(IInteractSubject sub)
        {

        }
        public void OnInteractedBy(IInteractSubject sub)
        {

        }
    }
}