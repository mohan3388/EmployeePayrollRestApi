using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace EmployeePayrll
{
    [TestClass]
    public class Employee
    {
        public int id { get; set; }
        public string name { get; set; }
        public long salary { get; set; }

        RestClient client;
        [TestInitialize]
        public void Setup()
        {
            client = new RestClient("http://localhost:4000");
        }

        private IRestResponse getEmployee()
        {
            RestRequest request = new RestRequest("/employees", Method.GET);
            IRestResponse restResponse = client.Execute(request);
            return restResponse;
        }
        [TestMethod]
        public void onCallingGetApi_returnEmployees()
        {
            IRestResponse restResponse = getEmployee();
            Assert.AreEqual(restResponse.StatusCode, System.Net.HttpStatusCode.OK);
            List<Employee> dataResponse = JsonConvert.DeserializeObject<List<Employee>>(restResponse.Content);
            Assert.AreEqual(2, dataResponse.Count());
            foreach (Employee e in dataResponse)
            {
                Console.WriteLine("Id: " + e.id + ", name: " + e.name + ", salary: " + e.salary);
            }
        }
        [TestMethod]
        public void AddEmployee()
        {
            RestRequest request = new RestRequest("/employees", Method.POST);
            JObject jobject = new JObject();
            jobject.Add("Id", 1);
            jobject.Add("name", "Kajal");
            jobject.Add("salary", 5000);

            request.AddParameter("application/json", jobject, ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);
            Assert.AreEqual(response.StatusCode, System.Net.HttpStatusCode.Created);
            Employee dataResponse = JsonConvert.DeserializeObject<Employee>(response.Content);
            Assert.AreEqual(1, dataResponse.id);
            Assert.AreEqual("Kajal", dataResponse.name);
            Assert.AreEqual(5000, dataResponse.salary);
            System.Console.WriteLine(response.Content);
        }
        [TestMethod]
        public void onCallingPutApi_returnEmployees()
        {
            RestRequest request = new RestRequest("/employees/2", Method.PUT);
            JObject jobject = new JObject();
            jobject.Add("Id", 1);
            jobject.Add("name", "Kajal");
            jobject.Add("salary", 5000);
            request.AddParameter("application/json", jobject, ParameterType.RequestBody);
            var response = client.Execute(request);
            Assert.AreEqual(response.StatusCode, HttpStatusCode.OK);
            Employee dataResponse = JsonConvert.DeserializeObject<Employee>(response.Content);
            Assert.AreEqual(1, dataResponse.id);
            Assert.AreEqual("Kajal", dataResponse.name);
            Assert.AreEqual(5000, dataResponse.salary);
            System.Console.WriteLine(response.Content);
            Console.WriteLine(response.Content);
        }
        [TestMethod]
        public void Given_EmployeeId_OnDelete_ShouldReturnSuccess()
        {
            //arrange
            RestRequest restRequest = new RestRequest("/employees/2", Method.DELETE);
            //Act
            IRestResponse response = client.Execute(restRequest);
            Assert.AreEqual(response.StatusCode, HttpStatusCode.OK);
            Console.WriteLine(response.Content);

        }
    }
}
