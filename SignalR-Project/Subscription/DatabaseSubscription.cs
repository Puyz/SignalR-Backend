using Microsoft.AspNetCore.SignalR;
using SignalR_Project.DataSources.EntityFramework;
using SignalR_Project.Hubs;
using TableDependency.SqlClient;

namespace SignalR_Project.Subscription
{
    public class DatabaseSubscription<T> : IDatabaseSubscription where T : class, new()
    {
        SqlTableDependency<T> _tableDependency;
        IConfiguration _configuration;
        IHubContext<SalesHub> _hubContext;

        public DatabaseSubscription(IConfiguration configuration, IHubContext<SalesHub> hubContext)
        {
            _configuration = configuration;
            _hubContext = hubContext;
        }

        public void Configure(string tableName)
        {
            _tableDependency = new SqlTableDependency<T>(_configuration.GetConnectionString("SQLServer"), tableName);
            //_tableDependency.OnChanged += _tableDependency_OnChanged;
            _tableDependency.OnChanged += async (o, e) =>
            {


                // örnek veriler
                SignalRContext signalRContext = new();
                var data = (from employee in signalRContext.Employees
                           join sale in signalRContext.Sales
                           on employee.Id equals sale.EmployeeId
                           select new { employee, sale }).ToList();

                List<object> datas = new(); // Client tarafındaki grafik için oluşturuldu.
                var employeeNames = data.Select(d => d.employee.Name).Distinct().ToList();

                employeeNames.ForEach(employeeName =>
                {
                    datas.Add(new {
                        type = "line",
                        name = employeeName,
                        data = data.Where(d => d.sale.Employee.Name == employeeName).Select(s => s.sale.Amount).ToList()
                    });
                });
                await _hubContext.Clients.All.SendAsync("receiveMessage", datas);

            };
            _tableDependency.OnError += (o, e) =>
            {

            };

            _tableDependency.Start();

        }

        //private void _tableDependency_OnChanged(object sender, TableDependency.SqlClient.Base.EventArgs.RecordChangedEventArgs<T> e)
        //{
        //    throw new NotImplementedException();
        //}



        ~DatabaseSubscription() // deconstructor
        {
            _tableDependency.Stop();
        }
    }   
}

