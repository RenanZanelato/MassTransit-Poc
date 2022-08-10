using System;

namespace Playground.Messages
{
    public class SampleItensCreated
    {
        public Guid Id { get; set; }
        public SampleExampleItem Sample { get; set; }
        public object Item { get; set; }
    }

    [Serializable]
    public class SampleItem
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Example { get; set; }
        public SampleExampleItem Sample { get; set; }
    }
    
    public enum SampleExampleItem
    {
        LittleSample = 1,
        BigSample = 2,
        MediumSample = 3
    }
}
