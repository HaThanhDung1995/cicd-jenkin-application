//using DemoCICD.Application.Abstractions.Messages;
using DemoCICD.Contract.Abstractions.Messages;
using FluentAssertions;
using NetArchTest.Rules;

namespace DemoCICD.Architecture.Tests
{
    public class UnitTest1
    {
        private const string DomainNamespace = "DemoCICD.Domain";
        private const string ApplicationNamespace = "DemoCICD.Application";
        private const string InfrastructureNamespace = "DemoCICD.Infrastructure";
        private const string PersistenceNamespace = "DemoCICD.Persitence";
        private const string PresentationNamespace = "DemoCICD.Presentation";
        private const string ApiNamespace = "DemoCICD.API";
        #region =============== Infrastructure Leyer ===============
        [Fact]
        public void Domain_Should_Not_HaveDependencyOnOtherProject()
        {
            // Arrage
            var assembly = Domain.AssemblyReference.Assembly;

            var otherProjects = new[]
            {
            ApplicationNamespace,
            InfrastructureNamespace,
            PersistenceNamespace,
            PresentationNamespace,
            ApiNamespace
        };

            // Act
            var testResult = Types
                .InAssembly(assembly)
                .ShouldNot()
                .HaveDependencyOnAny(otherProjects)
                .GetResult();

            // Assert
            testResult.IsSuccessful.Should().BeTrue();
        }

        [Fact]
        public void Application_Should_Not_HaveDependencyOnOtherProjects()
        {
            // Arrange
            var assembly = Application.AssemblyReference.Assembly;

            var otherProjects = new[]
            {
            InfrastructureNamespace,
            //PersistenceNamespace, // Due to Implement sort multi columns by apply RawQuery with EntityFramework
            PresentationNamespace,
            ApiNamespace
        };

            // Act
            var testResult = Types
                .InAssembly(assembly)
                .ShouldNot()
                .HaveDependencyOnAny(otherProjects)
                .GetResult();

            // Assert
            testResult.IsSuccessful.Should().BeTrue();
        }

        [Fact]
        public void Infrastructure_Should_Not_HaveDependencyOnOtherProjects()
        {
            // Arrange
            var assembly = Infrastructure.AssemblyReference.Assembly;

            var otherProjects = new[]
            {
            PresentationNamespace,
            ApiNamespace
        };

            // Act
            var testResult = Types
                .InAssembly(assembly)
                .ShouldNot()
                .HaveDependencyOnAny(otherProjects)
                .GetResult();

            // Assert
            testResult.IsSuccessful.Should().BeTrue();
        }

        [Fact]
        public void Persistence_Should_Not_HaveDependencyOnOtherProjects()
        {
            // Arrange
            var assembly = Persitence.AssemblyReference.Assembly;

            var otherProjects = new[]
            {
            ApplicationNamespace,
            InfrastructureNamespace,
            PresentationNamespace,
            ApiNamespace
        };

            // Act
            var testResult = Types
                .InAssembly(assembly)
                .ShouldNot()
                .HaveDependencyOnAny(otherProjects)
                .GetResult();

            // Assert
            testResult.IsSuccessful.Should().BeTrue();
        }

        [Fact]
        public void Presentation_Should_Not_HaveDependencyOnOtherProjects()
        {
            // Arrange
            var assembly = Presentation.AssemblyReference.Assembly;

            var otherProjects = new[]
            {
            InfrastructureNamespace,
            ApiNamespace,
            PersistenceNamespace
        };

            // Act
            var testResult = Types
                .InAssembly(assembly)
                .ShouldNot()
                .HaveDependencyOnAny(otherProjects)
                .GetResult();

            // Assert
            testResult.IsSuccessful.Should().BeTrue();
        }

        #endregion =============== Infrastructure Layer ===============

        #region
        [Fact]
        public void Command_Should_Have_NamingConventionCommand()
        {
            //Arrange
            var assembly = Application.AssemblyReference.Assembly;
            //Act
            var testResult = Types
                .InAssembly(assembly)
                .That()
                .ImplementInterface(typeof(ICommand))
                .Should()
                .HaveNameEndingWith("Command")
                .GetResult();
            //Assert
            testResult.IsSuccessful.Should().BeTrue();
        }
        [Fact]
        public void Command_Should_Have_NamingConventionCommandForResponse()
        {
            //Arrange
            var assembly = Application.AssemblyReference.Assembly;
            //Act
            var testResult = Types
                .InAssembly(assembly)
                .That()
                .ImplementInterface(typeof(ICommand<>))
                .Should()
                .HaveNameEndingWith("Command")
                .GetResult();
            //Assert
            testResult.IsSuccessful.Should().BeTrue();
        }
        [Fact]
        public void Command_Handler_Should_Have_NamingConventionCommandHandler()
        {
            //Arrange
            var assembly = Application.AssemblyReference.Assembly;
            //Act
            var testResult = Types
                .InAssembly(assembly)
                .That()
                .ImplementInterface(typeof(ICommandHandler<>))
                .Should()
                .HaveNameEndingWith("CommandHandler")
                .GetResult();
            //Assert
            testResult.IsSuccessful.Should().BeTrue();
        }
        [Fact]
        public void Command_Handler_Should_Have_NamingConventionCommandHandlerResponse()
        {
            //Arrange
            var assembly = Application.AssemblyReference.Assembly;
            //Act
            var testResult = Types
                .InAssembly(assembly)
                .That()
                .ImplementInterface(typeof(ICommandHandler<,>))
                .Should()
                .HaveNameEndingWith("CommandHandler")
                .GetResult();
            //Assert
            testResult.IsSuccessful.Should().BeTrue();
        }

        [Fact]
        public void Query_Should_Have_NamingConventionCommand()
        {
            //Arrange
            var assembly = Application.AssemblyReference.Assembly;
            //Act
            var testResult = Types
                .InAssembly(assembly)
                .That()
                .ImplementInterface(typeof(IQuery<>))
                .Should()
                .HaveNameEndingWith("Query")
                .GetResult();
            //Assert
            testResult.IsSuccessful.Should().BeTrue();
        }

        [Fact]
        public void Query_Handler_Should_Have_NamingConventionCommandHandler()
        {
            //Arrange
            var assembly = Application.AssemblyReference.Assembly;
            //Act
            var testResult = Types
                .InAssembly(assembly)
                .That()
                .ImplementInterface(typeof(IQueryHandler<,>))
                .Should()
                .HaveNameEndingWith("QueryHandler")
                .GetResult();
            //Assert
            testResult.IsSuccessful.Should().BeTrue();
        }

        #endregion
    }
}