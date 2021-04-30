using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GridExercises : MonoBehaviour {

    public ObserverQuery query;
    bool active;

    private void Start()
    {
        Invoke("begin", 1f);
    }
    void begin()
    {
        active = true;
    }

    private void LateUpdate() {

        if (!active) return;

        var ej1 = Exercise1();
        var ej2 = Exercise2();
        var ej3 = Exercise3();

        
        foreach (var enemy in ej1) {
            Debug.Log(enemy.name);
        }
        
        Debug.Log("Ejercicio 2:");
        foreach (var enemy in ej2) {
            Debug.Log(enemy.name);
        }
        
        Debug.Log("Ejercicio 3:");
        foreach (var enemy in ej3) {
            Debug.Log(enemy.name);
        }
    }

    //Obtener los enemigos(Enemy) débiles en el área de la Query que tengan menos de 5 de vida(hp).
    private IEnumerable<Enemy> Exercise1()
    {
        return query.Query().Cast<Enemy>().Where(x => x.HP <= 5);
    }

    //Obtener el enemigo en el área de la Query más cercano al SquareQuery.
    private IEnumerable<Enemy> Exercise2() {
        var aux = query;
        return aux.Query()
            .Cast<Enemy>()
            .OrderBy(x => (transform.position - x.transform.position).sqrMagnitude)
            .Take(1);
    }

    //Obtener los 3 primeros enemigos en el área de la Query más cercanos al SquareQuery.
    private IEnumerable<Enemy> Exercise3() {
        var aux = query;
        return aux.Query()
            .Cast<Enemy>()
            .OrderBy(x => (transform.position - x.transform.position).sqrMagnitude)
            .Take(3);
    }
    
}
