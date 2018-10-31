using Builder.Astralis.Generators;

namespace Builder.Astralis
{
    public interface IMakefileBlock
    {
        string BuilderName { get; }

        void Process(MakefileGenerator generator);
    }
}
