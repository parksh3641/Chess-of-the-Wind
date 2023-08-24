//using System.IO;
//using UnityEditor;
//using UnityEditor.iOS.Xcode;

//public class IOSBuildOption
//{
//    [UnityEditor.Callbacks.PostProcessBuild]
//    public static void ChangeXcodePlist(BuildTarget buildTarget, string pathToBuiltProject)
//    {

//        if (buildTarget == BuildTarget.iOS)
//        {
//            // Get plist
//            string plistPath = pathToBuiltProject + "/Info.plist";
//            PlistDocument plist = new PlistDocument();
//            plist.ReadFromString(File.ReadAllText(plistPath));

//            // Get root
//            PlistElementDict rootDict = plist.root;

//            // Change value of CFBundleVersion in Xcode plist
//            var buildKey = "UIBackgroundModes";
//            rootDict.CreateArray(buildKey).AddString("remote-notification");

//            rootDict.SetBoolean("Supports opening documents in place", true);
//            rootDict.SetBoolean("Application supports iTunes file sharing", true);
//            rootDict.SetBoolean("App Uses Non-Exempt Encryption", false);

//            // Write to file
//            File.WriteAllText(plistPath, plist.WriteToString());
//        }
//    }
//}
