using JsonLib.Interfaces;

namespace LobbyLib.ItemStuff
{
    public static class RigHelper
    {
        public static bool CheckCompatibleArmorPlate(this IRig rig, string ArmorPlateId)
        {
            if (rig == null) 
                return false;

            // If this emtpy means we accept every plate!
            if (rig.ArmorPlateAccepted.Count == 0)
                return true;

            //  alternatively we check if it contains
            return rig.ArmorPlateAccepted.Contains(ArmorPlateId);
        }

        public static bool CheckCompatibleItemType(this IRig rig, string ItemId)
        {
            if (rig == null)
                return false;

            // If this emtpy means we accept item!
            if (rig.ItemTypesAccepted.Count == 0)
                return true;

            //  alternatively we check if it contains
            return rig.SpecificItemsAccepted.Contains(ItemId);
        }

        public static bool CheckCompatibleItem(this IRig rig, string ItemId)
        {
            if (rig == null) 
                return false;

            // If this emtpy means we accept every plate!
            if (rig.SpecificItemsAccepted.Count == 0)
                return true;

            //  alternatively we check if it contains
            return rig.SpecificItemsAccepted.Contains(ItemId);
        }


        public static bool TrySetArmorPlate(this IRig rig, string ArmorPlateId)
        {
            if (rig == null) return false;

            if (!rig.CheckCompatibleArmorPlate(ArmorPlateId))
                return false;

            rig.PlateSlotId = ArmorPlateId;
            return true;
        }
    }
}
