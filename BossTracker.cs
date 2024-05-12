﻿using Modding;
using RandomizerMod.RandomizerData;
using RandomizerMod.Settings;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UObject = UnityEngine.Object;

namespace BossTracker
{
    public class BossTracker : Mod, IGlobalSettings<GlobalSettings>
    {
        internal static BossTracker Instance;
        public BossTracker()
        {
            Instance = this;
        }
        new public string GetName() => "BossSync";
        public override string GetVersion() => "1.0.1";

        public static GlobalSettings GS = new();
        public void OnLoadGlobal(GlobalSettings gs) => GS = gs;
        public GlobalSettings OnSaveGlobal() => GS;

        internal Dictionary<string, Func<List<VanillaDef>>> Interops = new();

        public override void Initialize()
        {
            if (ModHooks.GetMod("Randomizer 4") is not Mod || ModHooks.GetMod("ItemSync") is not Mod) return;

            ModHooks.HeroUpdateHook += OnHeroUpdate;
            ModHooks.OnReceiveDeathEventHook += OnEnemyDeath;
            //Menu.Hook();
            //Tracker.Hook();

            //if (ModHooks.GetMod("RandoSettingsManager") is not null)
            //{
            //    RSMInterop.Hook();
            //}
        }

        private void OnEnemyDeath(EnemyDeathEffects enemyDeathEffects, bool eventAlreadyReceived, ref float? attackDirection, ref bool resetDeathEvent, ref bool spellBurn, ref bool isWatery)
        {
            if (eventAlreadyReceived) return;

            Log("enemyDeathEffects name: " + enemyDeathEffects.name);
        }

        private void LogAllUnlockedBossScenes()
        {
            Log("Boss Scenes Unlocked:");
            foreach(var bossScene in PlayerData.instance.unlockedBossScenes)
            {
                Log(bossScene);
            }
        }

        public void OnHeroUpdate()
        {
            if (Input.GetKeyDown(KeyCode.O))
            {
                LogAllUnlockedBossScenes();
            }

            if (Input.GetKeyDown(KeyCode.P))
            {
                LogFalseKnight();
            }

            if (Input.GetKeyDown(KeyCode.I))
            {
                KillFalseKnight();
            }
        }

        internal void LogFalseKnight()
        {
            Log("falseKnightDefeated = " + PlayerData.instance.falseKnightDefeated);
            Log("killedFalseKnight = " + PlayerData.instance.killedFalseKnight);
            Log("killsFalseKnight = " + PlayerData.instance.killsFalseKnight);
            Log("newDataFalseKnight = " + PlayerData.instance.newDataFalseKnight);
        }

        internal void KillFalseKnight()
        {
            PlayerData.instance.SetBool("falseKnightDefeated", true);
            PlayerData.instance.SetBool("killedFalseKnight", true);
            PlayerData.instance.SetInt("killsFalseKnight", 0);
            PlayerData.instance.SetBool("newDataFalseKnight", true);
        }

        internal void CustomLog(string log)
        {
            Log(log);
        }
    }
}