﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using TS.FastNugetUpdate;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TS.FastNugetUpdate.Tests
{
	[TestClass]
	public class NugetUpdateTests
	{
		[TestMethod]
		public void ApplyTest()
		{
			var fileRoot = Environment.CurrentDirectory;

			var sut = new NugetUpdate("My.Package", "0.0.2", s => { }, s => { });

			Assert.IsTrue(sut.Apply());
			var projectFile = Path.Combine(fileRoot, "demo", "demo.csproj");
			var packagesFile = Path.Combine(fileRoot, "demo", "packages.config");
			Assert.IsTrue(File.ReadAllText(projectFile).Contains("0.0.2"));
			Assert.IsTrue(File.ReadAllText(packagesFile).Contains("0.0.2"));
		}
	}
}