using System;
using System.Runtime.InteropServices;

namespace Mindscape.Raygun4Net.Helpers
{
  public class LocalComputerInfo
  {
    private InternalMemoryStatus _internalMemoryStatus = null;

    public ulong TotalPhysicalMemory
    {
      get
      {
        return MemoryStatus.TotalPhysicalMemory;
      }
    }

    public ulong AvailablePhysicalMemory
    {
      get
      {
        return MemoryStatus.AvailablePhysicalMemory;
      }
    }

    public ulong TotalVirtualMemory
    {
      get
      {
        return MemoryStatus.TotalVirtualMemory;
      }
    }

    public ulong AvailableVirtualMemory
    {
      get
      {
        return this.MemoryStatus.AvailableVirtualMemory;
      }
    }

    private InternalMemoryStatus MemoryStatus
    {
      get
      {
        if (_internalMemoryStatus == null)
        {
          _internalMemoryStatus = new InternalMemoryStatus();
        }

        return _internalMemoryStatus;
      }
    }

    private class InternalMemoryStatus
    {
      private NativeMethods.MEMORYSTATUSEX _memoryStatusEx;

      internal ulong TotalPhysicalMemory
      {
        get
        {
          Refresh();
          return _memoryStatusEx.ullTotalPhys;
        }
      }

      internal ulong AvailablePhysicalMemory
      {
        get
        {
          Refresh();
          return _memoryStatusEx.ullAvailPhys;
        }
      }

      internal ulong TotalVirtualMemory
      {
        get
        {
          Refresh();
          return _memoryStatusEx.ullTotalVirtual;
        }
      }

      internal ulong AvailableVirtualMemory
      {
        get
        {
          Refresh();
          return _memoryStatusEx.ullAvailVirtual;
        }
      }

      private void Refresh()
      {
        _memoryStatusEx = new NativeMethods.MEMORYSTATUSEX();
        _memoryStatusEx.Init();

        try
        {
          if (!NativeMethods.GlobalMemoryStatusEx(ref this._memoryStatusEx))
          {
            throw new InvalidOperationException();
          }
        }
        catch (EntryPointNotFoundException)
        {
        }
      }
    }
  }

  [ComVisible(false)]
  internal sealed class NativeMethods
  {
    [DllImport("Kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    internal static extern bool GlobalMemoryStatusEx(ref NativeMethods.MEMORYSTATUSEX lpBuffer);

    internal struct MEMORYSTATUSEX
    {
      internal uint dwLength;
      internal uint dwMemoryLoad;
      internal ulong ullTotalPhys;
      internal ulong ullAvailPhys;
      internal ulong ullTotalPageFile;
      internal ulong ullAvailPageFile;
      internal ulong ullTotalVirtual;
      internal ulong ullAvailVirtual;
      internal ulong ullAvailExtendedVirtual;

      internal void Init()
      {
        dwLength = checked((uint)Marshal.SizeOf(typeof(NativeMethods.MEMORYSTATUSEX)));
      }
    }
  }
}
