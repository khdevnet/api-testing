using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using LightBDD.Framework.Scenarios;
using LightBDD.XUnit2;

namespace OrderApi.ComponentTests.Features.Common;

public class Base_feature : FeatureFixture
{
    protected Task RunScenarioAsync<TSteps>(params Expression<Func<TSteps, Task>>[] steps)
       => Runner.WithContext<TSteps>()
                .RunScenarioAsync(steps);
}
