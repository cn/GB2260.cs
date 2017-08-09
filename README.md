# GB2260.csharp
The .NET Core implementation for looking up Chinese administrative divisions.

## GB/T 2260

[![GB/T 2260](https://img.shields.io/badge/GB%2FT%202260-v0.2-blue.svg)](https://github.com/cn/GB2260)
[![Build status](https://ci.appveyor.com/api/projects/status/6o5vfi0xcn2i1hbx?svg=true)](https://ci.appveyor.com/project/codeyu/gb2260-csharp)
[![NuGet Badge](https://buildstats.info/nuget/gb2260)](https://www.nuget.org/packages/gb2260/)

The latest GB/T 2260 codes. Read the [GB2260 Specification](https://github.com/cn/GB2260/blob/v0.2/spec.md).

## Build

Install with nuget:

    Install-Package gb2260

## Usage

## GB2260

```cs
GB2260 gb = new GB2260(); // with default revision 2014
GB2260 gb = new GB2260(Revision.V201607); // specify the revision
```

Interface for GB2260.

### .GetDivision(code)

Get division for the given code.

```cs
Division division = gb.GetDivision("110105")
// 北京市 市辖区 朝阳区

division.Name
// 朝阳区
division.Code
// 110105
division.Revision
// 2014

division.GetProvince()
// 北京市
division.GetPrefecture()
// 市辖区

division.ToString()
// 北京市 市辖区 朝阳区
```

### .getProvinces()

Return a list of provinces in Division data structure.

```cs
gb.GetProvinces()
```

### .GetPrefectures(code)

Return a list of prefecture level cities in Division data structure.

```cs
gb.GetPrefectures("110000")
```

### .GetCounties(code)

Return a list of counties in Division data structure.

```cs
gb.GetCounties("110100")
```

## revisions

`Revision` contains a list of available revisions.

```cs
(int)Revision.V201607 // return 201607
```

## License

MIT.