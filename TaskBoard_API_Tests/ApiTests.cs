using NUnit.Framework;
using RestSharp;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.Json;

namespace API_Tests
{
    public class ApiTests
    {
        private const string url = "https://taskboard.nakov.repl.co/api/tasks";
        private RestClient client;
        private RestRequest request;

        [SetUp]
        public void Setup()
        {
            client = new RestClient();
        }

        [Test]
        public void Test_GetAllTasks_CheckFirstTask()
        {
            //Arrange
            this.request = new RestRequest(url);

            //Act
            var response = this.client.Execute(request);
            var tasks = JsonSerializer.Deserialize<List<Task>>(response.Content);
            Task first = tasks.First();
            string boardName = first.board.name;

            //Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(boardName, Is.EqualTo("Done"));
            Assert.That(first.title, Is.EqualTo("Project skeleton"));
        }

        [Test]
        public void Test_SearchByValidKeyword_CheckFirstResult()
        {
            //Arrange
            this.request = new RestRequest(url + "/search/{keyword}");
            request.AddUrlSegment("keyword", "home");

            //Act
            var response = client.Execute(request, Method.Get);
            var tasks = JsonSerializer.Deserialize<List<Task>>(response.Content);

            //Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(tasks.Count, Is.GreaterThan(0));
            Assert.That(tasks[0].title, Is.EqualTo("Home page"));

        }

        [Test]
        public void Test_SearchTask_CheckEmtyResult()
        {
            //Arrange
            this.request = new RestRequest(url + "/search/{keyword}");
            request.AddUrlSegment("keyword", "missing{randnum}");

            //Act
            var response = client.Execute(request, Method.Get);
            var tasks = JsonSerializer.Deserialize<List<Task>>(response.Content);

            //Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(tasks.Count, Is.EqualTo(0));
        }

        [Test]
        public void Test_CreateNewTask_WithInvalid_Data()
        {
            //Arrange
            this.request = new RestRequest(url);

            var body = new
            {
                description = "Description",
                board = "Open"
            };

            //Act
            var response = this.client.Execute(request, Method.Post);

            //Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
            Assert.That(response.Content, Is.EqualTo("{\"errMsg\":\"Title cannot be empty!\"}"));
        }

        [Test]
        public void Test_CreateNewTask_WithValid_Data()
        {
            //Arrange
            this.request = new RestRequest(url);

            var body = new
            {
                title = "Add Tests",
                description = "API + UI tests",
                name = "Done"
            };
            request.AddJsonBody(body);

            //Act
            var response = this.client.Execute(request, Method.Post);

            //Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Created));

            var allTasks = this.client.Execute(request, Method.Get);
            var task = JsonSerializer.Deserialize<List<Task>>(allTasks.Content);
            var lastTask = task.Last();

            Assert.That(lastTask.title, Is.EqualTo(body.title));
            Assert.That(lastTask.description, Is.EqualTo(body.description));
        }
    }
}