using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;

namespace IA2.Clase5
{
    public class GridExercises : MonoBehaviour
    {

        public SquareQuery query;

        private void Start()
        {
            Enumerable.Range(0, 30);

            Clase5();
            
        }
        void Clase5()
        {

        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space)) Query();
        }

        private void Query()
        {
            var ej1 = Exercise1();
            var ej2 = Exercise2();
            var ej3 = Exercise3();
            var ej4 = Exercise4();
            var ej5 = Exercise5();
            var ej6 = Exercise6();
            var ej7 = Exercise7();
            Debug.Log("Ejercicio 1:");
            foreach (var enemy in ej1) Debug.Log(enemy.name);
            Debug.Log("Ejercicio 2:");
            foreach (var enemy in ej2) Debug.Log(enemy.name);
            Debug.Log("Ejercicio 3:");
            Debug.Log(ej3);
            Debug.Log("Ejercicio 4:");
            foreach (var enemy in ej4) Debug.Log(enemy.name);
            Debug.Log("Ejercicio 5:");
            foreach (var enemy in ej5) Debug.Log(enemy.name);
            Debug.Log("Ejercicio 6:");
            foreach (var lootType in ej6) Debug.Log(lootType.ToString());
            Debug.Log("Ejercicio 7:");
            foreach (var lootType in ej7) Debug.Log(lootType.ToString());
        }

        //Ordene los enemigos con daño(damage) menor a 50, de menor a mayor.
        private IEnumerable<Enemy> Exercise1()
        {

            var aux = query;
            return aux.Query()
                .OfType<Enemy>()
                .Where(x => x.damage < 50)
                .OrderBy(x => x.damage);
        }

        //Ordene los enemigos con daño(damage) menor a 50, de menor a mayor. Además, ordenelos alfabéticamente por nombre (gameobject.name).
        private IEnumerable<Enemy> Exercise2()
        {
            var aux = query;
            return aux.Query()
                .OfType<Enemy>()
                .Where(x => x.damage < 50)
                .OrderBy(x => x.damage)
                .ThenBy(x => x.gameObject.name);
        }

        //Obtenga el daño total de los enemigos con daño mayor a 50.
        private int Exercise3()
        {
            var aux = query;
            return aux.Query()
                .OfType<Enemy>()
                .Where(x => x.damage > 50)
                .Sum(x => x.damage);
        }

        //Obtenga todos los enemigos hasta que encuentre uno de tipo(type) Archer.
        private IEnumerable<Enemy> Exercise4()
        {
            var aux = query;
            return aux.Query()
                .OfType<Enemy>()
                .TakeWhile(x => x.type != EnemyType.Archer);

        }

        //Saltee todos los enemigos hasta que encuentre uno de tipo(type) Mage.
        private IEnumerable<Enemy> Exercise5()
        {
            var aux = query;
            return aux.Query()
                .OfType<Enemy>()
                .SkipWhile(x => x.type != EnemyType.Mage);
        }

        //Obtenga todos los items que dropean(loot) los enemigos.
        private IEnumerable<LootType> Exercise6()
        {
            var aux = query;
            return aux.Query()
                .OfType<Enemy>()
                .SelectMany(x => x.loot);
        }

        //Obtenga todos los items distintos que dropean(loot) los enemigos..
        private IEnumerable<LootType> Exercise7()
        {
            var aux = query;
            return aux.Query()
                .OfType<Enemy>()
                .SelectMany(x => x.loot)
                .Distinct();//si hay dos cosas iguales las saca
        }
    }

}
