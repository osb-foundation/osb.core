# Changelog

## [0.1.0] - Olorun (Generation 0)

### Added
- Initial release of osb.core under OSB generation 0 (Olorun).
- Shared freestanding MiniCoreLib runtime stubs used by the kernel, shell, toolkit and other OSB components (linked sources, never a runtime DLL).
- Source files: MiniCoreLib, PrimitiveTypes, Delegates, Exceptions, RuntimeSupport, Console, CollectionsInterfaces, CompilerSupport, StringComparison, StringComparer, BitConverter, DateTime, Index, List, Math, TupleTypes.
- Consumed by sibling checkouts (osb.kernel, osb.shell, osbkt, osb.xwin) via linked sources from `../osb.core/src/*.cs`.

### Stable
- Build succeeds (`dotnet build -c Release osb.core.csproj`); provides the minimal type/runtime surface the freestanding components compile against.

### Experimental / Incomplete
- No BCL dependencies; no GC allocations; fixed/stack buffers where appropriate.
- Only the runtime surface actually required by OSB components is implemented; not a general-purpose BCL.