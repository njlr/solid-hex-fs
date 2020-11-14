# Solid Hex

F# library for manipulating cube coordinates (hexagon maps).

```bash
dotnet build
```

## Demo

The demo is a Fable app.

```bash
cd ./demo
yarn install
yarn webpack-dev-server
```

![Demo](./demo.png)

Click to select a hexagon.

## Install

Install using [Paket](https://fsprojects.github.io/Paket/).

Add this line to your `paket.dependencies`:

```
github njlr/solid-hex-fs SolidHex.fs
```

Add this line to your `paket.references`:

```
File: SolidHex.fs
```
