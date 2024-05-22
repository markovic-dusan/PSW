using System.ComponentModel.DataAnnotations;

namespace PSW_Dusan_Markovic.resources.model
{
    public class Interest
    {
        public EnumInterest InterestValue { get; set; }

        public Interest() { }
        public Interest(EnumInterest interestValue) {  InterestValue = interestValue; }


    }
}
