using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameKernal
{
    class Trigger : BaseTrigger, IMonoEntityHost
    {
        private MonoEntity _entity;

        public override Vector3 position
        {
            get
            {
                return base.position;
            }

            set
            {
                base.position = value;
            }
        }

        public override Vector3 size
        {
            get
            {
                return base.size;
            }

            set
            {
                base.size = value;
            }
        }

        public override void Initialize(TriggerDesc desc)
        {
            GameObject go = new GameObject("Trigger");
            _entity = go.AddComponent<MonoEntity>();
            _entity.SetHost(this);
            BoxCollider box = go.AddComponent<BoxCollider>();
            box.isTrigger = true;
            go.transform.position = desc.position;
            box.size = desc.size;
            box.center = new Vector3(0.0f, position.y / 2, 0.0f);
        }

        public void OnCollisionEnter(MonoEntity entity, Collision collision)
        {
            return;
        }

        public void OnCollisionExit(MonoEntity entity, Collision collision)
        {
            return;
        }

        public void OnCollisionStay(MonoEntity entity, Collision collision)
        {
            return;
        }

        public void OnFixedUpdate(MonoEntity entity)
        {
            return;
        }

        public void OnLateUpdate(MonoEntity entity, float deltaTime)
        {
            return;
        }

        public void OnStart(MonoEntity entity)
        {
            return;
        }

        public void OnTriggerEnter(MonoEntity entity, Collider other)
        {
            if (onTriggerEnter != null)
                onTriggerEnter();

            return;
        }

        public void OnTriggerExit(MonoEntity entity, Collider other)
        {
            if (onTriggerExit != null)
                onTriggerExit();

            return;
        }

        public void OnTriggerStay(MonoEntity entity, Collider other)
        {
            return;
        }

        public void OnUpdate(MonoEntity entity, float deltaTime)
        {
            return;
        }

        public override void Uninitialize()
        {
            if (_entity != null)
            {
                _entity.SetHost(null);
                GameObject.Destroy(_entity.gameObject);
            }
        }
    }
}