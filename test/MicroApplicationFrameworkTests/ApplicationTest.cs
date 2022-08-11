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
            // Ignored
        }

        public override void OnExit()
        {
            // Ignored
        }
    }

    public class AsyncTestApplication : Application
    {
        public bool IsTaskExecuted { get; set; }

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
            ApplicationContext.TaskCollector.Produce(Task.Run(async () =>
            {
                await Task.Delay(500);
                ApplicationContext.TaskCollector.Produce(Task.Run(async () =>
                {
                    await Task.Delay(500);
                    IsTaskExecuted = true;
                }));
            }));
        }

        public override void OnExit()
        {
            // Ignored
        }
    }

    public class AsyncExceptionTestApplication : Application
    {
        public bool IsTaskExecuted { get; set; }

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
            ApplicationContext.TaskCollector.Produce(Task.Run(async () =>
            {
                await Task.Delay(2000);
                IsTaskExecuted = true;
            }));

            ApplicationContext.TaskCollector.Produce(Task.Run(() => throw new Exception("My Task is called a unhandled Exception")));
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
            var mockApplication = new Mock<TestApplication>();
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
        public void AsyncApplicationShouldCleanExitIfApplicationContextRequestCancelIsCalled()
        {
            using (InitLoggerContext())
            {
                var application = new AsyncTestApplication();
                var bootstrapper = Bootstrapper.Create(application);

                Task.Run(async () =>
                {
                    await Task.Delay(500);
                    application.ApplicationContext.RequestCancel();
                });

                bootstrapper.Run();
                application.IsTaskExecuted.Should().BeFalse();
                TestCorrelator.GetLogEventsFromCurrentContext()
                    .Should().ContainSingle()
                    .Which.MessageTemplate.Text
                    .Should().Be("Operation was cancelled by application");
            }
        }

        [Fact]
        public void AsyncApplicationMultipleApplicationContextRequestCancelShouldDoNothing()
        {
            using (InitLoggerContext())
            {
                var application = new AsyncTestApplication();
                var bootstrapper = Bootstrapper.Create(application);

                Task.Run(async () =>
                {
                    await Task.Delay(500);
                    application.ApplicationContext.RequestCancel();
                    application.ApplicationContext.RequestCancel();
                });

                bootstrapper.Run();
                application.IsTaskExecuted.Should().BeFalse();
                TestCorrelator.GetLogEventsFromCurrentContext()
                    .Should().ContainSingle()
                    .Which.MessageTemplate.Text
                    .Should().Be("Operation was cancelled by application");
            }
        }

        [Fact]
        public void AsyncApplicationWaitsUntilAllTasksAreFinished()
        {
            using (InitLoggerContext())
            {
                var application = new AsyncTestApplication();
                var bootstrapper = Bootstrapper.Create(application);
                bootstrapper.Run();
                application.IsTaskExecuted.Should().BeTrue();
                TestCorrelator.GetLogEventsFromCurrentContext().Should().BeEmpty();
            }
        }

        [Fact]
        public void UnhandledTaskExceptionWillBeCaughtByBootstrapperAndLogged()
        {
            using (InitLoggerContext())
            {
                var application = new AsyncExceptionTestApplication();
                var bootstrapper = Bootstrapper.Create(application);
                bootstrapper.Run();
                application.IsTaskExecuted.Should().BeTrue();

                TestCorrelator.GetLogEventsFromCurrentContext().Should().HaveCount(2);
                TestCorrelator.GetLogEventsFromCurrentContext().Should().Contain(log => log.MessageTemplate.Text.Equals("The following exceptions have been thrown"));
                TestCorrelator.GetLogEventsFromCurrentContext().Should().Contain(log => (log.Exception != null));
            }
        }

        private static ITestCorrelatorContext InitLoggerContext()
        {
            Log.Logger = new LoggerConfiguration().WriteTo.TestCorrelator().CreateLogger();
            return TestCorrelator.CreateContext();
        }
    }
}