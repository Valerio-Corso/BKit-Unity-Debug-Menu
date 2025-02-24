using System;

namespace BashoKit.GameDebug.Unity {
    public interface IDebugProvider { }

    [AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
    public class DebugActionAttribute : Attribute {
        public string DisplayName { get; }
        public string HeaderName { get; }
        
        /// <param name="header">Set an header if you wish to group your debug Actions together and seperated by an header.</param>
        /// <param name="overrideDisplayName">the default name will be the one of the method but can be overriden here if desired.</param>
        public DebugActionAttribute(string header = null, string overrideDisplayName = null) {
            DisplayName = overrideDisplayName;
            HeaderName = header;
        }
    }
    
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public class DebugTabAttribute : Attribute {
        public string TabName { get; }
    
        public DebugTabAttribute(string tabName) {
            TabName = tabName;
        }
    }
}