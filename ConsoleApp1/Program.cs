using System;
using System.Collections.Immutable;
using Google.OrTools.LinearSolver;

List<int> vertices = [0, 1, 2, 3, 4, 5, 6];
List < List<int> > edges = [[1], [0, 2, 3, 4], [1], [1, 4, 5], [1, 3], [3], []];
int n = 7;
// Create the linear solver with the SCIP backend.
Solver solver = Solver.CreateSolver("SCIP");
if (solver is null)
{
    return;
}
Variable[] x = new Variable[n];
for (int j = 0; j < n; j++)
{
    x[j] = solver.MakeIntVar(0.0, 1.0, $"x_{j}");
}
Console.WriteLine("Number of variables = " + solver.NumVariables());

for (int i = 0; i < n; ++i)
{
    Constraint constraint = solver.MakeConstraint(1, n, "");
    constraint.SetCoefficient(x[i], 1);
    foreach (int j in edges[i])
    {
        constraint.SetCoefficient(x[j], 1);
    }
}
Console.WriteLine("Number of constraints = " + solver.NumConstraints());

Objective objective = solver.Objective();
for (int j = 0; j < n; ++j)
{
    objective.SetCoefficient(x[j], 1);
}
objective.SetMinimization();

Solver.ResultStatus resultStatus = solver.Solve();

// Check that the problem has an optimal solution.
if (resultStatus != Solver.ResultStatus.OPTIMAL)
{
    Console.WriteLine("The problem does not have an optimal solution!");
    return;
}

Console.WriteLine("Solution:");
Console.WriteLine("Optimal objective value = " + solver.Objective().Value());

for (int j = 0; j < n; ++j)
{
    Console.WriteLine("x[" + j + "] = " + x[j].SolutionValue());
}
