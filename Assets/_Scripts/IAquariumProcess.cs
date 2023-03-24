using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public interface IAquariumProcess
{
    public void DoProcess(Dictionary<Parameter, float> parameters);
}