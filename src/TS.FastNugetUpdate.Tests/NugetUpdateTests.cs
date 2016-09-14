﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;

namespace TS.FastNugetUpdate.Tests
{
	[TestClass]
	public class NugetUpdateTests
	{
		[TestMethod]
		public void Apply_ChangesVersionToCorrect()
		{
			var fileRoot = Environment.CurrentDirectory;

			var sut = new NugetUpdate("MK.Core", "1.3.1.7", Console.Out.WriteLine, Console.Error.WriteLine);

			Assert.IsTrue(sut.Apply(fileRoot));
			var projectFile = Path.Combine(fileRoot, "demo", "demo.csproj");
			var packagesFile = Path.Combine(fileRoot, "demo", "packages.config");
			Assert.IsTrue(File.ReadAllText(projectFile).Contains("1.3.1.7"));
			Assert.IsTrue(File.ReadAllText(packagesFile).Contains("1.3.1.7"));
		}

		[TestMethod]
		public void Apply_RemainsLinesNumberTheSame()
		{ 
			var fileRoot = Environment.CurrentDirectory;
			var projectFile = Path.Combine(fileRoot, "demo", "demo.csproj");
			var packagesFile = Path.Combine(fileRoot, "demo", "packages.config");
			var numbersBefore = new
			{
				csproj = File.ReadAllLines(projectFile).Length,
				packages = File.ReadAllLines(packagesFile).Length
			};

			var sut = new NugetUpdate("MK.Core", "1.3.1.7", Console.Out.WriteLine, Console.Error.WriteLine);

			sut.Apply(fileRoot);

			var numbersAfter = new
			{
				csproj = File.ReadAllLines(projectFile).Length,
				packages = File.ReadAllLines(packagesFile).Length
			};

			Assert.AreEqual(numbersBefore, numbersAfter);
		}

		[TestMethod]
		public void Apply_TakesAssemblyVersionFromRealDll()
		{
			var fileRoot = Environment.CurrentDirectory;

			var sut = new NugetUpdate("Functional.Maybe", "1.0.8", Console.Out.WriteLine, Console.Error.WriteLine);

			Assert.IsTrue(sut.Apply(fileRoot));
			var projectFile = File.ReadAllText(Path.Combine(fileRoot, "demo", "demo.csproj"));
			Assert.IsTrue(projectFile.Contains(@"Reference Include=""Functional.Maybe, Version=1.0.7.0"""));
			Assert.IsTrue(projectFile.Contains(@"packages\Functional.Maybe.1.0.8"));
		}
	}
}