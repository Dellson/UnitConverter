using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Android;

namespace UnitConverterTest
{
    [TestClass]
    public class UnitConverterTest
    {
        protected const string WindowsApplicationDriverUrl = "http://127.0.0.1:4723/wd/hub";
        protected static AndroidDriver<AppiumWebElement> driver;

        [ClassInitialize]
        public static void Setup(TestContext context)
        {
            DesiredCapabilities capabilities = new DesiredCapabilities();

            capabilities.SetCapability("deviceName", "emulator-5554");
            //capabilities.SetCapability("deviceName", "ZTSWRGL7U8AA7P49");
            capabilities.SetCapability("platformName", "Android");
            capabilities.SetCapability("platformVersion", "8.0.0");
            capabilities.SetCapability("appPackage", "kr.sira.unit");
            capabilities.SetCapability("appActivity", "kr.sira.unit.SmartUnit");

            driver = new AndroidDriver<AppiumWebElement>(
                new Uri(WindowsApplicationDriverUrl), capabilities);
            Assert.IsNotNull(driver);
            driver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(90));
        }

        [ClassCleanup]
        public static void TearDown()
        {
            driver.Dispose();
            driver = null;
        }

        [TestMethod]
        public void ConvertFromBeaufortScale()
        {
            MoveToLifeElement();
            ClickVelocityElement();
            ClickInputElement();
            EnterInput();
            AcceptInput();
            ClickUnitSelector();
            SelectUnit();
            ValidateResults();
        }

        private void MoveToLifeElement()
        {
            string id = "kr.sira.unit:id/pager";
            var swipeableElement = driver.FindElementById(id);

            int x = swipeableElement.Location.X;
            int y = swipeableElement.Location.Y;
            int width = swipeableElement.Size.Width;

            driver.Swipe(x + width - 1, y, 1, y, 600);
        }

        private void ClickVelocityElement()
        {
            string id = "kr.sira.unit:id/tab1_layout3";
            var velocityElement = driver.FindElementById(id);

            velocityElement.Click();
        }

        private void ClickInputElement()
        {
            string id = "kr.sira.unit:id/tab1_input";
            var inputElement = driver.FindElementById(id);

            inputElement.Click();
        }

        private void EnterInput()
        {
            string id = "kr.sira.unit:id/tab1_num";
            var num1 = driver.FindElementById(id + 1);
            var num5 = driver.FindElementById(id + 5);
            var num7 = driver.FindElementById(id + 7);

            //num1.Click();
            //num5.Click();
            num7.Click();
        }

        private void AcceptInput()
        {
            string id = "kr.sira.unit:id/tab1_numok";
            var numOK = driver.FindElementById(id);

            numOK.Click();
        }

        private void ClickUnitSelector()
        {
            string id = "kr.sira.unit:id/tab1_selector";
            var selector = driver.FindElementById(id);

            selector.Click();
        }

        private void SelectUnit()
        {
            string id = "android:id/text1";
            var list = driver.FindElementsById(id);

            foreach (var item in list)
            {
                if (item.Text == "Beaufort")
                {
                    item.Click();
                    return;
                } 
            }
        }

        private void ValidateResults()
        {
            string expectedSpeedKMH = "51";
            string expectedSpeedKnot = "28";
            string expectedSpeedMS = "14";
            var units = driver.FindElementsByXPath("//*[@resource-id='kr.sira.unit:id/unit_kind']");
            var values = driver.FindElementsById("kr.sira.unit:id/unit_value");

            for (int i = 0; i < units.Count; ++i)
            {
                if (units[i].Text == "km/h")
                {
                    //Console.WriteLine("Actual: ")
                    Assert.IsTrue(values[i].Text.StartsWith(expectedSpeedKMH));
                }
                    
                if (units[i].Text == "knot")
                    Assert.IsTrue(values[i].Text.StartsWith(expectedSpeedKnot));
                if (units[i].Text == "m/s")
                    Assert.IsTrue(values[i].Text.StartsWith(expectedSpeedMS));
            }
        }
    }
}