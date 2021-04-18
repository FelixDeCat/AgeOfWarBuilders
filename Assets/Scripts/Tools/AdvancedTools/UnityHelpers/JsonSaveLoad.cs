using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class JsonSaveLoad<T> {

    static string path = @"";
    string docname;
    public string convertedtext;

    bool is_a_list;

    //OBJECT
    T initializedModel;

    //LISTS
    ListObjs2 objList;
    List<T> current;
    class ListObjs { public T[] array_objects = null; }
    class ListObjs2 { public List<T> array_objects = null; }

    

    public JsonSaveLoad(string _docname, bool is_a_list, T model) {
        this.is_a_list = is_a_list;
        docname = _docname;

        initializedModel = model;

        if (is_a_list)
        {
            current = new List<T>();
            objList = new ListObjs2 { array_objects = new List<T>() };
        }

        path = @"" + docname + ".txt";
    }

    

    public void SaveList(List<T> _list) {
        if (!is_a_list) { Debug.LogError("ERROR: No fue configurado para guardar listas"); return; }
        current = _list;
        //objList.array_objects = _list.ToArray();
        objList.array_objects = _list;
        convertedtext = JsonUtility.ToJson(objList,true);
        File.WriteAllText(path, convertedtext);
    }

    public void SaveObject(T obj)
    {
        convertedtext = JsonUtility.ToJson(obj, true);
        File.WriteAllText(path, convertedtext);
    }

    public List<T> LoadList() {
        if (!is_a_list) { Debug.LogError("ERROR: No fue configurado para cargar listas"); return null; }
        Check();
        convertedtext = File.ReadAllText(path);
        objList = JsonUtility.FromJson<ListObjs2>(convertedtext);
        foreach (T o in objList.array_objects) { current.Add(o); }
        return current;
    }
    public T LoadObject()
    {
        Check();
        return JsonUtility.FromJson<T>(File.ReadAllText(path)); ;
    }


    object objeto;

    void SaveDefault()
    {
        convertedtext = JsonUtility.ToJson(initializedModel, true);
        File.WriteAllText(path, convertedtext);
    }

    void Check()
    {
        if (File.Exists(path))
        {
            Debug.Log("EL PATH EXISTE");
            convertedtext = File.ReadAllText(path);
            if (convertedtext == "")
            {
                Debug.Log("No hay nada");
                SaveDefault();
            }
        }
        else
        {
            Debug.Log("EL PATH NO EXISTE");

            File.Create(path);
            File.OpenWrite(path);

            Debug.Log("error mas abajo");

            SaveDefault();
        }
    }
}
