using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProvidersPactStates;

public record ProviderStateFunc(string State, Dictionary<string, string> Parameters) { }
