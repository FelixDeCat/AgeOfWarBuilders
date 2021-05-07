using System.Collections.Generic;
using System.Collections;
using System;
using System.Linq;

public class LazyEvaluationExercises {

    public Map Exercise1() {
        //Cambiar la linea de abajo
        var result = default(Map); 
        return result;
    }


    //El mapa anterior se asume que se guarda en una variable 
    //llamada map y en este ejercicio ya recibio valor.
    //Se deberá devolver una colección que contenga cada uno de 
    //los pasos(steps) a seguir y su advertencia(stepWarn) respecto, a su vez, 
    //se deberán evitar aquellos pasos peligrosos(Warnings.Dangerous).

    public IEnumerable<Tuple<Action, Warnings>> Exercise2() {
        var map = Exercise1();

        var result =
            map.steps
            .Zip(map.stepWarn, (a, e) => Tuple.Create(a, e))
            .Where(x => x.Item2 != Warnings.Dangerous);

        return result;
    }

}