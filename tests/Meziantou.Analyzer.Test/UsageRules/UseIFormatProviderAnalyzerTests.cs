﻿using Meziantou.Analyzer.UsageRules;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestHelper;

namespace Meziantou.Analyzer.Test.UsageRules
{
    [TestClass]
    public class UseStringComparisonAnalyzerTest : CodeFixVerifier
    {
        protected override DiagnosticAnalyzer GetCSharpDiagnosticAnalyzer() => new UseIFormatProviderAnalyzer();
        protected override string ExpectedDiagnosticId => "MA0011";
        protected override DiagnosticSeverity ExpectedDiagnosticSeverity => DiagnosticSeverity.Warning;

        [TestMethod]
        public void Int32ToStringWithCultureInfo_ShouldNotReportDiagnostic()
        {
            var project = new ProjectBuilder()
                  .WithSource(@"
class TypeName
{
    public void Test()
    {
        1.ToString(System.Globalization.CultureInfo.InvariantCulture);
    }
}");

            VerifyDiagnostic(project);
        }

        [TestMethod]
        public void Int32ToStringWithoutCultureInfo_ShouldReportDiagnostic()
        {
            var project = new ProjectBuilder()
                  .WithSource(@"
class TypeName
{
    public void Test()
    {
        1.ToString();
    }
}");
            var expected = CreateDiagnosticResult(line: 6, column: 9, message: "Use an overload of 'ToString' that has an IFormatProvider parameter");
            VerifyDiagnostic(project, expected);
        }

        [TestMethod]
        public void BooleanToStringWithoutCultureInfo_ShouldNotReportDiagnostic()
        {
            var project = new ProjectBuilder()
                  .WithSource(@"
class TypeName
{
    public void Test()
    {
        true.ToString();
    }
}");

            VerifyDiagnostic(project);
        }

        [TestMethod]
        public void Int32ParseWithoutCultureInfo_ShouldNotReportDiagnostic()
        {
            var project = new ProjectBuilder()
                  .WithSource(@"
class TypeName
{
    public void Test()
    {
        int.Parse("""");
        int.Parse("""", System.Globalization.NumberStyles.Any);
    }
}");

            var expected1 = CreateDiagnosticResult(line: 6, column: 9, message: "Use an overload of 'Parse' that has an IFormatProvider parameter");
            var expected2 = CreateDiagnosticResult(line: 7, column: 9, message: "Use an overload of 'Parse' that has an IFormatProvider parameter");
            VerifyDiagnostic(project, expected1, expected2);
        }

        [TestMethod]
        public void SingleTryParseWithoutCultureInfo_ShouldNotReportDiagnostic()
        {
            var project = new ProjectBuilder()
                  .WithSource(@"
class TypeName
{
    public void Test()
    {
        float.TryParse("""", out _);
    }
}");

            var expected = CreateDiagnosticResult(line: 6, column: 9, message: "Use an overload of 'TryParse' that has an IFormatProvider parameter");
            VerifyDiagnostic(project, expected);
        }

        [TestMethod]
        public void DateTimeTryParseWithoutCultureInfo_ShouldNotReportDiagnostic()
        {
            var project = new ProjectBuilder()
                  .WithSource(@"
class TypeName
{
    public void Test()
    {
        float.TryParse("""", out _);
    }
}");

            var expected = CreateDiagnosticResult(line: 6, column: 9, message: "Use an overload of 'TryParse' that has an IFormatProvider parameter");
            VerifyDiagnostic(project, expected);
        }
    }
}
