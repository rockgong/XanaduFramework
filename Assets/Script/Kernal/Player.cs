using UnityEngine;
using System;

namespace GameKernal
{
    class Player : BasePlayer
    {
        private GameObject _gameObject;

        public override void Initialize(PlayerCharacterDesc desc)
        {
            _gameObject = GameObject.Instantiate(desc.prototype);

            return;
        }

        public override void Uninitialize()
        {
            GameObject.Destroy(_gameObject);

            return;
        }
    }
}