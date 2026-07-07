# Changelog

All notable changes to this package are documented here. Format based on [Keep a Changelog](https://keepachangelog.com/en/1.1.0/), versioning follows [SemVer](https://semver.org/).

## [0.3.0] - 2026-07-07

### Added
- Optional fade to `UIHorizontalSlideTransition`, `UIVerticalSlideTransition`, `UIPopInTransition`, and `UIPopOutTransition`. Enable via `enableFade` to cross-fade a `CanvasGroup` from `startAlpha` to `endAlpha` alongside the slide/pop, sharing the transition's duration and ease.

## [0.2.1] - 2026-05-11

### Fixed
- Added missing `.meta` files for `UIHorizontalSlideTransition`, `UIVerticalSlideTransition`, `UIPopInTransition`, `UIPopOutTransition` — without these Unity skipped the scripts when consuming the package via Git URL.

## [0.2.0] - 2026-05-11

### Added
- `UIHorizontalSlideTransition` / `UIVerticalSlideTransition` slide transitions (configurable start/end/duration/ease).
- `UIPopInTransition` / `UIPopOutTransition` scale-pop transitions (configurable start/end scale/duration/ease).

### Changed
- **Breaking:** Renamed transition classes to `UI*Transition` convention. `GenericFadeIn` → `UIFadeInTransition`, `GenericFadeOut` → `UIFadeOutTransition`.
- **Breaking:** Renamed folder `Runtime/Trasition` → `Runtime/Transition` (typo fix). Script GUIDs preserved so scene/prefab references remain intact.

## [0.1.0] - 2026-05-11

### Added
- Initial package extraction from Minesweeper project.
- `IUIView`, `UIViewBase`, `UITransition`, `UITransitionView`.
- `GenericFadeIn` / `GenericFadeOut` transitions.
- `TestUI` sample scene.
