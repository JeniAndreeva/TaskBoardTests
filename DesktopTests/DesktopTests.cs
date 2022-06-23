using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Windows;
using System;

namespace DesktopTests
{
    public class DesktopTests
    {
        private const string AppiumUrl = "http://127.0.0.1:4723/wd/hub";
        private const string TaskBoardUrl = "https://taskboard.nakov.repl.co/api";
        private const string appLocation = @"D:\Jeni_SoftUni\QA-Automated\Work\ApiAndTasksForExam\TaskBoard.DesktopClient\TaskBoard.DesktopClient.exe";

        private WindowsDriver<WindowsElement> driver;
        private AppiumOptions options;

        [SetUp]
        public void Setup()
        {
            options = new AppiumOptions() { PlatformName = "Windows" };

            options.AddAdditionalCapability("app", appLocation);

            driver = new WindowsDriver<WindowsElement>(new Uri(AppiumUrl), options);
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
        }

        [Test]
        public void Test_SearchTasks_CheckFirstResult()
        {
            var urlField = driver.FindElementByAccessibilityId("textBoxApiUrl");
            urlField.Clear();
            urlField.SendKeys(TaskBoardUrl);
            var conectButton = driver.FindElementByAccessibilityId("buttonConnect");
            conectButton.Click();

            var openTable = driver.FindElementsByAccessibilityId("listViewTasks");

            foreach (var task in openTable)
            {
                if (task.Text.StartsWith("Project"))
                {
                    Assert.That(task.Text, Is.EqualTo("Project skeleton"));
                    break;
                }
            }
        }

        [Test]
        public void Test_AddNewTask_WithValidData()
        {
            var urlField = driver.FindElementByAccessibilityId("textBoxApiUrl");
            urlField.Clear();
            urlField.SendKeys(TaskBoardUrl);
            var conectButton = driver.FindElementByAccessibilityId("buttonConnect");
            conectButton.Click();
            var buttonAdd = driver.FindElementByAccessibilityId("buttonAdd");
            buttonAdd.Click();
            var titleFielf = driver.FindElementByAccessibilityId("textBoxTitle");
            titleFielf.SendKeys("JA New Tasks");
            var discriptionField = driver.FindElementByAccessibilityId("textBoxDescription");
            discriptionField.SendKeys("Created New Task from JA");
            var buttonCreate = driver.FindElementByAccessibilityId("buttonCreate");
            buttonCreate.Click();
            var textSearch = driver.FindElementByAccessibilityId("textBoxSearchText");
            textSearch.SendKeys("JA New Tasks");
            var buttonSearch = driver.FindElementByAccessibilityId("buttonSearch");
            buttonSearch.Click();

            var openTable = driver.FindElementsByAccessibilityId("listViewTasks");

            foreach (var task in openTable)
            {
                if (task.Text.StartsWith("JA"))
                {
                    Assert.That(task.Text, Is.EqualTo("JA New Tasks"));
                    break;
                }
            }
        }
    }
}