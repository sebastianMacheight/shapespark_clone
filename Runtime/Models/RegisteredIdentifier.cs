using ReupVirtualTwin.helpers;
using UnityEngine;
using ReupVirtualTwin.modelInterfaces;
using System.Collections.Generic;

namespace ReupVirtualTwin.models
{
    public class RegisteredIdentifier : UniqueId
    {
        public string manualId = "";
        private IObjectRegistry _objectRegistry;

        override protected void Start()
        {
            FindObjectRegistry();
            base.Start();
        }

        override public string GenerateId()
        {
            if (_objectRegistry == null)
            {
                FindObjectRegistry();
            }
            base.GenerateId();
            UpdateRegistry();
            return uniqueId;
        }

        public override string AssignId(string id)
        {
            if (_objectRegistry == null)
            {
                FindObjectRegistry();
            }
            base.AssignId(id);
            UpdateRegistry();
            return uniqueId;
        }

        private void UpdateRegistry()
        {
            UnRegisterObject();
            RegisterObject();
        }

        private void RegisterObject()
        {
            if (manualId != uniqueId && !string.IsNullOrEmpty(manualId))
            {
                uniqueId = manualId;
            }
            _objectRegistry.AddObject(gameObject);
        }

        private void UnRegisterObject()
        {
            _objectRegistry.RemoveObject(gameObject);
        }

        private void FindObjectRegistry()
        {
            _objectRegistry = ObjectFinder.FindObjectRegistry().GetComponent<IObjectRegistry>();
        }

    }
}
