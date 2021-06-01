using NUnit.Framework;
using JuanMartin.Utilities.Euler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JuanMartin.Utilities.Euler.Tests
{
    [TestFixture()]
    public class RomanNumeralTests
    {
        [Test()]
        public void ShouldBeAnInvalidRomanNumeral()
        {
            var actualRomanNumber = new RomanNumeral
            {
                Value = "ABC"
            };
            Assert.IsFalse(actualRomanNumber.IsValid(), $"{actualRomanNumber.Value} is invalid");

            actualRomanNumber.Value = "IXX";
            Assert.IsFalse(actualRomanNumber.IsValid(), $"{actualRomanNumber.Value} is invalid");

            actualRomanNumber.Value = "IL";
            Assert.IsFalse(actualRomanNumber.IsValid(), $"{actualRomanNumber.Value} is invalid");

            actualRomanNumber.Value = "VIV";
            Assert.IsFalse(actualRomanNumber.IsValid(), $"{actualRomanNumber.Value} is invalid");
        }


        [Test()]
        public void ShouldBeAnValidRomanNumeral()
        {
            var number = new RomanNumeral
            {
                Value = "XLIX"
            };
            Assert.IsTrue(number.IsValid(), $"{number.Value} is valid");

            number.Value = "XVIIII";
            Assert.IsTrue(number.IsValid(), $"{number.Value} is valid");

            number.Value = "XXXXVIIII";
            Assert.IsTrue(number.IsValid(), $"{number.Value} is valid");

            number.Value = "XIX";
            Assert.IsTrue(number.IsValid(), $"{number.Value} is valid");

            number.Value = "MCDVI";
            Assert.IsTrue(number.IsValid(), $"{number.Value} is valid");

        }

        [Test()]
        public void RomanNumeralsShouldHaveTheSameArabicRepresentation()
        {
            var expectedArabicValue = 49;
#pragma warning disable IDE0017 // Simplify object initialization
            var actualRomanNumber = new RomanNumeral();
#pragma warning restore IDE0017 // Simplify object initialization

            actualRomanNumber.Value = "XXXXIIIIIIIII";
            Assert.AreEqual(expectedArabicValue, actualRomanNumber.ToArabic(), $"{actualRomanNumber.Value} is not equal to {actualRomanNumber.ArabicValue}");

            actualRomanNumber.Value = "XXXXIX";
            Assert.AreEqual(expectedArabicValue, actualRomanNumber.ToArabic(), $"{actualRomanNumber.Value} is not equal to {actualRomanNumber.ArabicValue}");

            actualRomanNumber.Value = "XXXXVIIII";
            Assert.AreEqual(expectedArabicValue, actualRomanNumber.ToArabic(), $"{actualRomanNumber.Value} is not equal to {actualRomanNumber.ArabicValue}");

            actualRomanNumber.Value = "XLIIIIIIIII";
            Assert.AreEqual(expectedArabicValue, actualRomanNumber.ToArabic(), $"{actualRomanNumber.Value} is not equal to {actualRomanNumber.ArabicValue}");

            actualRomanNumber.Value = "XLVIIII";
            Assert.AreEqual(expectedArabicValue, actualRomanNumber.ToArabic(), $"{actualRomanNumber.Value} is not equal to {actualRomanNumber.ArabicValue}");

            actualRomanNumber.Value = "XLIX";
            Assert.AreEqual(expectedArabicValue, actualRomanNumber.ToArabic(), $"{actualRomanNumber.Value} is not equal to {actualRomanNumber.ArabicValue}");

        }

        [Test()]
        public void ShouldRetunrValidRomanFormOfMethodArgumentArabicNumberWhenGettingRomanNumeralItsMinimalForm()
        {
            var actualRomanNumber = new RomanNumeral();

            var expectedRomanNumber = "XLIX";
            var actualArabicNumber = 49;

            Assert.AreEqual(expectedRomanNumber, actualRomanNumber.GetMinimalForm(actualArabicNumber));

            expectedRomanNumber = "|VII|CCXXXI";
            actualArabicNumber = 7231;

            Assert.AreEqual(expectedRomanNumber, actualRomanNumber.GetMinimalForm(actualArabicNumber));

            expectedRomanNumber = "|LXXVI|CCXXX";
            actualArabicNumber = 76230;

            Assert.AreEqual(expectedRomanNumber, actualRomanNumber.GetMinimalForm(actualArabicNumber));
        }

        [Test()]
        public void ShouldRetunrValidRomanFormOfRomanNumeralClassValueWhenGettingItsMinimalRepresentation()
        {
            var actualRomanNumber = new RomanNumeral();
            // Generally the Romans tried to use as few numerals as possible when displaying numbers.For this reason, 
            // XIX would be the preferred form of nineteen over other valid combinations, like XIIIIIIIII or XVIIII.
            var expectedRomanNumeral = "XIX";
            var actualRomanNumeral = "XVIIII";
            actualRomanNumber.Value = actualRomanNumeral;
            Assert.AreEqual(expectedRomanNumeral, actualRomanNumber.GetMinimalForm());

            actualRomanNumeral = "XIIIIIIIII";
            actualRomanNumber.Value = actualRomanNumeral;
            Assert.AreEqual(expectedRomanNumeral, actualRomanNumber.GetMinimalForm());

            // In the church of Sant'Agnese fuori le Mura (St Agnes' outside 
            // the walls), found in Rome, the date, MCCCCCCVI (1606), is written on the gilded and coffered wooden 
            // ceiling; I am sure that many would argue that it should have been written MDCVI.
            expectedRomanNumeral = "MDCVI";
            actualRomanNumeral = "MCCCCCCVI";
            actualRomanNumber.Value = actualRomanNumeral;
            Assert.AreEqual(expectedRomanNumeral, actualRomanNumber.GetMinimalForm());


            expectedRomanNumeral = "MCCLXXIX";
            actualRomanNumeral = "MCCLXXVIIII";
            actualRomanNumber.Value = actualRomanNumeral;
            Assert.AreEqual(expectedRomanNumeral, actualRomanNumber.GetMinimalForm());
        }

        [Test()]
        public void ShouldSpecifyThatRomanNumeralMMCCLIVIsItsOwnMinimalForm()
        {
            var number = new RomanNumeral();
            var actualRomanNumeral = "MMCCLIV";
            var expectedRomanNumeral = actualRomanNumeral;
            number.Value = actualRomanNumeral;
            Assert.AreEqual(expectedRomanNumeral, number.GetMinimalForm());
        }

        [Test()]
        public void ShouldSpecifyThatRomanNumeralDIsItsOwnMinimalForm()
        {
            var number = new RomanNumeral();
            var actualRomanNumeral = "D";
            var expectedRomanNumeral = actualRomanNumeral;
            number.Value = actualRomanNumeral;
            Assert.AreEqual(expectedRomanNumeral, number.GetMinimalForm());
        }

        [Test()]
        public void ShouldSpecifyThatRomanNumeralIVIsItsOwnMinimalForm()
        {
            var number = new RomanNumeral();
            var actualRomanNumeral = "IV";
            var expectedRomanNumeral = actualRomanNumeral;
            number.Value = actualRomanNumeral;
            Assert.AreEqual(expectedRomanNumeral, number.GetMinimalForm());
        }
    }
}