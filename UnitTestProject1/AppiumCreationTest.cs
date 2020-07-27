using Microsoft.VisualStudio.TestTools.UnitTesting;

using OpenQA.Selenium;
using OpenQA.Selenium.Appium.Android;


namespace UnitTestProject1
{
	[TestClass]
	public class AppiumCreationTest
	{
		private static string _deviceIpPort;
		private static Android _android;
		private static int _iterations;

		[ClassInitialize]
		public static void Initialize(TestContext testContextInstance)
		{
			_android = new Android();
			_deviceIpPort = "192.168.1.19:5555";
			_iterations = 1;
		}

		[TestMethod]
		public void CreateRunDestroy()
		{
			for (var i = 0; i < _iterations; i++)
			{
				var androidDriver = _android.CreateAndroidDriver(_deviceIpPort);
				_android.TestMethod(androidDriver);
				_android.DestroyAndroidDriver(androidDriver);
			}
		}
	}
}

