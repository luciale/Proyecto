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
    public class Cifrado : ControllerBase
    {

        // POST api/values
        [HttpPost]
        public IActionResult SDES()
        {
            var file = Request.Form.Files[0];
            
            Encabezado encabezado = new Encabezado();
            int clave = Convert.ToInt32(Request.Form["clave"]);
           
            CifradoS cif = new CifradoS();
            var folderName = Path.Combine("Resources", "Files");
            var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);
            var pathP= Path.Combine(Directory.GetCurrentDirectory(), "Permutaciones.txt");
            var fullPath = Path.Combine(pathToSave, "subido.txt");
            string extension = Path.GetExtension(file.FileName);
           
            using (var stream = new FileStream(fullPath, FileMode.Create))
            {
                file.CopyTo(stream);
            }
            if (clave >= 0 && clave <= 1023)
            {
                cif.DecimalaBinario(clave, 10);
                cif.LeerPermutaciones(pathP);
                cif.GenerarLLaves();
            
                if (extension == ".txt")
                {
                    var pathg = Path.Combine(pathToSave, "nuevo.scif");
                    using (var stream = new FileStream(pathg, FileMode.Create))
                    {
                        
                    }

                    using (var stream = new FileStream(fullPath, FileMode.Open))
                    {
                        using (BinaryWriter wr = new BinaryWriter(stream))
                        {
                            using (BinaryReader br = new BinaryReader(stream))
                            {
                                var buffer = new byte[100];
                                while (br.BaseStream.Position != br.BaseStream.Length)
                                {
                                    buffer = br.ReadBytes(100); //llenar el buffer
                                    cif.ConvertirAscci(buffer);  //Obtengo los codigos Ascci del buffer
                                    cif.InicioCifrado();//inicio el cifrado del buffer   
                                    cif.EscribirArch(pathg);
                                }
                            }
                        }
                        stream.Close();
                    }
                    var stream1 = new FileStream(pathg, FileMode.Open);

                    return File(stream1, System.Net.Mime.MediaTypeNames.Application.Octet, "nuevo.scif");


                }
                else if (extension == ".scif")
                {
                    var pathg = Path.Combine(pathToSave, "nuevo.txt");
                    using (var stream = new FileStream(pathg, FileMode.Create))
                    {

                    }

                    using (var stream = new FileStream(fullPath, FileMode.Open))
                    {
                        using (BinaryWriter wr = new BinaryWriter(stream))
                        {
                            using (BinaryReader br = new BinaryReader(stream))
                            {
                                var buffer = new byte[100];
                                while (br.BaseStream.Position != br.BaseStream.Length)
                                {
                                    buffer = br.ReadBytes(100); //llenar el buffer
                                    cif.ConvertirAscci(buffer);  //Obtengo los codigos Ascci del buffer
                                    cif.InicioDesCifrado();//inicio el cifrado del buffer   
                                    cif.EscribirArchTXT(pathg);
                                }
                            }
                        }
                        stream.Close();
                    }
                    var stream1 = new FileStream(pathg, FileMode.Open);

                    return File(stream1, System.Net.Mime.MediaTypeNames.Application.Octet, "nuevo.txt");

                }
                else
                {
                    return null;
                }
            }


            else
            {
                return null;
            }
        }



    }
}
