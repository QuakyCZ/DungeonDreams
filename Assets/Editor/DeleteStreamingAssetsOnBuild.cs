using System.IO;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;

public class DeleteStreamingAssetsOnBuild : IPreprocessBuildWithReport{
    public int callbackOrder {
        get { return 0; }
    }

    public void OnPreprocessBuild(BuildReport report) {
        Directory.Delete("Assets/StreamingAssets");
        Directory.CreateDirectory("Assets/StreamingAssets");
    }
}