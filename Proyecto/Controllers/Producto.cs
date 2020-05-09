using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Proyecto.Models;

namespace Proyecto.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Producto : ControllerBase
    {
       
        [HttpPost]
        public void Nuevo()
        {
            Productos produc = new Productos();
            produc.Id = Convert.ToInt32(Request.Form["Id"]);
            produc.Nombre = Request.Form["Nombre"];
            produc.Precio = Convert.ToDouble(Request.Form["Precio"]);

            Encabezado encabezado = new Encabezado();
            var folderName = Path.Combine("Resources", "Files");
            var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);
            var pathArch = Path.Combine(pathToSave, "arbol.dat");
            if (Directory.Exists(pathToSave))
            {

                encabezado = encabezado.LeerEncabezado(pathArch);
            }
            Data.Instance.grado = encabezado.Grado;

            //LEER RAIZ
            Nodo<Productos> nodo = new Nodo<Productos>();
            int tamnodo = nodo.FixedSizeText + (produc.FixedSizeText * nodo.Values.Length);
            //nodo.Values[0] = produc;
            //Productos n = new Productos();
            //n.Id = 23;
            //n.Nombre = "prueba";
            //n.Precio = 1.3;
            //nodo.Values[1] = n;
            //n.EscribirNodo(pathArch, encabezado.FixedSizeText, nodo);
            ////CUANDO SE AGREGA UN NODO 
            //encabezado.Disponible = encabezado.Disponible + 1;
            //encabezado.EscribirEncabezado(pathArch);

            int posi = encabezado.FixedSizeText + tamnodo * encabezado.Raiz;
            Nodo<Productos> raiz = new Nodo<Productos>();
            raiz = produc.LeerNodo(pathArch,posi);
            string h = "hola";
            //nodo raiz = new Nodo<Productos>();
            //raiz.Values[0] = produc;
            //produc.Id = 2;
            //produc.Nombre = "probando";
            //produc.Precio = 2.3;
            //raiz.Values[1] = produc;
            //int pos = encabezado.FixedSizeText;



        }





    }
}
