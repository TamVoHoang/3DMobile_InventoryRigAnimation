
using UnityEngine;

public class AmmoWidget : MonoBehaviour
{
    public TMPro.TMP_Text ammoText;
    public void Refresh (int ammoCount)
    {
        ammoText.text = ammoCount.ToString (); 
    }
    public void Clear(int ammoClear) {
        ammoText.text = ammoClear.ToString (); 
    } 
}
