namespace GameProject.Character
{
    public interface ICharacterToolInput
    {
        public bool IsUse { get; set; }
        public bool IsDrop { get; set; }
        public bool IsSwitchingSlot { get; set; }
        public int SlotIndex { get; set; }
    }
}