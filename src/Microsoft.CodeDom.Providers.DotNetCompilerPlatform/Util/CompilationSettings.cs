﻿// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

﻿using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;

namespace Microsoft.CodeDom.Providers.DotNetCompilerPlatform {
    internal sealed class CompilerSettings : ICompilerSettings {

        private readonly string _compilerFullPath;
        private readonly int _compilerServerTimeToLive = 0; // seconds

        public CompilerSettings(string compilerFullPath, int compilerServerTimeToLive) {
            if (string.IsNullOrEmpty(compilerFullPath)) {
                throw new ArgumentNullException("compilerFullPath");
            }

            _compilerFullPath = compilerFullPath;
            _compilerServerTimeToLive = compilerServerTimeToLive;
        }

        string ICompilerSettings.CompilerFullPath {
            get {
                return _compilerFullPath;
            }
        }

        int ICompilerSettings.CompilerServerTimeToLive {
            get{
                return _compilerServerTimeToLive;
            }
        }
    }

    internal static class CompilationSettingsHelper {
        private const int DefaultCompilerServerTTL = 10; //seconds
        private const int DefaultCompilerServerTTLInDevEnvironment = 60 * 15;
        private const string DevEnvironmentVariableName = "DEV_ENVIRONMENT";
        private const string DebuggerAttachedEnvironmentVariable = "IN_DEBUG_MODE";
        private const string CustomTTLEnvironmentVariableName = "VBCSCOMPILER_TTL"; // the setting value is in seconds
        // Full path of the directory that contains the Roslyn binaries
        // and the hosting process has permission to access that path
        private const string CustomRoslynCompilerLocation = "ROSLYN_COMPILER_LOCATION";

        private static ICompilerSettings _csc;

        static CompilationSettingsHelper() {
            var ttl = DefaultCompilerServerTTL;
            var devEnvironmentSetting = Environment.GetEnvironmentVariable(DevEnvironmentVariableName, EnvironmentVariableTarget.Process);
            var debuggerAttachedEnvironmentSetting = Environment.GetEnvironmentVariable(DebuggerAttachedEnvironmentVariable, 
                EnvironmentVariableTarget.Process);
            var customTtlSetting = Environment.GetEnvironmentVariable(CustomTTLEnvironmentVariableName, EnvironmentVariableTarget.Process);
            var isDebuggerAttached = IsDebuggerAttached;
            int customTtl;

            // custom TTL setting always win
            if(int.TryParse(customTtlSetting, out customTtl))
            {
                ttl = customTtl;
            }
            else
            {
                if (!string.IsNullOrEmpty(devEnvironmentSetting) ||
                    !string.IsNullOrEmpty(debuggerAttachedEnvironmentSetting) ||
                    isDebuggerAttached)
                {
                    ttl = DefaultCompilerServerTTLInDevEnvironment;
                }
            }

            var customRoslynCompilerLocation = Environment.GetEnvironmentVariable(CustomRoslynCompilerLocation, EnvironmentVariableTarget.Process);
            if(customRoslynCompilerLocation != null)
            {
                var bin = @"c:\Work_Exe\prekesweb.v3\bin\roslyn";
                if (!File.Exists($"{customRoslynCompilerLocation}\\csc.exe")
                    && File.Exists($"{bin}\\csc.exe"))
                    customRoslynCompilerLocation = bin;
                
                _csc = new CompilerSettings($"{customRoslynCompilerLocation}\\csc.exe", ttl);
            }
            else
            {
                _csc = new CompilerSettings(CompilerFullPath(@"bin\roslyn\csc.exe"), ttl);
            }

            if (isDebuggerAttached) {
                Environment.SetEnvironmentVariable(DebuggerAttachedEnvironmentVariable, "1", EnvironmentVariableTarget.Process);
            }
        }

        public static ICompilerSettings CSC2 {
            get {
                return _csc;
            }
        }

        private static string CompilerFullPath(string relativePath) {
            string compilerFullPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, relativePath);
            return compilerFullPath;
        }

        private static bool IsDebuggerAttached {
            get {
                return IsDebuggerPresent() || Debugger.IsAttached;
            }
        }

        [DllImport("kernel32.dll")]
        private extern static bool IsDebuggerPresent();
    }
}
