﻿using Meziantou.Analyzer.Rules;
using Xunit;
using TestHelper;
using System.Threading.Tasks;

namespace Meziantou.Analyzer.Test.Rules
{
    public sealed class UseStringComparerAnalyzerTests
    {
        private static ProjectBuilder CreateProjectBuilder()
        {
            return new ProjectBuilder()
                .WithAnalyzer<UseStringComparerAnalyzer>()
                .WithCodeFixProvider<UseStringComparerFixer>();
        }

        [Fact]
        public async Task HashSet_Int32_ShouldNotReportDiagnostic()
        {
            const string SourceCode = @"
class TypeName
{
    public void Test()
    {
        new System.Collections.Generic.HashSet<int>();
    }
}";
            await CreateProjectBuilder()
                  .WithSourceCode(SourceCode)
                  .ValidateAsync();
        }

        [Fact]
        public async Task HashSet_String_ShouldReportDiagnostic()
        {
            const string SourceCode = @"
class TypeName
{
    public void Test()
    {
        [||]new System.Collections.Generic.HashSet<string>();
    }
}";
            const string CodeFix = @"
class TypeName
{
    public void Test()
    {
        new System.Collections.Generic.HashSet<string>(System.StringComparer.Ordinal);
    }
}";
            await CreateProjectBuilder()
                  .WithSourceCode(SourceCode)
                  .ShouldFixCodeWith(CodeFix)
                  .ValidateAsync();
        }

        [Fact]
        public async Task HashSet_String_StringEqualityComparer_ShouldNotReportDiagnostic()
        {
            const string SourceCode = @"
class TypeName
{
    public void Test()
    {
        new System.Collections.Generic.HashSet<string>(System.StringComparer.Ordinal);
    }
}";
            await CreateProjectBuilder()
                  .WithSourceCode(SourceCode)
                  .ValidateAsync();
        }

        [Fact]
        public async Task Dictionary_String_ShouldReportDiagnostic()
        {
            const string SourceCode = @"
class TypeName
{
    public void Test()
    {
        [||]new System.Collections.Generic.Dictionary<string, int>();
    }
}";
            const string CodeFix = @"
class TypeName
{
    public void Test()
    {
        new System.Collections.Generic.Dictionary<string, int>(System.StringComparer.Ordinal);
    }
}";
            await CreateProjectBuilder()
                  .WithSourceCode(SourceCode)
                  .ShouldFixCodeWith(CodeFix)
                  .ValidateAsync();
        }

        [Fact]
        public async Task ConcurrentDictionary_String_ShouldReportDiagnostic()
        {
            const string SourceCode = @"
class TypeName
{
    public void Test()
    {
        [||]new System.Collections.Concurrent.ConcurrentDictionary<string, int>();
    }
}";
            const string CodeFix = @"
class TypeName
{
    public void Test()
    {
        new System.Collections.Concurrent.ConcurrentDictionary<string, int>(System.StringComparer.Ordinal);
    }
}";
            await CreateProjectBuilder()
                  .WithSourceCode(SourceCode)
                  .ShouldFixCodeWith(CodeFix)
                  .ValidateAsync();
        }

        [Fact]
        public async Task EnumerableContains_String_ShouldReportDiagnostic()
        {
            const string SourceCode = @"using System.Linq;
class TypeName
{
    public void Test()
    {
        System.Collections.Generic.IEnumerable<string> obj = null;
        [||]obj.Contains("""");
    }
}";
            const string CodeFix = @"using System.Linq;
class TypeName
{
    public void Test()
    {
        System.Collections.Generic.IEnumerable<string> obj = null;
        obj.Contains("""", System.StringComparer.Ordinal);
    }
}";
            await CreateProjectBuilder()
                  .WithSourceCode(SourceCode)
                  .ShouldFixCodeWith(CodeFix)
                  .ValidateAsync();
        }

        [Fact]
        public async Task EnumerableToDictionary_String_ShouldReportDiagnostic()
        {
            const string SourceCode = @"using System.Linq;
class TypeName
{
    public void Test()
    {
        System.Collections.Generic.IEnumerable<string> obj = null;
        [||]obj.ToDictionary(p => p);
    }
}";
            const string CodeFix = @"using System.Linq;
class TypeName
{
    public void Test()
    {
        System.Collections.Generic.IEnumerable<string> obj = null;
        obj.ToDictionary(p => p, System.StringComparer.Ordinal);
    }
}";
            await CreateProjectBuilder()
                  .WithSourceCode(SourceCode)
                  .ShouldFixCodeWith(CodeFix)
                  .ValidateAsync();
        }
    }
}
