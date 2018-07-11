using System;

namespace Tar.Core.Compression
{
    /// <summary>
    /// ZipComponentFactory
    /// </summary>
    public static class ZipComponentFactory
    {
        /// <summary>
        /// Creates Default Zip Component.
        /// </summary>
        /// <returns></returns>
        public static IZipComponent CreateZipComponent()
        {
            return CreateZipComponent(ZipComponentType.DotNetZip);
        }

        /// <summary>
        /// Creates Zip Component.
        /// </summary>
        /// <param name="componentType"></param>
        /// <returns></returns>
        public static IZipComponent CreateZipComponent(ZipComponentType componentType)
        {
            if (componentType == ZipComponentType.DotNetZip)
                return new DotNetZipComponent();

            if (componentType == ZipComponentType.FastZip)
                return new FastZipComponent();

            throw new Exception(string.Format("Desteklenmeyen zip component tipi:{0}", componentType));
        }
    }
}