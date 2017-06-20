using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameKernal
{
    interface IMonoEntityHost
    {
        void OnStart(MonoEntity entity);
        void OnUpdate(MonoEntity entity, float deltaTime);
        void OnFixedUpdate(MonoEntity entity);
        void OnLateUpdate(MonoEntity entity, float deltaTime);

        void OnTriggerEnter(MonoEntity entity, Collider other);
        void OnTriggerStay(MonoEntity entity, Collider other);
        void OnTriggerExit(MonoEntity entity, Collider other);

        void OnCollisionEnter(MonoEntity entity, Collision collision);
        void OnCollisionStay(MonoEntity entity, Collision collision);
        void OnCollisionExit(MonoEntity entity, Collision collision);
    }

    public class MonoEntity : MonoBehaviour
    {
        IMonoEntityHost _host;

        internal void SetHost(IMonoEntityHost host)
        {
            this._host = host;
        }

        // Use this for initialization
        private void Start()
        {
            if (_host != null)
                _host.OnStart(this);
        }

        // Update is called once per frame
        private void Update()
        {
            if (_host != null)
                _host.OnUpdate(this, Time.deltaTime);
        }

        private void FixedUpdate()
        {
            if (_host != null)
                _host.OnFixedUpdate(this);
        }

        private void LateUpdate()
        {
            if (_host != null)
                _host.OnLateUpdate(this, Time.deltaTime);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (_host != null)
                _host.OnTriggerEnter(this, other);
        }

        private void OnTriggerStay(Collider other)
        {
            if (_host != null)
                _host.OnTriggerStay(this, other);
        }

        private void OnTriggerExit(Collider other)
        {
            if (_host != null)
                _host.OnTriggerExit(this, other);
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (_host != null)
                _host.OnCollisionEnter(this, collision);
        }

        private void OnCollisionStay(Collision collision)
        {
            if (_host != null)
                _host.OnCollisionStay(this, collision);
        }

        private void OnCollisionExit(Collision collision)
        {
            if (_host != null)
                _host.OnCollisionExit(this, collision);
        }
    }
}