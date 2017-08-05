using System;
using System.Reflection;
using System.Runtime.InteropServices;

[assembly: AssemblyCopyright("Copyright © Daniel J. Terry 2012-2017")]

#if DEBUG
[assembly: AssemblyConfiguration("Debug")]
#else
[assembly: AssemblyConfiguration("Release")]
#endif

[assembly: AssemblyVersion("0.0.0.*")]

[assembly: CLSCompliant(true)]
[assembly: ComVisible(false)]