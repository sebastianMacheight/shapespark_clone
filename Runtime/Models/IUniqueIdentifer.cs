
namespace ReupVirtualTwin.models
{
    public interface IUniqueIdentifer
    {
        public string getId();
        public bool isIdCorrect(string id);
    }
}
