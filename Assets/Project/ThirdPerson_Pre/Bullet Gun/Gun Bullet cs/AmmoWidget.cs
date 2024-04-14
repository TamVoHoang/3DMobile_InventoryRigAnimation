
using UnityEngine;

public class AmmoWidget : MonoBehaviour
{
    public TMPro.TMP_Text ammoText;
    public TMPro.TMP_Text clipText;

    public void Refresh (int ammoCount, int clipCount)
    {
        ammoText.text = ammoCount.ToString ();
        clipText.text = clipCount.ToString();
    }
    public void Clear(int ammoClear) {
        ammoText.text = ammoClear.ToString ();
        clipText.text = ammoClear.ToString ();

    } 
}
