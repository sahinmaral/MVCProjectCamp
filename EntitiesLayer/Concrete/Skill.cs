namespace EntityLayer.Concrete
{
    public class Skill
    {
        public int SkillId { get; set; }

        public int UserId { get; set; }
        public virtual User User { get; set; }

        public int SkillPercent { get; set; }

        public string SkillName { get; set; }
    }
}
