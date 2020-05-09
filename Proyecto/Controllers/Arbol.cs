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
    public class Arbol : ControllerBase
    {
        
        // POST api/values
        [HttpPost]
        public void CrearArbol()
        {
            Encabezado encabezado = new Encabezado();
            encabezado.Grado = Convert.ToInt32(Request.Form["grado"]);
            encabezado.Raiz = 0;
            encabezado.Disponible = -1;
            var folderName = Path.Combine("Resources", "Files");
            var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);
            var pathArch = Path.Combine(pathToSave, "arbol.dat");
            if (!Directory.Exists(pathToSave))
            {
                Directory.CreateDirectory(pathToSave);
                using (var stream = new FileStream(pathArch, FileMode.Create))
                {
                    //ya se creo el archivo
                    stream.Close();
                }
                using (var stream = new FileStream(pathArch, FileMode.Append))
                {
                    stream.Seek(0, SeekOrigin.Begin);
                    string escribir= encabezado.ToFixedSizeString();
                    stream.Write(Encoding.ASCII.GetBytes(escribir), 0, encabezado.FixedSizeText);
                    stream.Close();
                }
            }
            else
            {
                using (var stream = new FileStream(pathArch, FileMode.OpenOrCreate))
                {
                    stream.Seek(0, SeekOrigin.Begin);
                    string escribir = encabezado.ToFixedSizeString();
                    stream.Write(Encoding.ASCII.GetBytes(escribir), 0, encabezado.FixedSizeText);
                    stream.Close();
                }
            }
        }


     
    }
}
