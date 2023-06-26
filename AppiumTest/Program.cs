using OpenQA.Selenium.Appium.Android;
using OpenQA.Selenium.Appium.Service;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppiumTest
{
    internal class Program
    {
        private static AndroidDriver<IWebElement> _driver;

        static void Main(string[] args)
        {
            var appiumLocalService = new AppiumServiceBuilder()
                .UsingAnyFreePort()
                .Build();
            appiumLocalService.Start();
            string deviceName = GetDeviceName();
            var appiumOptions = new AppiumOptions();
            appiumOptions.AddAdditionalCapability("Udid", deviceName);
            appiumOptions.AddAdditionalCapability("DeviceName", deviceName);
            appiumOptions.AddAdditionalCapability("PlatformName", "Android");
            //App wont auto close for 60 mins
            appiumOptions.AddAdditionalCapability("newCommandTimeout", "3600");
            //appiumOptions.AddAdditionalCapability("appPackage", bankAppInfo.Package);
            //appiumOptions.AddAdditionalCapability("appActivity", bankAppInfo.Activity);

            _driver = new AndroidDriver<IWebElement>(appiumLocalService, appiumOptions);

            //Click element
            By locator = By.XPath("");
            _driver.FindElement(locator).Click();
            //Input Text
            By locator2 = By.XPath("");
            _driver.FindElement(locator2).SendKeys("");
        }

        static string GetDeviceName()
        {
            string[] devices;
            string deviceNameRecognizeKeyword = "device";
            Process process = new Process()
            {
                StartInfo = new ProcessStartInfo()
                {
                    FileName = "cmd.exe",
                    Arguments = "/c adb devices",
                    UseShellExecute = false,
                    RedirectStandardInput = true,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    CreateNoWindow = true
                }
            };

            process.Start();
            string output = process.StandardOutput.ReadToEnd();

            if (string.IsNullOrEmpty(output))
            { return string.Empty; }

            var deviceNameArray = output.Split('\r', '\n');
            devices = new string[deviceNameArray.Length];
            for (int i = 0; i < deviceNameArray.Length; i++)
            {
                if (deviceNameArray[i].Contains(deviceNameRecognizeKeyword))
                {
                    int deviceNameRecognizeKeywordIndex = deviceNameArray[i].IndexOf(deviceNameRecognizeKeyword);
                    devices[i] = deviceNameArray[i].Remove(deviceNameRecognizeKeywordIndex, deviceNameRecognizeKeyword.Length).Trim();
                }
            }
            process.Close();

            return devices[2]; //return as device name. ex: 192.168.121.129:5555
        }
    }
}
