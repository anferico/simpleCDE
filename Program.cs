using System.Threading;
using System.Collections.Generic;
using System.IO;
using System;

namespace simpleCDE 
{
    class MainClass 
    {
        public static void Main(string[] args) 
        {
            var cde = new EnhancedCDE();
            cde.DeployApplication("spec.yaml");

            // Update the services by providing a new specification
            cde.UpdateSpecification("spec2.yaml");
        }
    }
}