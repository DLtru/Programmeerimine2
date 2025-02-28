using KooliProjekt.Data;

namespace KooliProjekt.UnitTests.ControllerTests
{
    internal class BeersIndexModel
    {
        public PagedResult<Beer> Data { get; internal set; }
    }
}