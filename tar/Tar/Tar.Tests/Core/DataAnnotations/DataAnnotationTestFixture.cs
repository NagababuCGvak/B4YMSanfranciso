using System;
using System.Linq;
using NUnit.Framework;
using Tar.Core.DataAnnotations;
using Tar.ViewModel;

namespace Tar.Tests.Core.DataAnnotations
{
    [TestFixture]
    public class DataAnnotationTestFixture
    {
        [Test]
        public void Compare()
        {
            var validator = new DataAnnotationsValidatorManager();
            var entity = new CompareTestModel { NewPassword = "1", NewPasswordConfirm = "1" };
            validator.Validate(entity);
        }

        [Test, ExpectedException()]
        //[Test, ExpectedException(typeof(ValidationException))]
        public void ThrowCompare()
        {
            var validator = new DataAnnotationsValidatorManager();
            var entity = new CompareTestModel { NewPassword = "1", NewPasswordConfirm = "2" };
            validator.Validate(entity);
        }


        [Compare("NewPassword", "NewPasswordConfirm", ErrorMessage = "The new password and confirmation password do not match.")]
        public class CompareTestModel
        {
            public string NewPassword { get; set; }
            public string NewPasswordConfirm { get; set; }
        }

        [Test]
        public void Validation1()
        {
            Console.WriteLine("Validation1");
            var request = new XRequest
            {
                GeneralItemView = new GeneralItemView {Id = 1}
            };
            var validationErrorInfos = new DataAnnotationsValidatorManager().GetErrors(request);
            validationErrorInfos.ToList().ForEach(x => Console.WriteLine(x.FormatErrorMessage));
            Assert.AreEqual(0, validationErrorInfos.Count);
        }
        [Test]
        public void Validation2()
        {
            Console.WriteLine("Validation2");
            var request = new XRequest
            {
                GeneralItemView = new GeneralItemView { Id = 1 },
                XChild1 = new XChild1
                {
                    GeneralItemView = new GeneralItemView { Id = 1 }
                }
            };
            var validationErrorInfos = new DataAnnotationsValidatorManager().GetErrors(request);
            validationErrorInfos.ToList().ForEach(x => Console.WriteLine(x.FormatErrorMessage));
            Assert.AreEqual(0, validationErrorInfos.Count);
        }
        [Test]
        public void Validation3()
        {
            Console.WriteLine("Validation3");
            var request = new XRequest
            {
                GeneralItemView = new GeneralItemView { Id = 1 },
                XChild1 = new XChild1
                {
                    GeneralItemView = new GeneralItemView { Id = 1 },
                    XChild2 = new XChild2
                    {
                        GeneralItemView = new GeneralItemView { Id = 1 }
                    }
                }
            };
            var validationErrorInfos = new DataAnnotationsValidatorManager().GetErrors(request);
            validationErrorInfos.ToList().ForEach(x=>Console.WriteLine(x.FormatErrorMessage));
            Assert.AreEqual(0, validationErrorInfos.Count);
        }
        [Test]
        public void Validation4()
        {
            Console.WriteLine("Validation4");
            var request = new XRequest
            {
                GeneralItemView = new GeneralItemView { Id = 1 },
                XChild1 = new XChild1
                {
                    GeneralItemView = new GeneralItemView { Id = 1 },
                    XChild2 = new XChild2
                    {
                        GeneralItemView = new GeneralItemView { Id = 1 },
                        XChild3 = new XChild3
                        {
                            GeneralItemView = new GeneralItemView { Id = 1 }
                        }
                    }
                }
            };
            var validationErrorInfos = new DataAnnotationsValidatorManager().GetErrors(request);
            validationErrorInfos.ToList().ForEach(x => Console.WriteLine(x.FormatErrorMessage));
            Assert.AreEqual(0, validationErrorInfos.Count);
        }
        [Test]
        public void Validation5()
        {
            Console.WriteLine("Validation5");
            var request = new XRequest
            {
                GeneralItemView = new GeneralItemView { Id = 1 },
                XChild1 = new XChild1
                {
                    GeneralItemView = new GeneralItemView { Id = 1 },
                    XChild2 = new XChild2
                    {
                        GeneralItemView = new GeneralItemView { Id = 1 },
                        XChild3 = new XChild3
                        {
                            GeneralItemView = new GeneralItemView { Id = 1 },
                            XChild4 = new XChild4
                            {
                                GeneralItemView = new GeneralItemView { Id = 1 }
                            }
                        }
                    }
                }
            };
            var validationErrorInfos = new DataAnnotationsValidatorManager().GetErrors(request);
            validationErrorInfos.ToList().ForEach(x => Console.WriteLine(x.FormatErrorMessage));
            Assert.AreEqual(0, validationErrorInfos.Count);
        }
        [Test]
        public void Validation6()
        {
            Console.WriteLine("Validation6");
            var request = new XRequest
            {
                GeneralItemView = new GeneralItemView { Id = 1 },
                XChild1 = new XChild1
                {
                    GeneralItemView = new GeneralItemView { Id = 1 },
                    XChild2 = new XChild2
                    {
                        GeneralItemView = new GeneralItemView { Id = 1 },
                        XChild3 = new XChild3
                        {
                            GeneralItemView = new GeneralItemView { Id = 1 },
                            XChild4 = new XChild4
                            {
                                GeneralItemView = new GeneralItemView { Id = 1 },
                                XChild5 = new XChild5
                                {
                                    XChild6 = null
                                }
                            }
                        }
                    }
                }
            };
            Console.WriteLine("Validation6-Test1");
            IDataAnnotationsValidatorManager validationManager = new DataAnnotationsValidatorManager();
            var validationErrorInfos = validationManager.GetErrors(request);
            validationErrorInfos.ToList().ForEach(x => Console.WriteLine(x.FormatErrorMessage));
            Assert.AreEqual(0, validationErrorInfos.Count);

            Console.WriteLine("Validation6-Test2");
            validationManager.DefaultMaxDepthLevel = 6;
            validationErrorInfos = validationManager.GetErrors(request);
            validationErrorInfos.ToList().ForEach(x => Console.WriteLine(x.FormatErrorMessage));
            Assert.AreEqual(1, validationErrorInfos.Count);
            Console.WriteLine("Validation6-Complete");
        }

        [Test]
        public void GeneralItemViewValidation()
        {
            var validatorManager = new DataAnnotationsValidatorManager();
            var instance = new GeneralItemViewValidationSampleObject {Invoice = null};
            Assert.IsFalse(validatorManager.IsValid(instance));

            instance.Invoice = new GeneralItemView();
            Assert.IsFalse(validatorManager.IsValid(instance));

            instance.Invoice.Id = -1;
            Assert.IsTrue(validatorManager.IsValid(instance));

            instance.Invoice.Id = 0;
            Assert.IsFalse(validatorManager.IsValid(instance));

            instance.Invoice.Id = 1;
            Assert.IsTrue(validatorManager.IsValid(instance));
        }
    }
}
