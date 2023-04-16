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
                           new string[] {"BIGINT", "BIGINT"},
                           new string[] {"Item_ID", "Count"});
            reader = db.SelectTable(table_name);
        }
        db.ExecuteQuery("DELETE FROM  " + table_name);
        items.ForEach(item => {
            db.InsertValues(table_name,  new string[] {item.ID.ToString(), ((int)item.Count).ToString()});

        });
        db.Close();
    }

    public List<AssetItem> LoadItems(string table_name) {
        SQLiteConnection db = new SQLiteConnection(Application.streamingAssetsPath + "/AllItems.db");

        List<AssetItem> items = new List<AssetItem>();
        SqliteDataReader reader = db.SelectTable(table_name);
        if (reader == null) { 
            db.CreateTable(table_name,
                           new string[] {"BIGINT", "BIGINT"},
                           new string[] {"Item_ID", "Count"});
            reader = db.SelectTable(table_name);
        }
        if (reader.FieldCount != 0) {
            List<Tuple <int, int>> items_id = new List<Tuple <int, int>>();
            do {
                while(reader.Read()) {
                    int? tmp_id = reader.GetInt32("Item_ID");
                    int? tmp_count = reader.GetInt32("Count");
                    if (tmp_id.HasValue && tmp_count.HasValue) {
                        items_id.Add(new Tuple <int, int>(tmp_id.Value, tmp_count.Value));
                    }
                }
            } while (reader.NextResult());
            
            AssetItem[] allItems = Resources.LoadAll<AssetItem>("Prefabs/ItemsBase");
            items_id.ForEach(database_id => {
                foreach(AssetItem item_prefab in allItems) {
                    if (item_prefab.ID == database_id.Item1) {
                        AssetItem create_item = Instantiate(item_prefab);
                        create_item.Count = (uint)database_id.Item2;
                        items.Add(create_item);
                    }
                }
            });
        }
        db.Close();
        return items;
    }
}
