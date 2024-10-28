using UnityEngine;

public class GlobalUIState : MonoBehaviour
{

    public static string sceneToChangeTo;
    //quest

    public void setSceneToChangeTo(string s) {
        sceneToChangeTo = s;
    }

    public string getSceneToChangeTo() { 
        return sceneToChangeTo;
    }
}
