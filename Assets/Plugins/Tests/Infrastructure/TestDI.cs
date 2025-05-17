using Infrastructure.DI;
using NUnit.Framework;

namespace Plugins.Tests.Infrastructure
{
	[TestFixture]
	public class TestDI
	{
		public class TestProjectService
		{
		}

		public class TestSceneService
		{
			private readonly TestProjectService _testProjectService;

			public TestSceneService(TestProjectService testProjectService) => _testProjectService = testProjectService;
		}

		public class TestFactory
		{
			public TestObject CreateInstance(string id, int param0) => new TestObject(id, param0);
		}

		public class TestObject
		{
			public string id;
			public int param0;

			public TestObject(string id, int param0)
			{
				this.id = id;
				this.param0 = param0;
			}
		}

		[Test]
		public void CreateProjectContainerAndRegisterServices_IsCorrect()
		{
			var projectContainer = new DIContainer();
			
			Assert.AreNotEqual(projectContainer, null);
			
			projectContainer.RegisterAsSingle(c=>new TestProjectService());
			projectContainer.RegisterAsSingle("tag 0",c=>new TestProjectService());
			projectContainer.RegisterAsSingle("tag 1",c=>new TestProjectService());

			var resolve0 = projectContainer.Resolve<TestProjectService>();
			var resolve1 = projectContainer.Resolve<TestProjectService>("tag 0");
			var resolve2 = projectContainer.Resolve<TestProjectService>("tag 1");
			
			Assert.AreNotEqual(resolve0, null);
			Assert.AreNotEqual(resolve1, null);
			Assert.AreNotEqual(resolve2, null);
			
			var sceneContainer = new DIContainer(projectContainer);
			
			Assert.AreNotEqual(sceneContainer, null);
			
			sceneContainer.RegisterAsSingle(c=>new TestSceneService(c.Resolve<TestProjectService>()));
			sceneContainer.RegisterAsSingle("tag 0",c=>new TestSceneService(c.Resolve<TestProjectService>()));
			sceneContainer.RegisterAsSingle("tag 1",c=>new TestSceneService(c.Resolve<TestProjectService>()));
			
			var resolve3 = sceneContainer.Resolve<TestSceneService>();
			var resolve4 = sceneContainer.Resolve<TestSceneService>("tag 0");
			var resolve5 = sceneContainer.Resolve<TestSceneService>("tag 1");
			
			Assert.AreNotEqual(resolve3, null);
			Assert.AreNotEqual(resolve4, null);
			Assert.AreNotEqual(resolve5, null);
		}
		
		[Test]
		public void GetFactoryObjectInstance_IsCorrect()
		{
			var projectContainer = new DIContainer();
			var sceneContainer = new DIContainer(projectContainer);
			projectContainer.RegisterAsSingle(c=>new TestProjectService());
			sceneContainer.RegisterAsSingle(c=>new TestSceneService(c.Resolve<TestProjectService>()));
			sceneContainer.RegisterAsSingle(_=>new TestFactory());
			sceneContainer.RegisterInstance(new TestObject("handle", 10));
			
			var obj0 = sceneContainer.Resolve<TestObject>();
			Assert.AreNotEqual(obj0, null);
			Assert.AreEqual(obj0.id, "handle");
			Assert.AreEqual(obj0.param0, 10);
			
			var obj1 = sceneContainer.Resolve<TestFactory>().CreateInstance("factory", 20);
			Assert.AreNotEqual(obj1, null);
			Assert.AreEqual(obj1.id, "factory");
			Assert.AreEqual(obj1.param0, 20);
		}
	}
}