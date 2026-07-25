namespace DataStructures.Core.HashMap
{
    /// <summary>
    /// Represents an employee within an organization.
    /// </summary>
    public class Employee
    {
        /// <summary>
        /// Gets or sets the unique employee identifier.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the employee's name.
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the department to which the employee belongs.
        /// </summary>
        public string Department { get; set; } = string.Empty;

        public override string ToString()
        {
            return $"{Id} - {Name} ({Department})";
        }
    }
}
