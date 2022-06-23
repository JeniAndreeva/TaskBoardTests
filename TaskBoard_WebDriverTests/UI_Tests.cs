using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Linq;

namespace TaskBoard_DesktopTests
{
    public class UI_Tests
    {
        private const string url = "https://taskboard.nakov.repl.co/";
        private WebDriver driver;

        [SetUp]
        public void OpenBrowser()
        {
            driver = new ChromeDriver();
            driver.Manage().Window.Maximize();
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
        }

        [TearDown]
        public void CloseBrowser()
        {
            driver.Quit();
        }

        [Test]
        public void Test_ListTasks_CheckFirstTask()
        {
            //Arrange
            driver.Navigate().GoToUrl(url + "/boards");
            var taskBoardLink = driver.FindElement(By.CssSelector("body > aside > ul > li:nth-child(2) > a"));
            taskBoardLink.Click();

            //Act
            var boardDone = driver.FindElements(By.CssSelector("body > main > div > div:nth-child(3) > h1"));
            
            //Assert
            var firstTaskTitle = driver.FindElement(By.CssSelector("#task1 > tbody > tr.title > td")).Text;
            
            Assert.That(firstTaskTitle, Is.EqualTo("Project skeleton"));
        }

        [Test]
        public void Test_SearchTasks_CheckFirstResult()
        {
            //Arrange
            driver.Navigate().GoToUrl(url);
            var searchLink = driver.FindElement(By.CssSelector("body > aside > ul > li:nth-child(4) > a"));
            searchLink.Click();

            //Act
            var searchField = driver.FindElement(By.Id("keyword"));
            searchField.SendKeys("home");
            var searchButton = driver.FindElement(By.Id("search"));
            searchButton.Click();

            //Assert
            var taskName = driver.FindElement(By.CssSelector("#task2 > tbody > tr.title > td")).Text;

            Assert.That(taskName, Is.EqualTo("Home page"));
           
        }

        [Test]
        public void Test_SearchTasks_EmptyResult()
        {
            //Arrange
            driver.Navigate().GoToUrl(url);
            var searchLink = driver.FindElement(By.CssSelector("body > aside > ul > li:nth-child(4) > a"));
            searchLink.Click();

            //Act
            var searchField = driver.FindElement(By.Id("keyword"));
            searchField.SendKeys("invalid987456321");
            var searchButton = driver.FindElement(By.Id("search"));
            searchButton.Click();

            //Assert
            var resultMessage = driver.FindElement(By.Id("searchResult")).Text;
            Assert.That(resultMessage, Is.EqualTo("No tasks found."));

        }

        [Test]
        public void Test_CreateTask_InvalidData()
        {
            //Arrange
            driver.Navigate().GoToUrl(url);
            var createLink = driver.FindElement(By.CssSelector("body > aside > ul > li:nth-child(3) > a"));
            createLink.Click();

            //Act
            var description = driver.FindElement(By.Id("description"));
            description.SendKeys("create without title");
            var createButton = driver.FindElement(By.Id("create"));
            createButton.Click();

            //Assert
            var errortMessage = driver.FindElement(By.CssSelector("body > main > div")).Text;
            Assert.That(errortMessage, Is.EqualTo("Error: Title cannot be empty!"));

        }

        [Test]
        public void Test_CreateContacts_ValidData()
        {
            //Arrange
            driver.Navigate().GoToUrl(url);
            driver.FindElement(By.LinkText("Create")).Click();

            var title = "Koki" + DateTime.Now.Ticks;
            var description = "created task from Koki" + DateTime.Now.Ticks;
            
            //Act
            driver.FindElement(By.Id("title")).SendKeys(title);
            driver.FindElement(By.Id("description")).SendKeys(description);

            var optionValue = driver.FindElement(By.CssSelector("#boardName > option:nth-child(1)"));
            optionValue.Click();

            var createButton = driver.FindElement(By.Id("create"));
            createButton.Click();

            //Assert
            var taskBoardLink = driver.FindElement(By.CssSelector("body > aside > ul > li:nth-child(2) > a"));
            taskBoardLink.Click();

            var taskTitle = driver.FindElement(By.CssSelector("tr.title > td")).Text;

            if (taskTitle.Contains("Koki"))
            {
                Assert.That(taskTitle, Is.EqualTo(title));
            }
        }
    }
}