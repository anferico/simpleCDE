using System.Threading;
using System.Collections.Generic;
using System.IO;
using System;
namespace Progetto {
	class MainClass {
		public static void Main(string[] args) {
			var cde = new CDE();
			cde.DeployApplication("spec.yaml");
		}
	}
}