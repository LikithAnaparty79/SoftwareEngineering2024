﻿/******************************************************************************
 * Filename    = FileClonerLogger.cs
 *
 * Author      = Neeraj Krishna N
 * 
 * Project     = FileCloner
 *
 * Description = A custom implementation of a Logger class, which logs to a file
 *              named FileClonerModuleName.log in the log directory. Helps 
 *              developers in analysing any unforeseen error during the usage
 *              of the application
 *****************************************************************************/

using FileCloner.Models;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;

namespace FileCloner.FileClonerLogging;

public class FileClonerLogger
{
    // base path defined by folder
    private static readonly string s_basePath = Constants.BasePath;
    private static readonly string s_logDirectory = Path.Combine(s_basePath, "FileClonerLogs");
    public string LogFile { get; private set; }
    private readonly string? _logFile;
    private string _moduleName;

    // lock to write to Log Files
    private object _syncLock = new();

    /// <summary>
    /// Creates a Log File with the name moduleName.log inside the _logDirectory
    /// </summary>
    /// <param name="moduleName"></param>
    public FileClonerLogger(string moduleName)
    {
        _moduleName = moduleName;
        _syncLock = new();

        try
        {
            if (!Directory.Exists(s_logDirectory))
            {
                Directory.CreateDirectory(s_logDirectory);
            }
            _logFile = $"{s_logDirectory}\\FileCloner{_moduleName}.log";
            LogFile = _logFile;
            if (!File.Exists(_logFile))
            {
                File.Create(_logFile).Close();
            }
            lock (_syncLock)
            {
                File.WriteAllText(_logFile, $"{_moduleName} : Logging Started\n");
            }
        }
        catch (Exception ex)
        {
            Trace.WriteLine(ex.Message + "\n");
        }
    }

    /// <summary>
    /// API to be called to Log onto the file
    /// </summary>
    /// <param name="message"></param>
    /// <param name="memberName"></param>
    /// <param name="filePath"></param>
    /// <param name="lineNumber"></param>
    /// <param name="isErrorMessage"></param>
    public void Log(string message,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string filePath = "",
            [CallerLineNumber] int lineNumber = 0,
            bool isErrorMessage = false)
    {
        // string logToBeWritten = $"{_moduleName}:{filePath}->{memberName}->{lineNumber} :: {message}";
        try
        {
            string logToBeWritten = $"{Path.GetFileName(filePath)}->{memberName}->{lineNumber} :: {message}";
            if (isErrorMessage)
            {
                logToBeWritten = $"ERROR : {logToBeWritten}";
            }
            Write(logToBeWritten);
        }
        catch { }
    }

    /// <summary>
    /// Function which writes to the log File
    /// </summary>
    /// <param name="logToBeWritten"></param>
    private void Write(string logToBeWritten)
    {
        if (_logFile != null)
        {
            lock (_syncLock)
            {
                try
                {
                    File.AppendAllText(_logFile, logToBeWritten + "\n");
                }
                catch (Exception ex)
                {
                    Trace.WriteLine($"FILECLONERTRACE : {ex.Message}\n");
                }
            }
        }
    }


}
