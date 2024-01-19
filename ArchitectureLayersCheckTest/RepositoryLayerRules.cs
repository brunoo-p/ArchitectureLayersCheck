using ArchitectureLayersCheck.Application;
using ArchitectureLayersCheck.Domain;
using ArchitectureLayersCheck.Infrastructure;
using ArchUnitNET.Domain;
using ArchUnitNET.Fluent;
using ArchUnitNET.Loader;
using ArchUnitNET.xUnit;

using static ArchUnitNET.Fluent.ArchRuleDefinition;

namespace ArchitectureLayersCheckTest
{
    public class RepositoryLayerRules
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

        [Fact(DisplayName = "[Layers] Should test layers integrity")]
        public void Repository_Layer_Rules()
        {
            var applicationLayer = Classes().That().ResideInNamespace(BuildNamespace("Application")).As("Application Layer");
            var domainLayer = Classes().That().ResideInNamespace(BuildNamespace("Domain")).As("Domain Layer");
            var infrastructureLayer = Classes().That().ResideInNamespace(BuildNamespace("Infrastructure")).As("Infrastructure Layer");

            IArchRule applicationLayerDependencies = Classes().That()
                        .Are(applicationLayer).Should().NotDependOnAny(infrastructureLayer)
                        .Because("only the domain service can access infrastructure layer");

            IArchRule domainLayerDependencies = Classes().That()
                        .Are(domainLayer).Should().NotDependOnAny(infrastructureLayer)
                        .Because("infringement the domain rules")
                        .AndShould().NotDependOnAny(applicationLayer)
                        .Because("infringement the domain rules");

            IArchRule infrastructureLayerDependencies = Classes().That()
                        .Are(infrastructureLayer).Should().NotDependOnAny(applicationLayer)
                        .Because("infrastructure cannot know about application layer existence");

            IArchRule combinedRepositoryRules =
                        applicationLayerDependencies
                            .And(domainLayerDependencies)
                            .And(infrastructureLayerDependencies);

            combinedRepositoryRules.Check(_architecture);
        }
    }
}
