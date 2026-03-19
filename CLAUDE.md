# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project Overview

UnityMCPTest is a Unity 6 (6000.5.0a8) project configured with the Universal Render Pipeline (URP) and MCP (Model Context Protocol) relay integration for AI-assisted development. It runs on macOS ARM64.

## Building

This is a standard Unity project with no custom build scripts. Build via:

- Unity Editor: File > Build Settings
- Command line: `Unity -projectPath /Users/haibowang/Downloads/UnityMCPTest -buildTarget [platform] -batchmode -quit`

## Testing

Unity Test Framework (1.7.0) is included. Run tests via:

- Unity Editor: Window > General > Test Runner
- Command line: `Unity -projectPath . -runTests -testPlatform EditMode -batchmode -quit`

## Architecture

- Single scene: `Assets/Scenes/SampleScene.unity`
- Dual render pipeline configs: `Assets/Settings/` contains separate Mobile and PC URP renderer/asset pairs
- Input: `Assets/InputSystem_Actions.inputactions` (Input System 1.9.0)
- MCP relay config: `UserSettings/mcp.json` — connects to `/Users/haibowang/.unity/relay/relay_mac_arm64.app`

## Key Packages

- Universal Render Pipeline 17.5.0
- Input System 1.9.0
- AI Navigation 2.0.11
- Test Framework 1.7.0
- Visual Scripting 1.9.10
