﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto.Models
{
    public class Encabezado
    {
        public int Grado { get; set; }
        public int Raiz { get; set; }
        public int Disponible { get; set; }

        public int FixedSizeText
        {
            get
            {
                return 8;
            }
        }


        public override string ToString()
        {
            return ToFixedSizeString();
        }


        public string ToFixedSizeString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(Grado.ToString().PadLeft(2, '0'));
            sb.Append('|');
            sb.Append(Raiz.ToString().PadLeft(2, '0'));
            sb.Append('|');
            sb.Append(Disponible.ToString().PadLeft(2, '0'));
            return sb.ToString();
        }

        public void EscribirEncabezado(string pathArch,CifradoS cif)
        {
            using (var stream = new FileStream(pathArch, FileMode.OpenOrCreate))
            {
                stream.Seek(0, SeekOrigin.Begin);
                string escribir = ToFixedSizeString();
                cif.ConvertirAscci(Encoding.ASCII.GetBytes(escribir));  //Obtengo los codigos Ascci del buffer
                cif.InicioDesCifrado();//inicio el cifrado del buffer   
                stream.Write(cif.escribir, 0, cif.escribir.Length);
                stream.Close();
            }
        }
        public void LLenarEncabezado(string datos)
        {
            var datos1 = datos.Split('|');
            Grado = Convert.ToInt32(datos1[0].Trim());
            Raiz = Convert.ToInt32(datos1[1].Trim());
            Disponible = Convert.ToInt32(datos1[2].Trim());
        }

        public Encabezado LeerEncabezado(string pathArch, CifradoS cif)
        {
            Encabezado encabezado = new Encabezado();
            using (var stream = new FileStream(pathArch, FileMode.Open))
            {
                stream.Seek(0, SeekOrigin.Begin);
                byte[] lectura = new byte[encabezado.FixedSizeText];
                stream.Read(lectura, 0, encabezado.FixedSizeText);
                cif.ConvertirAscci(lectura);  //Obtengo los codigos Ascci del buffer
                cif.InicioDesCifrado();//inicio el cifrado del buffer 
                string datos = Encoding.UTF8.GetString(cif.escribir, 0, cif.escribir.Length);
                encabezado.LLenarEncabezado(datos);
                stream.Close();
            }
            return encabezado;
        }
    }
}
