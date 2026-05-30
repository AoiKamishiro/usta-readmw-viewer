# U-Stella ReadmeViewer

English | [日本語](README.ja.md)

U-Stella ReadmeViewer is a Unity editor extension for displaying Readme assets in the Inspector.
It is used by U-Stella packages and assets to show setup notes, update information, and related links directly inside Unity.

## Installation

Add the U-Stella VPM repository to VCC or ALCOM.

```text
https://ustl-vpm.kamishiro.online/index.json
```

If VCC or ALCOM can be launched from your browser, you can add the repository with this URL.

```text
vcc://vpm/addRepo?url=https%3A%2F%2Fustl-vpm.kamishiro.online%2Findex.json
```

After adding the repository, add the `U-Stella ReadmeViewer` package to your Unity project.

## Requirements

- Unity 2022.3
- A Unity project managed with VCC or ALCOM

No additional package dependencies are required.

## Basic Usage

1. Add the `U-Stella ReadmeViewer` package to your Unity project.
2. Select a Readme asset included with a U-Stella package or asset.
3. Read the displayed notes in the Inspector.
4. Click links in the Inspector to open related web pages in your browser.

When an imported UnityPackage contains an unread Readme asset, it may be selected automatically after import.

## License

[Apache License 2.0](LICENSE)
