using UnityEngine;

public class GlobalUIState : MonoBehaviour
{

    public static string sceneToChangeTo;
    public static string objectToSpawnAt;
    //quest

    public void setSceneToChangeTo(string s) {
        sceneToChangeTo = s;
    }

    public string getSceneToChangeTo() {
        return sceneToChangeTo;
    }

    public void setObjectToSpawnAt(string s)
    {
        objectToSpawnAt = s;
    }
    public string getObjectToSpawnAt() {
        return objectToSpawnAt;
    }
}
