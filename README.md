ExtensionMethods - collection of useful C# collection methods
================

*IEnumerableExtensions*
  * <b>ForEach</b> :  allows you to execute something for each of the items without any transformations/data manipulation 
  * <b>GetRandomElement</b> : returns radnom element from the collection
  * <b>MakeString(this IEnumerable<char> chars)</b> : Create string from IEnumerable of chars
  * <b>TakeLast</b> :  Take last x elements from sequence (in order as they are in the sequence)

*BoolExtensions*
  * <b>In</b> : Returns true if value is contained in given collection
  * <b>Between</b> : Returns true if the value is between lower and upper. Works for everything that implement IComparable e.g. Int, Double etc.
  * <b>IsIncremental</b> : Checks whether objects in this collection are in incremental order. (Objects must be comparable)

*GenericExtensions*
 * <b>ToStringOrValue</b> : Return this.ToString or alternative string value if this item does not satisfy the predicate condition

*IntExtensions*
  * <b>Times</b> : 5.Times(i => Console.writeLine(i));

*EnumExtensions*
  * <b>GetDescription</b> : Makes it easy to get the description (friendly name with spaces etc.) for a given value of the enum

*DateTimeExtensions*
 * <b>ToShortDateUnique</b> : return .ToShortDate() or ShortDate+hour:minutes if there are two or more dates of the same day in list of dates
 * <b>Quarter</b> : Get the quarter of the year for this datetime.
 * <b>FirstDayOfQUarter</b> : First day of quarter to which this date belongs.
 * <b>LastDayOfQUarter</b> : Last day of quarter to which this date belongs.
 * <b>StartOfWeek</b> : Get first day of the week (that this date is part of).
 
*TimeExtensions*
 * <b>HoursToDecimal</b> : 11:40 -> 11.6

*NullableDateTimeExtensions*
  * <b>ToShortDateString</b> : Returns short date string or empty string if the value is null
  * <b>ToAge</b> : Calculates age form date of birth

*NullableExtensions*
  * <b>ToStringOrEmpty</b> : Returns value of nullable type as string ot empty string if the value is null
  * <b>double_ToStringOrEmpty</b> : Reutrns double formated according to parameter or empty string

*ListExtensions*
 * <b>Swap</b> Swap two items in List

*StringExtensions*
  * <b>IsDigitsOnly</b> : Checks if the string consists of digits only
  * <b>Println</b> : Write string to console (System.Console.WriteLine(str);)
  * <b>PrintlnAndPause</b> Write string to console and wait for ReadKey
  * <b>GetFirstLine</b> First line from string
  * <b>GetFirstWord</b> 
  * <b>GetLines</b> 
  * <b>ContainsAny</b> "my text".ContainsAny("tems1", "term 2",...)
  * <b>ContainsAll</b> "my text".ContainsAll("tems1", "term 2",...)
  * <b>RemoveDiacritics</b> (experimental)
  * <b>FloatParseCzEn</b> 
  * <b>MakeValidFileName</b>  Removes characters that are not valid in file names in Windows
  * <b>SanitizeCzech</b> 
  * <b>FirstCharToUpper</b> Capitalize first letter
  * <b>IsNotEmpty</b> Instead of !string.IsNullOrEmpty(str) just do str.IsNotEmpty()
  * <b>IsNotNullOrWhiteSpace</b> Instead of !string.IsNullOrWhiteSpace(str) just do str.IsNotEmptyOrWhiteSpace()


*DirectoryInfoExtensions*
 * <b>CopyTo</b> : Copies all files from one directory to another
 
*Helper methods*

these are static methods, not extension methods
 * <b>FirstNonEmpty</b> : first string (from parameters) that is not null and not empty (e.g.: FirstNonEmpty(CustomColor, Person?.Color, "#6F3948")
 * <b>CountDays</b> : Counts how many Mondays (or Tuesdays,..) is in given date range



*UIExtensions (separete file Extensions_GUI.cs - needs WPF)*
  * <b>FindChild</b> : UI child finder, e.g.: comboBox.FindChild(typeof(TextBox), "PART_EditableTextBox") as TextBox;
  * <b>GetScrollViewer</b> : Returns ScrollViewer of this ListBox or null
