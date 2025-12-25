using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AV;
using Nautilus;
using UnityEngine;

namespace Violet.AV.MaterialUtilitys
{
    internal class ApplyPrecursorForceFieldAwake : MonoBehaviour
    {
        private void Awake()
        {
            MeshRenderer renderer = gameObject.GetComponent<MeshRenderer>();

            if (renderer == null)
            {
                Plugin.Log.LogError($"{gameObject.name} Dosnt have a mesh renderer are you sure you have it on the right game object?");
            }
            else
            {

                Nautilus.Utility.MaterialUtils.ApplySNShaders(gameObject);
                renderer.material = Nautilus.Utility.MaterialUtils.ForceFieldMaterial;
            }
        }

    }
}
