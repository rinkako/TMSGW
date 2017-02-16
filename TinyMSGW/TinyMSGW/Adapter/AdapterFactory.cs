using TinyMSGW.Utils;

namespace TinyMSGW.Adapter
{
    /// <summary>
    /// 适配器产生器
    /// </summary>
    internal static class AdapterFactory
    {
        /// <summary>
        /// 初始化适配器
        /// </summary>
        /// <returns>是否为替换操作</returns>
        public static bool InitAdapter()
        {
            if (AdapterFactory.syncObject == null)
            {
                // TODO: 当前只考虑online态
                AdapterFactory.syncObject = new OnlineAdapterImpl();
                LogUtil.Log("ACK: Init Online Adapter.");
                //if (GlobalDataPackage.RunType == Enums.RunTypeEnum.Local)
                //{
                //    AdapterFactory.syncObject = new LocalAdapterImpl();
                //    LogUtil.Log("ACK: Init Local Adapter.");
                //}
                //else
                //{
                //    AdapterFactory.syncObject = new OnlineAdapterImpl();
                //    LogUtil.Log("ACK: Init Online Adapter.");
                //}
                return false;
            }
            return true;
        }

        /// <summary>
        /// 获取适配器
        /// </summary>
        /// <returns>适配器的全局唯一实例</returns>
        public static IActionAdapter GetAdapter()
        {
            return AdapterFactory.syncObject;
        }

        /// <summary>
        /// 全局适配器实例
        /// </summary>
        private static IActionAdapter syncObject = null;
    }
}
