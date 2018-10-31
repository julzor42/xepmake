namespace Builder.Astralis.Descriptors
{
    public class Board : Descriptor
    {
        #region Members
        public CPU Processor => Catalog.FindProcessor(DescriptionAttribute("Processor"));
        #endregion
    }
}
