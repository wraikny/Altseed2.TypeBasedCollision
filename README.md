[![](https://github.com/wraikny/Altseed2.TypeBasedCollision/workflows/CI/badge.svg)](https://github.com/wraikny/Altseed2.TypeBasedCollision/actions?workflow=CI)

# Altseed2.TypeBasedCollision

Altsees2.TypeBasedCollision は、型をキーとして衝突対象の管理を平易かつ高速に行うためのライブラリです。

## Install
Copy [Altseed2.TypeBasedCollision.cs](src/Altseed2.TypeBasedCollision/Altseed2.TypeBasedCollision.cs) and paste it into your project directory.

## Examples
See the [example](./example/Altseed2.TypeBasedCollision.Example).

## Setup
```sh
$ dotnet tool restore
```

## Build

```sh
$ dotnet fake build [-- <DEBUG|RELEASE>]
```

Default configuration is DEBUG

## Format

```sh
$ dotnet fake build -t format
```
