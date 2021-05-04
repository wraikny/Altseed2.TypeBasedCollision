[![](https://github.com/wraikny/Altseed2.TypeBasedCollision/workflows/CI/badge.svg)](https://github.com/wraikny/Altseed2.TypeBasedCollision/actions?workflow=CI)
[![Nuget](https://img.shields.io/nuget/v/Altseed2.TypeBasedCollision?style=plastic)](https://www.nuget.org/packages/Altseed2.TypeBasedCollision/)

# Altseed2.TypeBasedCollision

Altsees2.TypeBasedCollision は、型をキーとして衝突対象の管理を平易かつ高速に行うためのライブラリです。

## Install
[NuGet Gallery](https://www.nuget.org/packages/Altseed2.TypeBasedCollision/2.0.0) からインストール可能です。

あるいは、
[Altseed2.TypeBasedCollision.cs](src/Altseed2.TypeBasedCollision/Altseed2.TypeBasedCollision.cs)
をプロジェクト内にコピーするだけでも利用可能です。


## Examples
[example](./example/Altseed2.TypeBasedCollision.Example) を見てください。

## Setup
```sh
$ dotnet tool restore
```

## Build

```sh
$ dotnet fake build [-- <DEBUG|RELEASE>]
```

デフォルトは `DEBUG`

## Format

```sh
$ dotnet fake build -t format
```
