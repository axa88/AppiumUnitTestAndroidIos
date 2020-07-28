using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace UnitTestProject1
{
	[TestClass]
	public class AndroidDriverCreationTest
	{
		private static string _deviceId;
		private static Android _device;
		private static int _iterations;

		[ClassInitialize]
		public static void Initialize(TestContext testContextInstance)
		{
			_device = new Android();
			_deviceId = "192.168.1.50:5555";
			_iterations = 1;
		}

		[TestMethod]
		public void CreateRunDestroyAndroid()
		{
			for (var i = 0; i < _iterations; i++)
			{
				var driver = _device.CreateDriver(_deviceId);
				_device.TestMethod(driver);
				_device.DestroyDriver(driver);
			}
		}
	}

	[TestClass]
	public class IosDriverCreationTest
	{
		private static string _deviceId;
		private static Ios _device;
		private static int _iterations;

		[ClassInitialize]
		public static void Initialize(TestContext testContextInstance)
		{
			_device = new Ios();
			_deviceId = "00008020-000E21DE0286002E";
			_iterations = 1;
		}

		[TestMethod]
		public void CreateRunDestroyIos()
		{
			for (var i = 0; i < _iterations; i++)
			{
				var driver = _device.CreateDriver(_deviceId);
				_device.TestMethod(driver);
				_device.DestroyDriver(driver);
			}
		}
	}
}

