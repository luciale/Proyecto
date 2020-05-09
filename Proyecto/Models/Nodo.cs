using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto.Models
{
    public class Nodo<T> where T : IComparable
    {
        public int m = Data.Instance.grado; // Grado del árbol 
        public T[] Values = new T[Data.Instance.grado - 1]; // Vector de sodas en el nodo
        public int[] Children = new int[Data.Instance.grado]; // Vector de hijos 
        public int father = -1;
        public override string ToString()
        {
            return ToFixedSizeString();
        }
        public int FixedSizeText
        {
            get
            {

                return 2 + Children.Length*3;
            }
        }

        public string ToFixedSizeString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(father.ToString().PadLeft(2, '0'));
            for(int i =0; i < Children.Length; i++)
            {
                sb.Append('|');
                sb.Append(Children[i].ToString().PadLeft(2, '0'));
                
            }
            return sb.ToString();
        }
        public void LlenarNodo(string datos)
        {
            var datos1 = datos.Split('|');
            father = Convert.ToInt32(datos1[0].Trim());
            int con = 1;
            for(int i =0; i< Children.Length; i++)
            {
                Children[i] = Convert.ToInt32(datos1[con].Trim());
                con++;
            }
        }
        public Nodo()
        {
            for (int i = 0; i < m - 1; i++)
            {
                Values[i] = default(T);
            }
            for (int j = 0; j < m; j++)
            {
                Children[j] = -1;
            }
        }




        public void Insert_Aux(T value) //Insertamos en el nodo el valor
        {
            for (int x = 0; x < m; x++)
            {
                if (object.Equals(this.Values[x], default(T)))
                {
                    this.Values[x] = value;
                    break;
                }
            }
        }
        public int Delete_Nodo_InList(Nodo<T> eliminar) //Devolver posicion del nodo a eliminar
        {
            for (int x = 0; x < m; x++)
            {
                if (this.Children[x].Equals(eliminar))
                {
                    return x;
                }
            }
            return -1;
        }
        public static Nodo<Productos> Insert_NewFather(Nodo<T> nodo_actual, Productos[] sortedList, int medio, List<int> list, string pathArch, int nodo_a,CifradoS cif)
        {
            Productos auxp = new Productos();

            int pos_nodos = Data.Instance.encabezado.FixedSizeText;
            Nodo<Productos> actual_father = new Nodo<Productos>();
            actual_father.Values[0] = sortedList[medio];

            Nodo<Productos> nodo_aux = new Nodo<Productos>();
            for (int x = 0; x < medio; x++)
            {
                nodo_aux.Values[x] = sortedList[x];

            }
            Nodo<Productos> nodo_aux1 = new Nodo<Productos>();
            int x_aux = 0;
            for (int x = medio + 1; x < sortedList.Length; x++)
            {
                nodo_aux1.Values[x_aux] = sortedList[x];
                x_aux++;
            }
            //ORdenar hijos si la lista no es vacia

            if (list != null)
            {

                //Ordenar los hijos
                List<int> list_aux = new List<int>(); //lista auxiliar para guardar hijos actuales
                List<Productos> list_aux_valores = new List<Productos>(); //lista auxiliar para guardar el primer valor de cada hijo
                for (int x = 0; x < nodo_actual.Children.Length; x++)
                {
                    if (nodo_actual.Children[x] != -1)
                    {
                        list_aux.Add(nodo_actual.Children[x]);
                        //IR A CADA UNO DE LOS HIJOS A TOMAR LOS VALORES
                        for (int i = 0; i < nodo_actual.Children.Length; i++)
                        {
                            Productos aux = new Productos();
                            pos_nodos = Data.Instance.encabezado.FixedSizeText + nodo_actual.Children[x] * (nodo_actual.FixedSizeText + aux.FixedSizeText * nodo_actual.Values.Length);

                            aux = aux.LeerProducto(pathArch, pos_nodos + nodo_actual.FixedSizeText,cif);
                            list_aux_valores.Add(aux);


                        }

                     
                    }
                }
                for (int x = 0; x < list.Count; x++)
                {
                    list_aux.Add(list[x]);
                    Productos aux = new Productos();
                    pos_nodos = Data.Instance.encabezado.FixedSizeText + list[x] * (nodo_actual.FixedSizeText + aux.FixedSizeText * nodo_actual.Values.Length);

                    aux = aux.LeerProducto(pathArch, pos_nodos + nodo_actual.FixedSizeText,cif);
                    list_aux_valores.Add(aux);
                    list_aux_valores.Add(aux);
                }
                //Ordenamos los primeros valores de cada hijo
                var sortedList1 = GenericComparation<T>.SortedList(list_aux_valores.ToArray(), Productos.CompareByName);
                nodo_aux.Children = new int[Data.Instance.grado];
                //asignamos los hijos de nodo_aux
                int hijos = sortedList1.Length / 2;
                for (int x = 0; x < hijos; x++)
                {
                    for (int y = 0; y < list_aux.Count; y++)
                    {
                        Nodo<Productos> n = new Nodo<Productos>();
                        
                        pos_nodos = Data.Instance.encabezado.FixedSizeText + list_aux[y] * (nodo_actual.FixedSizeText + auxp.FixedSizeText * nodo_actual.Values.Length);

                        n = auxp.LeerNodo(pathArch, pos_nodos,);
                        if (n.Values[0].Equals(sortedList1[x]))
                        {
                            nodo_aux.Children[x] = list_aux[x];
                            pos_nodos = Data.Instance.encabezado.FixedSizeText + nodo_aux.Children[x] * (nodo_actual.FixedSizeText + auxp.FixedSizeText * nodo_actual.Values.Length);
                            n = auxp.LeerNodo(pathArch, pos_nodos,cif);
                            n.father = Data.Instance.encabezado.Disponible;
                            auxp.EscribirNodo(pathArch, pos_nodos, n,cif);
                        }
                    }
                }


                nodo_aux1.Children = new int[Data.Instance.grado];
                //asignamos los hijos de nodo_aux1
                for (int x = hijos; x < sortedList1.Length; x++)
                {
                    for (int y = 0; y < list_aux.Count; y++)
                    {
                        Nodo<Productos> n = new Nodo<Productos>();

                        pos_nodos = Data.Instance.encabezado.FixedSizeText + list_aux[y] * (nodo_actual.FixedSizeText + auxp.FixedSizeText * nodo_actual.Values.Length);

                        n = auxp.LeerNodo(pathArch, pos_nodos,cif);
                        if (list_aux[y].Equals(sortedList1[x]))
                        {
                            nodo_aux1.Children[x] = list_aux[x];
                            nodo_aux1.Children[x] = list_aux[x];
                            pos_nodos = Data.Instance.encabezado.FixedSizeText + nodo_aux1.Children[x] * (nodo_actual.FixedSizeText + auxp.FixedSizeText * nodo_actual.Values.Length);
                            n = auxp.LeerNodo(pathArch, pos_nodos,cif);
                            n.father = Data.Instance.encabezado.Disponible;
                            auxp.EscribirNodo(pathArch, pos_nodos, n,cif);
                            
                        }
                    }
                }

            }


            nodo_aux.father = Data.Instance.encabezado.Disponible;
            nodo_aux1.father = Data.Instance.encabezado.Disponible;
            
            pos_nodos = Data.Instance.encabezado.FixedSizeText + nodo_aux.Children[Data.Instance.encabezado.Disponible] * (nodo_actual.FixedSizeText + auxp.FixedSizeText * nodo_actual.Values.Length);

            actual_father.Children[0] = nodo_a;

            actual_father.Children[1] = Data.Instance.encabezado.Disponible+1;
            actual_father.father = -1;
            auxp.EscribirNodo(pathArch, pos_nodos, actual_father,cif);
            Data.Instance.encabezado.Grado = Data.Instance.encabezado.Grado + 2;
            return actual_father;
        }



        //public void Insert(T value)
        //{
        //    if (raiz == null) //Si el árbol esta vacío
        //    {
        //        Nodo<T> nodo = new Nodo<T>();
        //        nodo.father = null;
        //        nodo.Values[0] = value;
        //        raiz = nodo;
        //    }
        //    else //El árbol ya tiene algun dato.
        //    {
        //        Insert(raiz, value, null);
        //    }
        //}
        //public void Insert(Nodo<T> nodo_actual, T value, List<Nodo<T>> list)
        //{

        //    if (object.Equals(nodo_actual.Children[0], default(T)) && object.Equals(nodo_actual.Children[1], default(T))) //El nodo es hoja
        //    {
        //        if (object.Equals(nodo_actual.Values[Nodo<T>.m - 2], default(T))) //Hay espacio en el nodo hoja
        //        {
        //            nodo_actual.Insert_Aux(value);
        //            var sortedList = GenericComparation<T>.SortedList(nodo_actual.Values, Soda.CompareByName);
        //            for (int x = 0; x < sortedList.Length; x++)
        //            {
        //                nodo_actual.Values[x] = sortedList[x]; //Inserto los valores ordenados por burbuja en el vector
        //            }
        //        }
        //        else //no hay espacio en el nodo hoja
        //        {
        //            int medio = Nodo<T>.m / 2;
        //            T[] aux = new T[Nodo<T>.m];
        //            for (int x = 0; x < aux.Length - 1; x++) //Inserto los valores que ya tengo en un arreglo auxiliar
        //            {
        //                aux[x] = nodo_actual.Values[x];
        //            }
        //            aux[aux.Length - 1] = value;
        //            var sortedList = GenericComparation<T>.SortedList(aux, Soda.CompareByName);


        //            if (object.Equals(nodo_actual.father, default(T)))
        //            { //No existe padre
        //                Nodo<T> actual_father = Nodo<T>.Insert_NewFather(nodo_actual, sortedList, medio, null);
        //                raiz = actual_father;

        //            }
        //            else
        //            { //Si existe padre
        //                Nodo<T> nodo_aux = new Nodo<T>();
        //                for (int x = 0; x < medio; x++)
        //                {
        //                    nodo_aux.Values[x] = sortedList[x];
        //                }
        //                Nodo<T> nodo_aux1 = new Nodo<T>();
        //                int x_aux = 0;
        //                for (int x = medio + 1; x < sortedList.Length; x++)
        //                {
        //                    nodo_aux1.Values[x_aux] = sortedList[x];
        //                    x_aux++;
        //                }

        //                List<Nodo<T>> list1 = new List<Nodo<T>>();
        //                list1.Add(nodo_aux);
        //                list1.Add(nodo_aux1);
        //                //Eliminamos el nodo en la lista de Nodos del padre
        //                int eliminar = nodo_actual.father.Delete_Nodo_InList(nodo_actual);
        //                if (eliminar != -1)
        //                {
        //                    nodo_actual.father.Children[eliminar] = null;
        //                }
        //                Insert(nodo_actual.father, sortedList[medio], list1); //enviamos valores nuevos para ingresar

        //            }

        //        }


        //    }

        //    else //El nodo no es hoja
        //    {
        //        if (list != null) //El nodo ya trae hijos si no trae hijos es porque es un valor nuevo
        //        {
        //            if (object.Equals(nodo_actual.Values[Nodo<T>.m - 2], default(T))) //Hay espacio en el nodo 
        //            {
        //                nodo_actual.Insert_Aux(value);
        //                var sortedList = GenericComparation<T>.SortedList(nodo_actual.Values, Soda.CompareByName);
        //                for (int x = 0; x < sortedList.Length; x++)
        //                {
        //                    nodo_actual.Values[x] = sortedList[x]; //Inserto los valores ordenados por burbuja en el vector
        //                }
        //                //Ordenar los hijos
        //                Nodo<T>.Children_Order(nodo_actual, list);


        //            }
        //            else //No hay espacio en el nodo
        //            {
        //                int medio = Nodo<T>.m / 2;
        //                T[] aux = new T[Nodo<T>.m];
        //                for (int x = 0; x < Nodo<T>.m - 1; x++) //Inserto los valores que ya tengo en un arreglo auxiliar
        //                {
        //                    aux[x] = nodo_actual.Values[x];
        //                }
        //                aux[aux.Length - 1] = value;
        //                var sortedList = GenericComparation<T>.SortedList(aux, Soda.CompareByName);

        //                if (object.Equals(nodo_actual.father, default(T)))
        //                { //No existe padre

        //                    Nodo<T> actual_father = Nodo<T>.Insert_NewFather(nodo_actual, sortedList, medio, list);
        //                    raiz = actual_father;

        //                    //unir todos los hijos de los hijos del padre

        //                }
        //                else
        //                { //Si existe padre
        //                    Nodo<T> nodo_aux = new Nodo<T>();
        //                    for (int x = 0; x < medio - 1; x++)
        //                    {
        //                        nodo_aux.Values[x] = sortedList[x];
        //                    }
        //                    Nodo<T> nodo_aux1 = new Nodo<T>();
        //                    int x_aux = 0;
        //                    for (int x = medio; x < sortedList.Length; x++)
        //                    {
        //                        nodo_aux1.Values[x_aux] = sortedList[x];
        //                        x_aux++;
        //                    }

        //                    list.Add(nodo_aux);
        //                    list.Add(nodo_aux1);
        //                    //Eliminamos el nodo en la lista de Nodos del padre
        //                    int eliminar = nodo_actual.father.Delete_Nodo_InList(nodo_actual);
        //                    if (eliminar != -1)
        //                    {
        //                        nodo_actual.father.Children[eliminar] = null;
        //                    }

        //                    //reordenar hijos de los nodos 
        //                    Insert(nodo_actual.father, sortedList[medio - 1], list); //enviamos valores nuevos para ingresar

        //                }
        //            }
        //        }
        //        else
        //        {
        //            int pos = GenericComparation<T>.Position(nodo_actual.Values, Soda.CompareByName, value);
        //            Insert(nodo_actual.Children[pos], value, null);

        //        }
        //    }


        //}
        public static void Children_Order(Nodo<Productos> nodo_actual, List<int> list, string pathArch)
        {
            //Ordenar los hijos
            int pos_nodos = Data.Instance.encabezado.FixedSizeText;
            List<int> list_aux = new List<int>(); //lista auxiliar para guardar hijos actuales
            List<Productos> list_aux_valores = new List<Productos>(); //lista auxiliar para guardar el primer valor de cada hijo
            for (int x = 0; x < nodo_actual.Children.Length; x++)
            {
                if (nodo_actual.Children[x] != -1)
                {
                    list_aux.Add(nodo_actual.Children[x]);
                    for (int i = 0; i < nodo_actual.Children.Length; i++)
                    {
                        Productos aux = new Productos();
                        pos_nodos = Data.Instance.encabezado.FixedSizeText + nodo_actual.Children[x] * (nodo_actual.FixedSizeText + aux.FixedSizeText * nodo_actual.Values.Length);

                        aux = aux.LeerProducto(pathArch, pos_nodos + nodo_actual.FixedSizeText);
                        list_aux_valores.Add(aux);


                    }
                }
            }
            for (int x = 0; x < list.Count; x++)
            {
                list_aux.Add(list[x]);
                Productos aux = new Productos();
                pos_nodos = Data.Instance.encabezado.FixedSizeText + list[x] * (nodo_actual.FixedSizeText + aux.FixedSizeText * nodo_actual.Values.Length);

                aux = aux.LeerProducto(pathArch, pos_nodos + nodo_actual.FixedSizeText);
                list_aux_valores.Add(aux);
                list_aux_valores.Add(aux);
            }
            //Ordenamos los primeros valores de cada hijo
            var sortedList1 = GenericComparation<T>.SortedList(list_aux_valores.ToArray(), Productos.CompareByName);

            //Ya ordenados los valores guardamos los hijos en su nueva posicion
            for (int x = 0; x < sortedList1.Length; x++)
            {

                for (int y = 0; y < list_aux.Count; y++)
                {

                    Productos aux = new Productos();
                    pos_nodos = Data.Instance.encabezado.FixedSizeText + list_aux[y] * (nodo_actual.FixedSizeText + aux.FixedSizeText * nodo_actual.Values.Length);

                    aux = aux.LeerProducto(pathArch, pos_nodos + nodo_actual.FixedSizeText);
                    

                    if (aux.Equals(sortedList1[x]))
                    {
                        nodo_actual.Children[x] = list_aux[y];
                        Nodo<Productos> n = new Nodo<Productos>();
                        pos_nodos = Data.Instance.encabezado.FixedSizeText + nodo_actual.Children[x] * (nodo_actual.FixedSizeText + aux.FixedSizeText * nodo_actual.Values.Length);
                        n = aux.LeerNodo(pathArch, pos_nodos);
                        n.father = Data.Instance.encabezado.Disponible;
                        aux.EscribirNodo(pathArch, pos_nodos, n);
                        
                    }
                }
            }
        }
    }
}
