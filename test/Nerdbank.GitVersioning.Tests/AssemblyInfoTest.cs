﻿// Copyright (c) .NET Foundation and Contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Build.Utilities;
using Nerdbank.GitVersioning.Tasks;
using Xunit;
using Xunit.Sdk;

public class AssemblyInfoTest : IClassFixture<MSBuildFixture> // The MSBuildFixture throws PlatformNotSupportedException when run on mono.
{
    [SkippableTheory(typeof(PlatformNotSupportedException))]
    [InlineData(false)]
    [InlineData(true)]
    [InlineData(null)]
    public void FSharpGenerator(bool? thisAssemblyClass)
    {
        var info = new AssemblyVersionInfo();
        info.AssemblyCompany = "company";
        info.AssemblyFileVersion = "1.3.1.0";
        info.AssemblyVersion = "1.3.0.0";
        info.AdditionalThisAssemblyFields =
            new TaskItem[]
            {
                new TaskItem(
                    "CustomString1",
                    new Dictionary<string, string>() { { "String", "abc" } }),
                new TaskItem(
                    "CustomString2",
                    new Dictionary<string, string>() { { "String", string.Empty } }),
                new TaskItem(
                    "CustomString3",
                    new Dictionary<string, string>() { { "String", string.Empty }, { "EmitIfEmpty", "true" } }),
                new TaskItem(
                    "CustomBool",
                    new Dictionary<string, string>() { { "Boolean", "true" } }),
                new TaskItem(
                    "CustomTicks",
                    new Dictionary<string, string>() { { "Ticks", "637509805729817056" } }),
            };
        info.CodeLanguage = "f#";

        if (thisAssemblyClass.HasValue)
        {
            info.EmitThisAssemblyClass = thisAssemblyClass.GetValueOrDefault();
        }

        string built = info.BuildCode();

        string expected = $@"//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

#nowarn ""CA2243""

namespace AssemblyInfo
[<assembly: global.System.Reflection.AssemblyVersionAttribute(""1.3.0.0"")>]
[<assembly: global.System.Reflection.AssemblyFileVersionAttribute(""1.3.1.0"")>]
[<assembly: global.System.Reflection.AssemblyInformationalVersionAttribute("""")>]
do()
{(thisAssemblyClass.GetValueOrDefault(true) ? $@"#if NETSTANDARD || NETFRAMEWORK || NETCOREAPP
[<global.System.CodeDom.Compiler.GeneratedCode(""{AssemblyVersionInfo.GeneratorName}"",""{AssemblyVersionInfo.GeneratorVersion}"")>]
#endif
#if NET40_OR_GREATER || NETCOREAPP2_0_OR_GREATER || NETSTANDARD2_0_OR_GREATER
[<global.System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage>]
#endif
type internal ThisAssembly() =
  static member internal AssemblyCompany = ""company""
  static member internal AssemblyFileVersion = ""1.3.1.0""
  static member internal AssemblyVersion = ""1.3.0.0""
  static member internal CustomBool = true
  static member internal CustomString1 = ""abc""
  static member internal CustomString3 = """"
  static member internal CustomTicks = new global.System.DateTime(637509805729817056L, global.System.DateTimeKind.Utc)
  static member internal IsPrerelease = false
  static member internal IsPublicRelease = false
  static member internal RootNamespace = """"
do()
" : string.Empty)}";

        Assert.Equal(expected, built);
    }

    [SkippableTheory(typeof(PlatformNotSupportedException))]
    [InlineData(null, "MyRootNamespace")]
    [InlineData("", "MyRootNamespace")]
    [InlineData("MyCustomNamespace", null)]
    [InlineData("MyCustomNamespace", "")]
    [InlineData("MyCustomNamespace", "MyRootNamespace")]
    public void FSharpGeneratorWithNamespace(string thisAssemblyNamespace, string rootNamespace)
    {
        var info = new AssemblyVersionInfo
        {
            AssemblyCompany = "company",
            AssemblyFileVersion = "1.3.1.0",
            AssemblyVersion = "1.3.0.0",
            CodeLanguage = "f#",
            RootNamespace = rootNamespace,
            ThisAssemblyNamespace = thisAssemblyNamespace,
        };

        string built = info.BuildCode();

        string expected = $@"//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

#nowarn ""CA2243""

namespace {(
    !string.IsNullOrWhiteSpace(thisAssemblyNamespace)
        ? thisAssemblyNamespace
        : !string.IsNullOrWhiteSpace(rootNamespace)
            ? rootNamespace
            : "AssemblyInfo")}
[<assembly: global.System.Reflection.AssemblyVersionAttribute(""1.3.0.0"")>]
[<assembly: global.System.Reflection.AssemblyFileVersionAttribute(""1.3.1.0"")>]
[<assembly: global.System.Reflection.AssemblyInformationalVersionAttribute("""")>]
do()
#if NETSTANDARD || NETFRAMEWORK || NETCOREAPP
[<global.System.CodeDom.Compiler.GeneratedCode(""{AssemblyVersionInfo.GeneratorName}"",""{AssemblyVersionInfo.GeneratorVersion}"")>]
#endif
#if NET40_OR_GREATER || NETCOREAPP2_0_OR_GREATER || NETSTANDARD2_0_OR_GREATER
[<global.System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage>]
#endif
type internal ThisAssembly() =
  static member internal AssemblyCompany = ""company""
  static member internal AssemblyFileVersion = ""1.3.1.0""
  static member internal AssemblyVersion = ""1.3.0.0""
  static member internal IsPrerelease = false
  static member internal IsPublicRelease = false
  static member internal RootNamespace = ""{rootNamespace}""
do()
";

        Assert.Equal(expected, built);
    }

    [SkippableTheory(typeof(PlatformNotSupportedException))]
    [InlineData(false)]
    [InlineData(true)]
    [InlineData(null)]
    public void CSharpGenerator(bool? thisAssemblyClass)
    {
        var info = new AssemblyVersionInfo();
        info.AssemblyCompany = "company";
        info.AssemblyFileVersion = "1.3.1.0";
        info.AssemblyVersion = "1.3.0.0";
        info.CodeLanguage = "c#";
        info.AdditionalThisAssemblyFields =
            new TaskItem[]
            {
                new TaskItem(
                    "CustomString1",
                    new Dictionary<string, string>() { { "String", "abc" } }),
                new TaskItem(
                    "CustomString2",
                    new Dictionary<string, string>() { { "String", string.Empty } }),
                new TaskItem(
                    "CustomString3",
                    new Dictionary<string, string>() { { "String", string.Empty }, { "EmitIfEmpty", "true" } }),
                new TaskItem(
                    "CustomBool",
                    new Dictionary<string, string>() { { "Boolean", "true" } }),
                new TaskItem(
                    "CustomTicks",
                    new Dictionary<string, string>() { { "Ticks", "637509805729817056" } }),
            };

        if (thisAssemblyClass.HasValue)
        {
            info.EmitThisAssemblyClass = thisAssemblyClass.GetValueOrDefault();
        }

        string built = info.BuildCode();

        string expected = $@"//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

#pragma warning disable CA2243

[assembly: global::System.Reflection.AssemblyVersionAttribute(""1.3.0.0"")]
[assembly: global::System.Reflection.AssemblyFileVersionAttribute(""1.3.1.0"")]
[assembly: global::System.Reflection.AssemblyInformationalVersionAttribute("""")]
{(thisAssemblyClass.GetValueOrDefault(true) ? $@"#if NETSTANDARD || NETFRAMEWORK || NETCOREAPP
[global::System.CodeDom.Compiler.GeneratedCode(""{AssemblyVersionInfo.GeneratorName}"",""{AssemblyVersionInfo.GeneratorVersion}"")]
#endif
#if NET40_OR_GREATER || NETCOREAPP2_0_OR_GREATER || NETSTANDARD2_0_OR_GREATER
[global::System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
#endif
internal static partial class ThisAssembly {{
    internal const string AssemblyCompany = ""company"";
    internal const string AssemblyFileVersion = ""1.3.1.0"";
    internal const string AssemblyVersion = ""1.3.0.0"";
    internal const bool CustomBool = true;
    internal const string CustomString1 = ""abc"";
    internal const string CustomString3 = """";
    internal static readonly global::System.DateTime CustomTicks = new global::System.DateTime(637509805729817056L, global::System.DateTimeKind.Utc);
    internal const bool IsPrerelease = false;
    internal const bool IsPublicRelease = false;
    internal const string RootNamespace = """";
}}
" : string.Empty)}";

        Assert.Equal(expected, built);
    }

    [SkippableTheory(typeof(PlatformNotSupportedException))]
    [InlineData(null, "MyRootNamespace")]
    [InlineData("", "MyRootNamespace")]
    [InlineData("MyCustomNamespace", null)]
    [InlineData("MyCustomNamespace", "")]
    [InlineData("MyCustomNamespace", "MyRootNamespace")]
    public void CSharpGeneratorWithNamespace(string thisAssemblyNamespace, string rootNamespace)
    {
        var info = new AssemblyVersionInfo
        {
            AssemblyCompany = "company",
            AssemblyFileVersion = "1.3.1.0",
            AssemblyVersion = "1.3.0.0",
            CodeLanguage = "c#",
            RootNamespace = rootNamespace,
            ThisAssemblyNamespace = thisAssemblyNamespace,
        };

        string built = info.BuildCode();

        (string nsStart, string nsEnd) = !string.IsNullOrWhiteSpace(thisAssemblyNamespace)
            ? ($"{Environment.NewLine}namespace {thisAssemblyNamespace} {{", $"{Environment.NewLine}}}")
            : (string.Empty, string.Empty);

        string expected = $@"//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

#pragma warning disable CA2243

[assembly: global::System.Reflection.AssemblyVersionAttribute(""1.3.0.0"")]
[assembly: global::System.Reflection.AssemblyFileVersionAttribute(""1.3.1.0"")]
[assembly: global::System.Reflection.AssemblyInformationalVersionAttribute("""")]{nsStart}
#if NETSTANDARD || NETFRAMEWORK || NETCOREAPP
[global::System.CodeDom.Compiler.GeneratedCode(""{AssemblyVersionInfo.GeneratorName}"",""{AssemblyVersionInfo.GeneratorVersion}"")]
#endif
#if NET40_OR_GREATER || NETCOREAPP2_0_OR_GREATER || NETSTANDARD2_0_OR_GREATER
[global::System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
#endif
internal static partial class ThisAssembly {{
    internal const string AssemblyCompany = ""company"";
    internal const string AssemblyFileVersion = ""1.3.1.0"";
    internal const string AssemblyVersion = ""1.3.0.0"";
    internal const bool IsPrerelease = false;
    internal const bool IsPublicRelease = false;
    internal const string RootNamespace = ""{rootNamespace}"";
}}{nsEnd}
";

        Assert.Equal(expected, built);
    }

    [SkippableTheory(typeof(PlatformNotSupportedException))]
    [InlineData(false)]
    [InlineData(true)]
    [InlineData(null)]
    public void VisualBasicGenerator(bool? thisAssemblyClass)
    {
        var info = new AssemblyVersionInfo();
        info.AssemblyCompany = "company";
        info.AssemblyFileVersion = "1.3.1.0";
        info.AssemblyVersion = "1.3.0.0";
        info.CodeLanguage = "vb";

        if (thisAssemblyClass.HasValue)
        {
            info.EmitThisAssemblyClass = thisAssemblyClass.GetValueOrDefault();
        }

        string built = info.BuildCode();

        string expected = $@"'------------------------------------------------------------------------------
' <auto-generated>
'     This code was generated by a tool.
'     Runtime Version:4.0.30319.42000
'
'     Changes to this file may cause incorrect behavior and will be lost if
'     the code is regenerated.
' </auto-generated>
'------------------------------------------------------------------------------

#Disable Warning CA2243

<Assembly: Global.System.Reflection.AssemblyVersionAttribute(""1.3.0.0"")>
<Assembly: Global.System.Reflection.AssemblyFileVersionAttribute(""1.3.1.0"")>
<Assembly: Global.System.Reflection.AssemblyInformationalVersionAttribute("""")>
{(thisAssemblyClass.GetValueOrDefault(true) ? $@"#If NET40_OR_GREATER  Or  NETCOREAPP2_0_OR_GREATER  Or  NETSTANDARD2_0_OR_GREATER Then
<Global.System.CodeDom.Compiler.GeneratedCode(""{AssemblyVersionInfo.GeneratorName}"",""{AssemblyVersionInfo.GeneratorVersion}"")>
<Global.System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage>
Partial Friend NotInheritable Class ThisAssembly
#ElseIf NETSTANDARD  Or  NETFRAMEWORK  Or  NETCOREAPP Then
<Global.System.CodeDom.Compiler.GeneratedCode(""{AssemblyVersionInfo.GeneratorName}"",""{AssemblyVersionInfo.GeneratorVersion}"")>
Partial Friend NotInheritable Class ThisAssembly
#Else
Partial Friend NotInheritable Class ThisAssembly
#End If
    Friend Const AssemblyCompany As String = ""company""
    Friend Const AssemblyFileVersion As String = ""1.3.1.0""
    Friend Const AssemblyVersion As String = ""1.3.0.0""
    Friend Const IsPrerelease As Boolean = False
    Friend Const IsPublicRelease As Boolean = False
    Friend Const RootNamespace As String = """"
End Class
" : string.Empty)}";

        Assert.Equal(expected, built);
    }

    [SkippableTheory(typeof(PlatformNotSupportedException))]
    [InlineData(null, "MyRootNamespace")]
    [InlineData("", "MyRootNamespace")]
    [InlineData("MyCustomNamespace", null)]
    [InlineData("MyCustomNamespace", "")]
    [InlineData("MyCustomNamespace", "MyRootNamespace")]
    public void VisualBasicGeneratorWithNamespace(string thisAssemblyNamespace, string rootNamespace)
    {
        var info = new AssemblyVersionInfo
        {
            AssemblyCompany = "company",
            AssemblyFileVersion = "1.3.1.0",
            AssemblyVersion = "1.3.0.0",
            CodeLanguage = "vb",
            RootNamespace = rootNamespace,
            ThisAssemblyNamespace = thisAssemblyNamespace,
        };

        string built = info.BuildCode();

        (string nsStart, string nsEnd) = !string.IsNullOrWhiteSpace(thisAssemblyNamespace)
            ? ($"{Environment.NewLine}Namespace {thisAssemblyNamespace}", $"{Environment.NewLine}End Namespace")
            : (string.Empty, string.Empty);

        string expected = $@"'------------------------------------------------------------------------------
' <auto-generated>
'     This code was generated by a tool.
'     Runtime Version:4.0.30319.42000
'
'     Changes to this file may cause incorrect behavior and will be lost if
'     the code is regenerated.
' </auto-generated>
'------------------------------------------------------------------------------

#Disable Warning CA2243

<Assembly: Global.System.Reflection.AssemblyVersionAttribute(""1.3.0.0"")>
<Assembly: Global.System.Reflection.AssemblyFileVersionAttribute(""1.3.1.0"")>
<Assembly: Global.System.Reflection.AssemblyInformationalVersionAttribute("""")>{nsStart}
#If NET40_OR_GREATER  Or  NETCOREAPP2_0_OR_GREATER  Or  NETSTANDARD2_0_OR_GREATER Then
<Global.System.CodeDom.Compiler.GeneratedCode(""{AssemblyVersionInfo.GeneratorName}"",""{AssemblyVersionInfo.GeneratorVersion}"")>
<Global.System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage>
Partial Friend NotInheritable Class ThisAssembly
#ElseIf NETSTANDARD  Or  NETFRAMEWORK  Or  NETCOREAPP Then
<Global.System.CodeDom.Compiler.GeneratedCode(""{AssemblyVersionInfo.GeneratorName}"",""{AssemblyVersionInfo.GeneratorVersion}"")>
Partial Friend NotInheritable Class ThisAssembly
#Else
Partial Friend NotInheritable Class ThisAssembly
#End If
    Friend Const AssemblyCompany As String = ""company""
    Friend Const AssemblyFileVersion As String = ""1.3.1.0""
    Friend Const AssemblyVersion As String = ""1.3.0.0""
    Friend Const IsPrerelease As Boolean = False
    Friend Const IsPublicRelease As Boolean = False
    Friend Const RootNamespace As String = ""{rootNamespace}""
End Class{nsEnd}
";

        Assert.Equal(expected, built);
    }
}
