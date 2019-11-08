using System;
using System.Reflection;

namespace utils
{
    class ProgramLoader : IProgramLoader
    {
        public void CallFunction(Assembly assembly, string className, string methodName)
        {
            //
            // 1. Get the type
            //
            Type type = assembly.GetType(className, true);

            //
            // 2. Invoke the method
            //
            var methodInfo = type.GetMethod(methodName, new Type[] { });
            if (methodInfo == null)
            {
                throw new Exception("No such method exists.");
            }

            //
            // 3. Create instance of the class
            //
            var classInstance = Activator.CreateInstance(type);

            //
            // 4. Invoke the method
            //
            var result = methodInfo.Invoke(classInstance, null);
            Console.WriteLine(result);
        }
    }
}
