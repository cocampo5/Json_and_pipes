using System;
using System.Text;
using System.IO.Pipes;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Pipes.Structura;

namespace PipeServer
{
	class Program
	{
		static void Main(string[] args)
		{
			Program server = new Program();
			server.jsonPipeEntry();
			server.normalPipeEntry();
		}

		/**
		 * Este metodo recibe escritura desde la consola cliente
		**/
		void normalPipeEntry()
		{
			Console.WriteLine("Esperando conexión");
			while (true)
			{
				var namedPipeServerStream = new NamedPipeServerStream("Normal_Pipe");
				namedPipeServerStream.WaitForConnection();
				byte[] buffer = new byte[255];
				namedPipeServerStream.Read(buffer, 0, 255);

				string request = ASCIIEncoding.ASCII.GetString(buffer);
				Console.WriteLine(request);
				request = request.Trim('\0');
				if (request == "exit")
					break;
				namedPipeServerStream.Close();

			}
			Console.WriteLine("Me mandaron a salir");
			//Console.ReadLine();
		}

		/**
		 * Este metodo recibe un string lo convierte en un json 
		 * y lo deserializa para imprimirlo
		**/
		void jsonPipeEntry()
		{
			var namedPipeServerStream = new NamedPipeServerStream("Json_Pipe");
			namedPipeServerStream.WaitForConnection();
			byte[] buffer = new byte[255];
			namedPipeServerStream.Read(buffer, 0, 255);
			string request = ASCIIEncoding.ASCII.GetString(buffer);
			request = request.Trim('\0');
			namedPipeServerStream.Close();

			JObject json = JObject.Parse(request);
			//Console.WriteLine("valSeq recibido es: "+json.GetValue("valSeq"));
			//Console.WriteLine("muertes recibidas son: " + json.GetValue("muertes"));
			//DTO_InfoMuerte[] arr = JObject.Parse(request)["muertes"].ToObject<DTO_InfoMuerte[]>();
			//Console.WriteLine(arr[0].id+arr[1].id);
			var output = JsonConvert.SerializeObject(json, Formatting.Indented);
			Console.WriteLine(output);

		}
	}
}

