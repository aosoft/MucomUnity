MUCOM88 for Unity
====

## このプラグインについて

[OPEN MUCOM88](https://github.com/onitama/mucom88) を Unity の AudioClip として使えるようにしたプラグインです。

* ぼうきちさんの作られた "MucomModule" クラスを .NET で使用できるようにした DLL 、 C# コード
* MucomModule を AudioClip 化するクラス

で構成されています。

"MucomModule" の .NET 版は Windows に依存していない (はず) ので、 C++ コードがビルドできれば Unity 以外の .NET / mono 環境や Windows 以外の OS でも使用できると思います。

## ビルドの仕方

1. Unity Editor 下にある "Editor\Data\PluginAPI" をプロジェクト下にフォルダー名 "PluginAPI" でそのままコピーする。
2. Submodule を更新する。
3. NativePluginProject/NativePluginProject.sln を開いてビルドする。

## ライセンス

* [CC BY-NC-SA 4.0](https://creativecommons.org/licenses/by-nc-sa/4.0/deed.ja)

本プラグインは [onitama/mucom88](https://github.com/onitama/mucom88) をぼうきちさんが fork 、拡張をした [BouKiCHi/mucom88](https://github.com/BouKiCHi/mucom88) を利用しています。

    Copyright   : 
    OpenMucom88 Ver.1.7 Copyright 1987-2019(C) Yuzo Koshiro
    Z80 emulation by Yasuo Kuwahara 2002-2018(C)
    FM Sound Generator by cisc 1998, 2003(C)
    SCCI system / adpcm converter by gasshi 2018(C)

    Windows version by ONION software/onitama 2018-2019(C)

