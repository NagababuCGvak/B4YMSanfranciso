using System;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;

namespace Tar.Core.Utils
{
    /// <summary>
    /// Provides methods for checking method arguments for validity and throwing localizable exceptions for invalid
    /// arguments or argument combinations.
    /// </summary>
    [DebuggerStepThrough]
    public static class ArgumentValidation
    {
        #region Null/Empty Checking

        /// <summary>
        /// Checks the specified parameter to ensure it is not null and if so, throws an <see cref="T:ArgumentNullException"/>.
        /// </summary>
        /// <param name="value">The parameter value to compare with null.</param>
        /// <param name="parameterName">Name of the parameter.</param>
        /// <exception cref="T:ArgumentNullException">The parameter is null.</exception>
        public static void ThrowIfNull(object value, string parameterName = null)
        {
            if (value == null)
            {
                throw new ArgumentNullException(parameterName);
            }
        }

        /// <summary>
        /// Checks the specified string to ensure it is not null or empty and if so, throws an <see cref="T:ArgumentNullException"/>.
        /// </summary>
        /// <param name="value">The parameter value to check.</param>
        /// <param name="parameterName">Name of the parameter.</param>
        /// <exception cref="T:ArgumentNullException">The parameter is null.</exception>
        /// <exception cref="T:ArgumentException">The parameter is an empty string or a string consisting of only whitespace.</exception>
        public static void ThrowIfNullOrEmpty(string value, string parameterName = null)
        {
            if (value == null)
            {
                throw new ArgumentNullException(parameterName);
            }

            if (value.Length == 0)
            {
                throw new ArgumentException(String.Format("{0} cannot be an empty string.", parameterName ?? "Value"), parameterName);
            }
        }

        /// <summary>
        /// Checks the specified string to ensure it is not null, empty, or consists solely of whitespace and if so, throws an <see cref="T:ArgumentNullException"/>.
        /// </summary>
        /// <param name="value">The parameter value to check.</param>
        /// <param name="parameterName">Name of the parameter.</param>
        /// <exception cref="T:ArgumentNullException">The parameter is null.</exception>
        /// <exception cref="T:ArgumentException">The parameter is an empty string or a string consisting of only whitespace.</exception>
        public static void ThrowIfNullOrWhitespace(string value, string parameterName = null)
        {
            if (value == null)
            {
                throw new ArgumentNullException(parameterName);
            }

            if (value.Length == 0)
            {
                throw new ArgumentException(String.Format("{0} cannot be an empty string.", parameterName ?? "Value"), parameterName);
            }

            for (int i = 0; i < value.Length; i++)
            {
                if (!Char.IsWhiteSpace(value, i))
                {
                    // Found a non-whitespace character
                    return;
                }
            }

            throw new ArgumentException(String.Format("{0} cannot consist entirely of whitespace.", parameterName ?? "Value"), parameterName);
        }

        /// <summary>
        /// Checks the specified array to ensure it is not null or empty and if so, throws an <see cref="T:ArgumentNullException"/>.
        /// </summary>
        /// <param name="array">The parameter value to check.</param>
        /// <param name="parameterName">Name of the parameter.</param>
        /// <exception cref="T:ArgumentNullException">The parameter is null.</exception>
        /// <exception cref="T:ArgumentException">The parameter is an empty array.</exception>
        public static void ThrowIfNullOrEmpty(Array array, string parameterName = null)
        {
            if (array == null)
            {
                throw new ArgumentNullException(parameterName);
            }

            if (array.Length == 0)
            {
                throw new ArgumentException(String.Format("{0} cannot be an empty array.", parameterName ?? "Array"), parameterName);
            }
        }

        /// <summary>
        /// Checks the specified array to ensure it does not contain any null references and if so,
        /// throws an <see cref="T:ArgumentException"/>.
        /// </summary>
        /// <remarks>
        /// If the value specified in the <paramref name="array"/> parameter is itself null, no check
        /// is performed. Use <see cref="M:ThrowIfNull"/> or <see cref="M:ThrowIfNullOrEmpty"/> if
        /// additional validation is needed.
        /// </remarks>
        /// <param name="array">The array to check. Only the first dimension is checked.</param>
        /// <param name="parameterName">Name of the parameter.</param>
        /// <exception cref="T:ArgumentException">The array contains at least one null reference.</exception>
        public static void ThrowIfNullElement(Array array, string parameterName = null)
        {
            // Don't check null arrays
            if (array != null)
            {
                for (long i = 0; i < array.LongLength; i++)
                {
                    if (array.GetValue(i) == null)
                    {
                        throw new ArgumentException(String.Format("{0} cannot contain null references.", parameterName ?? "Array"), parameterName);
                    }
                }
            }
        }

        /// <summary>
        /// Checks the specified collection to ensure it does not contain any null references and if so,
        /// throws an <see cref="T:ArgumentException"/>.
        /// </summary>
        /// <remarks>
        /// If the value specified in the <paramref name="collection"/> parameter is itself null, no check
        /// is performed. Use <see cref="M:ThrowIfNull"/> or <see cref="M:ThrowIfNullOrEmpty"/> if
        /// additional validation is needed.
        /// The collections non-generic enumerator will be used to enumerate the collection, even if the
        /// type implements IList.
        /// </remarks>
        /// <param name="collection">The collection to check.</param>
        /// <param name="parameterName">Name of the parameter.</param>
        /// <exception cref="T:ArgumentException">The collection contains at least one null reference.</exception>
        public static void ThrowIfNullElement(System.Collections.IEnumerable collection, string parameterName = null)
        {
            // Don't check null collections
            if (collection != null)
            {
                foreach (object item in collection)
                {
                    if (item == null)
                    {
                        throw new ArgumentException(String.Format("{0} cannot contain null references.", parameterName ?? "Collection"), parameterName);
                    }
                }
            }
        }

        #endregion Null/Empty Checking

        #region Pattern Checking

        /// <summary>
        /// Matches the specified <paramref name="value"/> against a regular expression, <paramref name="regex"/>,
        /// and throws an <see cref="T:ArgumentException"/> if the pattern does not match.
        /// </summary>
        /// <remarks>
        /// If the value specified in the <paramref name="value"/> parameter is itself null, no check
        /// is performed. Use <see cref="M:ThrowIfNull"/> or <see cref="M:ThrowIfNullOrEmpty"/> if
        /// additional validation is needed.
        /// </remarks>
        /// <param name="value">The parameter, which is not checked if it's null.</param>
        /// <param name="regex">The regular expression pattern to match.</param>
        /// <exception cref="T:ArgumentException"><paramref name="value"/> does not match the regular expression.</exception>
        public static void ThrowIfPatternFails(string value, string regex)
        {
            ThrowIfPatternFails(value, null, new Regex(regex));
        }

        /// <summary>
        /// Matches the specified <paramref name="value"/> against a regular expression, <paramref name="regex"/>,
        /// and throws an <see cref="T:ArgumentException"/> if the pattern does not match.
        /// </summary>
        /// <remarks>
        /// If the value specified in the <paramref name="value"/> parameter is itself null, no check
        /// is performed. Use <see cref="M:ThrowIfNull"/> or <see cref="M:ThrowIfNullOrEmpty"/> if
        /// additional validation is needed.
        /// </remarks>
        /// <param name="value">The parameter, which is not checked if it's null.</param>
        /// <param name="parameterName">Name of the parameter.</param>
        /// <param name="regex">The regular expression pattern to match.</param>
        /// <exception cref="T:ArgumentException"><paramref name="value"/> does not match the regular expression.</exception>
        public static void ThrowIfPatternFails(string value, string parameterName, string regex)
        {
            ThrowIfPatternFails(value, parameterName, new Regex(regex));
        }

        /// <summary>
        /// Matches the specified <paramref name="value"/> against a regular expression, <paramref name="regex"/>,
        /// and throws an <see cref="T:ArgumentException"/> if the pattern does not match.
        /// </summary>
        /// <remarks>
        /// If the value specified in the <paramref name="value"/> parameter is itself null, no check
        /// is performed. Use <see cref="M:ThrowIfNull"/> or <see cref="M:ThrowIfNullOrEmpty"/> if
        /// additional validation is needed.
        /// </remarks>
        /// <param name="value">The parameter, which is not checked if it's null.</param>
        /// <param name="regex">The regular expression pattern to match.</param>
        /// <exception cref="T:ArgumentException"><paramref name="value"/> does not match the regular expression.</exception>
        public static void ThrowIfPatternFails(string value, Regex regex)
        {
            ThrowIfPatternFails(value, null, regex);
        }

        /// <summary>
        /// Matches the specified <paramref name="value"/> against a regular expression, <paramref name="regex"/>,
        /// and throws an <see cref="T:ArgumentException"/> if the pattern does not match.
        /// </summary>
        /// <remarks>
        /// If the value specified in the <paramref name="value"/> parameter is itself null, no check
        /// is performed. Use <see cref="M:ThrowIfNull"/> or <see cref="M:ThrowIfNullOrEmpty"/> if
        /// additional validation is needed.
        /// </remarks>
        /// <param name="value">The parameter, which is not checked if it's null.</param>
        /// <param name="parameterName">Name of the parameter.</param>
        /// <param name="regex">The regular expression pattern to match.</param>
        /// <exception cref="T:ArgumentException"><paramref name="value"/> does not match the regular expression.</exception>
        public static void ThrowIfPatternFails(string value, string parameterName, Regex regex)
        {
            // Don't check null strings
            if (value != null)
            {
                if (!regex.IsMatch(value))
                {
                    throw new ArgumentException(String.Format("{0} does not match the expected format.", parameterName ?? "Value"), parameterName);
                }
            }
        }

        #endregion Pattern Checking

        #region Range Checking

        /// <summary>
        /// Throws an <see cref="T:ArgumentOutOfRangeException"/> exception of the specified <paramref name="value"/> is not
        /// a valid value within the specified range.
        /// </summary>
        /// <param name="value">The enum value to validate.</param>
        /// <param name="parameterName">The name of the parameter.</param>
        /// <exception cref="T:ArgumentOutOfRangeException"><paramref name="value"/> is not a valid enum value.</exception>
        public static void ThrowIfOutOfRange(Enum value, string parameterName = null)
        {
            Type enumType = value.GetType();

            if (!Enum.IsDefined(enumType, value))
            {
                ThrowArgumentOutOfRangeException(parameterName, value, String.Format("{0} is not a valid {1} value.", parameterName ?? "Value", enumType.Name));
            }
        }

        /// <summary>
        /// Throws an <see cref="T:ArgumentOutOfRangeException"/> exception of the specified <paramref name="value"/> is not
        /// a valid value within the specified range.
        /// </summary>
        /// <param name="value">The parameter value to validate.</param>
        /// <param name="parameterName">The name of the parameter.</param>
        /// <param name="min">The minimum allowable value (inclusive).</param>
        /// <param name="max">The maximum allowable value (inclusive).</param>
        /// <exception cref="T:ArgumentOutOfRangeException"><paramref name="value"/> is not within the inclusive range.</exception>
        public static void ThrowIfOutOfRange(int value, string parameterName = null, int min = 0, int max = Int32.MaxValue)
        {
            if (value > max || value < min)
            {
                ThrowArgumentOutOfRangeException(parameterName, value, String.Format("{0} is out of range.", parameterName ?? "Value"));
            }
        }

        /// <summary>
        /// Throws an <see cref="T:ArgumentOutOfRangeException"/> exception of the specified <paramref name="value"/> is not
        /// a valid value within the specified range.
        /// </summary>
        /// <param name="value">The parameter value to validate.</param>
        /// <param name="parameterName">The name of the parameter.</param>
        /// <param name="min">The minimum allowable value (inclusive).</param>
        /// <param name="max">The maximum allowable value (inclusive).</param>
        /// <exception cref="T:ArgumentOutOfRangeException"><paramref name="value"/> is not within the inclusive range.</exception>
        public static void ThrowIfOutOfRange(long value, string parameterName = null, long min = 0, long max = Int64.MaxValue)
        {
            if (value > max || value < min)
            {
                ThrowArgumentOutOfRangeException(parameterName, value, String.Format("{0} is out of range.", parameterName ?? "Value"));
            }
        }

        /// <summary>
        /// Throws an <see cref="T:ArgumentOutOfRangeException"/> exception of the specified <paramref name="value"/> is not
        /// a valid value within the specified range.
        /// </summary>
        /// <param name="value">The parameter value to validate.</param>
        /// <param name="parameterName">The name of the parameter.</param>
        /// <param name="min">The minimum allowable value (inclusive).</param>
        /// <param name="max">The maximum allowable value (inclusive).</param>
        /// <exception cref="T:ArgumentOutOfRangeException"><paramref name="value"/> is not within the inclusive range.</exception>
        public static void ThrowIfOutOfRange(decimal value, string parameterName = null, decimal min = 0, decimal max = Decimal.MaxValue)
        {
            if (value > max || value < min)
            {
                ThrowArgumentOutOfRangeException(parameterName, value, String.Format("{0} is out of range.", parameterName ?? "Value"));
            }
        }

        /// <summary>
        /// Throws an <see cref="T:ArgumentOutOfRangeException"/> exception of the specified <paramref name="value"/> is not
        /// a valid value within the specified range.
        /// </summary>
        /// <param name="value">The parameter value to validate.</param>
        /// <param name="parameterName">The name of the parameter.</param>
        /// <param name="min">The minimum allowable value (inclusive).</param>
        /// <param name="max">The maximum allowable value (inclusive).</param>
        /// <exception cref="T:ArgumentOutOfRangeException"><paramref name="value"/> is not within the inclusive range.</exception>
        public static void ThrowIfOutOfRange(double value, string parameterName = null, double min = 0, double max = Double.MaxValue)
        {
            if (value > max || value < min)
            {
                ThrowArgumentOutOfRangeException(parameterName, value, String.Format("{0} is out of range.", parameterName ?? "Value"));
            }
        }

        /// <summary>
        /// Throws an <see cref="T:ArgumentOutOfRangeException"/> exception of the specified <paramref name="value"/> is not
        /// a valid value within the specified range.
        /// </summary>
        /// <remarks>
        /// If the value specified in the <paramref name="value"/> parameter is a null reference, the check will not
        /// be performed. Use <see cref="M:ThrowIfNull"/> if additional validation is needed.
        /// </remarks>
        /// <param name="value">The parameter value to validate. If this value is null, no check is performed.</param>
        /// <param name="min">The minimum allowable value (inclusive).</param>
        /// <param name="max">The maximum allowable value (inclusive).</param>
        /// <exception cref="T:ArgumentOutOfRangeException"><paramref name="value"/> is not within the inclusive range.</exception>
        public static void ThrowIfOutOfRange<T>(T value, T min, T max) where T : IComparable
        {
            // Can't use optional parameters here because we do not know what sensible defaults to use for
            // min and max value - especially if T is a reference type
            ThrowIfOutOfRange<T>(value, null, min, max);
        }

        /// <summary>
        /// Throws an <see cref="T:ArgumentOutOfRangeException"/> exception of the specified <paramref name="value"/> is not
        /// a valid value within the specified range.
        /// </summary>
        /// <remarks>
        /// If the value specified in the <paramref name="value"/> parameter is a null reference, the check will not
        /// be performed. Use <see cref="M:ThrowIfNull"/> if additional validation is needed.
        /// </remarks>
        /// <param name="value">The parameter value to validate. If this value is null, no check is performed.</param>
        /// <param name="parameterName">The name of the parameter.</param>
        /// <param name="min">The minimum allowable value (inclusive).</param>
        /// <param name="max">The maximum allowable value (inclusive).</param>
        /// <exception cref="T:ArgumentOutOfRangeException"><paramref name="value"/> is not within the inclusive range.</exception>
        public static void ThrowIfOutOfRange<T>(T value, string parameterName, T min, T max) where T : IComparable
        {
            // Skip null values
            if (value != null)
            {
                if (min != null && value.CompareTo(min) < 0)
                {
                    ThrowArgumentOutOfRangeException(parameterName, value, String.Format("{0} is out of range.", parameterName ?? "Value"));
                }

                if (max != null && value.CompareTo(max) > 1)
                {
                    ThrowArgumentOutOfRangeException(parameterName, value, String.Format("{0} is out of range.", parameterName ?? "Value"));
                }
            }
        }

        #endregion Range Checking

        #region Negative Checking

        /// <summary>
        /// Throws an <see cref="T:ArgumentOutOfRangeException"/> exception of the specified <paramref name="value"/> is less
        /// than zero.
        /// </summary>
        /// <param name="value">The parameter value to validate.</param>
        /// <param name="parameterName">The name of the parameter.</param>
        /// <exception cref="T:ArgumentOutOfRangeException"><paramref name="value"/> is negative.</exception>
        public static void ThrowIfNegative(int value, string parameterName = null)
        {
            if (value < 0)
            {
                ThrowArgumentOutOfRangeException(parameterName, value, String.Format("{0} cannot be negative.", parameterName ?? "Value"));
            }
        }

        /// <summary>
        /// Throws an <see cref="T:ArgumentOutOfRangeException"/> exception of the specified <paramref name="value"/> is less
        /// than zero.
        /// </summary>
        /// <param name="value">The parameter value to validate.</param>
        /// <param name="parameterName">The name of the parameter.</param>
        /// <exception cref="T:ArgumentOutOfRangeException"><paramref name="value"/> is negative.</exception>
        public static void ThrowIfNegative(long value, string parameterName = null)
        {
            if (value < 0L)
            {
                ThrowArgumentOutOfRangeException(parameterName, value, String.Format("{0} cannot be negative.", parameterName ?? "Value"));
            }
        }

        /// <summary>
        /// Throws an <see cref="T:ArgumentOutOfRangeException"/> exception of the specified <paramref name="value"/> is less
        /// than zero.
        /// </summary>
        /// <param name="value">The parameter value to validate.</param>
        /// <param name="parameterName">The name of the parameter.</param>
        /// <exception cref="T:ArgumentOutOfRangeException"><paramref name="value"/> is negative.</exception>
        public static void ThrowIfNegative(decimal value, string parameterName = null)
        {
            if (value < 0m)
            {
                ThrowArgumentOutOfRangeException(parameterName, value, String.Format("{0} cannot be negative.", parameterName ?? "Value"));
            }
        }

        /// <summary>
        /// Throws an <see cref="T:ArgumentOutOfRangeException"/> exception of the specified <paramref name="value"/> is less
        /// than zero.
        /// </summary>
        /// <param name="value">The parameter value to validate.</param>
        /// <param name="parameterName">The name of the parameter.</param>
        /// <exception cref="T:ArgumentOutOfRangeException"><paramref name="value"/> is negative.</exception>
        public static void ThrowIfNegative(double value, string parameterName = null)
        {
            if (value < 0d)
            {
                ThrowArgumentOutOfRangeException(parameterName, value, String.Format("{0} cannot be negative.", parameterName ?? "Value"));
            }
        }

        /// <summary>
        /// Throws an <see cref="T:ArgumentOutOfRangeException"/> exception of the specified <paramref name="value"/> is less
        /// than zero.
        /// </summary>
        /// <param name="value">The parameter value to validate.</param>
        /// <param name="parameterName">The name of the parameter.</param>
        /// <exception cref="T:ArgumentOutOfRangeException"><paramref name="value"/> is negative.</exception>
        public static void ThrowIfNegative(TimeSpan value, string parameterName = null)
        {
            if (value.Ticks < 0L)
            {
                ThrowArgumentOutOfRangeException(parameterName, value, String.Format("{0} cannot be negative.", parameterName ?? "Value"));
            }
        }

        /// <summary>
        /// Throws an <see cref="T:ArgumentOutOfRangeException"/> exception of the specified <paramref name="value"/> is less
        /// than one.
        /// </summary>
        /// <param name="value">The parameter value to validate.</param>
        /// <param name="parameterName">The name of the parameter.</param>
        /// <exception cref="T:ArgumentOutOfRangeException"><paramref name="value"/> is not greater than zero.</exception>
        public static void ThrowIfNegativeOrZero(int value, string parameterName)
        {
            if (value <= 0)
            {
                ThrowArgumentOutOfRangeException(parameterName, value, String.Format("{0} must be greater than zero.", parameterName ?? "Value"));
            }
        }

        /// <summary>
        /// Throws an <see cref="T:ArgumentOutOfRangeException"/> exception of the specified <paramref name="value"/> is less
        /// than one.
        /// </summary>
        /// <param name="value">The parameter value to validate.</param>
        /// <param name="parameterName">The name of the parameter.</param>
        /// <exception cref="T:ArgumentOutOfRangeException"><paramref name="value"/> is not greater than zero.</exception>
        public static void ThrowIfNegativeOrZero(long value, string parameterName)
        {
            if (value <= 0L)
            {
                ThrowArgumentOutOfRangeException(parameterName, value, String.Format("{0} must be greater than zero.", parameterName ?? "Value"));
            }
        }

        /// <summary>
        /// Throws an <see cref="ArgumentOutOfRangeException"/> exception of the specified <paramref name="value"/> is less
        /// than one.
        /// </summary>
        /// <param name="value">The parameter value to validate.</param>
        /// <param name="parameterName">The name of the parameter.</param>
        /// <exception cref="T:ArgumentOutOfRangeException"><paramref name="value"/> is not greater than zero.</exception>
        public static void ThrowIfNegativeOrZero(decimal value, string parameterName)
        {
            if (value <= 0m)
            {
                ThrowArgumentOutOfRangeException(parameterName, value, String.Format("{0} must be greater than zero.", parameterName ?? "Value"));
            }
        }

        /// <summary>
        /// Throws an <see cref="ArgumentOutOfRangeException"/> exception of the specified <paramref name="value"/> is less
        /// than or equal to zero.
        /// </summary>
        /// <param name="value">The parameter value to validate.</param>
        /// <param name="parameterName">The name of the parameter.</param>
        /// <exception cref="T:ArgumentOutOfRangeException"><paramref name="value"/> is not greater than zero.</exception>
        public static void ThrowIfNegativeOrZero(double value, string parameterName)
        {
            if (value <= 0d)
            {
                ThrowArgumentOutOfRangeException(parameterName, value, String.Format("{0} must be greater than zero.", parameterName ?? "Value"));
            }
        }

        /// <summary>
        /// Throws an <see cref="ArgumentOutOfRangeException"/> exception of the specified <paramref name="value"/> is less
        /// than or equal to zero.
        /// </summary>
        /// <param name="value">The parameter value to validate.</param>
        /// <param name="parameterName">The name of the parameter.</param>
        /// <exception cref="T:ArgumentOutOfRangeException"><paramref name="value"/> is not greater than zero.</exception>
        public static void ThrowIfNegativeOrZero(TimeSpan value, string parameterName = null)
        {
            if (value.Ticks <= 0L)
            {
                ThrowArgumentOutOfRangeException(parameterName, value, String.Format("{0} must be greater than zero.", parameterName ?? "Value"));
            }
        }

        #endregion Negative Checking

        #region IO Exceptions

        const int MAX_PATH = 260;

#if !SILVERLIGHT

        /// <summary>
        /// Throws an <see cref="T:ArgumentException"/> if the specified <paramref name="fileName"/> contains
        /// characters that are invalid in a file name such as directory separators or other reserved characters.
        /// Throws a <see cref="T:PathTooLongException"/> if the file name is too long.
        /// </summary>
        /// <remarks>
        /// If the <paramref name="fileName"/> parameter is null, no checks will be performed. Use the
        /// <see cref="M:ThrowIfNull"/> if additional checks are necessary.
        /// </remarks>
        /// <param name="fileName">The file name to validate.</param>
        /// <param name="parameterName">The name of the parameter.</param>
        /// <exception cref="T:PathTooLongException">The file name contains too many characters.</exception>
        /// <exception cref="T:ArgumentException">The file name contains invalid characters.</exception>
        public static void ThrowIfInvalidFileName(string fileName, string parameterName = null)
        {
            if (fileName != null)
            {
                if (fileName.Length > MAX_PATH)
                {
                    throw new PathTooLongException(String.Format("{0} is too long.", parameterName ?? "Path"));
                }

                char[] invalidChars = Path.GetInvalidFileNameChars();

                foreach (char c in fileName)
                {
                    if (Array.IndexOf(invalidChars, c) > -1)
                    {
                        throw new ArgumentException(String.Format("{0} contains invalid characters.", parameterName ?? "File name"), parameterName);
                    }
                }
            }
        }

        /// <summary>
        /// Throws an <see cref="T:ArgumentException"/> if the specified <paramref name="path"/> contains
        /// characters that are invalid in a path name such as directory separators or other reserved characters.
        /// Throws a <see cref="T:PathTooLongException"/> if the path is too long.
        /// </summary>
        /// <remarks>
        /// If the <paramref name="path"/> parameter is null, no checks will be performed. Use the
        /// <see cref="M:ThrowIfNull"/> if additional checks are necessary.
        /// </remarks>
        /// <param name="path">The path to validate.</param>
        /// <param name="parameterName">The name of the parameter.</param>
        /// <exception cref="T:PathTooLongException">The path contains too many characters.</exception>
        /// <exception cref="T:ArgumentException">The path contains invalid characters.</exception>
        public static void ThrowIfInvalidPath(string path, string parameterName = null)
        {
            if (path != null)
            {
                if (path.Length > MAX_PATH)
                {
                    throw new PathTooLongException(String.Format("{0} is too long.", parameterName ?? "Path"));
                }

                char[] invalidChars = Path.GetInvalidPathChars();

                foreach (char c in path)
                {
                    if (Array.IndexOf(invalidChars, c) > -1)
                    {
                        throw new ArgumentException(String.Format("{0} contains invalid characters.", parameterName ?? "Path"), parameterName);
                    }
                }
            }
        }

        /// <summary>
        /// Throws a <see cref="T:FileNotFoundException"/> if the specified file does not exist on disk.
        /// </summary>
        /// <remarks>
        /// If the value specified in the <paramref name="fileName"/> parameter is itself null, no check
        /// is performed. Use <see cref="M:ThrowIfNull"/> or <see cref="M:ThrowIfNullOrEmpty"/> if
        /// additional validation is needed.
        /// </remarks>
        /// <param name="fileName">The path and file name to check the existence of.</param>
        /// <param name="parameterName">The name of the parameter.</param>
        /// <exception cref="T:FileNotFoundException">The <paramref name="fileName"/> specified does not exist.</exception>
        public static void ThrowIfFileNotFound(string fileName, string parameterName = null)
        {
            if (fileName != null)
            {
                if (!File.Exists(fileName))
                {
                    throw new FileNotFoundException(String.Format("{0}: File not found <{1}>", parameterName ?? "File", fileName), fileName);
                }
            }
        }

        /// <summary>
        /// Throws a <see cref="T:DirectoryNotFoundException"/> if the specified file does not exist on disk.
        /// </summary>
        /// <remarks>
        /// If the value specified in the <paramref name="directory"/> parameter is itself null, no check
        /// is performed. Use <see cref="M:ThrowIfNull"/> or <see cref="M:ThrowIfNullOrEmpty"/> if
        /// additional validation is needed.
        /// </remarks>
        /// <param name="directory">The directory path to check the existence of.</param>
        /// <param name="parameterName">The name of the parameter.</param>
        /// <exception cref="T:DirectoryNotFoundException">The <paramref name="directory"/> specified does not exist.</exception>
        public static void ThrowIfDirectoryNotFound(string directory, string parameterName = null)
        {
            if (directory != null)
            {
                if (!Directory.Exists(directory))
                {
                    throw new DirectoryNotFoundException(String.Format("{0}: Directory not found <{1}>", parameterName ?? "Directory", directory));
                }
            }
        }

#endif

        #endregion IO Exceptions

        #region Exception Helpers

        /// <summary>
        /// Throws a <see cref="T:ArgumentOutOfRangeException"/>
        /// </summary>
        /// <param name="paramName">The name of the parameter that caused the exception.</param>
        /// <param name="actualValue">The value of the argument that causes this exception.</param>
        /// <param name="message">The message that describes the error.</param>
        private static void ThrowArgumentOutOfRangeException(string paramName, object actualValue, string message)
        {
#if !SILVERLIGHT
            throw new ArgumentOutOfRangeException(paramName, actualValue, message);
#else
    throw new ArgumentOutOfRangeException( paramName, message );
#endif
        }

        #endregion Exception Helpers
    }
}