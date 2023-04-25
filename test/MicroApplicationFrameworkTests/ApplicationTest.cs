using DryIoc;
using FluentAssertions;
using MicroApplicationFramework;
using MicroApplicationFrameworkExample;
using MicroApplicationFrameworkExample.Interface;
using Moq;
using Serilog;
using Serilog.Sinks.TestCorrelator;

namespace MicroApplicationFrameworkTests
{
    public class TestApplication : Application
    {
    }
    
    public class ApplicationTest
    {
        [Fact]
        public void ApplicationShouldExecuteAllLifeCycleMethodsOnce()
        {
            var mockApplication = new Mock<TestApplication>
            {
                CallBase = true
            };
            using (InitLoggerContext())
            {
                Bootstrapper.Create(mockApplication.Object).Run();
                mockApplication.Verify(app => app.OnInit(), Times.Once);
                mockApplication.Verify(app => app.OnRegister(), Times.Once);
                mockApplication.Verify(app => app.OnExecute(), Times.Once);
                mockApplication.Verify(app => app.OnExecuteAsync(), Times.Once);
                mockApplication.Verify(app => app.OnExit(), Times.Once);
                TestCorrelator.GetLogEventsFromCurrentContext().Should().BeEmpty();
            }
        }

        [Fact]
        public void OnExitIsAlwaysCalledFromApplicationIfOnRegisterThrowsException()
        {
            var mockApplication = new Mock<TestApplication>();
            mockApplication.Setup(app => app.OnRegister()).Throws(new Exception("Any Exception By Register"));
            mockApplication.CallBase = true;

            using (InitLoggerContext())
            {
                Bootstrapper.Create(mockApplication.Object).Run();
                mockApplication.Verify(app => app.OnRegister(), Times.Once);
                mockApplication.Verify(app => app.OnInit(), Times.Never);
                mockApplication.Verify(app => app.OnExecute(), Times.Never);
                mockApplication.Verify(app => app.OnExecuteAsync(), Times.Never);
                mockApplication.Verify(app => app.OnExit(), Times.Once);

                var logs = TestCorrelator.GetLogEventsFromCurrentContext().ToArray();
                logs.Should().Contain(log => log.MessageTemplate.Text == "Exception called" &&
                                             log.Exception != null
                                             && log.Exception.Message == "Any Exception By Register");
            }
        }

        [Fact]
        public void OnExitIsAlwaysCalledFromApplicationIfOnInitThrowsException()
        {
            var mockApplication = new Mock<TestApplication>();
            mockApplication.Setup(app => app.OnInit()).Throws(new Exception("Any Exception By Init"));
            mockApplication.CallBase = true;

            using (InitLoggerContext())
            {
                Bootstrapper.Create(mockApplication.Object).Run();
                mockApplication.Verify(app => app.OnRegister(), Times.Once);
                mockApplication.Verify(app => app.OnInit(), Times.Once);
                mockApplication.Verify(app => app.OnExecute(), Times.Never);
                mockApplication.Verify(app => app.OnExecuteAsync(), Times.Never);
                mockApplication.Verify(app => app.OnExit(), Times.Once);

                var logs = TestCorrelator.GetLogEventsFromCurrentContext().ToArray();
                logs.Should().Contain(log => log.MessageTemplate.Text == "Exception called" && 
                                             log.Exception != null 
                                             && log.Exception.Message == "Any Exception By Init");

            }
        }

        [Fact]
        public void OnExitIsAlwaysCalledFromApplicationIfOnExecuteThrowsException()
        {
            var mockApplication = new Mock<TestApplication>();
            mockApplication.Setup(app => app.OnExecute()).Throws(new Exception("Any Exception By OnExecute"));
            mockApplication.CallBase = true;

            using (InitLoggerContext())
            {
                Bootstrapper.Create(mockApplication.Object).Run();
                mockApplication.Verify(app => app.OnRegister(), Times.Once);
                mockApplication.Verify(app => app.OnInit(), Times.Once);
                mockApplication.Verify(app => app.OnExecute(), Times.Once);
                mockApplication.Verify(app => app.OnExecuteAsync(), Times.Never);
                mockApplication.Verify(app => app.OnExit(), Times.Once);

                var logs = TestCorrelator.GetLogEventsFromCurrentContext().ToArray();
                logs.Should().Contain(log => log.MessageTemplate.Text == "Exception called" &&
                                             log.Exception != null
                                             && log.Exception.Message == "Any Exception By OnExecute");
            }
        }
        
        [Fact]
        public void OnExitIsAlwaysCalledFromApplicationIfOnExecuteAsyncThrowsException()
        {
            var mockApplication = new Mock<TestApplication>();
            mockApplication.Setup(app => app.OnExecuteAsync()).Throws(new Exception("Any Exception By OnExecuteAsync"));
            mockApplication.CallBase = true;

            using (InitLoggerContext())
            {
                Bootstrapper.Create(mockApplication.Object).Run();
                mockApplication.Verify(app => app.OnRegister(), Times.Once);
                mockApplication.Verify(app => app.OnInit(), Times.Once);
                mockApplication.Verify(app => app.OnExecute(), Times.Once);
                mockApplication.Verify(app => app.OnExecuteAsync(), Times.Once);
                mockApplication.Verify(app => app.OnExit(), Times.Once);

                var logs = TestCorrelator.GetLogEventsFromCurrentContext().ToArray();
                logs.Should().Contain(log => log.MessageTemplate.Text == "Exception called" &&
                                             log.Exception != null
                                             && log.Exception.Message == "Any Exception By OnExecuteAsync");
            }
        }

        [Fact] 
        public void ContainerElementCanBeOverrideToMockResults()
        {
            using (InitLoggerContext())
            {
                var app = new App();

                app.OnRegisterFinishedEventHandler += container =>
                {
                    // Replace service results by mocking
                    var mockModule = new Mock<IModule>();
                    mockModule.Setup(mock => mock.Foo()).Returns("mockbar");
                    var mockBModule = new Mock<IModuleB>();
                    mockBModule.Setup(mock => mock.Bar()).Returns("mockfoo");

                    container.RegisterInstance(mockModule.Object, IfAlreadyRegistered.Replace);
                    container.RegisterInstance(mockBModule.Object, IfAlreadyRegistered.Replace);
                };

                Bootstrapper.Create(app).Run();

                var logs = TestCorrelator.GetLogEventsFromCurrentContext().ToArray();
                logs.Should().Contain(log => log.MessageTemplate.Text == "mockbar");
                logs.Should().Contain(log => log.MessageTemplate.Text == "mockfoo");
            }
        }

        private static ITestCorrelatorContext InitLoggerContext()
        {
            Log.Logger = new LoggerConfiguration().WriteTo.TestCorrelator().CreateLogger();
            return TestCorrelator.CreateContext();
        }
    }
}