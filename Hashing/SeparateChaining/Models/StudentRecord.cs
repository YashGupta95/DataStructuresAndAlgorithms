namespace SeparateChaining
{
    internal class StudentRecord
    {
        private int _studentId;
        private string _studentName;

        public StudentRecord(int studentId, string studentName)
        {
            _studentId = studentId;
            _studentName = studentName;
        }

        public int GetStudentId()
        {
            return _studentId;
        }

        public void SetStudentId(int studentId)
        {
            _studentId = studentId;
        }

        public new string ToString()
        {
            return $" {_studentId}  {_studentName} ";
        }
    }
}
