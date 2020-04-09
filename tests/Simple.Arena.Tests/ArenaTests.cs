using System;
using Xunit;

namespace Simple.Arena.Tests
{
    public class ArenaTests
    {     

        [Fact]
        public void Can_allocate_span_and_use_memory()
        {
            using var arena = new Arena();
            var array = arena.Allocate<byte>(100);
            
            for(byte i = 0; i < array.Length; i++)
                array[i] = i;
        }

        [Fact]
        public void Can_allocate_float_span_and_use_memory()
        {
            using var arena = new Arena();
            var array = arena.Allocate<float>(100);
            
            for(int i = 0; i < array.Length; i++)
                array[i] = i;
        }

        [Fact]
        public void Should_fail_to_allocate_on_disposed_allocate()
        {
            using var arena = new Arena();

            arena.Dispose();

            Assert.Throws<ObjectDisposedException>(() => arena.Allocate<float>(100));
        }

        [Fact]
        public void Should_take_into_account_type_sizes()
        {
            using var arena = new Arena(101);

            //the initial size is in bytes, but memory needed for 100 floats is much more than 101 bytes...
            Assert.Throws<OutOfMemoryException>(() => arena.Allocate<float>(100));
        }

         [Fact]
        public void Should_take_into_account_type_sizes_with_tryallocate()
        {
            using var arena = new Arena(101);

            //the initial size is in bytes, but 100 floats is much more than 101 bytes...
            Assert.False(arena.TryAllocate<float>(100, out _));
        }

        [Fact]
        public void Should_fail_to_allocatebytes_beyond_size()
        {
            using var arena = new Arena(1024);

            var segment1 = arena.Allocate<byte>(1000);
            var segment2 = arena.Allocate<byte>(24);

            Assert.Throws<OutOfMemoryException>(() => arena.Allocate<byte>(1));
        }

        [Fact]
        public void Should_fail_to_tryallocatebytes_beyond_size()
        {
            using var arena = new Arena(1024);

            var segment1 = arena.Allocate<byte>(1000);
            var segment2 = arena.Allocate<byte>(24);

            Assert.False(arena.TryAllocate<byte>(1, out _));
        }

         [Fact]
        public void Should_fail_to_tryallocate_beyond_size()
        {
            using var arena = new Arena(1024);

            var segment1 = arena.Allocate<byte>(1000);
            var segment2 = arena.Allocate<byte>(24);

            Assert.False(arena.TryAllocate<byte>(1, out _));
        }

        [Fact]
        public void Reset_should_increment_ResetCount()
        {
            using var arena = new Arena(1024);

            arena.Reset();
            arena.Reset();

            Assert.Equal(2, arena.ResetCount);
        }
    }
}
