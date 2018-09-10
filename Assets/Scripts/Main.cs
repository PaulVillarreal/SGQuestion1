using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Struct to hold each persons info
/// </summary>
[System.Serializable]
public struct lifeTime
{
  public string userName;
  public int birthYear;
  public int deathYear;
}

/// <summary>
/// Struct to hold all the relevant year data
/// </summary>
[System.Serializable]
public class YearData
{
  public int year;
  public int numBirths;
  public int numDeaths;
  public int numPopulation;

  public void Init(int _year)
  {
    year = _year;
    numBirths = 0;
    numDeaths = 0;
    numPopulation = 0;
  }
}


public class Main : MonoBehaviour 
{
  const int NUM_YEARS = 101;
 
  //Container for year data
  public List<YearData> yearDatabase = new List<YearData>();

  //Container for DataBase
  public List<lifeTime> dataBase = new List<lifeTime>();

  public YearData mostPopulousYear;

  [SerializeField]
  int DatabaseSize;

  [SerializeField]
  DatabaseRandomizer databaseRandomizer;

  [SerializeField]
  GameObject mainCanvas;

  #region GraphVisualizers
  [SerializeField]
  GraphVisualizer populationVis;
  [SerializeField]
  GraphVisualizer birthVis;
  [SerializeField]
  GraphVisualizer deathVis;
  #endregion

  [SerializeField]
  GameObject arrowMarker;

  [SerializeField]
  GameObject loadingText;

  private void Start()
  {
    InitYears(NUM_YEARS);
  }

  /// <summary>
  /// Creates random database and saves it to Load path
  /// </summary>
  public void RandomizeList()
  {
    loadingText.SetActive(true);
    dataBase = databaseRandomizer.InitList(DatabaseSize);
    mainCanvas.SetActive(false);
    LoadDataBase(dataBase);
  }

  /// <summary>
  /// Loads last saved randomized database
  /// </summary>
  public void LoadList()
  {
    loadingText.SetActive(true);
    DatabaseManager databaseManager = DatabaseManager.Load();
    dataBase = databaseManager.lifeTimes;
    mainCanvas.SetActive(false);
    LoadDataBase(dataBase);
  }

  /// <summary>
  /// Resets Year Data
  /// </summary>
  /// <param name="numYears"></param>
  private void InitYears(int numYears)
  {
    yearDatabase.Clear();
    for (int i = 0; i < numYears; ++i)
    {
      YearData newYear = new YearData();
      newYear.Init(1900 + i);
      yearDatabase.Add(newYear);
    }

    mostPopulousYear = yearDatabase[0];
  }

  /// <summary>
  /// Loads in database and populates yearData array with data
  /// </summary>
  /// <param name="dataBase"></param>
  public void LoadDataBase(List<lifeTime> dataBase)
  {
    InitYears(NUM_YEARS);
    for (int i = 0; i < dataBase.Count; ++i)
    {
      yearDatabase[dataBase[i].birthYear].numBirths++;
      yearDatabase[dataBase[i].deathYear].numDeaths++;
      for (int j = dataBase[i].birthYear; j <= dataBase[i].deathYear; ++j)
      {
        yearDatabase[j].numPopulation++;
      }
    }

    mostPopulousYear = GetMostPopulous();

    DrawPopulationGraph();
    DrawDeathGraph();
    DrawBirthGraph();

    Debug.Log("MOST POPULOUS YEAR: " + mostPopulousYear.year + " with a population of: " + mostPopulousYear.numPopulation);
  }

  /// <summary>
  /// Uses a sort function to find the most populous year
  /// </summary>
  /// <returns>YearData object of the most populous year</returns>
  public YearData GetMostPopulous()
  {
    List<YearData> tempList = new List<YearData>(yearDatabase);
    tempList.Sort(SortByPopulation);
    return tempList[NUM_YEARS - 1];
  }

  /// <summary>
  /// Sets up line renderer to display graph of population across the years
  /// </summary>
  public void DrawPopulationGraph()
  {
    int[] dataArray = new int[NUM_YEARS];

    for (int i = 0; i < NUM_YEARS; ++i)
    {
      dataArray[i] = yearDatabase[i].numPopulation;
    }

    int yearIndex = mostPopulousYear.year - 1900;

    arrowMarker.transform.position = new Vector3(yearIndex * 0.5f, (dataArray[yearIndex]/mostPopulousYear.numPopulation) * 25, 0);
    arrowMarker.SetActive(true);
    populationVis.InitLine(dataArray, mostPopulousYear.numPopulation);

  }

  /// <summary>
  /// Sets up line renderer to display graph of deaths across the years
  /// </summary>
  public void DrawDeathGraph()
  {
    int[] dataArray = new int[NUM_YEARS];

    for (int i = 0; i < NUM_YEARS; ++i)
    {
      dataArray[i] = yearDatabase[i].numDeaths;
    }

    deathVis.InitLine(dataArray, mostPopulousYear.numPopulation);
  }

  /// <summary>
  /// Sets up line renderer to display graph of births across the years
  /// </summary>
  public void DrawBirthGraph()
  {
    int[] dataArray = new int[NUM_YEARS];

    for (int i = 0; i < NUM_YEARS; ++i)
    {
      dataArray[i] = yearDatabase[i].numBirths;
    }

    birthVis.InitLine(dataArray, mostPopulousYear.numPopulation);
  }

  #region SortingMethods
  static int SortByPopulation(YearData year1, YearData year2)
  {
    return year1.numPopulation.CompareTo(year2.numPopulation);
  }

  static int SortByDeaths(YearData year1, YearData year2)
  {
    return year1.numDeaths.CompareTo(year2.numDeaths);
  }

  static int SortByBirths(YearData year1, YearData year2)
  {
    return year1.numBirths.CompareTo(year2.numBirths);
  }
  #endregion
}
