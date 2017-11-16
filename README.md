## Web.config sample

```
<configuration>
  <system.codedom>
    <compilers>
      <!--  , Version=1.0.8.0, Culture=neutral, PublicKeyToken=b213442182ae2f7a -->
      <compiler
        language="c#;cs;csharp"  extension=".cs"
        type="Microsoft.CodeDom.Providers.DotNetCompilerPlatform.CSharpCodeProvider, DotNetCompilerPlatform"
        warningLevel="4" compilerOptions="/langversion:default /nowarn:1659;1699;1701" />
      <!--         
        -->
    </compilers>
  </system.codedom>
  <system.net>
  </system.net>
</configuration>
```

### Microsoft.CodeDom.Providers.DotNetCompilerPlatform

Replacement CodeDOM providers that use the new .NET Compiler Platform ("Roslyn") compiler as a service APIs. This provides support for new language features in systems using CodeDOM (e.g. ASP.NET runtime compilation) as well as improving the compilation performance of these systems.

Please see the blog [Enabling the .NET Compiler Platform (“Roslyn”) in ASP.NET applications](https://blogs.msdn.microsoft.com/webdev/2014/05/12/enabling-the-net-compiler-platform-roslyn-in-asp-net-applications/) 
for an introduction to Microsoft.CodeDom.Providers.DotNetCompilerPlatform.

## Updates
*[Announcing the DotNetCompilerPlatform 1.0.2 release](https://blogs.msdn.microsoft.com/webdev/2016/09/20/announcing-the-dotnetcompilerplatform-1-0-2-release/)
