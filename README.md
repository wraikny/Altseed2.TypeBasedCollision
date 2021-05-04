[![](https://github.com/wraikny/Altseed2.TypeBasedCollision/workflows/CI/badge.svg)](https://github.com/wraikny/Altseed2.TypeBasedCollision/actions?workflow=CI)

# Altseed2.TypeBasedCollision

Altsees2.TypeBasedCollision は、型をキーとして衝突対象の管理を平易かつ高速に行うためのライブラリです。

## Install
[Altseed2.TypeBasedCollision.cs](src/Altseed2.TypeBasedCollision/Altseed2.TypeBasedCollision.cs)
をプロジェクト内にコピーするだけで利用可能です。

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
