using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherForcastContractTests;

public class VerifyChecksTests
{
    [Fact]
    public Task Run() => VerifyChecks.Run();
}
