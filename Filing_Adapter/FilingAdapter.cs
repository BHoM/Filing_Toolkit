using BH.Adapter;
using BH.oM.Base;
using System.IO.Abstractions;

namespace BH.Adapter.Filing
{
    public partial class FilingAdapter : BHoMAdapter
    {
        /***************************************************/
        /**** Properties                                ****/
        /***************************************************/

        public virtual IFileSystem FileSystem { get; protected set; } = new FileSystem();
        public string Path { get; private set; }

        /***************************************************/
        /**** Constructors                              ****/
        /***************************************************/

        public FilingAdapter(string path)
        {
            Path = path;
        }

        /***************************************************/

        public FilingAdapter(IFileSystem filesystem, string path)
        {
            FileSystem = filesystem;
            Path = path;
        }

        /***************************************************/
    }
}
