using UnityEngine;

// Builds a tiny scene at runtime so the project needs no hand-made scene file.
public static class Bootstrap
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    private static void Init()
    {
        var camGo = new GameObject("Main Camera");
        camGo.tag = "MainCamera";
        var cam = camGo.AddComponent<Camera>();
        cam.clearFlags = CameraClearFlags.SolidColor;
        cam.backgroundColor = new Color(0.05f, 0.05f, 0.08f);
        camGo.transform.position = new Vector3(0f, 1f, -4f);

        var lightGo = new GameObject("Sun");
        var light = lightGo.AddComponent<Light>();
        light.type = LightType.Directional;
        lightGo.transform.rotation = Quaternion.Euler(50f, -30f, 0f);

        var cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        cube.transform.position = new Vector3(0f, 1f, 0f);
        cube.AddComponent<Spinner>();

        new GameObject("HUD").AddComponent<SmokeHud>();
    }
}

public class Spinner : MonoBehaviour
{
    private void Update()
    {
        transform.Rotate(30f * Time.deltaTime, 45f * Time.deltaTime, 0f);
    }
}

public class SmokeHud : MonoBehaviour
{
    private float _fps;

    private void Update()
    {
        _fps = Mathf.Lerp(_fps, 1f / Mathf.Max(Time.unscaledDeltaTime, 0.0001f), 0.1f);
    }

    private void OnGUI()
    {
        var style = new GUIStyle(GUI.skin.label) { fontSize = Mathf.RoundToInt(Screen.height * 0.05f) };
        GUI.Label(new Rect(20, 20, Screen.width, Screen.height * 0.2f),
            "STRIKE FORCE MOBILE - build OK\nFPS: " + _fps.ToString("F0"), style);
    }
}
