# U-Stella ReadmeViewer

[English](README.md) | 日本語

U-Stella ReadmeViewer は、Unity の Inspector 上で Readme アセットを表示するための Unity エディター拡張です。
U-Stella のパッケージやアセットに同梱されるセットアップ案内、更新情報、関連リンクを Unity 上で確認するために使用します。

## インストール

VCCまたはALCOMに、U-StellaのVPMリポジトリを追加してください。

```text
https://ustl-vpm.kamishiro.online/index.json
```

VCCまたはALCOMがブラウザーから起動できる環境では、以下のURLからリポジトリを追加できます。

```text
vcc://vpm/addRepo?url=https%3A%2F%2Fustl-vpm.kamishiro.online%2Findex.json
```

リポジトリ追加後、対象のUnityプロジェクトで `U-Stella ReadmeViewer` パッケージを追加します。

## 必要環境

- Unity 2022.3
- VCCまたはALCOMで管理しているUnityプロジェクト

追加の依存パッケージはありません。

## 基本的な使い方

1. Unityプロジェクトに `U-Stella ReadmeViewer` パッケージを追加します。
2. U-Stellaのパッケージやアセットに同梱されているReadmeアセットを選択します。
3. Inspectorに表示された案内を確認します。
4. Inspector上のリンクをクリックすると、関連ページをブラウザーで開けます。

インポートしたUnityPackageに未読のReadmeアセットが含まれている場合、インポート後に自動で選択されることがあります。

## ライセンス

[Apache License 2.0](LICENSE)
