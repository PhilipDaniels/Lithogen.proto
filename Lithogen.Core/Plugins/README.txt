To implement a Lithogen Plugin, create an assembly that references the
Lithogen.Interfaces assembly, implement one or more interfaces on a concrete
type, and drop the compiled DLL in here. It will be automatically loaded
whenever Lithogen runs, and your types will be used in preference to the
Lithogen Default types.
