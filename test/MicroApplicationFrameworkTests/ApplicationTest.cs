using FluentAssertions;
using MicroApplicationFramework;
using Moq;

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
            ApplicationContext.TaskScheduler.Add(Task.Run(async () =>
            {
                await Task.Delay(2000);
                IsTaskExecuted = true;
            }));
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

            Bootstrapper.Create(mockApplication.Object).Run();

            mockApplication.Verify(app => app.OnInit(), Times.Once);
            mockApplication.Verify(app => app.OnRegister(), Times.Once);
            mockApplication.Verify(app => app.OnExecute(), Times.Once);
            mockApplication.Verify(app => app.OnExit(), Times.Once);
        }

        [Fact]
        public void OnExitIsAlwaysCalledFromApplicationIfOnRegisterThrowsException()
        {
            var mockApplication = new Mock<TestApplication>();
            mockApplication.Setup(app => app.OnRegister()).Throws(new Exception("Any Exception By Init"));

            Bootstrapper.Create(mockApplication.Object).Run();

            mockApplication.Verify(app => app.OnRegister(), Times.Once);
            mockApplication.Verify(app => app.OnInit(), Times.Never);
            mockApplication.Verify(app => app.OnExecute(), Times.Never);
            mockApplication.Verify(app => app.OnExit(), Times.Once);
        }

        [Fact]
        public void OnExitIsAlwaysCalledFromApplicationIfOnInitThrowsException()
        {
            var mockApplication = new Mock<TestApplication>();
            mockApplication.Setup(app => app.OnInit()).Throws(new Exception("Any Exception By Init"));

            Bootstrapper.Create(mockApplication.Object).Run();

            mockApplication.Verify(app => app.OnRegister(), Times.Once);
            mockApplication.Verify(app => app.OnInit(), Times.Once);
            mockApplication.Verify(app => app.OnExecute(), Times.Never);
            mockApplication.Verify(app => app.OnExit(), Times.Once);
        }

        [Fact]
        public void OnExitIsAlwaysCalledFromApplicationIfOnExecuteThrowsException()
        {
            var mockApplication = new Mock<TestApplication>();
            mockApplication.Setup(app => app.OnExecute()).Throws(new Exception("Any Exception By Init"));

            Bootstrapper.Create(mockApplication.Object).Run();

            mockApplication.Verify(app => app.OnRegister(), Times.Once);
            mockApplication.Verify(app => app.OnInit(), Times.Once);
            mockApplication.Verify(app => app.OnExecute(), Times.Once);
            mockApplication.Verify(app => app.OnExit(), Times.Once);
        }

        [Fact]
        public void AsyncApplicationShouldCleanExitIfApplicationContextRequestCancelIsCalled()
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
        }

        [Fact]
        public void AsyncApplicationMultipleApplicationContextRequestCancelShouldDoNothing()
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
        }

        [Fact]
        public void AsyncApplicationWaitsUntilAllTasksAreFinished()
        {
            var application = new AsyncTestApplication();
            var bootstrapper = Bootstrapper.Create(application);
            bootstrapper.Run();
            application.IsTaskExecuted.Should().BeTrue();
        }
    }
}