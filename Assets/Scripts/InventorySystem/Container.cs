using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[System.Serializable]
public class Container //La caja de bombones
{
    [SerializeField] float myTotalWeight;
    [SerializeField] List<Slot> slots;

    Dictionary<ElementData, List<Slot>> element_register = new Dictionary<ElementData, List<Slot>>();

    public Container(int capacity)
    {
        slots = new List<Slot>();
        for (int i = 0; i < capacity; i++)
        {
            Slot slot = new Slot(i);
            slots.Add(slot);
        }
    }

    public void Configure()
    {
        
    }

    public void AddElement()
    {

    }

    public bool AddElement(ElementData data, int quantity)
    {        
        //me creo una variable de cantidad de elementos a agregar
        //porque tal vez vamos a pasar por el registro y luego por el buscador de casillas vacias
        var aux_temp = quantity;

        Debug.Log("Intento agregar:: " + aux_temp + " " + data.Element_Name);

        //primero me fijo si hay algun registro
        //si lo hay me fijo que casilleros de ese registro, el primero tiene espacio
        if (!element_register.ContainsKey(data))
        {
            element_register.Add(data, new List<Slot>());
        }
        else
        {
            var collection_to_check = element_register[data];
            foreach (var slot in collection_to_check)
            {
                
                var free = slot.Stack.FreeSpaces;
                Debug.Log("Espacio vacio es: " + free);

                if (aux_temp <= free)
                {
                    Debug.Log("El valor es menor o igual, agrego todo y corto");
                    //hago la suma directamente y corto la ejecucion porque ya agregaria todo
                    slot.AddElement(aux_temp);
                    aux_temp = 0;
                }
                else
                {
                    Debug.Log("El valor es mayor, le quito lo que nesecito al aux y relleno el resto");
                    aux_temp -= free;
                    slot.AddElement(free);

                    Debug.Log("me sobró: " + aux_temp);
                }

                if (aux_temp <= 0) break;
                else continue; //continuo revisando si hay otro para seguir restando
            }
        }

        if (aux_temp == 0) return true;
        if (aux_temp < 0) throw new System.Exception("Hubo un error, mi cantidad temporal no puede ser menor a cero");

        //si mi aux_temp es mayor a cero... todavia tengo elementos para agregar

        //si los registros estan todos llenos...

        Debug.Log("Buscando un casillero vacio en Slots");

        //busco un casillero vacio
        foreach (var slot in slots)
        {
            if (element_register[data].Contains(slot)) continue;

            if (slot.IsEmpty) 
            {
                Debug.Log("Encontré uno que esta vacio y no tiene los mismos datos");

                slot.CreateNewStack(data);

                
                var free = slot.Stack.FreeSpaces;
                Debug.Log("Espacio vacio es: " + free);

                if (aux_temp <= free)
                {
                    Debug.Log("El valor es menor o igual, agrego todo y corto");
                    //hago la suma directamente y corto la ejecucion porque ya agregaria todo
                    slot.AddElement(aux_temp);
                    aux_temp = 0;
                }
                else
                {
                    Debug.Log("El valor es mayor, le quito lo que nesecito al aux y relleno el resto");
                    aux_temp -= free;
                    slot.AddElement(free);

                    Debug.Log("me sobró: " + aux_temp);
                }

                //y lo agrego al registro para la proxima pasada
                var reg_slots = element_register[data];
                reg_slots.Add(slot);

                if (aux_temp == 0) return true;
                if (aux_temp < 0) throw new System.Exception("Hubo un error, mi cantidad temporal no puede ser menor a cero");
            }
            else
            {
                if (slot.HasSameData(data))
                {
                    throw new System.Exception("Hubo un error, no deberia haber uno igual porque sino deberia estar en el diccionario");
                }
                else
                {
                    Debug.Log("Esto ´puede ser cuando agregue otros de otro tipo");
                    
                }
            }
        }

        if (aux_temp == 0) return true;
        if (aux_temp < 0) throw new System.Exception("Hubo un error, mi cantidad temporal no puede ser menor a cero");

        return false;

    }
}

[System.Serializable]
public class Slot //Los separadores de bombones, [ESTÁTICO]
{
    [SerializeField] int position;
    public int Position { get { return position; } }
    [SerializeField] StackedPile stack;
    public StackedPile Stack => stack;

    public bool IsEmpty => stack.IsEmpty;
    public void CreateNewStack(ElementData element)
    {
        stack = new StackedPile();
        stack.SetElement(element);
    }

    public bool AddElement(int quantity)
    {
        return stack.Add_SAFE(quantity);
    }

    public bool HasSameData(ElementData elementData)
    {
        return stack.Element_Is_Equal(elementData);
    }

    #region Contructor
    public Slot(int position)
    {
        this.position = position;
        stack = new StackedPile();
    }
    #endregion

    #region Drag & Drop Functions
    public void OverrideStack(StackedPile stack)
    {
        this.stack = stack;
    }
    public StackedPile DropStack(StackedPile origin_stack)
    {
        if (this.stack.Equals(origin_stack))
        {
            var result = origin_stack.Copy();

            int bigger = Mathf.Max(this.stack.Quantity, origin_stack.Quantity);
            int smaller = Mathf.Min(this.stack.Quantity, origin_stack.Quantity);

            int raw_result = bigger + smaller;
            int diference = raw_result - this.stack.MaxStack;

            //le paso los valores crudos porque internamente se encarga de hacer el recorte
            this.stack.ModifyQuantity(raw_result);
            result.ModifyQuantity(diference);

            return result;
        }
        else
        {
            var result = this.stack.Copy();
            this.stack = origin_stack;
            return result;
        }
    }
    #endregion

    public void Empty()
    {
        stack.Force_to_Empty();
        stack = null;
    }
}

[System.Serializable]
public class StackedPile //La bolsita del bombon
{
    #region Vars
    [SerializeField] int quant = 0;
    [SerializeField] ElementData element = null; //El Bombon
    [SerializeField] float weight = 0f;
    #endregion

    #region Getters & Setters
    public float Weight  => weight; 
    public int MaxStack => element.MaxStack;
    public bool IsEmpty => Quantity <= 0 || element == null;
    public bool IsFull => Quantity >= MaxStack;
    public int Quantity
    {
        get => quant;
        set
        {
            quant = value;
            if (quant <= 0)
            {
                element = null;
                quant = 0;
            }
            weight = element != null ? quant * element.Weight : 0;
        }
    }
    public int FreeSpaces => MaxStack - quant;
    public bool Element_Is_Equal(ElementData element) => this.element.Equals(element);
    #endregion

    #region Constructor
    public StackedPile()
    {
        element = null;
        Quantity = 0;
    }
    #endregion

    #region Copy
    public StackedPile Copy()
    {
        StackedPile copy = new StackedPile();
        copy.element = element;
        copy.quant = quant;
        copy.weight = weight;
        return copy;
    }
    #endregion

    #region Modifiers
    public void SetElement(ElementData element)
    {
        this.element = element;
    }

    public void ModifyQuantity(int quant_to_modify)
    {
        if (quant_to_modify > MaxStack) Quantity = MaxStack;
        else Quantity = quant_to_modify;
    }
    #endregion

    #region ADD FUNCTIONS
    public bool Add_SAFE(int quant_to_add = 1)
    {
        var aux = Quantity + quant_to_add;
        if (aux > MaxStack) return false;
        Quantity = aux;
        return true;
    }
    public void Add_RAW(int quant_to_add = 1)
    {
        Quantity = quant_to_add;
        if (Quantity > MaxStack) Quantity = MaxStack;
    }
    public void Add_UNSAFE(int quant_to_add = 1)
    {
        Quantity = quant_to_add;
    }
    #endregion

    #region REMOVE FUNCTIONS
    public bool Remove_SAFE(int quant_to_remove = 1)
    {
        var aux = Quantity - quant_to_remove;
        if (aux < 0) return false;
        Quantity = aux;
        return true;
    }
    public void Remove_RAW(int quant_to_remove = 1)
    {
        Quantity = quant_to_remove;
    }
    #endregion

    #region Fill or Empty
    public void Force_to_Fill()
    {
        Quantity = MaxStack;
    }
    public void Force_to_Empty()
    {
        Quantity = 0;
        element = null;
    }
    #endregion

    #region Object Override
    public override bool Equals(object obj)
    {
        var stack = (StackedPile)obj;
        return stack.Equals(element);
    }
    public override int GetHashCode()
    {
        var hashCode = 464553162;
        hashCode = hashCode * -1521134295 + quant.GetHashCode();
        hashCode = hashCode * -1521134295 + EqualityComparer<ElementData>.Default.GetHashCode(element);
        hashCode = hashCode * -1521134295 + weight.GetHashCode();
        hashCode = hashCode * -1521134295 + Weight.GetHashCode();
        hashCode = hashCode * -1521134295 + MaxStack.GetHashCode();
        hashCode = hashCode * -1521134295 + IsEmpty.GetHashCode();
        hashCode = hashCode * -1521134295 + IsFull.GetHashCode();
        hashCode = hashCode * -1521134295 + Quantity.GetHashCode();
        return hashCode;
    }
    #endregion
}
