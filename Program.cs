using System;
using System.Linq;
using System.Reflection;
using AdventOfCode21;

const string dayNo = "06";

var dataReader = new DataReader();

var data = dataReader.Read(dayNo);

var solutionType = Assembly.GetExecutingAssembly()
    .GetTypes()
    .Where(x => x.IsClass && x.IsAssignableTo(typeof(ISolution)) && x.Name.EndsWith(dayNo))
    .Single();

var solution = (ISolution)Activator.CreateInstance(solutionType)!;

var firstAnswer = solution.GetFirstAnswer(data);
var secondAnswer = solution.GetSecondAnswer(data);

Console.WriteLine($"Day: {dayNo}");
Console.WriteLine($"FirstAnswer: {firstAnswer}");
Console.WriteLine($"SecondAnswer: {secondAnswer}");