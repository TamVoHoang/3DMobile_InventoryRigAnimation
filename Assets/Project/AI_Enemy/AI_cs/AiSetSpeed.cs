using UnityEngine;

public class AiSetSpeed : MonoBehaviour
{
    // gameobject = all kind of aiagents
    // luu tam gia tri speed 
    // sau khi count down tra ve gia tri intial speed tuy tung intialState duoc xet trong aiAgent

    public float IntialSpeed;

    private void Awake() {
        DontDestroyOnLoad(this);
    }


}
