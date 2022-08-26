using FluentAssertions;
using MicroApplicationFramework;
using Moq;
using Serilog;
using Serilog.Sinks.TestCorrelator;

namespace MicroApplicationFrameworkTests
{
    public class TestApplication : Application
    {
        public override void OnRegister()
        {
            // Ignored
        }

        public override void OnInit()
        {
            // Ignored
        }

        public override void OnExecute()
        {
            ApplicationContext.RequestCancel();
        }

        public override void OnExit()
        {
            // Ignored
        }
    }

    public class NoRequestCancelApplication : Application
    {
        public override void OnRegister()
        {
            // Ignored
        }

        public override void OnInit()
        {
            // Ignored
        }

        public override void OnExecute()
        {
            // Ignored
        }

        public override void OnExit()
        {
            // Ignored
        }
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
                mockApplication.Verify(app => app.OnExit(), Times.Once);
                TestCorrelator.GetLogEventsFromCurrentContext().Should().BeEmpty();
            }
        }

        [Fact]
        public void OnExitIsAlwaysCalledFromApplicationIfOnRegisterThrowsException()
        {
            var mockApplication = new Mock<TestApplication>();
            mockApplication.Setup(app => app.OnRegister()).Throws(new Exception("Any Exception By Init"));
            mockApplication.CallBase = true;

            using (InitLoggerContext())
            {
                Bootstrapper.Create(mockApplication.Object).Run();
                mockApplication.Verify(app => app.OnRegister(), Times.Once);
                mockApplication.Verify(app => app.OnInit(), Times.Never);
                mockApplication.Verify(app => app.OnExecute(), Times.Never);
                mockApplication.Verify(app => app.OnExit(), Times.Once);
                TestCorrelator.GetLogEventsFromCurrentContext()
                    .Should().ContainSingle()
                    .Which.MessageTemplate.Text
                    .Should().Be("Exception called");
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
                mockApplication.Verify(app => app.OnExit(), Times.Once);
                TestCorrelator.GetLogEventsFromCurrentContext()
                    .Should().ContainSingle()
                    .Which.MessageTemplate.Text
                    .Should().Be("Exception called");
            }
        }

        [Fact]
        public void OnExitIsAlwaysCalledFromApplicationIfOnExecuteThrowsException()
        {
            var mockApplication = new Mock<TestApplication>();
            mockApplication.Setup(app => app.OnExecute()).Throws(new Exception("Any Exception By Init"));
            mockApplication.CallBase = true;

            using (InitLoggerContext())
            {
                Bootstrapper.Create(mockApplication.Object).Run();
                mockApplication.Verify(app => app.OnRegister(), Times.Once);
                mockApplication.Verify(app => app.OnInit(), Times.Once);
                mockApplication.Verify(app => app.OnExecute(), Times.Once);
                mockApplication.Verify(app => app.OnExit(), Times.Once);
                TestCorrelator.GetLogEventsFromCurrentContext()
                    .Should().ContainSingle()
                    .Which.MessageTemplate.Text
                    .Should().Be("Exception called");
            }
        }

        [Fact]
        public void TimeoutIsCalledIfReached()
        {
            var mockApplication = new NoRequestCancelApplication();
            mockApplication.ApplicationContext.Timeout = 5000;

            using (InitLoggerContext())
            {
                Bootstrapper.Create(mockApplication).Run();
                TestCorrelator.GetLogEventsFromCurrentContext()
                    .Should().ContainSingle()
                    .Which.MessageTemplate.Text
                    .Should().Be("Timeout reached...application will be canceled");
            }
        }

        private static ITestCorrelatorContext InitLoggerContext()
        {
            Log.Logger = new LoggerConfiguration().WriteTo.TestCorrelator().CreateLogger();
            return TestCorrelator.CreateContext();
        }
    }
}