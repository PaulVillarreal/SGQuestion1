using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml.Serialization;
using System.IO;
using System.Xml;

/// <summary>
/// Handles saving and loading of databases. Currently only supports one database
/// </summary>
[XmlRoot("LifetimeList")]

public class DatabaseManager 
{
  [XmlArray("LifeTimes")]
  [XmlArrayItem("LifeTime")]
  public List<lifeTime> lifeTimes;// = new List<lifeTime>();
  
  public void Save(List<lifeTime> newDatabase)
  {
    lifeTimes = new List<lifeTime>(newDatabase);
    string path = Path.Combine(Application.dataPath, "Dataset.xml");
    var serializer = new XmlSerializer(typeof(DatabaseManager));
    using (var stream = new FileStream(path, FileMode.Create))
    {
      var xmlWriter = new XmlTextWriter(stream, System.Text.Encoding.UTF8);
      serializer.Serialize(xmlWriter, this);
    }
  }

  public static DatabaseManager Load()
  {
    string path = Path.Combine(Application.dataPath, "Dataset.xml");
    var serializer = new XmlSerializer(typeof(DatabaseManager));
    using (var stream = new FileStream(path, FileMode.Open))
    {
      return serializer.Deserialize(stream) as DatabaseManager;
    }
  }
}
