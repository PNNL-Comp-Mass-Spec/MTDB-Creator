namespace MTDBFramework.Data
{
    /// <summary>
    /// Post Translational Modification data container
    /// </summary>
    public class PostTranslationalModification
    {
        /// <summary>
        /// Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Modification Location
        /// </summary>
        public int Location { get; set; }

        /// <summary>
        /// Modification Formula
        /// </summary>
        public string Formula { get; set; }

        /// <summary>
        /// Modification Mass
        /// </summary>
        public double Mass { get; set; }

        /// <summary>
        /// Short name of the PTM
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Target containing the modification
        /// </summary>
        public ConsensusTarget Parent { get; set; }

        /// <summary>
        /// Overloaded string for debugging output
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            var temp = "Name:" + Name + ";";
            temp += "Mass:" + Mass + ";";
            temp += "Formula:" + Formula;
            return temp;
        }
    }
}
