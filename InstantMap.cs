using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

// ReSharper disable once CheckNamespace
[UsedImplicitly]
public class InstantMap : Mod
{
    private const string ModName = "InstantMap";
    
    private const string CommandName = "instantmap";
    private const string CommandDocumentation = "Unlocks the map.";
    private const string CommandParameterSimple = "simple";
    private const string CommandParameterFull = "full";

    [UsedImplicitly]
    public void Start()
    {
        Debug.Log(string.Format("Mod {0} has been loaded!", ModName));
    }

    [UsedImplicitly]
    [ConsoleCommand(CommandName, CommandDocumentation)]
    public static void Command(string[] args)
    {
        if (!ValidateArguments(args))
        {
            Debug.Log("Please use either \n" +
                      "- \"instantmap simple\" - Unlock the map only\n" +
                      "- \"instantmap full\" - Unlock the map including all places of interest");
            return;
        }

        var argument = args[0].ToLower();
        switch (argument)
        {
            case CommandParameterSimple:
            {
                ActivateMap();
                break;
            }
            case CommandParameterFull:
            {
                ActivateMap();
                UnlockPois();
                break;
            }
            default:
            {
                throw new ArgumentOutOfRangeException("args");
            }
        }

    }

    private static bool ValidateArguments(IReadOnlyList<string> args)
    {
        return args != null && 
               args.Count > 0 &&
               (CommandParameterSimple.Equals(args[0], StringComparison.OrdinalIgnoreCase) ||
               CommandParameterFull.Equals(args[0], StringComparison.OrdinalIgnoreCase));
    }

    private static void ActivateMap()
    {
        var mapTab = MapTab.Get();
        if (mapTab == null || mapTab.m_MapDatas.Count == 0)
        {
            Debug.Log(string.Format("{0}: Map could not be unlocked.", ModName));
            return;
        }

        foreach (var map in mapTab.m_MapDatas)
        {
            if (map.Value.m_Unlocked)
            {
                continue;
            }

            mapTab.UnlockPage(map.Key);
        }

        Debug.Log(string.Format("{0}: Map unlocked.", ModName));
    }

    private static void UnlockPois()
    {
        var mapTab = MapTab.Get();
        if (mapTab == null)
        {
            Debug.Log(string.Format("{0}: Places of interest could not be unlocked.", ModName));
            return;
        }

        foreach (var map in mapTab.m_MapDatas)
        {
            foreach (var mapElement in map.Value.m_Elemets)
            {
                mapTab.UnlockElement(mapElement.name);
            }
        }

        Debug.Log(string.Format("{0}: Places of interest unlocked.", ModName));
    }

    [UsedImplicitly]
    public void OnModUnload()
    {
        Debug.Log(string.Format("Mod {0} has been unloaded!", ModName));
    }
}