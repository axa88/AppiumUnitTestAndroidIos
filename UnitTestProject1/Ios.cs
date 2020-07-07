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
	public class Ios
	{
		public void TestMethod(string deviceUuid, string deviceName = "")
		{
			if (string.IsNullOrWhiteSpace(deviceName))
				deviceName = deviceUuid;

			var homeButtonId = @"Home";
			const string roomsButtonId = @"Rooms";
			const string roomsTitle = "//XCUIElementTypeStaticText[@name=\"Rooms\"]";

			// start appium service
			var builder = new AppiumServiceBuilder();
			var appiumLocalService = builder.UsingAnyFreePort().Build();
			appiumLocalService.Start();

			// create appium driver capabilities
			var options = new AppiumOptions { PlatformName = "iOS" };
			options.AddAdditionalCapability("deviceName", deviceName);

			// add app or appPackage / appActivity depending on preference
			options.AddAdditionalCapability("udid", deviceUuid);
			options.AddAdditionalCapability("automationName", "XCUITest"); // this one is important

			options.AddAdditionalCapability("platformVersion", "13.5.1");
			options.AddAdditionalCapability("noReset", "true");


			options.AddAdditionalCapability("bundleId", "com.crestron.enterprise.phoenix");
			options.AddAdditionalCapability("xcodeOrgId", "U46GHFG9K3");
			options.AddAdditionalCapability("xcodeSigningId", "iPhone Developer");

			// these are optional, but I find them to be helpful -- see DesiredCapabilities Appium docs to learn more
			//options.AddAdditionalCapability("autoGrantPermissions", true);
			//options.AddAdditionalCapability("allowSessionOverride", true);


			// start the driver
			var driver = new AndroidDriver<IWebElement>(appiumLocalService.ServiceUrl, options);
			driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);

			//var mainBottomBar = driver.FindElementById("com.crestron.phoenix.touchscreen:id/main_bottomNavigationView");
			//var homeButton = driver.FindElementByXPath(_homeButtonId);

			var wait = new WebDriverWait(driver , TimeSpan.FromSeconds(10));
			var roomsButton = wait.Until(webDriver => driver.FindElementByXPath(roomsButtonId));
			roomsButton.Click();

			var roomTitle = wait.Until(webDriver => driver.FindElementById(roomsTitle).Text);

			//var roomTitle = driver.FindElementById("com.crestron.phoenix.touchscreen:id/fragmentRoomsTitle").Text;
			Assert.AreEqual(roomTitle, "Rooms");

			driver.Quit();
			appiumLocalService.Dispose();
		}
	}
}

