namespace Core.Enums
{
    public class E_DouglasEnums
    {
        public enum StudentType
        {
            Regular = 1,
            WorkingClass = 2
        }

        public enum ProfileCompletionStage
        {
            Basic = 0,
            BioData = 1,
            Documents = 2,
            Complete = 3
        }

        public enum ContentType
        {
            Video = 1,
            Pdf = 2,
            Image = 3
        }

        public enum EnrollmentStatus
        {
            Pending = 1,
            Active = 2,
            Completed = 3,
            Cancelled = 4
        }

        public enum PaymentStatus
        {
            Pending = 1,
            Paid = 2,
            Failed = 3
        }

        public enum PaymentChannel
        {
            Paystack = 1,
            Flutterwave = 2,
            Manual = 3
        }

    }
}
