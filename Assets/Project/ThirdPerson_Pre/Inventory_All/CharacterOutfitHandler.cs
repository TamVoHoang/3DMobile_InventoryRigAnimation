using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/* using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;
using UnityEngine.UI; */

//todo gameObject = player
//todo quet skins va thay doi skins truoc khi vao game

public class CharacterOutfitHandler : MonoBehaviour
{
    [Header("Character parts current")]
    [SerializeField] GameObject playerSkins;
    [SerializeField] Transform skins; // parent transform hold all skins
    List<GameObject> skinPrefabs = new List<GameObject>();
    
    // button change skin
    //[SerializeField] Button ChangeSkinsButton;

    struct NetworkOutFit
    {
        public int skinPrefabID;
    }

    NetworkOutFit networkOutFit {get; set;}


    private void Awake() {

        // add tat ca doi tuong con trong transform skins vao list
        foreach (Transform child in skins) {
            skinPrefabs.Add(child.gameObject);
        }
        Debug.Log(skinPrefabs.Count);
    }

    private void Start() {
        NetworkOutFit newOutFit = networkOutFit;

        // random gia tri phan tu trong list
        newOutFit.skinPrefabID = (byte)Random.Range(0, skinPrefabs.Count);

        networkOutFit = newOutFit;
    }

    void ReplaceBodyTransform(GameObject currentBodyPart, GameObject prefabNewBodyPart, List<GameObject> a) {
        foreach (var item in a)
        {
            if(item == prefabNewBodyPart) {
                item.SetActive(true);
            } else {
                item.SetActive(false);
            }
        }
    }

    void ReplaceBodyPart() {
        //?replace skins
        ReplaceBodyTransform(playerSkins, skinPrefabs[networkOutFit.skinPrefabID], skinPrefabs);
    }

    // nut nhan + trong canvas game goi
    public void OnCycleSkin() {
        NetworkOutFit newOutFit = networkOutFit; // kieu byte vi tri torng list

        newOutFit.skinPrefabID ++;

        if(newOutFit.skinPrefabID > skinPrefabs.Count - 1) {
            newOutFit.skinPrefabID = 0;
        }
        networkOutFit = newOutFit;
        ReplaceBodyPart();
    }

    // nhan nut - skins
    public void OnCycleSkinPrevious() {
        NetworkOutFit newOutFit = networkOutFit; // kieu byte vi tri torng list

        newOutFit.skinPrefabID --;

        if(newOutFit.skinPrefabID < 0) {
            newOutFit.skinPrefabID = skinPrefabs.Count - 1;
        }
        networkOutFit = newOutFit;
        ReplaceBodyPart();
    }

    //todo
}
