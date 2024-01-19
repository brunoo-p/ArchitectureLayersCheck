using ArchitectureLayersCheck.Application;
using ArchitectureLayersCheck.Domain;
using ArchitectureLayersCheck.Infrastructure;
using ArchUnitNET.Domain;
using ArchUnitNET.Fluent;
using ArchUnitNET.Fluent.Slices;
using ArchUnitNET.Loader;
using ArchUnitNET.xUnit;

using static ArchUnitNET.Fluent.ArchRuleDefinition;

namespace ArchitectureLayersCheckTest
{
    public class RepositoryEntityRules
    {
        private readonly string projectName = "ArchitectureLayersCheck";
        private string BuildNamespace( string contextName ) => $"{projectName}.{contextName}";

        private static readonly Architecture _architecture =
            new ArchLoader()
                .LoadAssemblies(
                    typeof(ApplicationService).Assembly,
                    typeof(DomainService).Assembly,
                    typeof(DataInMemory).Assembly)
                .Build();
        
        [Fact(DisplayName = "[Entity] Should test entity integrity")]
        public void Repository_Entity_Test()
        {
            IArchRule domainEntityRule = Classes()
                        .That().ResideInNamespace($"{BuildNamespace("Domain")}.Entities")
                        .Should().NotBePublic()
                        .Because("they should be internal");

            IArchRule combinedRepositoryRules =
                        domainEntityRule;

            combinedRepositoryRules.Check(_architecture);
        }

    }
}
