using System.IO;
using UnityEditor;
using UnityEditor.Callbacks;
#if UNITY_IOS
using UnityEditor.iOS.Xcode;
#endif
using UnityEngine;

public class PostProcessBuild : MonoBehaviour
{
    [PostProcessBuild]
    public static void OnPostProcessBuild(BuildTarget buildTarget, string path)
    {
        if (buildTarget == BuildTarget.iOS)
        {
            #if UNITY_IOS
            //Modify PList File and add AdMob ID
            string plistPath = path + "/Info.plist";
            PlistDocument plist = new PlistDocument();
            plist.ReadFromString(File.ReadAllText(plistPath));

            PlistElementDict rootDict = plist.root;

            string buildKey = "CFBundleVersion";
            rootDict.SetString(buildKey, "1");

            rootDict["GADApplicationIdentifier"] = new PlistElementString("ca-app-pub-8277769580123099~4622099382");

            PlistElementDict nsAppTransportSecurityDict = rootDict["NSAppTransportSecurity"].AsDict();
            nsAppTransportSecurityDict["NSAllowsArbitraryLoads"] = new PlistElementBoolean(true);

            rootDict["NSAppTransportSecurity"] = nsAppTransportSecurityDict;

            File.WriteAllText(plistPath, plist.WriteToString());


            //Add AdSupport.framework
            string projPath = path + "/Unity-iPhone.xcodeproj/project.pbxproj";

            PBXProject proj = new PBXProject();
            proj.ReadFromString(File.ReadAllText(projPath));

            string target = proj.GetUnityMainTargetGuid();

            proj.AddFrameworkToProject(target, "AdSupport.framework", true);

            File.WriteAllText(projPath, proj.WriteToString());
            #endif
        }
    }
}
