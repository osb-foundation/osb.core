# AGENTS.md — OSB Core

This file guides coding agents working in the `osb.core` repository.

## Project Identity

**OSB Core** is the shared minimal runtime library for freestanding OSB projects. It provides the `MiniCoreLib` runtime stubs required by the freestanding NativeAOT kernel, userspace shell, toolkit, and other OSB components.

## Code Policy

1. **Implementation Language**
   - Use **C#** for runtime stubs and type definitions.

2. **Freestanding Constraints**
   - No BCL convenience, no GC allocations, no exceptions for normal control flow.
   - Fixed/stack buffers where appropriate.
   - Deterministic, bounded behavior over convenience frameworks.
   - Preserve freestanding safety: no managed heap allocation.

3. **Compatibility**
   - This library is shared across multiple OSB projects.
   - Changes must maintain compatibility with all dependent projects.
   - Test with at least osb.kernel and osbkt after changes.

## Build and Validation

```bash
dotnet build
```

## Change Workflow for Agents

1. Inspect relevant files first.
2. Keep edits targeted.
3. Do not break compatibility with dependent projects.
4. Check file lengths before finishing (no file may exceed 400 lines).

## Non-Goals and Cautions

- Do not introduce BCL dependencies.
- Do not use exceptions for ordinary control flow.
- Do not expose private person data or secrets.

## Documentation Anchors

- `README.md`: High-level project/user documentation.
- `src/*.cs`: The shared runtime library implementation (MiniCoreLib.cs plus
  PrimitiveTypes.cs, CompilerSupport.cs, RuntimeSupport.cs and friends).