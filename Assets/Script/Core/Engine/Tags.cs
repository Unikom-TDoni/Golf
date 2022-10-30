using System;
using Lncodes.Module.Unity.Editor;
using UnityEngine;

namespace Edu.Golf.Core
{
    [Serializable]
    public struct Tags
    {
        [TagSelector]
        [SerializeField]
        private string _hole;

        public string Hole { get => _hole; }
    }
}
