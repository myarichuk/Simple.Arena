### Arena Allocator
Simple implementation of Arena allocator, an unmanaged memory allocator that has very cheap allocations of O(1) complexity, but for performance sake, doesn't allow freeing of allocated memory segments.
Useful for things like allocating temporary, short-lived buffers for handling a server request.

### Example

Usage of ``Arena`` allocator is pretty self explanatory. Initialize it, then use it to allocate. Calling ``Dispose()`` frees all allocated memory.  
Note that ``Arena`` allocates an unmanaged memory block on initialization and each call to ``Allocate<T>()`` allocates part of that initial block. 
When there is not enough memory left in the initial block, an ``OutOfMemoryException`` will be thrown.

```cs
//initialize Arena allocator with 4MB of block to use
//note: this allocates umanaged memory
using var arena = new Arena(initialBlockSize: 1024 * 1024 * 4);

//from the initial block of 4MB, allocate enough memory to handle array of 100 floats
Span<float> floatArray = arena.Allocate<float>(100);

//from the memory that is left after previous allocation allocate 100 bytes
Span<byte> byteArray = arena.Allocate<byte>(100); 
```