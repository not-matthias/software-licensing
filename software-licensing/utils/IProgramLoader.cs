using System.Reflection;

namespace utils
{
    public interface IProgramLoader
    {
        public void CallFunction(Assembly assembly, string className, string methodName);
    }
}
