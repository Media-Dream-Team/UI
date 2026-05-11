# MeDream UI

Reusable UI view + transition primitives for Unity.

- `IUIView` / `UIViewBase` — show/hide lifecycle with sync + async (UniTask) entry points and `WhenBeforeShow` / `WhenAfterShow` / `WhenBeforeHide` / `WhenAfterHide` hooks.
- `UITransition` — abstract base for a single play-once transition (`PlayAsync`, `ResetTransition`, `SetupTransition`).
- `UITransitionView` — `UIViewBase` that drives in/out `UITransition` components on a `CanvasGroup`, with cancellation-safe show/hide.
- `UIFadeInTransition` / `UIFadeOutTransition` — ready-to-use fade transitions.
- `UIHorizontalSlideTransition` / `UIVerticalSlideTransition` — slide-in/out by anchored position.
- `UIPopInTransition` / `UIPopOutTransition` — scale-based pop transitions.

## Install

### Via Unity Package Manager (Git URL)

In `Packages/manifest.json`:

```json
{
  "dependencies": {
    "com.medream.ui": "https://github.com/Media-Dream-Team/UI.git"
  }
}
```

Or in Unity: **Window → Package Manager → + → Install package from git URL...**

Pin to a tag/branch:

```
https://github.com/Media-Dream-Team/UI.git#v0.1.0
```

### Required dependencies (install first)

This package does **not** declare these in `package.json` because they ship via Git URL or as paid assets:

- **UniTask** — `https://github.com/Cysharp/UniTask.git?path=src/UniTask/Assets/Plugins/UniTask`
- **DOTween** — Asset Store (required by built-in transitions: `UIFadeInTransition`, `UIFadeOutTransition`, `UIHorizontalSlideTransition`, `UIVerticalSlideTransition`, `UIPopInTransition`, `UIPopOutTransition`)
- **Odin Inspector** — Asset Store (used by `UITransitionView` for editor UI)

## Samples

Open **Package Manager → MeDream UI → Samples → Import** to bring the `TestUI` scene into your project.

## License

MIT — see [LICENSE.md](LICENSE.md).
