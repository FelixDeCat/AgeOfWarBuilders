namespace Tools.Pool
{
    //hay que hacer otro sin IPooleable, y le pasamos el On y Off por Callback
    using System.Collections.Generic;
    using System;
    public class GenericPoolManager<T>
    {
        protected Queue<T> pool = new Queue<T>();

        bool isExponential;
        int max_size = 0;
        bool I_HAVE_NO_OBJECTS => pool.Count == 0;
        Func<Func<object>, T> create;
        Action<T> TurnOn;
        Action<T> TurnOff;
        object indexerType;

        public GenericPoolManager() { }
        public GenericPoolManager(Func<Func<object>,T> _create, Action<T> _TurnOn, Action<T> _TurnOff, object indexerType, bool isAutoExponential = true, int prewarm = 5)
        {
            this.indexerType = indexerType;
            create = _create;
            TurnOn = _TurnOn;
            TurnOff = _TurnOff;
            isExponential = isAutoExponential;
            //AddObject(prewarm);
        }

        public virtual T Get()
        {
            if (I_HAVE_NO_OBJECTS) AddObject();
            var obj = pool.Dequeue();
            TurnOn(obj);
            return obj;
        }

        public void Return(T obj)
        {
            TurnOff(obj);
            pool.Enqueue(obj);
        }
        
        protected virtual void AddObject(int amount = -1)
        {
            int quantity_to_add = 1;

            if (amount != -1)
            {
                //si me dieron un valor: lo uso
                quantity_to_add = amount;
            }
            else
            {
                //si no me dieron un valor: busco uno

                if (isExponential)
                {
                    //si es exponencial: agrego el doble del maximo. Ej: tenia 17, agrego 17 mas
                    quantity_to_add = max_size == 0 ? 1 : max_size;
                }
                else
                {
                    //si es tranqui: lo voy agregando uno a uno
                    quantity_to_add = 1;
                }
            }

            for (int i = 0; i < quantity_to_add; i++)
            {
                pool.Enqueue(create.Invoke(() => indexerType));

                if (isExponential && pool.Count > max_size)
                    max_size = pool.Count;
            }
        }
    }    

}