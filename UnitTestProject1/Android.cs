using System;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using OpenQA.Selenium;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Android;
using OpenQA.Selenium.Appium.Service;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Support.UI;


namespace UnitTestProject1
{
	public class Android
	{
		private AppiumLocalService _appiumLocalService;

		public AndroidDriver<IWebElement> CreateDriver(string deviceIpPort, string deviceName = "")
		{
			Environment.SetEnvironmentVariable(AppiumServiceConstants.NodeBinaryPath, @"C:\Program Files\nodejs\node.exe");

			if (string.IsNullOrWhiteSpace(deviceName))
				deviceName = deviceIpPort;

			// start appium service
			var builder = new AppiumServiceBuilder();
			//_appiumLocalService = builder.UsingAnyFreePort().Build();
			_appiumLocalService = builder.UsingPort(4724).Build();
			_appiumLocalService.Start();
			_appiumLocalService.OutputDataReceived += (sender, args) =>
													{
														Console.WriteLine(args.Data);
													};

			// create appium driver capabilities
			var options = new AppiumOptions { PlatformName = "Android" };
			options.AddAdditionalCapability("deviceName", deviceName);

			// add app or appPackage / appActivity depending on preference
			options.AddAdditionalCapability("appPackage", "com.crestron.phoenix.touchscreen");
			options.AddAdditionalCapability("appActivity", "com.crestron.phoenix.app.host.MainActivity");

			options.AddAdditionalCapability("udid", deviceIpPort);
			options.AddAdditionalCapability("automationName", "UiAutomator2"); // this one is important

			options.AddAdditionalCapability("platformVersion", "5.1.1");
			options.AddAdditionalCapability("noReset", "true");

			// these are optional, but I find them to be helpful -- see DesiredCapabilities Appium docs to learn more
			options.AddAdditionalCapability("autoGrantPermissions", true);
			options.AddAdditionalCapability("allowSessionOverride", true);

			// create the driver
			var driver = new AndroidDriver<IWebElement>(_appiumLocalService.ServiceUrl, options);
			driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
			return driver;
		}

		public void DestroyDriver(AndroidDriver<IWebElement> driver)
		{
			driver.Quit();
			_appiumLocalService.Dispose();
		}

		public void TestMethod(AndroidDriver<IWebElement> driver)
		{
			var homeButtonId = @"/hierarchy/android.widget.FrameLayout/android.widget.LinearLayout/android.widget.FrameLayout/android.widget.LinearLayout/android.widget.FrameLayout/android.widget.LinearLayout/android.widget.FrameLayout/android.widget.LinearLayout/android.widget.LinearLayout[2]/android.widget.FrameLayout[1]";
			const string roomsButtonId = @"/hierarchy/android.widget.FrameLayout/android.widget.LinearLayout/android.widget.FrameLayout/android.widget.LinearLayout/android.widget.FrameLayout/android.widget.LinearLayout/android.widget.FrameLayout/android.widget.LinearLayout/android.widget.LinearLayout[2]/android.widget.FrameLayout[2]";
			const string roomsTitle = "com.crestron.phoenix.touchscreen:id/fragmentRoomsTitle";

			var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
			var roomsButton = wait.Until(webDriver => driver.FindElementByXPath(roomsButtonId));
			roomsButton.Click();

			var roomTitle = wait.Until(webDriver => driver.FindElementById(roomsTitle).Text);

			//var roomTitle = driver.FindElementById("com.crestron.phoenix.touchscreen:id/fragmentRoomsTitle").Text;
			Assert.AreEqual(roomTitle, "Rooms");
		}
	}
}

