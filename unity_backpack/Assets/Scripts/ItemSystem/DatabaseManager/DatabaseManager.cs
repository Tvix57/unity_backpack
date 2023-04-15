using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

using Arcspark.DataToolkit;
using Mono.Data.Sqlite;
 

public class DatabaseManager : MonoBehaviour {
    public void SaveItems(string table_name, List<AssetItem> items) {
        SQLiteConnection db = new SQLiteConnection(Application.streamingAssetsPath + "/AllItems.db");

        SqliteDataReader reader = db.SelectTable(table_name);
        if (reader == null) { 
            db.CreateTable(table_name,
                           new string[] {"BIGINT"},
                           new string[] {"Item_ID"});
            reader = db.SelectTable(table_name);
        }
        db.ExecuteQuery("DELETE FROM  " + table_name);
        items.ForEach(item => {
            db.InsertValues(table_name,  new string[] {item.ID.ToString()});
        });
        db.Close();
    }

    public List<AssetItem> LoadItems(string table_name) {
        SQLiteConnection db = new SQLiteConnection(Application.streamingAssetsPath + "/AllItems.db");

        List<AssetItem> items = new List<AssetItem>();
        SqliteDataReader reader = db.SelectTable(table_name);
        if (reader == null) { 
            db.CreateTable(table_name,
                           new string[] {"BIGINT"},
                           new string[] {"Item_ID"});
            reader = db.SelectTable(table_name);
        }
        if (reader.FieldCount != 0) {
            List<int> items_id = new List<int>();
            do {
                while (reader.Read()) {
                    int? tmp_id = reader.GetInt32("Item_ID");
                    if (tmp_id.HasValue) {
                        items_id.Add(tmp_id.Value);
                    }
                }
            } while (reader.NextResult());

            AssetItem[] allItems = Resources.LoadAll<AssetItem>("Prefabs/ItemsBase/Equipment");
            items_id.ForEach(database_id => {
                foreach(AssetItem item_prefab in allItems) {
                    if (item_prefab.ID == database_id) {
                        items.Add(Instantiate(item_prefab));
                    }
                }
            });
        }
        db.Close();
        return items;
    }
}
