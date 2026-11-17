# OSB Core

Shared minimal runtime library for OSB (Operating System Basic) freestanding projects.

## Overview

`osb.core` provides the minimal `MiniCoreLib` runtime stubs required by freestanding NativeAOT programs in the OSB ecosystem. This library is shared across multiple OSB projects (kernel, shell, toolkit, xwin, osbkt) to ensure consistent freestanding constraints.

## What's Included

- **MiniCoreLib.cs** - Minimal runtime/type stubs for freestanding NativeAOT

## Usage

Other OSB projects reference this library directly or include `MiniCoreLib.cs` as a local copy. The library enforces freestanding constraints:
- No BCL convenience
- No GC allocations
- No exceptions for normal control flow
- Fixed/stack buffers where appropriate

## Building

```bash
dotnet build
```

## Part of OSB

This is part of the [OSB](https://github.com/osb-foundation) - Operating System Basic project, a DOS-like operating system built from scratch.

## License

MIT License - see [LICENSE](LICENSE) for details.