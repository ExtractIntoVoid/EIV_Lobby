using JsonLib.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LobbyLib.ItemStuff
{
    public static class GunHelper
    {

        public static bool CheckMagazineCompatible(this IGun gun, string MagazineId)
        {
            if (gun == null)
                return false;
            return gun.MagazineSupport.Contains(MagazineId);
        }

        public static bool TryInsertMagazine(this IGun gun, string MagazineId, string AmmoId, uint AmmoToInsert)
        {
            if (gun == null)
                return false;

            if (!gun.CheckMagazineCompatible(MagazineId))
                return false;


            var magazine = ItemMaker.CreateItem<IMagazine>(MagazineId);
            if (magazine == null)
                return false;

            if (!magazine.TryInsertAmmos(AmmoId, AmmoToInsert))
                return false;
            gun.Magazine = magazine;
            return true;
        }

        public static bool TryInsertMagazine(this IGun gun, string MagazineId, string AmmoId)
        {
            if (gun == null)
                return false;

            if (!gun.CheckMagazineCompatible(MagazineId))
                return false;


            var magazine = ItemMaker.CreateItem<IMagazine>(MagazineId);
            if (magazine == null)
                return false;

            if (!magazine.TryInsertAmmo(AmmoId))
                return false;
            gun.Magazine = magazine;
            return true;
        }
    }
}
