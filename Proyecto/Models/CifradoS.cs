using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto.Models
{
    public class CifradoS
    {
        int[] P10 = new int[10];
        int[] P8 = new int[8];
        int[] P4 = new int[4];
        int[] Ep = new int[8];
        int[] Ip = new int[8];
        int[] Swap = new int[8];
        int[] Ip_1 = new int[8];
        string Key = string.Empty; //clave ingresada por el usuario
        public string K1 = string.Empty;
        public string K2 = string.Empty;
        string[,] S0 = new string[4, 4] { { "01", "00", "11", "10" }, { "11", "10", "01", "00" }, { "00", "10", "01", "11" }, { "11", "01", "11", "10" } };
        string[,] S1 = new string[4, 4] { { "00", "01", "10", "11" }, { "10", "00", "01", "11" }, { "11", "00", "01", "00" }, { "10", "01", "00", "11" } };
        public byte[] escribir;
        string auxiliar = string.Empty;
        List<int> codigos_as = new List<int>();


        //public void CrearArchTXT()
        //{
        //    string path = System.Web.Hosting.HostingEnvironment.MapPath("~/Cifrados/");

        //    if (!Directory.Exists(path))
        //    {
        //        Directory.CreateDirectory(path);
        //    }

        //    string filePath = path + "Archivo.txt";
        //    FileStream fs = File.Create(filePath);
        //    fs.Close();
        //}
        public void EscribirArchTXT(string path) //escribimos por buffers
        {
            
         
            using (var stream = new FileStream(path, FileMode.Append))
            {
                using (BinaryWriter wr = new BinaryWriter(stream))
                {
                    wr.Write(escribir);
                }
                stream.Close();
            }
            escribir = new byte[0];
        }
        //public void CrearArchSCIF()
        //{
        //    string path = System.Web.Hosting.HostingEnvironment.MapPath("~/Cifrados/");

        //    if (!Directory.Exists(path))
        //    {
        //        Directory.CreateDirectory(path);
        //    }

        //    string filePath = path + "Archivo.scif";
        //    FileStream fs = File.Create(filePath);
        //    fs.Close();
        //}
        public void EscribirArch(string path) //escribimos por buffers
        {
           
            string filePath = path + "Archivo.scif";
            using (var stream = new FileStream(path, FileMode.Append))
            {
                using (BinaryWriter wr = new BinaryWriter(stream))
                {
                    wr.Write(escribir);
                }
                stream.Close();
            }
            escribir = new byte[0];
        }

        public void InicioDesCifrado()
        {
            escribir = new byte[0];
            for (int x = 0; x < codigos_as.Count(); x++)
            {
                DecimalaBinario(codigos_as[x], 8);
                Cifrar(K2, K1, auxiliar);
            }
        }

        public void InicioCifrado()
        {
            escribir = new byte[0];
            for (int x = 0; x < codigos_as.Count(); x++)
            {
                DecimalaBinario(codigos_as[x], 8);
                Cifrar(K1, K2, auxiliar);
            }
        }
        public void DecimalaBinario(int deci, int tam) //convierte el decimal enviado a un binario
        {
            string binario = string.Empty;
            int residuo = 0;
            for (int x = 0; deci > 1; x++)
            {
                residuo = deci % 2;
                deci = deci / 2;
                binario = residuo.ToString() + binario;
            }
            if (deci == 1)
            {
                binario = deci.ToString() + binario;
            }
            int au = binario.Length;
            if (au != tam)
            {
                for (int d = 0; d < (tam - au); d++)
                {
                    binario = '0' + binario;
                }
            }
            if (tam == 10)
            {
                Key = binario;
            }
            else if (tam == 8)
            {
                auxiliar = binario;
            }
        }
        public void LeerPermutaciones(string path) //lectura del archivo con las permutaciones
        {

            string filePath = path + "Permutaciones.txt";
            using (var stream = new FileStream(filePath, FileMode.Open))
            {

                using (BinaryReader br = new BinaryReader(stream))
                {
                    var buffer = new byte[54];
                    buffer = br.ReadBytes(54); //llenar el buffer
                    LlenarPermutaciones(buffer);

                }

                stream.Close();

            }
        }
        public void LlenarPermutaciones(byte[] buffer)
        {
            string b = Encoding.ASCII.GetString(buffer);
            int x = 0;
            char val;

            for (int y = 0; y < 10; y++) //llenando p10
            {
                val = Convert.ToChar(b[x]);
                P10[y] = Convert.ToInt32(val - '0');
                x++;
            }
            for (int y = 0; y < 8; y++) //llenando p8
            {
                val = Convert.ToChar(b[x]);
                P8[y] = Convert.ToInt32(val - '0');
                x++;
            }
            for (int y = 0; y < 4; y++) //llenando p4
            {
                val = Convert.ToChar(b[x]);
                P4[y] = Convert.ToInt32(val - '0');
                x++;
            }
            for (int y = 0; y < 8; y++) //llenando ep
            {
                val = Convert.ToChar(b[x]);
                Ep[y] = Convert.ToInt32(val - '0');
                x++;
            }
            for (int y = 0; y < 8; y++) //llenando ip
            {
                val = Convert.ToChar(b[x]);
                Ip[y] = Convert.ToInt32(val - '0');
                x++;
            }
            for (int y = 0; y < 8; y++) //llenando swap
            {
                val = Convert.ToChar(b[x]);
                Swap[y] = Convert.ToInt32(val - '0');
                x++;
            }
            for (int y = 0; y < 8; y++) //llenando ip_1
            {
                val = Convert.ToChar(b[x]);
                Ip_1[y] = Convert.ToInt32(val - '0');
                x++;
            }
        }
        public void GenerarLLaves()
        {
            string aux = string.Empty; //almacena la llave con p10
            for (int x = 0; x < 10; x++)
            {
                aux = aux + Key[P10[x]];
            }

            Key = string.Empty; //Almacenara el aux como LS1
            for (int x = 1; x < 5; x++)
            {
                Key = Key + aux[x];
            }
            Key = Key + aux[0];
            for (int x = 6; x < 10; x++)
            {
                Key = Key + aux[x];
            }
            Key = Key + aux[5];

            //Generar K1, P8 a Key
            for (int x = 0; x < 8; x++)
            {
                K1 = K1 + Key[P8[x]];
            }

            aux = string.Empty; //Almacenara el Key con LS2
            for (int x = 2; x < 5; x++)
            {
                aux = aux + Key[x];
            }
            aux = aux + Key[0] + Key[1];
            for (int x = 7; x < 10; x++)
            {
                aux = aux + Key[x];
            }
            aux = aux + Key[5] + Key[6];

            //Generar K2, P8 a aux
            for (int x = 0; x < 8; x++)
            {
                K2 = K2 + aux[P8[x]];
            }

        }
        public void ConvertirAscci(byte[] buffer)
        {
            codigos_as = new List<int>();
            for (int x = 0; x < buffer.Length; x++)
            {
                codigos_as.Add(Convert.ToInt32(buffer[x]));
            }
        }
        public void Cifrar(string ke1, string ke2, string val)
        {
            string aux1 = string.Empty; //Almacenara IP(0 a 3) de val
            string aux2 = string.Empty; //Almacena IP(4 a 7) de val
            for (int x = 0; x < 4; x++)
            {
                aux1 = aux1 + val[Ip[x]];
            }
            for (int x = 4; x < 8; x++)
            {
                aux2 = aux2 + val[Ip[x]];
            }

            val = string.Empty; //Almacena EP con los ultimos 4 digitos de IP
            for (int x = 0; x < 8; x++)
            {
                val = val + aux2[Ep[x]];
            }
            string xor = string.Empty; //Almacena XOR entre EP(val) y k1
            for (int x = 0; x < 8; x++)
            {
                xor = xor + (val[x] ^ ke1[x]).ToString("X");
            }

            string box = string.Empty;//Almacena los valores de las Sbox
            int fila = (xor[0] - '0') * 2 + (xor[3] - '0'); //Calculo posicones para Sb0
            int col = (xor[1] - '0') * 2 + (xor[2] - '0');
            box = S0[fila, col];
            fila = (xor[4] - '0') * 2 + (xor[7] - '0'); //Calculo posiciones para Sb1
            col = (xor[5] - '0') * 2 + (xor[6] - '0');
            box = box + S1[fila, col];

            val = string.Empty; //Almacena los valores de P4 de box
            for (int x = 0; x < 4; x++)
            {
                val = val + box[P4[x]];
            }

            xor = string.Empty;// Almacena XOR entre IP[0-3](aux1) con P4(val)
            for (int x = 0; x < 4; x++)
            {
                xor = xor + (val[x] ^ aux1[x]).ToString("X");
            }

            val = string.Empty;//Almacena la union entre xor con IP[4-7](aux2)
            val = xor + aux2;

            aux1 = string.Empty;//Almacena  SWAP[0-3]
            aux2 = string.Empty; //Almacena  [4-7]
            for (int x = 0; x < 4; x++)
            {
                aux1 = aux1 + val[Swap[x]];
            }
            for (int x = 4; x < 8; x++)
            {
                aux2 = aux2 + val[Swap[x]];
            }

            val = string.Empty; //Almacena EP de Swap[4-7](aux2)
            for (int x = 0; x < 8; x++)
            {
                val = val + aux2[Ep[x]];
            }

            xor = string.Empty; //Almacena el XOR de Ep(val) con k2
            for (int x = 0; x < 8; x++)
            {
                xor = xor + (val[x] ^ ke2[x]).ToString("X");
            }

            box = string.Empty;//Almacena los valores de las Sbox
            fila = (xor[0] - '0') * 2 + (xor[3] - '0'); //Calculo posicones para Sb0
            col = (xor[1] - '0') * 2 + (xor[2] - '0');
            box = S0[fila, col];
            fila = (xor[4] - '0') * 2 + (xor[7] - '0'); //Calculo posiciones para Sb1
            col = (xor[5] - '0') * 2 + (xor[6] - '0');
            box = box + S1[fila, col];

            val = string.Empty; //Almacena los valores de P4 de box
            for (int x = 0; x < 4; x++)
            {
                val = val + box[P4[x]];
            }

            xor = string.Empty; //Almacena el Xor de P4(val) con Swap[0-3](aux1)
            for (int x = 0; x < 4; x++)
            {
                xor = xor + (val[x] ^ aux1[x]).ToString("X");
            }

            val = string.Empty; //Almacena la union entre Xor y Ip[4-7](aux2)
            val = xor + aux2;

            char[] resultado = new char[8]; //Almacena el resultado del proceso
            for (int x = 0; x < 8; x++)
            {
                resultado[x] = val[Ip_1[x]];
            }

            Cod_Decimal(resultado);
        }

        public void Cod_Decimal(char[] binario) //convertir los binarios a decimales
        {
            int valor_decimal = 0; // valor del decimal a convertir
            for (int c = 7; c >= 0; c--)
            {
                int d = 7 - c;
                double v = Convert.ToDouble(binario[d].ToString()) * Math.Pow(2, c);
                valor_decimal = valor_decimal + Convert.ToInt32(v);

            }
            Cod_ASCII(valor_decimal); //enviar el decimal
        }
        public void Cod_ASCII(int dec) //decimal a ascci
        {
            int tamactual = 0;
            char asc;
            if (escribir != null)
            {
                tamactual = escribir.Length;
                byte[] aux = new byte[tamactual];
                System.Buffer.BlockCopy(escribir, 0, aux, 0, tamactual);
                byte bit = Convert.ToByte(dec);
                asc = Convert.ToChar(bit);
                escribir = new byte[tamactual + 1];
                System.Buffer.BlockCopy(aux, 0, escribir, 0, tamactual);
            }
            else
            {
                byte bit = Convert.ToByte(dec);
                asc = Convert.ToChar(bit);
                escribir = new byte[tamactual + 1];
            }

            escribir[tamactual] = Convert.ToByte(asc);

            // agrego el ascci a el buffer de escritura


        }
    }
}
