using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace BashoKit.GameDebug.Unity {
    public static class DebugResolver {
        public static IEnumerable<(MethodInfo method, DebugActionAttribute actionAttribute)> GetDebugActions(string assemblyName) {
            var assembly = AppDomain.CurrentDomain.GetAssemblies().FirstOrDefault(a => a.FullName.Contains(assemblyName));
            if (assembly == null) {
                Debug.LogError("Couldn't find assembly with name " + assemblyName);
            }
            
            foreach (var type in assembly.GetTypes()) {
                foreach (var method in type.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static)) {
                    var actionAttr = method.GetCustomAttribute<DebugActionAttribute>();
                    if (actionAttr != null) {
                        yield return (method, actionAttr);
                    }
                }
            }
        }
    }
}