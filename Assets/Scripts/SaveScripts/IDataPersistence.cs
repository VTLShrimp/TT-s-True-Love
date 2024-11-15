using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDataPersistence
{
    void SavePlayerData(PlayerData playerData);
    void LoadPlayerData(PlayerData playerData);

}
