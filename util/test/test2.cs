using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace test
{
	class test2
	{
		static void Main(string[] args)
		{
			new test2().a();
			Console.WriteLine(4);
			Console.ReadLine();
		}

		void write(object o)
		{
			Console.WriteLine();
		}

		async void a()
		{
			Console.WriteLine(1);
			await Task.Run((Action)b);
			Console.WriteLine(2);
		}

		void b()
		{
			Task.Delay(3000);
			Console.WriteLine(3);
		}
	}
}
