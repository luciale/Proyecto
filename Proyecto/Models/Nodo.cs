﻿using System;
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

        //public static Nodo<T> Insert_NewFather(Nodo<T> nodo_actual, T[] sortedList, int medio, List<Nodo<T>> list)
        //{
        //    int actual_father = -1;
        //        //leer padre;
        //    //actual_father.Values[0] = sortedList[medio];

        //    Nodo<T> nodo_aux = new Nodo<T>();
        //    for (int x = 0; x < medio; x++)
        //    {
        //        nodo_aux.Values[x] = sortedList[x];

        //    }
        //    Nodo<T> nodo_aux1 = new Nodo<T>();
        //    int x_aux = 0;
        //    for (int x = medio + 1; x < sortedList.Length; x++)
        //    {
        //        nodo_aux1.Values[x_aux] = sortedList[x];
        //        x_aux++;
        //    }
        //    //ORdenar hijos si la lista no es vacia

        //    if (list != null)
        //    {

        //        //Ordenar los hijos
        //        List<Nodo<T>> list_aux = new List<Nodo<T>>(); //lista auxiliar para guardar hijos actuales
        //        List<T> list_aux_valores = new List<T>(); //lista auxiliar para guardar el primer valor de cada hijo
        //        for (int x = 0; x < nodo_actual.Children.Length; x++)
        //        {
        //            //if (nodo_actual.Children[x] != null)
        //            //{
        //            //   // list_aux.Add(nodo_actual.Children[x]);
        //            //   // list_aux_valores.Add(nodo_actual.Children[x].Values[0]);
        //            //}
        //        }
        //        for (int x = 0; x < list.Count; x++)
        //        {
        //            list_aux.Add(list[x]);
        //            list_aux_valores.Add(list[x].Values[0]);
        //        }
        //        //Ordenamos los primeros valores de cada hijo
        //        var sortedList1 = GenericComparation<T>.SortedList(list_aux_valores.ToArray(), Producto.CompareByName);
        //        //nodo_aux.Children = new Nodo<T>[m];
        //        //asignamos los hijos de nodo_aux
        //        int hijos = sortedList1.Length / 2;
        //        for (int x = 0; x < hijos; x++)
        //        {
        //            for (int y = 0; y < list_aux.Count; y++)
        //            {
        //                //if (list_aux[y].Values[0].Equals(sortedList1[x]))
        //                //{
        //                //    nodo_aux.Children[x] = list_aux[x];
        //                //    nodo_aux.Children[x].father = nodo_aux;
        //                //}
        //            }
        //        }


        //        //nodo_aux1.Children = new Nodo<T>[m];
        //        //asignamos los hijos de nodo_aux1
        //        for (int x = hijos; x < sortedList1.Length; x++)
        //        {
        //            for (int y = 0; y < list_aux.Count; y++)
        //            {
        //                if (list_aux[y].Equals(sortedList1[x]))
        //                {
        //                    nodo_aux1.Children[x] = list_aux[x];
        //                    nodo_aux1.Children[x].father = nodo_aux1;
        //                }
        //            }
        //        }

        //    }


        //    nodo_aux.father = actual_father;
        //    nodo_aux1.father = actual_father;
        //    actual_father.Children[0] = nodo_aux;
        //    actual_father.Children[1] = nodo_aux1;
        //    actual_father.father = null;
        //    return actual_father;
        //}

        //public static void Children_Order(Nodo<T> nodo_actual, List<Nodo<T>> list)
        //{
        //    //Ordenar los hijos
        //    List<Nodo<T>> list_aux = new List<Nodo<T>>(); //lista auxiliar para guardar hijos actuales
        //    List<T> list_aux_valores = new List<T>(); //lista auxiliar para guardar el primer valor de cada hijo
        //    for (int x = 0; x < nodo_actual.Children.Length; x++)
        //    {
        //        if (nodo_actual.Children[x] != null)
        //        {
        //            list_aux.Add(nodo_actual.Children[x]);
        //            list_aux_valores.Add(nodo_actual.Children[x].Values[0]);
        //        }
        //    }
        //    for (int x = 0; x < list.Count; x++)
        //    {
        //        list_aux.Add(list[x]);
        //        list_aux_valores.Add(list[x].Values[0]);
        //    }
        //    //Ordenamos los primeros valores de cada hijo
        //    var sortedList1 = GenericComparation<T>.SortedList(list_aux_valores.ToArray(), Producto.CompareByName);

        //    //Ya ordenados los valores guardamos los hijos en su nueva posicion
        //    for (int x = 0; x < sortedList1.Length; x++)
        //    {

        //        for (int y = 0; y < list_aux.Count; y++)
        //        {
        //            if (list_aux[y].Values[0].Equals(sortedList1[x]))
        //            {
        //                nodo_actual.Children[x] = list_aux[y];
        //                nodo_actual.Children[x].father = nodo_actual;
        //            }
        //        }
        //    }
        //}
    }
}
