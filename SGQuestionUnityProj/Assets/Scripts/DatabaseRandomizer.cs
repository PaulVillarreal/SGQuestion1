using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class DatabaseRandomizer : MonoBehaviour 
{
  /// <summary>
  /// Creates a randomized list and saves it to Load path
  /// </summary>
  /// <param name="numEntries"></param>
  /// <returns></returns>
  public List<lifeTime> InitList(int numEntries)
  {
    List<lifeTime> dataBase = new List<lifeTime>();

    for (int i = 0; i < numEntries; i++)
    {
      lifeTime life = new lifeTime();
      life.userName = "Person"+ i;
      life.birthYear = Random.Range(0, 101);
      life.deathYear = Random.Range(0, 101);

      while (life.deathYear < life.birthYear)
      {
        life.deathYear = Random.Range(0, 101);
      }

      dataBase.Add(life);
    }

    DatabaseManager newDataBase = new DatabaseManager();
    newDataBase.Save(dataBase);
    return dataBase;
  }
}
