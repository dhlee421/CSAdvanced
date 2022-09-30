```cs
string[] cars;

string[] cars = { "Volvo", "BMW", "Ford", "Mazda" };

int [] myNum = { 10, 20, 30, 40 };
```

```cs
string[] cars = new string[4];

string[] cars = new[4] { "Volvo", "BMW", "Ford", "Hyundai" };

```


## Arrays.Copy
```cs
string[] folderPaths;
folderPaths = System.IO.Directory.GetDirectories(pathFolder);

string[] filePaths;
filePaths = System.IO.Directory.GetFiles(pathFolder, "*.*");

paths = new string[filePaths.Length + folderPaths.Length];

// join like this
Array.Copy(folderPaths, paths, folderPaths.Length);
Array.Copy(filePaths, 0, paths, folderPaths.Length, filePaths.Length);

// or like this
paths = folderPaths.Union(filePaths).ToArray();
```

## Buffer.BlockCopy

```cs
int[] vals1 = { 1111, 2222, 1234 };
Console.WriteLine("vals1: " + string.Join(", ", vals1.Select(x=>x.ToString())));

byte[] bytes = new byte[sizeof(int) * vals1.Length];
Buffer.BlockCopy(vals1, 0, bytes, 0, bytes.Length);
Console.WriteLine("bytes: " + string.Join(", ", bytes.Select(x=>x.ToString())));

int[] vals2 = new int[bytes.Length / sizeof(int)];
Buffer.BlockCopy(bytes, 0, vals2, 0, bytes.Length);
Console.WriteLine("vals2: " + string.Join(", ", vals2.Select(x=>x.ToString())));
```
