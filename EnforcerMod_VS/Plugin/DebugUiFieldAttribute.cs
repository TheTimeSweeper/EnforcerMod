using System;
using UnityEngine;

namespace EnforcerPlugin {
    [AttributeUsage(AttributeTargets.Field, Inherited = true, AllowMultiple = true)]
    public class DebugUiFieldAttribute : Attribute {

        private KeyCode increaseKey;
        private KeyCode decreaseKey;

        private float increaseAmount;

        public DebugUiFieldAttribute(KeyCode increaseKey_, KeyCode decreaseKey_, float increaseAmount_) {

            increaseKey = increaseKey_;
            decreaseKey = decreaseKey_;
            increaseAmount = increaseAmount_;
        }

        public void incrementValue() {

        }
    }
}