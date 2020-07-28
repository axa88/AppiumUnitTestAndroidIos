using System;

using OpenQA.Selenium;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.iOS;
using OpenQA.Selenium.Appium.MultiTouch;
using OpenQA.Selenium.Appium.Service;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Support.UI;


namespace UnitTestProject1
{
	public class Ios
	{
		private AppiumLocalService _appiumLocalService;

		public IOSDriver<IWebElement> CreateDriver(string deviceUuid, string deviceName = "")
		{
			Environment.SetEnvironmentVariable(AppiumServiceConstants.NodeBinaryPath, @"C:\Program Files\nodejs\node.exe");

			if (string.IsNullOrWhiteSpace(deviceName))
				deviceName = deviceUuid;

			// start appium service
			var builder = new AppiumServiceBuilder();
			//_appiumLocalService = builder.UsingAnyFreePort().Build();
			_appiumLocalService = builder.UsingPort(6001).Build();
			_appiumLocalService.Start();
			_appiumLocalService.OutputDataReceived += (sender, args) =>
													{
														Console.WriteLine(args.Data);
													};

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
			options.AddAdditionalCapability("autoGrantPermissions", true);
			options.AddAdditionalCapability("allowSessionOverride", true);

			// create the driver
			var driver = new IOSDriver<IWebElement>(new Uri("http://192.168.1.27:5001/wd/hub"), options);
			driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
			return driver;
		}

		public void DestroyDriver(IOSDriver<IWebElement> driver)
		{
			driver.Quit();
			_appiumLocalService.Dispose();
		}

		public void TestMethod(IOSDriver<IWebElement> driver)
		{
			const string homeSwitcher = @"CresHomeSwitcherCollectionView";
			const string roomsButtonId = "//XCUIElementTypeButton[@name=\"Rooms\"]";
			const string roomGroupId = "//XCUIElementTypeButton[@name=\"ALL ROOMS\"]";
			const string lightGroupId = "//XCUIElementTypeApplication[@name=\"Crestron Home\"]/XCUIElementTypeWindow[1]/XCUIElementTypeOther/XCUIElementTypeOther/XCUIElementTypeTable/XCUIElementTypeCell[4]/XCUIElementTypeOther[1]";
			const string bathroomId = "//XCUIElementTypeStaticText[@name=\"Bathroom\"]";
			(int X, int Y) ellipsisId = (165, 408);
			const string masterIncId = "CresRightLightMasterButton";
			(int X, int Y) masterIncXyId = (304, 512);
			const string firstLightId = "//XCUIElementTypeCollectionView[@name=\"CresLightControlsCollectionView\"]/XCUIElementTypeCell[1]/XCUIElementTypeOther/XCUIElementTypeOther";
			const string firstValueId = "//*[contains(.,\"%\")]";
			const string secondLightId = "//XCUIElementTypeCollectionView[@name=\"CresLightControlsCollectionView\"]/XCUIElementTypeCell[2]/XCUIElementTypeOther/XCUIElementTypeOther";
			const string secondValueId = "//*[contains(.,\"%\")]";


			var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));

			// press rooms
			var roomsButton = wait.Until(webDriver => driver.FindElementByAccessibilityId(roomsButtonId));
			roomsButton.Click();

			var roomGroupDropDown = wait.Until(webDriver => driver.FindElementByXPath(roomGroupId));
			roomGroupDropDown.Click();

			var lightGroup = wait.Until(webDriver => driver.FindElementByXPath(lightGroupId));
			lightGroup.Click();

			var bathroom = wait.Until(webDriver => driver.FindElementByXPath(bathroomId));
			bathroom.Click();

			new TouchAction(driver).Tap(ellipsisId.X, ellipsisId.Y).Perform();

			new TouchAction(driver).Tap(masterIncXyId.X, masterIncXyId.Y).Perform();

			var firstLight = wait.Until(webDriver => driver.FindElementByXPath(firstLightId));
			var firstValue = firstLight.FindElement(By.XPath(firstValueId));

			var secondLight = wait.Until(webDriver => driver.FindElementByXPath(secondLightId));
			var secondValue = secondLight.FindElement(By.XPath(secondValueId));
		}
	}
}

