using System;
using System.Collections.Generic;

namespace BashoKit.GameDebug.Unity {
    public static class DebugInstanceRegistry {
        private static readonly Dictionary<Type, object> _instances = new Dictionary<Type, object>();

        public static object GetInstance(Type type) {
            if (_instances.TryGetValue(type, out var instance))
                return instance;

            try {
                // Create a new instance if one is not found.
                instance = Activator.CreateInstance(type);
                _instances.Add(type, instance);
                return instance;
            }
            catch (Exception ex) {
                UnityEngine.Debug.LogError($"Failed to create instance of {type}: {ex}");
                return null;
            }
        }
    }
}
