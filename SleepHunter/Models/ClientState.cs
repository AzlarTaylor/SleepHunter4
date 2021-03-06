﻿using System;
using System.Diagnostics;
using System.IO;
using System.Text;

using SleepHunter.Common;
using SleepHunter.IO.Process;

namespace SleepHunter.Models
{
  public sealed class ClientState : ObservableObject
  {
    static readonly string ActivePanelKey = @"ActivePanel";
    static readonly string InventoryExpandedKey = @"InventoryExpanded";
    static readonly string MinimizedModeKey = @"MinimizedMode";
    static readonly string DialogOpenKey = @"DialogOpen";
    static readonly string SenseOpenKey = @"SenseOpen";
    static readonly string UserChattingKey = @"UserChatting";

    Player owner;
    string versionKey;
    InterfacePanel activePanel;
    bool isInventoryExpanded;
    bool isMinimizedMode;
    bool isDialogOpen;
    bool isSenseOpen;
    bool isUserChatting;

    public Player Owner
    {
      get { return owner; }
      set { SetProperty(ref owner, value); }
    }

    public string VersionKey
    {
      get { return versionKey; }
      set { SetProperty(ref versionKey, value); }
    }

    public InterfacePanel ActivePanel
    {
      get { return activePanel; }
      set { SetProperty(ref activePanel, value); }
    }

    public bool IsInventoryExpanded
    {
      get { return isInventoryExpanded; }
      set { SetProperty(ref isInventoryExpanded, value); }
    }

    public bool IsMinimizedMode
    {
      get { return isMinimizedMode; }
      set { SetProperty(ref isMinimizedMode, value); }
    }

    public bool IsDialogOpen
    {
      get { return isDialogOpen; }
      set { SetProperty(ref isDialogOpen, value); }
    }

    public bool IsSenseOpen
    {
      get { return isSenseOpen; }
      set { SetProperty(ref isSenseOpen, value); }
    }

    public bool IsUserChatting
    {
      get { return isUserChatting; }
      set { SetProperty(ref isUserChatting, value); }
    }

    public ClientState()
       : this(null) { }

    public ClientState(Player owner)
    {
      this.owner = owner;
    }

    public void Update()
    {
      if (owner == null)
        throw new InvalidOperationException("Player owner is null, cannot update.");

      Update(owner.Accessor);
    }

    public void Update(ProcessMemoryAccessor accessor)
    {
      if (accessor == null)
        throw new ArgumentNullException("accessor");

      var version = Owner.Version;

      if (version == null)
      {
        ResetDefaults();
        return;
      }

      var activePanelVariable = version.GetVariable(ActivePanelKey);
      var inventoryExpandedVariable = version.GetVariable(InventoryExpandedKey);
      var minimizedModeVariable = version.GetVariable(MinimizedModeKey);
      var dialogOpenVariable = version.GetVariable(DialogOpenKey);
      var senseOpenVariable = version.GetVariable(SenseOpenKey);
      var userChattingVariable = version.GetVariable(UserChattingKey);

      byte activePanelByte;
      bool isInventoryExpanded;
      bool isMinimizedMode;
      bool isDialogOpen;
      bool isUserChatting;

      Debug.WriteLine($"Updating client state (pid={accessor.ProcessId})...");

      Stream stream = null;
      try
      {
        stream = accessor.GetStream();
        using (var reader = new BinaryReader(stream, Encoding.ASCII))
        {
          stream = null;

          if (activePanelVariable != null && activePanelVariable.TryReadByte(reader, out activePanelByte))
            ActivePanel = (InterfacePanel)activePanelByte;
          else
            ActivePanel = InterfacePanel.Unknown;

          if (inventoryExpandedVariable != null && inventoryExpandedVariable.TryReadBoolean(reader, out isInventoryExpanded))
            IsInventoryExpanded = isInventoryExpanded;
          else
            IsInventoryExpanded = false;

          if (minimizedModeVariable != null && minimizedModeVariable.TryReadBoolean(reader, out isMinimizedMode))
            IsMinimizedMode = isMinimizedMode;
          else
            IsMinimizedMode = false;

          if (dialogOpenVariable != null && dialogOpenVariable.TryReadBoolean(reader, out isDialogOpen))
            IsDialogOpen = isDialogOpen;
          else
            IsDialogOpen = false;

          if (senseOpenVariable != null && senseOpenVariable.TryReadBoolean(reader, out isSenseOpen))
            IsSenseOpen = isSenseOpen;
          else
            IsSenseOpen = false;

          if (userChattingVariable != null && userChattingVariable.TryReadBoolean(reader, out isUserChatting))
            IsUserChatting = isUserChatting;
          else
            IsUserChatting = false;
        }
      }
      finally { stream?.Dispose(); }

      Debug.WriteLine($"ActivePanel = {ActivePanel}");
      Debug.WriteLine($"IsInventoryExpanded = {IsInventoryExpanded}");
      Debug.WriteLine($"IsMinimizedMode = {IsMinimizedMode}");
      Debug.WriteLine($"IsDialogOpen = {IsDialogOpen}");
      Debug.WriteLine($"IsSenseOpen = {IsSenseOpen}");
      Debug.WriteLine($"IsUserChatting = {IsUserChatting}");
    }

    public void ResetDefaults()
    {
      ActivePanel = InterfacePanel.Unknown;
      IsInventoryExpanded = false;
      IsMinimizedMode = false;
      IsDialogOpen = false;
      IsUserChatting = false;
    }
  }
}
