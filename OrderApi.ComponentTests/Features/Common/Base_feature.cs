using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using LightBDD.Framework.Scenarios;
using LightBDD.XUnit2;

namespace OrderApi.ComponentTests.Features.Common;

public abstract class Base_feature : FeatureFixture
{
    protected async Task RunScenarioAsync<TSteps>(params Expression<Func<TSteps, Task>>[] steps)
        => await Runner.WithContext<TSteps>()
               .RunScenarioAsync(steps);
}
