// using System.Diagnostics;
// using System.Text.Encodings.Web;
// using System.Text.Json;
// using QuickFuzzr;

// namespace QuickPulse.Show.Tests;

// public class Perf_Spike
// {
//     [Fact(Skip = "For benchmarking")]
//     public void Print_Armageddon()
//     {

//         // Arrange - Let the fuzzer go wild
//         var fuzzr =
//             from _ in Fuzz.For<Department>().Depth(1, 2)

//             let fiscalyear =
//                 from start in Fuzz.DateTime()
//                 from end in Fuzz.DateTime().Nullable()
//                 select (start, end)
//             from __ in Fuzz.For<Department>().Customize(a => a.FiscalYear, fiscalyear)

//             from budget in Fuzz.Decimal().Nullable()
//             from ____ in Fuzz.For<Department>().Customize(a => a.Budget, budget)

//             from depEmps in Fuzz.For<Department>().Customize(
//                 a => a.Employees, Fuzz.One<Employee>().Many(1, 3).ToList())

//             let kv =
//                 from dictKey in Fuzz.String().Unique("my key")
//                 from employee in Fuzz.One<Employee>()
//                 select KeyValuePair.Create(dictKey, employee)
//             from dictionary in kv.Many(1, 2)
//             from emps in Fuzz.For<Enterprise>().Customize(
//                 a => a.KeyEmployees,
//                 Fuzz.Constant(new Dictionary<string, Employee>(dictionary)))

//             from deps in Fuzz.For<Enterprise>().Customize(
//                 a => a.Departments,
//                 Fuzz.One<Department>().Many(1, 3).ToList())

//             from enterprise in Fuzz.One<Enterprise>()
//             select enterprise;
//         // Warm up JIT/caches first
//         // var warmup = fuzzr.Generate();
//         // _ = Introduce.This(warmup);
//         // _ = JsonSerializer.Serialize(warmup);

//         var satan = fuzzr.Generate();

//         // Act - Battle royale
//         var stopwatch = Stopwatch.StartNew();

//         var startMem = GC.GetTotalMemory(true);

//         var qpResult = Introduce.This(satan);
//         var qpTime = stopwatch.ElapsedMilliseconds;

//         var puzzles = new JsonSerializerOptions
//         {
//             WriteIndented = true,
//             PropertyNamingPolicy = null, // ← Kill camelCase
//             Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping // ← Less \u-escape
//         };
//         stopwatch.Restart();

//         //var jsonResult = JsonSerializer.Serialize(satan);
//         var jsonResult = JsonSerializer.Serialize(satan, puzzles);
//         var jsonTime = stopwatch.ElapsedMilliseconds;

//         Signal.ToFile<string>()
//             .Pulse($"QuickPulse: {qpTime}ms | JSON: {jsonTime}ms")
//             .Pulse($"QP Length: {qpResult.Length} vs JSON: {jsonResult.Length}")
//             .Pulse($"Memory delta: {GC.GetTotalMemory(true) - startMem} bytes")
//             .Pulse("----- QuickPulse.Show -----")
//             .Pulse(qpResult)
//             .Pulse("----- JsonSerializer -----")
//             .Pulse(jsonResult);

//         // Sanity check (because we're professionals)
//         Assert.DoesNotContain("StackOverflowException", qpResult);
//         Assert.Contains(satan.RootDepartment.Name, qpResult);
//     }

//     public class Enterprise
//     {
//         public Guid Id { get; set; }
//         public string Name { get; set; }
//         public List<Department> Departments { get; set; } = new();
//         public Dictionary<string, Employee> KeyEmployees { get; set; } = new();
//         public Department RootDepartment { get; set; }
//         public object Metadata { get; set; } // For polymorphic madness
//     }

//     public class Department
//     {
//         public string Name { get; set; }
//         public decimal Budget { get; set; }
//         public List<Employee> Employees { get; set; } = new();
//         public Department ParentDepartment { get; set; } // Circular reference
//         public (DateTime Start, DateTime? End) FiscalYear { get; set; }
//     }

//     public class Employee
//     {
//         public int Id { get; set; }
//         public string Name { get; set; }
//         public List<Skill> Skills { get; set; } = new();
//         public Employee Manager { get; set; } // Self-reference
//         public dynamic ExtraData { get; set; } // For extra chaos
//     }

//     public record Skill(string Name, byte Proficiency);

//     // For polymorphic fun
//     public interface IMetadata { }
//     public record ProductMetadata(string SKU) : IMetadata;
//     public record FinancialMetadata(decimal Valuation) : IMetadata;
// }