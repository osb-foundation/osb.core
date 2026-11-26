# Changelog

## [0.1.0] - Olorun (Generation 0)

### Added
- Initial release of osb.core under OSB generation 0 (Olorun).
- Shared freestanding MiniCoreLib runtime stubs used by kernel, shell, toolkit and other OSB components.
- Public APIs: Program.cs, Shell/Environment.cs, Layout/Memory, Processes/Scheduler, Storage/Ata/Ext4, User/auth, Environment vars (GETCwd, GETArgs, GetKeyScancode, Exit, Spawn, Wait, GetPid, GetABI, ResolvePath, DirNext).
- Links MiniCoreLib from ../osb.core/src/*.cs; consumes linked sources from sibling checkouts (osb.shell, osbkt, osb.kernel, osb.xwin).

### Stable
- Build succeeds (`dotnet build -c Release Osb.Core.csproj`); host unit tests pass; Elf links; ABI contract consistent.

### Experimental / Incomplete
- No BCL dependencies; no GC allocations; fixed/stack buffers where appropriate.
- Memory layout constants (Layout.MaxEnvVars, task slots) defined in Shell/Environment.cs.
