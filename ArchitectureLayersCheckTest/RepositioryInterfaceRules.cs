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
    public class RepositioryInterfaceRules
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

        [Fact(DisplayName = "[Interface - Repository] Should test interface and Repositories integrity")]
        public void Repository_Entity_Test()
        {
            IArchRule repositorySufix = Classes()
                        .That().HaveNameContaining("Repository")
                        .Should().ResideInNamespace("^(.*?)\\.Repositories", true)
                        .And()
                            .Classes()
                                .That().ResideInNamespace("^(.*?)\\.Repositories", true)
                                .Should().HaveNameEndingWith("Repository")
                        .Because("was an architectural decision");

            IArchRule combinedRepositoryRules =
                        repositorySufix;

            combinedRepositoryRules.Check(_architecture);
        }
    }
}
