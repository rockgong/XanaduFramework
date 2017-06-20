using UnityEngine;
using System;

namespace GameKernal
{
    class Player : BasePlayer, IMonoEntityHost
    {
        private MonoEntity _entity;

        public override void Initialize(PlayerCharacterDesc desc)
        {
            GameObject gameObject = GameObject.Instantiate(desc.prototype);
            _entity = gameObject.AddComponent<MonoEntity>();
            _entity.SetHost(this);

            return;
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
            //Do nothing...
            return;
        }

        public override void Uninitialize()
        {
            _entity.SetHost(null);
            GameObject.Destroy(_entity.gameObject);

            return;
        }
    }
}