using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PactNet;

namespace ProvidersPactStates;

public sealed class ProviderStateExtend : ProviderState
{
    public ProviderStateExtend(string provider, string state, string description)
    {
        Provider = provider;
        State = state;
        Description = description;
    }

    public string Description { get; init; }
    public string Provider { get; init; }
}
