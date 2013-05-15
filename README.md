ExtensionMethods - collection of usable C# collection methods
================


*IEnumerableExtensions*
  * <b>ForEach</b> :  allows you to execute something for each of the items without any transformations/data manipulation 

*BoolExtensions*
  * <b>In</b> : Returns true if value is contained in given collection
  * <b>Between</b> : Returns true if the value is between lower and upper. Works for everything that implement IComparable e.g. Int, Double etc.
  * <b>IsIncremental</b> : Checks whether objects in this collection are in incremental order. (Objects must be comparable)

*EnumExtensions*
  * <b>GetDescription</b> : Makes it easy to get the description (friendly name with spaces etc.) for a given value of the enum

*NullableDateTimeExtensions*
  * <b>ToShortDateString</b> : Returns short date string or empty string if the value is null
  * <b>ToAge</b> : Calculates age form date of birth

*NullableExtensions*
  * <b>ToStringOrEmpty</b> : Returns value of nullable type as string ot empty string if the value is null
  * <b>double_ToStringOrEmpty</b> : Reutrns double formated according to parameter or empty string

*ListExtensions*
 * <b>Swap</b> Swap two items in List

*UIExtensions*
  * <b>FindChild</b> : UI child finder, e.g.: comboBox.FindChild(typeof(TextBox), "PART_EditableTextBox") as TextBox;
  * <b>GetScrollViewer</b> : Returns ScrollViewer of this ListBox or null

*StringExtensions*
  * <b>IsDigitsOnly</b> : Checks if the string consists of digits only
  * <b>Println</b> : Write string to console (System.Console.WriteLine(str);)
  * <b>PrintlnAndPause</b> Write string to console and wait for ReadKey
  * <b>GetFirstLine</b> First line from string

*DirectoryInfoExtensions*
 * <b>CopyTo</b> : Copies all files from one directory to another
