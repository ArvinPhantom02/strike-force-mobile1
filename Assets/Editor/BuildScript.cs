using System;
using System.IO;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEditor.SceneManagement;

// Called by GameCI: buildMethod = BuildScript.BuildAndroid
public static class BuildScript
{
    public static void BuildAndroid()
    {
        Directory.CreateDirectory("Assets/Scenes");
        var scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
        const string scenePath = "Assets/Scenes/Main.unity";
        EditorSceneManager.SaveScene(scene, scenePath);

        PlayerSettings.companyName = "StrikeForce";
        PlayerSettings.productName = "Strike Force Mobile";
        PlayerSettings.SetApplicationIdentifier(NamedBuildTarget.Android, "com.strikeforce.mobile");
        PlayerSettings.SetScriptingBackend(NamedBuildTarget.Android, ScriptingImplementation.IL2CPP);
        PlayerSettings.Android.targetArchitectures = AndroidArchitecture.ARM64;
        PlayerSettings.Android.minSdkVersion = AndroidSdkVersions.AndroidApiLevel24;
        PlayerSettings.defaultInterfaceOrientation = UIOrientation.LandscapeLeft;
        EditorUserBuildSettings.buildAppBundle = false;

        string outPath = "build/Android/StrikeForce.apk";
        var args = Environment.GetCommandLineArgs();
        for (int i = 0; i < args.Length - 1; i++)
        {
            if (args[i] == "-customBuildPath") outPath = args[i + 1];
        }
        if (!outPath.EndsWith(".apk", StringComparison.OrdinalIgnoreCase))
            outPath = Path.Combine(outPath, "StrikeForce.apk");
        Directory.CreateDirectory(Path.GetDirectoryName(outPath));

        var report = BuildPipeline.BuildPlayer(new BuildPlayerOptions
        {
            scenes = new[] { scenePath },
            target = BuildTarget.Android,
            locationPathName = outPath,
            options = BuildOptions.None
        });

        if (report.summary.result != BuildResult.Succeeded)
        {
            Console.Error.WriteLine("Build failed: " + report.summary.result);
            EditorApplication.Exit(1);
        }
    }
}
