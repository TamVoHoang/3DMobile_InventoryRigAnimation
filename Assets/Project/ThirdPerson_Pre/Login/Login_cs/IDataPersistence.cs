using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDataPersistence
{
    void LoadData(PlayerJson playerJsonData);
    void SaveData(PlayerJson playerJsonData);
}
