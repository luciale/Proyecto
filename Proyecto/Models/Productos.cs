using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto.Models
{
    public class Productos : IComparable
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public double Precio { get; set; }

        public int FixedSizeText
        {
            get
            {
                return 29;
            }
        }


        public override string ToString()
        {
            return ToFixedSizeString();
        }


        public string ToFixedSizeString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(Id.ToString().PadLeft(2, '0'));
            sb.Append('|');
            sb.Append(Nombre.PadLeft(20, '\0'));
            sb.Append('|');
            sb.Append(Precio.ToString().PadLeft(5, '0'));
            return sb.ToString();
        }
        public void LLenarProducto(string datos)
        {
            var datos1 = datos.Split('|');
            Id = Convert.ToInt32(datos1[0].Trim());
            string n= (datos1[1].Trim());
            for(int i =0; i < n.Length; i++)
            {
                if(n[i]!= '\0')
                {
                    Nombre = Nombre + n[i];
                }
            }
            Precio = Convert.ToDouble(datos1[2].Trim());
        }
        public Productos LeerProducto(string pathArch, int posi,CifradoS cif)
        {
            Productos aux = new Productos();
            using (var stream = new FileStream(pathArch, FileMode.Open))
            {
                
                    
                    stream.Seek(posi, SeekOrigin.Begin);
                    byte[] lectura = new byte[aux.FixedSizeText];

                    stream.Read(lectura, 0, aux.FixedSizeText);
                cif.ConvertirAscci(lectura);  //Obtengo los codigos Ascci del buffer
                cif.InicioDesCifrado();//inicio el cifrado del buffer   
                string datos = Encoding.UTF8.GetString(cif.escribir, 0, cif.escribir.Length);
                    aux.LLenarProducto(datos);
                    

                
                stream.Close();
            }
            return aux;
        }
        public Nodo<Productos> LeerNodo(string pathArch, int posi, CifradoS cif)
        {
            Nodo<Productos> nodo = new Nodo<Productos>();
            using (var stream = new FileStream(pathArch, FileMode.Open))
            {
                //int posi = encabezado.FixedSizeText + tamnodo * encabezado.Raiz;

                stream.Seek(posi, SeekOrigin.Begin);
                byte[] lectura = new byte[nodo.FixedSizeText];
                stream.Read(lectura, 0, nodo.FixedSizeText);
                cif.ConvertirAscci(lectura);  //Obtengo los codigos Ascci del buffer
                cif.InicioDesCifrado();//inicio el cifrado del buffer   
                string datos = Encoding.UTF8.GetString(cif.escribir, 0, cif.escribir.Length);
                nodo.LlenarNodo(datos);
                posi = posi + nodo.FixedSizeText;
                for (int i = 0; i < nodo.Values.Length; i++)
                {
                    Productos aux = new Productos();
                    stream.Seek(posi + (aux.FixedSizeText * i), SeekOrigin.Begin);
                    lectura = new byte[aux.FixedSizeText];
                    stream.Read(lectura, 0, aux.FixedSizeText);
                    cif.ConvertirAscci(lectura);  //Obtengo los codigos Ascci del buffer
                    cif.InicioDesCifrado();//inicio el cifrado del buffer   
                    datos = Encoding.UTF8.GetString(cif.escribir, 0, cif.escribir.Length);
                    aux.LLenarProducto(datos);
                    nodo.Values[i] = aux;

                }
                stream.Close();
            }
            return nodo;

        }
        public void EscribirNodo(string pathArch, int pos, Nodo<Productos> nodo,CifradoS cif)
        {
            Productos vacio = new Productos();
            vacio.Id = -1;
            vacio.Nombre = "";
            vacio.Precio = 00.00;
            using (var stream = new FileStream(pathArch, FileMode.Open))
            {
                stream.Seek(pos, SeekOrigin.Begin);
                string escribir = nodo.ToFixedSizeString();
                stream.Write(Encoding.ASCII.GetBytes(escribir), 0, nodo.FixedSizeText);
                for (int i = 0; i < nodo.Values.Length; i++)
                {
                    if (nodo.Values[i] != null)
                    {
                        escribir = nodo.Values[i].ToFixedSizeString();
                        cif.ConvertirAscci(Encoding.ASCII.GetBytes(escribir));  //Obtengo los codigos Ascci del buffer
                        cif.InicioCifrado();//inicio el cifrado del buffer   
                        stream.Seek(pos + nodo.FixedSizeText + (i * nodo.Values[i].FixedSizeText), SeekOrigin.Begin);
                        stream.Write(cif.escribir, 0, escribir.Length);
                    }
                    else
                    {
                        escribir = vacio.ToFixedSizeString();
                        cif.ConvertirAscci(Encoding.ASCII.GetBytes(escribir));  //Obtengo los codigos Ascci del buffer
                        cif.InicioCifrado();//inicio el cifrado del buffer   
                        stream.Seek((pos + nodo.FixedSizeText + (i * escribir.Length)), SeekOrigin.Begin);
                        stream.Write(cif.escribir, 0, escribir.Length);
                    }
                }
                stream.Close();
            }
        }
        public int CompareTo(object obj) //Comparación del Nombre de las bebidas
                                         //retorna los siguientes 3 valores -1 menor, 0 igual, 1 mayor
        {

            return this.Id.CompareTo(((Productos)obj).Id);

        }
        public static Comparison<Productos> CompareByName = delegate (Productos p1, Productos p2)
        {
            return p1.CompareTo(p2);
        };
    }
}
