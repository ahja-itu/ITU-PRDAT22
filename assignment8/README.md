# Assignment 8

Authors: adbo, aljb and ahja

This code can also be found online at <https://github.com/andreaswachs/ITU-PRDAT22>.

We recommend reading this Markdown page on [GitHub](https://github.com/andreaswachs/ITU-PRDAT22/blob/main/assignment8/README.md) or VSCode because it makes the diffs easier to read.

## Solutions

### PLC 9.1

#### i

We have used the following link as reference: <https://en.wikipedia.org/wiki/List_of_CIL_instructions>.

```cli
    .method public static hidebysig 
           default void SelectionSort (int32[] arr)  cil managed 
    {
        // Method begins at RVA 0x20b4
	// Code size 57 (0x39)
	.maxstack 4
	.locals init (
		int32	V_0,
		int32	V_1,
		int32	V_2,
		int32	V_3)
	IL_0000:  ldc.i4.0      // Push 0 onto the stack
	IL_0001:  stloc.0       // Pop from the stack into variable index 0
	IL_0002:  br.s IL_0032  // Go to IL_0032

	IL_0004:  ldloc.0       // Load int i onto the stack
	IL_0005:  stloc.1       // Store i to variable index 1 (int least)
	IL_0006:  ldloc.0       // Load i onto the stack again
	IL_0007:  ldc.i4.1      // Push 1 onto the stack
	IL_0008:  add           // Calculate i + 1
	IL_0009:  stloc.3       // Save i + 1 in variable index 3 (int j)
	IL_000a:  br.s IL_001a  // Go to IL_001a

	IL_000c:  ldarg.0       // Get arr
	IL_000d:  ldloc.3       // Get j
	IL_000e:  ldelem.i4     // Get arr[j]
	IL_000f:  ldarg.0       // Get arr
	IL_0010:  ldloc.1       // Load least
	IL_0011:  ldelem.i4     // Get arr[least]
	IL_0012:  bge.s IL_0016 // If arr[j] >= arr[least], go to IL_0016

    // Once arr[j] < arr[least]:
	IL_0014:  ldloc.3       // Get j
	IL_0015:  stloc.1       // Set least = j
	IL_0016:  ldloc.3       // Get j
	IL_0017:  ldc.i4.1      // Push 1 onto the stack
	IL_0018:  add           // Calculate j + 1
	IL_0019:  stloc.3       // Update j = j + 1
	IL_001a:  ldloc.3       // Load i + 1 onto the stack
	IL_001b:  ldarg.0       // Load i onto the stack
	IL_001c:  ldlen         // Load the length of arr onto the stack
	IL_001d:  conv.i4       // Convert to 32 bit integer
	IL_001e:  blt.s IL_000c // If i < arr.Length, go to IL_000c

    // Once i >= arr.Length:
	IL_0020:  ldarg.0       // Get arr
	IL_0021:  ldloc.0       // Get i
	IL_0022:  ldelem.i4     // Get arr[i]
	IL_0023:  stloc.2       // Save arr[i] in variable index 2 (int tmp)
	IL_0024:  ldarg.0       // Get arr
	IL_0025:  ldloc.0       // Get i
	IL_0026:  ldarg.0       // Get arr
	IL_0027:  ldloc.1       // Get least
	IL_0028:  ldelem.i4     // Get arr[least]
	IL_0029:  stelem.i4     // Set arr[i] = arr[least]
	IL_002a:  ldarg.0       // Get arr
	IL_002b:  ldloc.1       // Get least
	IL_002c:  ldloc.2       // Get tmp
	IL_002d:  stelem.i4     // Set arr[least] = tmp
	IL_002e:  ldloc.0       // Get i
	IL_002f:  ldc.i4.1      // Push 1 onto the stack
	IL_0030:  add           // Calculate i + 1
	IL_0031:  stloc.0       // Set i = i+1
	IL_0032:  ldloc.0       // Put the value in variable index 0 onto the stack (int i)
	IL_0033:  ldarg.0       // Put the value in argument 0 onto the stack (the int[] arr)
	IL_0034:  ldlen         // Put the length of the array onto the stack (arr.Length)
	IL_0035:  conv.i4       // Convert the number to a 32-bit integer
	IL_0036:  blt.s IL_0004 // If variable in index 0 is less than the length, go to IL_0004 (i < arr.Length)

	IL_0038:  ret           // return
    } // end of method Selsort::SelectionSort
```
