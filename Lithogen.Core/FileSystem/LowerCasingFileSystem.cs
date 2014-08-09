﻿using Lithogen.Interfaces.FileSystem;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Lithogen.Core.FileSystem
{
    /// <summary>
    /// The <code>LowerCasingFileSystem</code> can be used to wrap an inner file system.
    /// It will force all file and directory names used as inputs and returned as results
    /// to be converted to lower case. It is an example of the Decorator pattern.
    /// </summary>
    public class LowerCasingFileSystem : IFileSystem
    {
        readonly IFileSystem WrappedFileSystem;

        public LowerCasingFileSystem(IFileSystem wrappedFileSystem)
        {
            WrappedFileSystem = wrappedFileSystem.ThrowIfNull("wrappedFileSystem");
        }

        public bool FileExists(string filename)
        {
            return WrappedFileSystem.FileExists(MakeLower(filename));
        }

        public bool DirectoryExists(string directory)
        {
            return WrappedFileSystem.DirectoryExists(MakeLower(directory));
        }

        public void CreateDirectory(string directory)
        {
            WrappedFileSystem.CreateDirectory(MakeLower(directory));
        }

        public void CreateParentDirectory(string filename)
        {
            WrappedFileSystem.CreateParentDirectory(MakeLower(filename));
        }

        public void WriteAllBytes(string filename, byte[] bytes)
        {
            WrappedFileSystem.WriteAllBytes(MakeLower(filename), bytes);
        }

        public byte[] ReadAllBytes(string filename)
        {
            return WrappedFileSystem.ReadAllBytes(MakeLower(filename));
        }

        public void DeleteFile(string filename)
        {
            WrappedFileSystem.DeleteFile(MakeLower(filename));
        }

        public void DeleteDirectory(string directory)
        {
            WrappedFileSystem.DeleteDirectory(MakeLower(directory));
        }

        public IEnumerable<string> EnumerateFiles(string directory)
        {
            return from f in WrappedFileSystem.EnumerateFiles(MakeLower(directory))
                   select f.ToLowerInvariant();
        }

        public IEnumerable<string> EnumerateFiles(string directory, string searchPattern)
        {
            return from f in WrappedFileSystem.EnumerateFiles(MakeLower(directory), searchPattern)
                   select f.ToLowerInvariant();
        }

        public IEnumerable<string> EnumerateFiles(string directory, string searchPattern, SearchOption searchOption)
        {
            return from f in WrappedFileSystem.EnumerateFiles(MakeLower(directory), searchPattern, searchOption)
                   select f.ToLowerInvariant();
        }

        public string ReadAllText(string filename)
        {
            return WrappedFileSystem.ReadAllText(MakeLower(filename));
        }

        public string ReadAllText(string filename, Encoding encoding)
        {
            return WrappedFileSystem.ReadAllText(MakeLower(filename), encoding);
        }

        public void WriteAllText(string filename, string contents)
        {
            WrappedFileSystem.WriteAllText(MakeLower(filename), contents);
        }

        public void WriteAllText(string filename, string contents, Encoding encoding)
        {
            WrappedFileSystem.WriteAllText(MakeLower(filename), contents, encoding);
        }

        string MakeLower(string filename)
        {
            return filename.ThrowIfNullOrWhiteSpace("filename").ToLowerInvariant();
        }
    }
}