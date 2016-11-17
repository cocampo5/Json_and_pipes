using System;
using System.Text;
using System.IO.Pipes;
using Pipes.Structura;
using Newtonsoft.Json;

namespace PipeCliente
{
	class Program
	{
		static void Main(string[] args)
		{
			//Creo una muerte
			InfoMuerte inf = new InfoMuerte();
			inf.id = "Muerte Sencilla por exceso de alcohol";
			inf.nDecesos = 1;

			//Creo otra muerte
			InfoMuerte inf2 = new InfoMuerte();
			inf2.id = "Muerte por friendzone";
			inf2.nDecesos = 9000;

			//Hago un array de muertes
			InfoMuerte[] muerteArray = new InfoMuerte[2];
			muerteArray[0] = inf;
			muerteArray[1] = inf2;

			//Creo la memoria compartida desde la estructura
			MemoriaCompartida mem = new MemoriaCompartida();
			mem.n = 0;
			mem.valSeq = 556391;
			mem.muertes = muerteArray;

			//Convierto a json
			var json = JsonConvert.SerializeObject(mem);

			//Console.Write(json);

			Program p = new Program();
			p.sendJsonThroughPipe(json);
			p.normalPipe();
		}

		/**
		 * Este metodo envia el json a traves del pipe
		**/
		void sendJsonThroughPipe(string json_to_send)
		{
				var clienteStream = new NamedPipeClientStream("Json_Pipe");
				clienteStream.Connect(60);
				Console.WriteLine("Mandando json con info...");
				byte[] buffer = ASCIIEncoding.ASCII.GetBytes(json_to_send);
				clienteStream.Write(buffer, 0, buffer.Length);
				clienteStream.Close();
		}

		/**
		 * Este metodo es el funcionamiento normal del pipe esperando 
		 * que se escriba en la consola
		**/
		void normalPipe()
		{
			while (true)
			{
				var clienteStream = new NamedPipeClientStream("Normal_Pipe");
				clienteStream.Connect(60);
				Console.WriteLine("Escribe un Mensaje");
				string line = Console.ReadLine();

				byte[] buffer = ASCIIEncoding.ASCII.GetBytes(line);
				clienteStream.Write(buffer, 0, buffer.Length);
				clienteStream.Close();

				if (line.ToLower() == "exit")
					break;
			}
		
		}

	}
}

