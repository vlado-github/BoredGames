using Orleans;
using Xunit.Gherkin.Quick;

namespace BoredGames.Server.Tests.BDD.Base;

public class BDDDefinitionsBase : Feature
{
    protected readonly IGrainFactory _grainFactory;
    
    public BDDDefinitionsBase(IGrainFactory grainFactory)
    {
        _grainFactory = grainFactory;
    }
    
}