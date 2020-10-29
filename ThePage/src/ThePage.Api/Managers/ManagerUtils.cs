using MonkeyCache.LiteDB;

namespace ThePage.Api
{
    public static class ManagerUtils
    {
        public static void ClearPageBarrels(string key, string singleKey = null, string id = null)
        {
            var page = 1;
            var barrelkey = key + page;

            bool barrelFound = Barrel.Current.Exists(barrelkey);
            while (barrelFound)
            {
                Barrel.Current.Empty(barrelkey);

                page++;
                barrelkey = key + page;
                barrelFound = Barrel.Current.Exists(barrelkey);
            }

            if (singleKey != null && id != null)
            {
                singleKey += id;
                Barrel.Current.Empty(singleKey);
            }
        }
    }
}
