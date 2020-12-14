using System;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using SQLite4Unity3d;

public class Item
{
    public int Id { get; set; }

    public int ModelId { get; set; }

    public int Type { get; set; }

    public int Index { get; set; }

    public string Name { get; set; }
}

static public class DatabaseManager
{
    static public string DatabasePath(string name)
    {
        string filePath = string.Format("{0}/{1}", Application.persistentDataPath, name);
        bool fileExists = File.Exists(filePath);

        switch (Application.platform)
        {
            default:
                {
                    // alternatively implement an assumed fallback
                    throw new NotSupportedException();
                }

            case RuntimePlatform.WindowsEditor:
            case RuntimePlatform.OSXEditor:
            case RuntimePlatform.LinuxEditor:
                {
                    return string.Format("Assets/StreamingAssets/{0}", name);
                }

            case RuntimePlatform.Android:
                {
                    if (fileExists)
                    {
                        return filePath;
                    }

                    // this is the path to your StreamingAssets in android
                    string path = string.Format("jar:file://{0}!/assets/{1}", Application.dataPath, name);
                    var req = UnityWebRequest.Get(path).SendWebRequest();

                    // NOTE: may want to add some checks to this
                    while (!req.isDone) { }

                    File.WriteAllBytes(filePath, req.webRequest.downloadHandler.data);
                    break;
                }

            case RuntimePlatform.IPhonePlayer:
                {
                    if (fileExists)
                    {
                        return filePath;
                    }

                    // this is the path to your StreamingAssets in iOS
                    string path = string.Format("/{0}Raw/{1}", Application.dataPath, name);
                    File.Copy(path, filePath);
                    break;
                }
        }

        return filePath;
    }

    static public IEnumerable<ItemBaseData> GetItems()
    {
        var connection = new SQLiteConnection(DatabasePath("Expedition.s3db"), SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create);

        var result = connection.Query<ItemBaseData>("SELECT * FROM Item");

        connection.Close();
        
        return result;
    }

    static public void AddItem()
    {
        var connection = new SQLiteConnection(DatabasePath("Expedition.s3db"), SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create);

        connection.Query<ItemBaseData>("INSERT INTO Item(ModelId, Type, 'Index', Name) VALUES(?, ?, ?, ?)", 1, 0, 2, "DB Item" + UnityEngine.Random.Range(1, 100));

        connection.Close();

        GetItems();
    }
}
