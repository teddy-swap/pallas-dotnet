using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace PallasDotnetRs
{    
    public static class PallasDotnetRs
    {
        public class RustException: Exception {
            public RustException(string message) : base(message) { }
        }

        public interface IOpaqueHandle: IEquatable<IOpaqueHandle>, IDisposable {}

        
        public struct NetworkMagic {
        }
        public struct Point {
            public ulong slot;
            public List<byte> hash;
        }
        public struct Block {
            public ulong slot;
            public List<byte> hash;
            public ulong number;
        }
        public struct NextResponse {
            public byte action;
            public Block tip;
            public Block block;
        }
        public struct NodeClientWrapper {
            public UIntPtr clientPtr;
            public UIntPtr chainSyncPtr;
        }
        public static ulong MainnetMagic(
        ) {
            return _FnMainnetMagic();
        }
        public static ulong TestnetMagic(
        ) {
            return _FnTestnetMagic();
        }
        public static ulong PreviewMagic(
        ) {
            return _FnPreviewMagic();
        }
        public static ulong PreProductionMagic(
        ) {
            return _FnPreProductionMagic();
        }
        public static NodeClientWrapper Connect(
            string socketPath,
            ulong networkMagic
        ) {
            return (_FnConnect(_AllocStr(socketPath),networkMagic)).Decode();
        }
        public static Point FindIntersect(
            NodeClientWrapper clientWrapper,
            Point knownPoint
        ) {
            return _DecodeOption(_FnFindIntersect(_StructNodeClientWrapper.Encode(clientWrapper),_StructPoint.Encode(knownPoint)), _arg1 => (_arg1).Decode());
        }
        public static NextResponse ChainSyncNext(
            NodeClientWrapper clientWrapper
        ) {
            return _DecodeOption(_FnChainSyncNext(_StructNodeClientWrapper.Encode(clientWrapper)), _arg2 => (_arg2).Decode());
        }
        public static bool ChainSyncHasAgency(
            NodeClientWrapper clientWrapper
        ) {
            return (_FnChainSyncHasAgency(_StructNodeClientWrapper.Encode(clientWrapper)) != 0);
        }
        public static void Disconnect(
            NodeClientWrapper clientWrapper
        ) {
            _FnDisconnect(_StructNodeClientWrapper.Encode(clientWrapper));
        }
        [StructLayout(LayoutKind.Sequential)]
        private struct _StructNetworkMagic {
            public static _StructNetworkMagic Encode(NetworkMagic structArg) {
                return new _StructNetworkMagic {
                };
            }
            public NetworkMagic Decode() {
                return new NetworkMagic {
                };
            }
        }
        [StructLayout(LayoutKind.Sequential)]
        private struct _StructPoint {
            public ulong slot;
            public _RawSlice hash;
            public static _StructPoint Encode(Point structArg) {
                return new _StructPoint {
                    slot = structArg.slot,
                    hash = _AllocSlice<byte, byte>(structArg.hash, 1, 1, _arg3 => _arg3)
                };
            }
            public Point Decode() {
                return new Point {
                    slot = this.slot,
                    hash = _FreeSlice<byte, byte, List<byte>>(this.hash, 1, 1, _arg4 => _arg4)
                };
            }
        }
        [StructLayout(LayoutKind.Sequential)]
        private struct _StructBlock {
            public ulong slot;
            public _RawSlice hash;
            public ulong number;
            public static _StructBlock Encode(Block structArg) {
                return new _StructBlock {
                    slot = structArg.slot,
                    hash = _AllocSlice<byte, byte>(structArg.hash, 1, 1, _arg5 => _arg5),
                    number = structArg.number
                };
            }
            public Block Decode() {
                return new Block {
                    slot = this.slot,
                    hash = _FreeSlice<byte, byte, List<byte>>(this.hash, 1, 1, _arg6 => _arg6),
                    number = this.number
                };
            }
        }
        [StructLayout(LayoutKind.Sequential)]
        private struct _StructNextResponse {
            public byte action;
            public _RawTuple0 tip;
            public _RawTuple0 block;
            public static _StructNextResponse Encode(NextResponse structArg) {
                return new _StructNextResponse {
                    action = structArg.action,
                    tip = _EncodeOption(structArg.tip, _arg7 => _StructBlock.Encode(_arg7)),
                    block = _EncodeOption(structArg.block, _arg8 => _StructBlock.Encode(_arg8))
                };
            }
            public NextResponse Decode() {
                return new NextResponse {
                    action = this.action,
                    tip = _DecodeOption(this.tip, _arg9 => (_arg9).Decode()),
                    block = _DecodeOption(this.block, _arg10 => (_arg10).Decode())
                };
            }
        }
        [StructLayout(LayoutKind.Sequential)]
        private struct _StructNodeClientWrapper {
            public UIntPtr clientPtr;
            public UIntPtr chainSyncPtr;
            public static _StructNodeClientWrapper Encode(NodeClientWrapper structArg) {
                return new _StructNodeClientWrapper {
                    clientPtr = structArg.clientPtr,
                    chainSyncPtr = structArg.chainSyncPtr
                };
            }
            public NodeClientWrapper Decode() {
                return new NodeClientWrapper {
                    clientPtr = this.clientPtr,
                    chainSyncPtr = this.chainSyncPtr
                };
            }
        }
        [DllImport("pallas_dotnet_rs", EntryPoint = "rnet_export_mainnet_magic", CallingConvention = CallingConvention.Cdecl)]
        private static extern ulong _FnMainnetMagic(
        );
        [DllImport("pallas_dotnet_rs", EntryPoint = "rnet_export_testnet_magic", CallingConvention = CallingConvention.Cdecl)]
        private static extern ulong _FnTestnetMagic(
        );
        [DllImport("pallas_dotnet_rs", EntryPoint = "rnet_export_preview_magic", CallingConvention = CallingConvention.Cdecl)]
        private static extern ulong _FnPreviewMagic(
        );
        [DllImport("pallas_dotnet_rs", EntryPoint = "rnet_export_pre_production_magic", CallingConvention = CallingConvention.Cdecl)]
        private static extern ulong _FnPreProductionMagic(
        );
        [DllImport("pallas_dotnet_rs", EntryPoint = "rnet_export_connect", CallingConvention = CallingConvention.Cdecl)]
        private static extern _StructNodeClientWrapper _FnConnect(
            _RawSlice socketPath,
            ulong networkMagic
        );
        [DllImport("pallas_dotnet_rs", EntryPoint = "rnet_export_find_intersect", CallingConvention = CallingConvention.Cdecl)]
        private static extern _RawTuple1 _FnFindIntersect(
            _StructNodeClientWrapper clientWrapper,
            _StructPoint knownPoint
        );
        [DllImport("pallas_dotnet_rs", EntryPoint = "rnet_export_chain_sync_next", CallingConvention = CallingConvention.Cdecl)]
        private static extern _RawTuple2 _FnChainSyncNext(
            _StructNodeClientWrapper clientWrapper
        );
        [DllImport("pallas_dotnet_rs", EntryPoint = "rnet_export_chain_sync_has_agency", CallingConvention = CallingConvention.Cdecl)]
        private static extern byte _FnChainSyncHasAgency(
            _StructNodeClientWrapper clientWrapper
        );
        [DllImport("pallas_dotnet_rs", EntryPoint = "rnet_export_disconnect", CallingConvention = CallingConvention.Cdecl)]
        private static extern void _FnDisconnect(
            _StructNodeClientWrapper clientWrapper
        );
        [StructLayout(LayoutKind.Sequential)]
        private struct _RawTuple0 {
            public _StructBlock elem0;
            public byte elem1;
        }
        private static _RawTuple0 _EncodeOption<T>(T arg, Func<T, _StructBlock> converter) {
            if (arg != null) {
                return new _RawTuple0 { elem0 = converter(arg), elem1 = 1 };
            } else {
                return new _RawTuple0 { elem0 = default(_StructBlock), elem1 = 0 };
            }
        }
        private static T _DecodeOption<T>(_RawTuple0 arg, Func<_StructBlock, T> converter) {
            if (arg.elem1 != 0) {
                return converter(arg.elem0);
            } else {
                return default(T);
            }
        }
        [StructLayout(LayoutKind.Sequential)]
        private struct _RawTuple1 {
            public _StructPoint elem0;
            public byte elem1;
        }
        private static _RawTuple1 _EncodeOption<T>(T arg, Func<T, _StructPoint> converter) {
            if (arg != null) {
                return new _RawTuple1 { elem0 = converter(arg), elem1 = 1 };
            } else {
                return new _RawTuple1 { elem0 = default(_StructPoint), elem1 = 0 };
            }
        }
        private static T _DecodeOption<T>(_RawTuple1 arg, Func<_StructPoint, T> converter) {
            if (arg.elem1 != 0) {
                return converter(arg.elem0);
            } else {
                return default(T);
            }
        }
        [StructLayout(LayoutKind.Sequential)]
        private struct _RawTuple2 {
            public _StructNextResponse elem0;
            public byte elem1;
        }
        private static _RawTuple2 _EncodeOption<T>(T arg, Func<T, _StructNextResponse> converter) {
            if (arg != null) {
                return new _RawTuple2 { elem0 = converter(arg), elem1 = 1 };
            } else {
                return new _RawTuple2 { elem0 = default(_StructNextResponse), elem1 = 0 };
            }
        }
        private static T _DecodeOption<T>(_RawTuple2 arg, Func<_StructNextResponse, T> converter) {
            if (arg.elem1 != 0) {
                return converter(arg.elem0);
            } else {
                return default(T);
            }
        }


        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void _ManageDelegateDelegate(IntPtr ptr, int adjust);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void _DropOpaqueDelegate(IntPtr ptr);

        private static Dictionary<IntPtr, (int, Delegate, Delegate)> _ActiveDelegates = new Dictionary<IntPtr, (int, Delegate, Delegate)>();

        private static _ManageDelegateDelegate _manageDelegate = _ManageDelegate;
        private static IntPtr _manageDelegatePtr = Marshal.GetFunctionPointerForDelegate(_manageDelegate);

        private static void _ManageDelegate(IntPtr ptr, int adjust)
        {
            lock (_ActiveDelegates)
            {
                var item = _ActiveDelegates[ptr];
                item.Item1 += adjust;
                if (item.Item1 > 0)
                {
                    _ActiveDelegates[ptr] = item;
                }
                else
                {
                    _ActiveDelegates.Remove(ptr);
                }
            }
        }

        private static _RawDelegate _AllocDelegate(Delegate d, Delegate original)
        {
            var ptr = Marshal.GetFunctionPointerForDelegate(d);
            lock (_ActiveDelegates)
            {
                if (_ActiveDelegates.ContainsKey(ptr))
                {
                    var item = _ActiveDelegates[ptr];
                    item.Item1 += 1;
                    _ActiveDelegates[ptr] = item;
                } else
                {
                    _ActiveDelegates.Add(ptr, (1, d, original));
                }
            }
            return new _RawDelegate
            {
                call_fn = ptr,
                drop_fn = _manageDelegatePtr,
            };
        }

        private static Delegate _FreeDelegate(_RawDelegate d)
        {
            var ptr = d.call_fn;
            lock (_ActiveDelegates)
            {
                var item = _ActiveDelegates[ptr];
                item.Item1 -= 1;
                if (item.Item1 > 0)
                {
                    _ActiveDelegates[ptr] = item;
                }
                else
                {
                    _ActiveDelegates.Remove(ptr);
                }
                return item.Item3;
            }
        }

        [DllImport("pallas_dotnet_rs", EntryPoint = "rnet_alloc", CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr _Alloc( UIntPtr size, UIntPtr align);

        [DllImport("pallas_dotnet_rs", EntryPoint = "rnet_free", CallingConvention = CallingConvention.Cdecl)]
        private static extern void _Free(IntPtr ptr, UIntPtr size, UIntPtr align);

        [StructLayout(LayoutKind.Sequential)]
        private struct _RawSlice
        {
            public IntPtr ptr;
            public UIntPtr len;

            public static _RawSlice Alloc(UIntPtr len, int size, int align)
            {
                if (len == UIntPtr.Zero)
                {
                    return new _RawSlice {
                        ptr = (IntPtr)align,
                        len = UIntPtr.Zero,
                    };
                } else
                {
                    return new _RawSlice
                    {
                        ptr = _Alloc((UIntPtr)((UInt64)len * (UInt64)size), (UIntPtr)align),
                        len = len,
                    };
                }
            }

            public void Free(int size, int align)
            {
                if (len != UIntPtr.Zero)
                {
                    _Free(ptr, (UIntPtr)((UInt64)len * (UInt64)size), (UIntPtr)align);
                    ptr = (IntPtr)1;
                    len = UIntPtr.Zero;
                }
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct _RawOpaqueHandle
        {
            public IntPtr ptr;
            public IntPtr drop_fn;
            public ulong type_id;

            public void Drop()
            {
                if (ptr != IntPtr.Zero)
                {
                    var drop = Marshal.GetDelegateForFunctionPointer<_DropOpaqueDelegate>(drop_fn);
                    drop(ptr);
                    ptr = IntPtr.Zero;
                }
            }
        }

        private class _OpaqueHandle : IOpaqueHandle
        {
            private _RawOpaqueHandle inner;

            public _OpaqueHandle(_RawOpaqueHandle inner)
            {
                this.inner = inner;
            }

            public _RawOpaqueHandle ToInner(ulong type_id)
            {
                if (type_id != inner.type_id)
                {
                    throw new InvalidCastException("Opaque handle does not have the correct type");
                }
                return this.inner;
            }

            ~_OpaqueHandle()
            {
                inner.Drop();
            }

            public override bool Equals(object obj)
            {
                return Equals(obj as _OpaqueHandle);
            }

            public bool Equals(IOpaqueHandle other)
            {
                var casted = other as _OpaqueHandle;
                return casted != null &&
                       inner.ptr == casted.inner.ptr && inner.type_id == casted.inner.type_id;
            }

            public override int GetHashCode()
            {
                return inner.ptr.GetHashCode() + inner.type_id.GetHashCode();
            }

            public void Dispose()
            {
                inner.Drop();
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct _RawDelegate
        {
            public IntPtr call_fn;
            public IntPtr drop_fn;
        }

        private static IntPtr _AllocBox<T>(T arg, int size, int align)
        {
            if (size > 0) {
                var ptr = _Alloc((UIntPtr)size, (UIntPtr)align);
                Marshal.StructureToPtr(arg, ptr, false);
                return ptr;
            } else {
                return (IntPtr)align;
            }
        }

        private static _RawSlice _AllocStr(string arg)
        {
            var nb = Encoding.UTF8.GetByteCount(arg);
            var slice = _RawSlice.Alloc((UIntPtr)nb, 1, 1);
            unsafe
            {
                fixed (char* firstChar = arg)
                {
                    nb = Encoding.UTF8.GetBytes(firstChar, arg.Length, (byte*)slice.ptr, nb);
                }
            }
            return slice;
        }

        private static _RawSlice _AllocSlice<T, U>(IReadOnlyCollection<T> collection, int size, int align, Func<T, U> converter) {
            var count = collection.Count;
            var slice = _RawSlice.Alloc((UIntPtr)count, size, align);
            var ptr = slice.ptr;
            foreach (var item in collection) {
                Marshal.StructureToPtr(converter(item), ptr, false);
                ptr = (IntPtr)(ptr.ToInt64() + (long)size);
            }
            return slice;
        }

        private static _RawSlice _AllocDict<TKey, TValue, U>(IReadOnlyDictionary<TKey, TValue> collection, int size, int align, Func<(TKey, TValue), U> converter) where U: unmanaged
        {
            var count = collection.Count;
            var slice = _RawSlice.Alloc((UIntPtr)count, size, align);
            var ptr = slice.ptr;
            foreach (var item in collection)
            {
                Marshal.StructureToPtr<U>(converter((item.Key, item.Value)), ptr, false);
                ptr = (IntPtr)(ptr.ToInt64() + (long)size);
            }
            return slice;
        }

        private static T _FreeBox<T>(IntPtr ptr, int size, int align)
        {
            var res = Marshal.PtrToStructure<T>(ptr);
            if (size > 0) {
                _Free(ptr, (UIntPtr)size, (UIntPtr)align);
            }
            return res;
        }

        private static String _FreeStr(_RawSlice arg)
        {
            unsafe
            {
                var res = Encoding.UTF8.GetString((byte*)arg.ptr, (int)arg.len);
                arg.Free(1, 1);
                return res;
            }
        }

        private static TList _FreeSlice<T, U, TList>(_RawSlice arg, int size, int align, Func<U, T> converter) where TList: ICollection<T>, new()
        {
            unsafe
            {
                var res = new TList();
                var ptr = arg.ptr;
                for (var i = 0; i < (int)arg.len; ++i) {
                    res.Add(converter(Marshal.PtrToStructure<U>(ptr)));
                    ptr = (IntPtr)(ptr.ToInt64() + (long)size);
                }
                arg.Free(size, align);
                return res;
            }
        }

        private static TDict _FreeDict<TKey, TValue, U, TDict>(_RawSlice arg, int size, int align, Func<U, (TKey, TValue)> converter) where U : unmanaged where TDict: IDictionary<TKey, TValue>, new()
        {
            unsafe
            {
                var res = new TDict();
                var ptr = arg.ptr;
                for (var i = 0; i < (int)arg.len; ++i)
                {
                    var item = converter(Marshal.PtrToStructure<U>(ptr));
                    res.Add(item.Item1, item.Item2);
                    ptr = (IntPtr)(ptr.ToInt64() + (long)size);
                }
                arg.Free(size, align);
                return res;
            }
        }
    }
}
