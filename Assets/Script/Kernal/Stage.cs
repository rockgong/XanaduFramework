using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GameKernal
{
    class Stage : BaseStage, IMonoEntityHost
    {
        private MonoEntity _entity;
        private MonoStage _stage;
        private Scene _unityScene;

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

        public override void Initialize(StageDesc desc, System.Action onEnd = null)
        {
            /*
            GameObject newGameObject = GameObject.Instantiate<GameObject>(desc.prototype);
            _entity = newGameObject.AddComponent<MonoEntity>();
            _entity.SetHost(this);
            _stage = _entity.GetComponent<MonoStage>();
            */

            if (_stage != null)
            {
                GameObject.Destroy(_stage.gameObject);
                _stage = null;
                _entity = null;
            }
            SceneManager.LoadScene(desc.sceneName, LoadSceneMode.Additive);

            /*
            if (_unityScene.isLoaded)
            {
                GameObject[] rootGOs = _unityScene.GetRootGameObjects();
                if (rootGOs != null || rootGOs.Length > 0)
                {
                    _entity = rootGOs[0].AddComponent<MonoEntity>();
                    _entity.SetHost(this);
                    _stage = _entity.GetComponent<MonoStage>();
                }
                else
                {
                    Debug.Log("No Root GameObject");
                }
            }
            else
            {
                Debug.Log("Failed To Load Scene");
            }
            */

            MonoDelegate monoDelegate = null;
            monoDelegate = MonoDelegate.Create(() =>
            {
                _unityScene = SceneManager.GetSceneByName(desc.sceneName);
                if (_unityScene.isLoaded)
                {
                    if (monoDelegate != null)
                        GameObject.Destroy(monoDelegate.gameObject);
                    GameObject[] rootGOs = _unityScene.GetRootGameObjects();
                    if (rootGOs != null || rootGOs.Length > 0)
                    {
                        _entity = rootGOs[0].AddComponent<MonoEntity>();
                        _entity.SetHost(this);
                        _stage = _entity.GetComponent<MonoStage>();
                        if (onEnd != null)
                            onEnd();
                    }
                    else
                    {
                        Debug.Log("No Root GameObject");
                    }
                }
            }, "_Delegate", 0);

            return;
        }

        public override void Uninitialize()
        {
            if (_entity != null)
            {
                _entity.SetHost(null);
                _entity = null;
            }
            if (_unityScene != null)
            {
                SceneManager.UnloadSceneAsync(_unityScene);
            }

            Debug.Log("Uninitialize Called");

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