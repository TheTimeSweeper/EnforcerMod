using RoR2;
using System.Collections.Generic;

namespace Modules.Characters {
    public abstract class ItemDisplaysBase {

        public virtual bool printUnused { get; }

        public void SetItemDIsplays(ItemDisplayRuleSet itemDisplayRuleSet) {

            List<ItemDisplayRuleSet.KeyAssetRuleGroup> itemDisplayRules = new List<ItemDisplayRuleSet.KeyAssetRuleGroup>();
            
            //if(printUnused)
            //    ItemDisplays.recordUnused();

            SetItemDisplayRules(itemDisplayRules);

            if(printUnused)
                ItemDisplays.printUnused();

            itemDisplayRuleSet.keyAssetRuleGroups = itemDisplayRules.ToArray();
            itemDisplayRuleSet.GenerateRuntimeValues();
        }

        protected abstract void SetItemDisplayRules(List<ItemDisplayRuleSet.KeyAssetRuleGroup> itemDisplayRules);
    }
}