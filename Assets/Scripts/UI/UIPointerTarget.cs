using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

namespace Planetarity.UI
{
    public class UIPointerTarget
    {
        public Transform transform;
        public Sprite sprite;
        public RectTransform pointer;


        public UIPointerTarget(Transform transform, Sprite sprite)
        {
            this.transform = transform;
            this.sprite = sprite;
        }
    }
}
