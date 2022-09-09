using RestSharp;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
namespace EmployeePayroll
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
    }
}