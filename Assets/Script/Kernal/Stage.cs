using System;
using UnityEngine;

namespace GameKernal
{
    class Stage : BaseStage, IMonoEntityHost
    {
        private MonoEntity _entity;
        private MonoStage _stage;

        public override Vector3 GetStagePoint(string name)
        {
            if (_stage != null)
            {
                Transform trans = _stage.GetPointTrans(name);
                if (trans != null)
                    return trans.position;
            }

            return Vector3.zero;
        }

        public override Vector3 GetStagePointSize(string name)
        {
            if (_stage != null)
            {
                Transform trans = _stage.GetPointTrans(name);
                if (trans != null)
                    return trans.localScale;
            }

            return Vector3.zero;
        }

        public override void Initialize(StageDesc desc)
        {
            GameObject newGameObject = GameObject.Instantiate<GameObject>(desc.prototype);
            _entity = newGameObject.AddComponent<MonoEntity>();
            _entity.SetHost(this);
            _stage = _entity.GetComponent<MonoStage>();

            return;
        }

        public override void Uninitialize()
        {
            _entity.SetHost(null);
            GameObject.Destroy(_entity.gameObject);

            return;
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
    }
}