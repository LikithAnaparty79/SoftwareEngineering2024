// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FA1.Model;
/// <summary>
/// Class to represent blob information in the response.
/// </summary>
public class BlobInfo
{
    public string? Name { get; set; }
    public string? ContentType { get; set; }
    public long? Size { get; set; }
    public DateTimeOffset? LastModified { get; set; }
    public string? Uri { get; set; }
}
