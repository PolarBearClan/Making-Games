using UnityEngine;

public class StaticStateManager : MonoBehaviour
{

    public static string sceneToChangeTo;
    public static string objectToSpawnAt;
    public static float timeToLoadInSeconds;
    public static bool hasActivatedGateJumpscare;
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
    
    public void setTimeToLoad(float f) {
        timeToLoadInSeconds = f;
    }

    public float getTimeToLoad() {
        return timeToLoadInSeconds;
    }
    
    public void setHasActivatedGateJumpsscare(bool s) {
        hasActivatedGateJumpscare = s;
    }
    
    public bool getHasActivatedGateJumpsscare()
    {
        return hasActivatedGateJumpscare;
    }
}
