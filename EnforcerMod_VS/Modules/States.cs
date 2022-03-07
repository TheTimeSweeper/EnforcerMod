using EntityStates;
using MonoMod.RuntimeDetour;
using RoR2;
using RoR2.Skills;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Modules
{
    internal static class States
    {
        internal static List<Type> entityStates = new List<Type>();
        internal static List<SkillDef> skillDefs = new List<SkillDef>();
        internal static List<SkillFamily> skillFamilies = new List<SkillFamily>();

        private static Hook set_stateTypeHook;
        private static Hook set_typeNameHook;
        private static readonly BindingFlags allFlags = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.NonPublic;
        private delegate void set_stateTypeDelegate(ref SerializableEntityStateType self, Type value);
        private delegate void set_typeNameDelegate(ref SerializableEntityStateType self, String value);

        internal static void AddSkill(Type t)
        {
            entityStates.Add(t);
        }

        internal static void AddSkillDef(SkillDef i)
        {
            skillDefs.Add(i);
        }

        internal static void AddSkillFamily(SkillFamily i)
        {
            skillFamilies.Add(i);
        }

        internal static void FixStates()
        {
            Type type = typeof(SerializableEntityStateType);
            HookConfig cfg = default;
            cfg.Priority = Int32.MinValue;
            set_stateTypeHook = new Hook(type.GetMethod("set_stateType", allFlags), new set_stateTypeDelegate(SetStateTypeHook), cfg);
            set_typeNameHook = new Hook(type.GetMethod("set_typeName", allFlags), new set_typeNameDelegate(SetTypeName), cfg);
        }

        private static void SetStateTypeHook(ref this SerializableEntityStateType self, Type value)
        {
            self._typeName = value.AssemblyQualifiedName;
        }

        private static void SetTypeName(ref this SerializableEntityStateType self, String value)
        {
            Type t = GetTypeFromName(value);
            if (t != null)
            {
                self.SetStateTypeHook(t);
            }
        }

        private static Type GetTypeFromName(String name)
        {
            Type[] types = EntityStateCatalog.stateIndexToType;
            return Type.GetType(name);
        }
    }
}