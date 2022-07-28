using System;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Utility
{
    public class JITHelper
    {
        private static void PreJitMarkedMethods(Type type)
        {
            // get the type of all the methods within this instance
            var methods = type.GetMethods(BindingFlags.DeclaredOnly |
                                          BindingFlags.NonPublic |
                                          BindingFlags.Public |
                                          BindingFlags.Instance |
                                          BindingFlags.Static);

            // for each time, jit methods marked with prejit attribute
            foreach (var method in methods)
            {
                // checks if the [PreJit] Attribute is present
                if (ContainsPreJitAttribute(method))
                {
                    // jitting of the method happends here.
                    RuntimeHelpers.PrepareMethod(method.MethodHandle);
                }
            }
        }

        // (helper method) checks if the [PreJit] attribute is present on a method
        private static bool ContainsPreJitAttribute(MethodInfo methodInfo)
        {
            var attributes = methodInfo.GetCustomAttributes(typeof(PreJitAttribute), false);
            if (attributes != null)
                if (attributes.Length > 0)
                {
                    // attribute found return true
                    return true;
                }

            return false;
        }
        public static void PreJitAll<T>() where T : class
        {
            //Debug.Log("PreJIT: " + typeof(T).ToString());
            PreJitAllMethods(typeof(T));
        }
        private static void PreJitAllMethods(Type type)
        {
            // get the type of all the methods within this instance
            var methods = type.GetMethods(BindingFlags.DeclaredOnly |
                                          BindingFlags.NonPublic |
                                          BindingFlags.Public |
                                          BindingFlags.Instance |
                                          BindingFlags.Static);

            // Jit all methods
            foreach (var method in methods)
            {
                // jitting of the method happends here.
                //Debug.Log("PreJit Method: " + method.ToString());
                method.MethodHandle.GetFunctionPointer();
                //RuntimeHelpers.PrepareMethod(method.MethodHandle);
            }
        }
    }
}
