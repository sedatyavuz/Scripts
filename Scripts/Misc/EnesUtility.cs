#if UNITY_EDITOR
using System.Reflection;
#endif

public class EnesUtility
{
    public static void ClearConsole()
    {
        #if UNITY_EDITOR
        var assembly = Assembly.GetAssembly(typeof(UnityEditor.Editor));
        var type = assembly.GetType("UnityEditor.LogEntries");
        var method = type.GetMethod("Clear");
        method.Invoke(new object(), null);
        #endif
    }
}
